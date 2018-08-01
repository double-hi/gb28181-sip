using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcPtzControl;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Servers.SIPMonitor;

namespace GrpcAgent.WebsocketRpcServer
{
    public class PtzControlImpl : PtzControl.PtzControlBase
    {
        private ISIPServiceDirector _sipServiceDirector = null;

        public PtzControlImpl(ISIPServiceDirector sipServiceDirector)
        {
            _sipServiceDirector = sipServiceDirector;
        }

        public override Task<PtzDirectReply> PtzDirect(PtzDirectRequest request, ServerCallContext context)
        {
            if (request.Xyz.X == 0 && request.Xyz.Y == 4 && request.Xyz.Z == 0)
            {
                _sipServiceDirector.PtzControl(PTZCommand.Up, request.Speed * 50, request.Deviceid);
            }
            else if (request.Xyz.X == 0 && request.Xyz.Y == -4 && request.Xyz.Z == 0)
            {
                _sipServiceDirector.PtzControl(PTZCommand.Down, request.Speed * 50, request.Deviceid);
            }
            else if (request.Xyz.X == 4 && request.Xyz.Y == 0 && request.Xyz.Z == 0)
            {
                _sipServiceDirector.PtzControl(PTZCommand.Left, request.Speed * 50, request.Deviceid);
            }
            else if (request.Xyz.X == -4 && request.Xyz.Y == 0 && request.Xyz.Z == 0)
            {
                _sipServiceDirector.PtzControl(PTZCommand.Right, request.Speed * 50, request.Deviceid);
            }
            else if (request.Xyz.X == 0 && request.Xyz.Y == 0 && request.Xyz.Z == 4)
            {
                _sipServiceDirector.PtzControl(PTZCommand.Zoom1, request.Speed * 50, request.Deviceid);
            }
            else if (request.Xyz.X == 0 && request.Xyz.Y == 0 && request.Xyz.Z == -4)
            {
                _sipServiceDirector.PtzControl(PTZCommand.Zoom2, request.Speed * 50, request.Deviceid);
            }
            else
            {
                _sipServiceDirector.PtzControl(PTZCommand.Stop, request.Speed * 50, request.Deviceid);
            }
            string x = "Status: 200 OK";
            return Task.FromResult(new PtzDirectReply { Message = x });
        }
    }
}
