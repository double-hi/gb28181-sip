using System;
using System.Collections.Generic;
using System.Text;

namespace SIPSorcery.GB28181.Sys
{
    public class EnvironmentVariables
    {
        private const string MICRO_REGISTRY_ADDRESS = "MICRO_REGISTRY_ADDRESS";
        private static string _localip;
        public static string LocalIp
        {
            get { return _localip ?? Environment.GetEnvironmentVariable(MICRO_REGISTRY_ADDRESS); }
            set { _localip = value; }
        }
    }
}

