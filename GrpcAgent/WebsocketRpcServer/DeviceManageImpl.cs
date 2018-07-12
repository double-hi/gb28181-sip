using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Manage;
//using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Servers.SIPMessage;

namespace GrpcAgent.WebsocketRpcServer
{
    public class DeviceManageImpl : Manage.Manage.ManageBase
    {
        //private ISIPMonitorCore _sipMonitorCore = null;
        private ISipMessageCore _sipMessageCore = null;

        public DeviceManageImpl(ISipMessageCore sipMessageCore)
        {
            _sipMessageCore = sipMessageCore;
        }

        //// Server side handler of the AddDevice RPC
        //public override Task<AddDeviceResponse> AddDevice(AddDeviceRequest request, ServerCallContext context)
        //{
        //    try
        //    {
        //        //
        //        string x;
        //    }
        //    catch
        //    {
        //        return Task.FromResult(new AddDeviceResponse { Status = OP_RESULT_STATUS.OpException });
        //    }
        //    return Task.FromResult(new AddDeviceResponse { Status = OP_RESULT_STATUS.OpSuccess });
        //}

        //// Server side handler of the DeleteDevice RPC
        //public override Task<DeleteDeviceResponse> DeleteDevice(DeleteDeviceRequest request, ServerCallContext context)
        //{
        //    try
        //    {
        //        //
        //    }
        //    catch
        //    {
        //        return Task.FromResult(new DeleteDeviceResponse { Status = Manage.OP_RESULT_STATUS.OpException });
        //    }
        //    return Task.FromResult(new DeleteDeviceResponse { Status = Manage.OP_RESULT_STATUS.OpSuccess });
        //}
    }
}
