using SIPSorcery.GB28181.Sys.XML;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SIPSorcery.GB28181.Servers
{
    public interface ISIPServiceDirector
    {
        Dictionary<string, Catalog> Catalogs { get; }
        Queue<NotifyCatalog.Item> NotifyCatalogItem { get; }
        ISIPMonitorCore GetTargetMonitorService(string gbid);
        Dictionary<string, DeviceStatus> DeviceStatuses { get; }
        Dictionary<string, RecordInfo> RecordInfoes { get; }

        //ip/port/protocol/ 
        Task<Tuple<string, int, ProtocolType>> MakeVideoRequest(string gbid, int[] mediaPort, string receiveIP);

        //Stop 
        Task<Tuple<string, int, ProtocolType>> Stop(string gbid);

        /// <summary>
        /// Device Catalog Query
        /// </summary>
        /// <param name="deviceId"></param>
        void DeviceCatalogQuery(string deviceId);
        /// <summary>
        /// Device Catalog Subscribe
        /// </summary>
        /// <param name="deviceId"></param>
        void DeviceCatalogSubscribe(string deviceId);
        /// <summary>
        /// PTZ Control
        /// </summary>
        /// <param name="ptzCommand"></param>
        /// <param name="speed"></param>
        /// <param name="deviceid"></param>
        void PtzControl(SIPMonitor.PTZCommand ptzCommand, int speed, string deviceid);
        void DeviceStateQuery(string deviceid);
        int RecordFileQuery(string deviceId, DateTime startTime, DateTime endTime, string type);
    }
}
