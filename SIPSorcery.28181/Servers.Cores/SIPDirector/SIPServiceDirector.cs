using Logger4Net;
using SIPSorcery.GB28181.Net;
using SIPSorcery.GB28181.Servers.SIPMessage;
using SIPSorcery.GB28181.Sys;
using SIPSorcery.GB28181.Sys.XML;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SIPSorcery.GB28181.Servers
{
    public class SIPServiceDirector : ISIPServiceDirector
    {
        private static ILog logger = AppState.logger;
        private ISipMessageCore _sipCoreMessageService;
        private Dictionary<string, Catalog> _Catalogs = new Dictionary<string, Catalog>();
        public Dictionary<string, Catalog> Catalogs => _Catalogs;

        public SIPServiceDirector(ISipMessageCore sipCoreMessageService)
        {
            _sipCoreMessageService = sipCoreMessageService;
            _sipCoreMessageService.OnCatalogReceived += _sipCoreMessageService_OnCatalogReceived;
        }
        
        public ISIPMonitorCore GetTargetMonitorService(string gbid)
        {
            if (_sipCoreMessageService == null)
            {
                throw new NullReferenceException("instance not exist!");
            }

            if (_sipCoreMessageService.NodeMonitorService.ContainsKey(gbid))
            {
                return _sipCoreMessageService.NodeMonitorService[gbid];
            }

            return null;

        }

        //make real Request
        async public Task<Tuple<string, int, ProtocolType>> MakeVideoRequest(string gbid, int[] mediaPort, string receiveIP)
        {
            logger.Debug("Make video request started.");
            var target = GetTargetMonitorService(gbid);

            if (target == null)
            {
                return null;
            }

            var taskResult = await Task.Factory.StartNew(() =>
           {

               var cSeq = target.RealVideoReq(mediaPort, receiveIP, true);

               var result = target.WaitRequestResult();

               return result;
           });

            var ipaddress = _sipCoreMessageService.GetReceiveIP(taskResult.Item2.Body);

            var port = _sipCoreMessageService.GetReceivePort(taskResult.Item2.Body, SDPMediaTypesEnum.video);

            return Tuple.Create(ipaddress, port, ProtocolType.Udp);
        }


        //stop real Request
        async public Task<Tuple<string, int, ProtocolType>> Stop(string gbid)
        {
            var target = GetTargetMonitorService(gbid);

            if (target == null)
            {
                return null;
            }

            //stop
            target.ByeVideoReq();
            return null;

            //var taskResult = await Task.Factory.StartNew(() =>
            //{
            //    //stop
            //    target.ByeVideoReq();

            //    var result = target.WaitRequestResult();

            //    return result;
            //});

            //var ipaddress = _sipCoreMessageService.GetReceiveIP(taskResult.Item2.Body);

            //var port = _sipCoreMessageService.GetReceivePort(taskResult.Item2.Body, SDPMediaTypesEnum.video);

            //return Tuple.Create(ipaddress, port, ProtocolType.Udp);

        }

        #region 设备目录
        private void _sipCoreMessageService_OnCatalogReceived(Catalog obj)
        {
            if (!Catalogs.ContainsKey(obj.DeviceID))
            {
                Catalogs.Add(obj.DeviceID, obj);
            }
        }
        /// <summary>
        /// 设备目录查询
        /// </summary>
        public void GetCatalog(string deviceId)
        {
            logger.Debug("Device Catalog Query started.");
            _sipCoreMessageService.DeviceCatalogQuery(deviceId);
        }
        #endregion
    }
}
