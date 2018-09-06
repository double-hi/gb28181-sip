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
                //{
                //    "_id": ObjectID("5b8f8b0aba6730933a2bdaf5"),
                //    "uuid": "005199cd-7d06-43dc-a9cc-a2d9cf42a118",
                //    "name": "testgbname",
                //    "users": [
                //        {
                //            "loginname": "admin",
                //            "loginpwd": "123456"
                //        }
                //    ],
                //    "tag": [],
                //    "ptztype": 0,
                //    "description": "",
                //    "protocoltype": 0,
                //    "ip": "",
                //    "port": 0,
                //    "gbid": "42010000001310000184",
                //    "gbparentid": "",
                //    "mediasrctype": [],
                //    "mediainfo": null,
                //    "longitude": 0,
                //    "latitude": 0,
                //    "parentid": "",
                //    "did": "",
                //    "cid": "",
                //    "pid": "",
                //    "sid": "",
                //    "shapetype": 2
                //}
                _device.Guid = Guid.NewGuid().ToString();
                _device.Name = "testgbname" + new Random().Next(100);
                _device.LoginUser.Add(new LoginUser() { LoginName = sIPAccount.SIPUsername ?? "admin", LoginPwd = sIPAccount.SIPPassword ?? "123456" });
                _device.PtzType = 0;
                _device.ProtocolType = 0;
                _device.IP = "10.77.37.217";
                _device.Port = 5060;
                _device.GBID = sipTransaction.TransactionRequestFrom.URI.User;//42010000001310000184
                _device.ShapeType = ShapeType.Dome;
                Channel channel = new Channel(EnvironmentVariables.GBServerChannelAddress ?? "10.78.115.182:8080", ChannelCredentials.Insecure);
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
