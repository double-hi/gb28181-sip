using System.Threading.Tasks;
using Grpc.Core;
using GrpcDeviceCatalog;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Sys.XML;
using Newtonsoft.Json;

namespace GrpcAgent.WebsocketRpcServer
{
    public class DeviceCatalogImpl : DeviceCatalog.DeviceCatalogBase
    {
        private ISIPServiceDirector _sipServiceDirector = null;

        public DeviceCatalogImpl(ISIPServiceDirector sipServiceDirector)
        {
            _sipServiceDirector = sipServiceDirector;
        }
        
        public override Task<GetCatalogReply> GetCatalog(GetCatalogRequest request, ServerCallContext context)
        {
            _sipServiceDirector.GetCatalog(request.Deviceid);
            Catalog _Catalog = null;
            while (true)
            {
                foreach (Catalog obj in _sipServiceDirector.Catalogs.Values)
                {
                    if (request.Deviceid.Equals(obj.DeviceID))
                    {
                        _Catalog = obj;
                    }
                }
                if (_Catalog == null)
                {
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }
            string jsonCatalog = JsonConvert.SerializeObject(_Catalog)
                .Replace("\"Block\":null", "\"Block\":\"null\"")
                .Replace("\"ParentID\":null", "\"ParentID\":\"null\"")
                .Replace("\"BusinessGroupID\":null", "\"BusinessGroupID\":\"null\"")
                .Replace("\"CertNum\":null", "\"CertNum\":\"null\"")
                .Replace("\"Certifiable\":null", "\"Certifiable\":0")
                .Replace("\"ErrCode\":null", "\"ErrCode\":0")
                .Replace("\"EndTime\":null", "\"EndTime\":\"null\"")
                .Replace("\"Secrecy\":null", "\"Secrecy\":0")
                .Replace("\"Password\":null", "\"Password\":\"null\"")
                .Replace("\"Longitude\":null", "\"Longitude\":0")
                .Replace("\"Latitude\":null", "\"Latitude\":0");
            Instance instance = JsonConvert.DeserializeObject<Instance>(jsonCatalog);
            return Task.FromResult(new GetCatalogReply { Catalog = instance });
        }
    }
}
