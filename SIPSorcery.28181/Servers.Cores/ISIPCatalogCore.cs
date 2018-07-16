using System;
using System.Collections.Generic;
using System.Text;
using SIPSorcery.GB28181.Servers.SIPMessage;
using SIPSorcery.GB28181.Sys.XML;
using System.Linq;
using System.Threading.Tasks;

namespace SIPSorcery.GB28181.Servers
{
    public interface ISIPCatalogCore
    {
        /// <summary>
        /// 设备列表
        /// </summary>
        Dictionary<string, string> DevList { get; }

        /// <summary>
        /// 获取设备目录
        /// </summary>
        void GetCatalog();
        /// <summary>
        /// 获取设备目录
        /// </summary>
        void GetCatalog(string deviceId);
        /// <summary>
        /// 目录订阅
        /// </summary>
        void DeviceCatalogSubscribe(string deviceId);

    }
}
