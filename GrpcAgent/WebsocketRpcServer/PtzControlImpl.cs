using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcPtzControl;
//using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Servers.SIPMessage;

namespace GrpcAgent.WebsocketRpcServer
{
    public class PtzControlImpl : PtzControl.PtzControlBase
    {
        //private ISIPMonitorCore _sipMonitorCore = null;
        private ISipMessageCore _sipMessageCore = null;

        public PtzControlImpl(ISipMessageCore sipMessageCore)
        {
            _sipMessageCore = sipMessageCore;
        }

        // Server side handler of the SayHello RPC
        public override Task<PtzDirectReply> PtzDirect(PtzDirectRequest request, ServerCallContext context)
        {
            if (request.Xyz.X == 0 && request.Xyz.Y == 4 && request.Xyz.Z == 0)
            {
                _sipMessageCore.PtzControl(SIPSorcery.GB28181.Servers.SIPMonitor.PTZCommand.Up, request.Speed * 50);
            }
            else if (request.Xyz.X == 0 && request.Xyz.Y == -4 && request.Xyz.Z == 0)
            {
                _sipMessageCore.PtzControl(SIPSorcery.GB28181.Servers.SIPMonitor.PTZCommand.Down, request.Speed * 50);
            }
            else if (request.Xyz.X == 4 && request.Xyz.Y == 0 && request.Xyz.Z == 0)
            {
                _sipMessageCore.PtzControl(SIPSorcery.GB28181.Servers.SIPMonitor.PTZCommand.Left, request.Speed * 50);
            }
            else if (request.Xyz.X == -4 && request.Xyz.Y == 0 && request.Xyz.Z == 0)
            {
                _sipMessageCore.PtzControl(SIPSorcery.GB28181.Servers.SIPMonitor.PTZCommand.Right, request.Speed * 50);
            }
            else if (request.Xyz.X == 0 && request.Xyz.Y == 0 && request.Xyz.Z == 4)
            {
                _sipMessageCore.PtzControl(SIPSorcery.GB28181.Servers.SIPMonitor.PTZCommand.Zoom1, request.Speed * 50);
            }
            else if (request.Xyz.X == 0 && request.Xyz.Y == 0 && request.Xyz.Z == -4)
            {
                _sipMessageCore.PtzControl(SIPSorcery.GB28181.Servers.SIPMonitor.PTZCommand.Zoom2, request.Speed * 50);
            }
            else
            {
                _sipMessageCore.PtzControl(SIPSorcery.GB28181.Servers.SIPMonitor.PTZCommand.Stop, request.Speed * 50);
            }
            string x = "Status: 200 OK";
            return Task.FromResult(new PtzDirectReply { Message = x });
        }
    }
}
