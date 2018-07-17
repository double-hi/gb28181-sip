using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcDeviceCatalog;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Servers.SIPMessage;
using SIPSorcery.GB28181.Sys.XML;

namespace GrpcAgent.WebsocketRpcServer
{
    public class DeviceCatalogImpl : DeviceCatalog.DeviceCatalogBase
    {
        private ISIPCatalogCore _sipCatalogCore = null;
        private ISipMessageCore _sipMessageCore = null;
        private Dictionary<string, Catalog> _catalogs = new Dictionary<string, Catalog>();

        public DeviceCatalogImpl(ISIPCatalogCore sipCatalogCore, ISipMessageCore sipMessageCore)
        {
            _sipCatalogCore = sipCatalogCore;
            _sipMessageCore = sipMessageCore;
            _sipMessageCore.OnCatalogReceived += _sipMessageCore_OnCatalogReceived;
        }

        private void _sipMessageCore_OnCatalogReceived(Catalog cata)
        {
            if (!_catalogs.ContainsKey(cata.DeviceID))
            {
                _catalogs.Add(cata.DeviceID, cata);
            }
        }

        // Server side handler of the SayHello RPC
        public override Task<GetCatalogReply> GetCatalog(GetCatalogRequest request, ServerCallContext context)
        {
            _sipCatalogCore.GetCatalog(request.Deviceid);
            string x = "Status: 200 OK";
            Catalog _Catalog = null;
            while (true)
            {
                foreach (Catalog obj in _catalogs.Values)
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
            return Task.FromResult(new GetCatalogReply { Message = x });
        }

        // Server side handler of the SayHello RPC
        public override Task<DeviceCatalogSubscribeReply> DeviceCatalogSubscribe(DeviceCatalogSubscribeRequest request, ServerCallContext context)
        {
            _sipCatalogCore.DeviceCatalogSubscribe(request.Deviceid);
            string x = "Status: 200 OK";
            return Task.FromResult(new DeviceCatalogSubscribeReply { Message = x });
        }
    }
}
