using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcDeviceCatalog;
using SIPSorcery.GB28181.Servers;
//using SIPSorcery.GB28181.Servers.SIPMessage;

namespace GrpcAgent.WebsocketRpcServer
{
    public class DeviceCatalogImpl : DeviceCatalog.DeviceCatalogBase
    {
        private ISIPCatalogCore _sipCatalogCore = null;

        public DeviceCatalogImpl(ISIPCatalogCore sipCatalogCore)
        {
            _sipCatalogCore = sipCatalogCore;
        }

        // Server side handler of the SayHello RPC
        public override Task<GetCatalogReply> GetCatalog(GetCatalogRequest request, ServerCallContext context)
        {
            _sipCatalogCore.GetCatalog(request.Deviceid);
            string x = "Status: 200 OK";
            return Task.FromResult(new GetCatalogReply { Message = x });
        }
    }
}
