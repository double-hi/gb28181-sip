using System.Threading.Tasks;
using Grpc.Core;
using GrpcDeviceCatalog;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Sys.XML;
using Newtonsoft.Json;
using System;

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
            //{"CmdType":2,"SN":42058,"DeviceID":"34030000002000000001","SumNum":12,"DeviceList":{"Items":[{"DeviceID":"31011550","Name":"浦东新区张江高科","Manufacturer":null,"Model":null,"Owner":null,"CivilCode":null,"Block":"null","Address":null,"Parental":null,"ParentalValue":null,"ParentID":"null","BusinessGroupID":"null","SafetyWay":null,"SafetyWayValue":null,"RegisterWay":null,"RegisterWayValue":null,"CertNum":"null","Certifiable":0,"CertifiableValue":null,"ErrCode":0,"ErrCodeValue":null,"EndTime":"null","Secrecy":0,"SecrecyValue":null,"IPAddress":null,"Port":null,"PortValue":null,"Password":"null","Status":0,"Longitude":0,"LongitudeValue":null,"Latitude":0,"LatitudeValue":null,"InfList":null,"RemoteEP":"10.77.38.86:5060"}]}}
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
                .Replace("\"Latitude\":null", "\"Latitude\":0")
                .Replace("\"Parental\":null", "\"Parental\":0")
                .Replace("\"SafetyWay\":null", "\"SafetyWay\":0")
                .Replace("\"RegisterWay\":null", "\"RegisterWay\":0")
                .Replace("\"Port\":null", "\"Port\":0")
                .Replace(":null", ":\"null\"")
                .Replace(",\"InfList\":\"null\"", "");//delete InfList
            Instance instance = JsonConvert.DeserializeObject<Instance>(jsonCatalog);
            return Task.FromResult(new GetCatalogReply { Catalog = instance });
        }

        public override Task<DeviceCatalogSubscribeReply> DeviceCatalogSubscribe(DeviceCatalogSubscribeRequest request, ServerCallContext context)
        {
            string msg = "OK";
            try
            {
                _sipServiceDirector.DeviceCatalogSubscribe(request.Deviceid);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Task.FromResult(new DeviceCatalogSubscribeReply { Message = msg });
        }

        public override Task<DeviceCatalogNotifyReply> DeviceCatalogNotify(DeviceCatalogNotifyRequest request, ServerCallContext context)
        {
            string msg = "OK";
            NotifyCatalog.Item _NotifyCatalogItem = null;
            Item instance = null;
            try
            {
                while (_sipServiceDirector.NotifyCatalogItem.Count > 0)
                {
                    lock (_sipServiceDirector.NotifyCatalogItem)
                    {
                        _NotifyCatalogItem = _sipServiceDirector.NotifyCatalogItem.Dequeue();
                    }
                }
                string jsonObj = JsonConvert.SerializeObject(_NotifyCatalogItem)
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
                .Replace("\"Latitude\":null", "\"Latitude\":0")
                .Replace("\"Parental\":null", "\"Parental\":0")
                .Replace("\"SafetyWay\":null", "\"SafetyWay\":0")
                .Replace("\"RegisterWay\":null", "\"RegisterWay\":0")
                .Replace("\"Port\":null", "\"Port\":0")
                .Replace(":null", ":\"null\"")
                .Replace(",\"InfList\":\"null\"", "");//delete InfList
                instance = JsonConvert.DeserializeObject<Item>(jsonObj);
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Task.FromResult(new DeviceCatalogNotifyReply { Item = instance });
        }
    }
}
