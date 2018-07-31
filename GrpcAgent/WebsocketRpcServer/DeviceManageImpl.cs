using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Logger4Net;
using Manage;
using SIPSorcery.GB28181.Servers;
using SIPSorcery.GB28181.SIP;
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

        private void _sipRegistrarCore_RPCDmsRegisterReceived(SIPTransaction sipTransaction)
        {
            logger.Debug("Device[" + sipTransaction.TransactionRequest.RemoteSIPEndPoint + "] is registering DMS.");

            Device _device = new Device();
            //string guid = 1;  //device的guid 主键
            //string name = 2;
            //TypeCode type_code = 3;
            //ProtoType protocol_type = 4;  //用以区别设备接入时采用的协议类型,必须存储
            //string description = 5;
            //double geo_long = 6; //
            //double geo_lat = 7;
            //uint32 ptz_type = 8;         //PTZ类型,      
            //ProtocolInfo protocol_detail = 9; //协议相关的细节
            //MediaInfo media_detail = 10;
            //repeated LoginUser login_user = 11;  //登陆设备的用户信息
            //repeated string tag = 12;   //对设备做的标注，比如打个标签，用于检索和参数携带，允许多个标注
            //repeated uint32 media_srcType = 13;      //设备的提供数据类型（提供视频/音频/文本数据/二进制数据包，0 for nil ,1 for video 2 for audio 3 for binary
            //int32 channel_id = 14;
            SIPRequest sipRequest = sipTransaction.TransactionRequest;
            _device.Name = sipTransaction.TransactionRequestFrom.Name ?? string.Empty;
            _device.Guid = sipTransaction.TransactionRequestFrom.URI.User;//42010000001310000184
            _device.ProtocolType = ProtoType.ProtoGb28181;
            try
            {
                Channel channel = new Channel(EnvironmentVariables.GBServerChannelAddress ?? "10.78.115.149:5000", ChannelCredentials.Insecure);
                var client = new Manage.Manage.ManageClient(channel);
                AddDeviceRequest _AddDeviceRequest = new AddDeviceRequest();
                _AddDeviceRequest.Device.Add(_device);
                _AddDeviceRequest.LoginRoleId = "admin";
                var reply = client.AddDevice(_AddDeviceRequest);

                logger.Debug("Device[" + sipTransaction.TransactionRequest.RemoteSIPEndPoint + "] register DMS successed.");
            }
            catch (Exception ex)
            {
                logger.Error("Device[" + sipTransaction.TransactionRequest.RemoteSIPEndPoint + "] register DMS failed. " + ex.Message);
            }
        }
    }
}
