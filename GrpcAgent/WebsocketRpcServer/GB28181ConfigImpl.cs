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
using GrpcGb28181Config;
using Newtonsoft.Json;

namespace GrpcAgent.WebsocketRpcServer
{
    public class GB28181ConfigImpl : Gb28181Config.Gb28181ConfigBase
    {
        private static ILog logger = LogManager.GetLogger("RpcServer");
        private ISIPServiceDirector _sipServiceDirector = null;

        public GB28181ConfigImpl(ISIPServiceDirector sipServiceDirector)
        {
            _sipServiceDirector = sipServiceDirector;
        }

        //private List<SIPSorcery.GB28181.SIP.App.SIPAccount> _sipServiceDirector_RPCGBServerConfigReceived()
        //{
        //    try
        //    {
        //        //logger.Debug("Get GB Server Config started.");
        //        string GBConfigIP = "10.77.38.86:50050";
        //        Channel channel = new Channel(GBConfigIP, ChannelCredentials.Insecure);
        //        logger.Debug("Channel of GB Server Config IP is:" + GBConfigIP);
        //        var client = new Gb28181Config.Gb28181ConfigClient(channel);
        //        //GbConfigRequest _GbConfigRequest = new GbConfigRequest();
        //        GbConfigReply _GbConfigReply = new GbConfigReply();
        //        _GbConfigReply = client.GbConfig(new GbConfigRequest() { });
                
        //        List<SIPSorcery.GB28181.SIP.App.SIPAccount> _lstSIPAccount = new List<SIPSorcery.GB28181.SIP.App.SIPAccount>();
        //        foreach (SIPAccount item in _GbConfigReply.Sipaccount)
        //        {
        //            SIPSorcery.GB28181.SIP.App.SIPAccount obj = new SIPSorcery.GB28181.SIP.App.SIPAccount();
        //            obj.Id = Guid.NewGuid();
        //            //obj.Owner = item.Name;
        //            obj.GbVersion = item.GbVersion;
        //            obj.LocalID = item.LocalID;
        //            obj.LocalIP = System.Net.IPAddress.Parse(item.LocalIP);
        //            obj.LocalPort = Convert.ToUInt16(item.LocalPort);
        //            obj.RemotePort = Convert.ToUInt16(item.RemotePort);
        //            obj.Authentication = Boolean.Parse(item.Authentication);
        //            obj.SIPUsername = item.SIPUsername;
        //            obj.SIPPassword = item.SIPPassword;
        //            obj.MsgProtocol = System.Net.Sockets.ProtocolType.Udp;
        //            obj.StreamProtocol = System.Net.Sockets.ProtocolType.Udp;
        //            obj.TcpMode = SIPSorcery.GB28181.Net.RTP.TcpConnectMode.passive;
        //            obj.MsgEncode = item.MsgEncode;
        //            obj.PacketOutOrder = Boolean.Parse(item.PacketOutOrder);
        //            obj.KeepaliveInterval = Convert.ToUInt16(item.KeepaliveInterval);
        //            obj.KeepaliveNumber = Convert.ToByte(item.KeepaliveNumber);
        //            _lstSIPAccount.Add(obj);
        //        }
        //        return _lstSIPAccount;

        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Get GB Server Config failed. " + ex.Message);
        //        return null;
        //    }
        //}

        //public List<SIPAccount> GetGBConfig()
        //{
        //    try
        //    {
        //        logger.Debug("Get GB Server Config started.");
        //        string GBConfigIP = "10.77.38.86:50051";
        //        Channel channel = new Channel(GBConfigIP, ChannelCredentials.Insecure);
        //        logger.Debug("Channel of GB Config IP is:" + GBConfigIP);
        //        var client = new Gb28181Config.Gb28181ConfigClient(channel);
        //        GbConfigRequest _GbConfigRequest = new GbConfigRequest();
        //        GbConfigReply _GbConfigReply = new GbConfigReply();
        //        _GbConfigReply = client.GbConfig(_GbConfigRequest);

        //        List<SIPAccount> lstSIPAccount = new List<SIPAccount>();
        //        foreach (SIPAccount item in _GbConfigReply.Sipaccount)
        //        {
        //            lstSIPAccount.Add(item);
        //        }
        //        return lstSIPAccount;
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Get GB Server Config failed. " + ex.Message);
        //        return null;
        //    }
        //}        
    }
}
