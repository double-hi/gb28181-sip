using SIPSorcery.GB28181.SIP.App;
using System.Collections.Generic;
using System.Net;

namespace SIPSorcery.GB28181.SIP
{
    public delegate void RPCDmsRegisterDelegate(SIPTransaction sipTransaction);
    public delegate List<SIPAccount> RPCGBServerConfigDelegate();
}
