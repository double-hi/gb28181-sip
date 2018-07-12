using System;
using System.Collections.Generic;
using System.Text;
using SIPSorcery.GB28181.Servers.SIPMessage;
using SIPSorcery.GB28181.Sys.XML;
using System.Linq;
using System.Threading.Tasks;

namespace SIPSorcery.GB28181.Servers.SIPCatalog
{
    public class SIPCatalogCore : ISIPCatalogCore
    {
        private static SIPCatalogCore _instance;
        private Dictionary<string, string> _devList;
        public SIPMessageCoreService MessageCore;
        private ISipMessageCore _sipMsgCoreService;

        public Action<Catalog> OnCatalogReceived;

        public Action<NotifyCatalog> OnNotifyCatalogReceived;

        public SIPCatalogCore()
        {
            _devList = new Dictionary<string, string>();
        }
        public SIPCatalogCore(ISipMessageCore sipMessageCore)
        {
            _sipMsgCoreService = sipMessageCore;
        }

        /// <summary>
        /// 设备列表
        /// </summary>
        public Dictionary<string, string> DevList
        {
            get { return _devList; }
        }

        /// <summary>
        /// 以单例模式访问
        /// </summary>
        public static SIPCatalogCore Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new SIPCatalogCore();
                }
                return _instance;
            }
        }

        /// <summary>
        /// 获取设备目录
        /// </summary>
        public void GetCatalog()
        {
            _sipMsgCoreService.DeviceCatalogQuery();
        }
        /// <summary>
        /// 获取设备目录
        /// </summary>
        public void GetCatalog(string deviceId)
        {
            _sipMsgCoreService.DeviceCatalogQuery(deviceId);
        }
    }
}
