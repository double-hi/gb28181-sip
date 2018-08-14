using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcProtocol;
using Newtonsoft.Json;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Servers.SIPMonitor;
using SIPSorcery.GB28181.Sys.XML;

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
            _sipServiceDirector.DeviceStateQuery(request.Deviceid);
            DeviceStatus _DeviceStatus = null;
            while (true)
            {
                foreach (DeviceStatus obj in _sipServiceDirector.DeviceStatuses.Values)
                {
                    if (request.Deviceid.Equals(obj.DeviceID))
                    {
                        _DeviceStatus = obj;
                    }
                }
                if (_DeviceStatus == null)
                {
                    System.Threading.Thread.Sleep(500);
                }
                else
                {
                    break;
                }
            }
            string json = JsonConvert.SerializeObject(_DeviceStatus)
                .Replace("\"SN\":null", "\"SN\":0")
                .Replace(":null", ":\"null\"");
            Instance instance = JsonConvert.DeserializeObject<Instance>(json);
            return Task.FromResult(new DeviceStateQueryReply { DeviceStatus = instance });
        }
    }
}
