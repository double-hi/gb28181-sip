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
        ISIPMonitorCore GetTargetMonitorService(string gbid);
        
        //ip/port/protocol/ 
        Task<Tuple<string, int, ProtocolType>> MakeVideoRequest(string gbid, int[] mediaPort, string receiveIP);

        //Stop 
        Task<Tuple<string, int, ProtocolType>> Stop(string gbid);

        /// <summary>
        /// 设备目录查询
        /// </summary>
        void GetCatalog(string deviceId);
    }
}
