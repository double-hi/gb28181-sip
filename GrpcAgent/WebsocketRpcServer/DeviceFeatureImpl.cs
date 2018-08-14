using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcProtocol;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Servers.SIPMonitor;

namespace GrpcAgent.WebsocketRpcServer
{
    public class DeviceFeatureImpl : DeviceFeature.DeviceFeatureBase
    {
        private ISIPServiceDirector _sipServiceDirector = null;

        public DeviceFeatureImpl(ISIPServiceDirector sipServiceDirector)
        {
            _sipServiceDirector = sipServiceDirector;
        }

        public override Task<DeviceStateQueryReply> DeviceStateQuery(DeviceStateQueryRequest request, ServerCallContext context)
        {
            string msg = "OK";
            try
            {
                _sipServiceDirector.DeviceStateQuery(request.Deviceid);               
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            return Task.FromResult(new DeviceStateQueryReply { Message = msg });
        }
    }
}
