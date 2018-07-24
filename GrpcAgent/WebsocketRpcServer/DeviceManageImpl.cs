using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Logger4Net;
using Manage;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.Servers.SIPMessage;
using SIPSorcery.GB28181.SIP;

namespace GrpcAgent.WebsocketRpcServer
{
    public class DeviceManageImpl : Manage.Manage.ManageBase
    {
        private static ILog logger = LogManager.GetLogger("RpcServer");
        private ISipMessageCore _sipMessageCore = null;
        private ISIPRegistrarCore _sipRegistrarCore = null;

        public DeviceManageImpl(ISipMessageCore sipMessageCore,ISIPRegistrarCore sipRegistrarCore)
        {
            _sipMessageCore = sipMessageCore;
            _sipRegistrarCore = sipRegistrarCore;
            _sipRegistrarCore.RPCDmsRegisterReceived += RPCDmsRegister;
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
        
        public void RPCDmsRegister(SIPTransaction sipTransaction)
        {
            logger.Debug("Device[" + sipTransaction.TransactionRequest.RemoteSIPEndPoint + "] is registering DMS.");

            //Device _device = new Device();
            ////DeviceList dl = new DeviceList();
            ////dl.Item.Add(_device);
            //try
            //{
            //    Channel channel = new Channel("10.78.115.152:5000", ChannelCredentials.Insecure);
            //    var client = new Manage.Manage.ManageClient(channel);
            //    AddDeviceRequest _AddDeviceRequest = new AddDeviceRequest();
            //    _device.Name = "Carmera01";
            //    _device.Guid = "42010000001310000184";
            //    _AddDeviceRequest.Device.Add(_device);
            //    _AddDeviceRequest.LoginRoleId = "admin";
            //    var reply = client.AddDevice(_AddDeviceRequest);

            //    logger.Debug("Device[" + sipTransaction.TransactionRequest.RemoteSIPEndPoint + "] register DMS successed.");
            //}
            //catch (Exception ex)
            //{
            //    logger.Error("Device[" + sipTransaction.TransactionRequest.RemoteSIPEndPoint + "] register DMS failed. " + ex.Message);
            //}
        }        
    }
}
