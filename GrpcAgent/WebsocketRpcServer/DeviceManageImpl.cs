using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Logger4Net;
using Manage;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.SIP;
using SIPSorcery.GB28181.SIP.App;
using SIPSorcery.GB28181.Sys;

namespace GrpcAgent.WebsocketRpcServer
{
    public class DeviceManageImpl : Manage.Manage.ManageBase
    {
        private static ILog logger = LogManager.GetLogger("RpcServer");
        private ISIPRegistrarCore _sipRegistrarCore = null;

        public DeviceManageImpl(ISIPRegistrarCore sipRegistrarCore)
        {
            _sipRegistrarCore = sipRegistrarCore;
            _sipRegistrarCore.RPCDmsRegisterReceived += _sipRegistrarCore_RPCDmsRegisterReceived;
        }

        private void _sipRegistrarCore_RPCDmsRegisterReceived(SIPTransaction sipTransaction, SIPSorcery.GB28181.SIP.App.SIPAccount sIPAccount)
        {
            try
            {
                Device _device = new Device();
                SIPRequest sipRequest = sipTransaction.TransactionRequest;
                _device.Name = sIPAccount.Owner ?? string.Empty;
                _device.Guid = sipTransaction.TransactionRequestFrom.URI.User;//42010000001310000184
                _device.ProtocolType = ProtoType.ProtoGb28181;
                _device.LoginUser.Add(new LoginUser() { LoginName = sIPAccount.SIPUsername ?? string.Empty, LoginPwd = sIPAccount.SIPPassword ?? string.Empty });

                Channel channel = new Channel(EnvironmentVariables.GBServerChannelAddress ?? "10.78.115.182:5000", ChannelCredentials.Insecure);
                var client = new Manage.Manage.ManageClient(channel);
                AddDeviceRequest _AddDeviceRequest = new AddDeviceRequest();
                _AddDeviceRequest.Device.Add(_device);
                _AddDeviceRequest.LoginRoleId = "admin";
                var reply = client.AddDevice(_AddDeviceRequest);

                logger.Debug("Device[" + sipTransaction.TransactionRequest.RemoteSIPEndPoint + "] have completed registering DMS service.");
            }
            catch (Exception ex)
            {
                logger.Error("Device[" + sipTransaction.TransactionRequest.RemoteSIPEndPoint + "] register DMS failed: " + ex.Message);
            }
        }
    }
}
