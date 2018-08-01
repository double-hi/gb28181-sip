using System;
using System.Collections.Generic;
using System.Text;

namespace SIPSorcery.GB28181.Sys
{
    public class EnvironmentVariables
    {
        private const string GB_SERVICE_LOCAL_IP = "GB_SERVICE_LOCAL_IP";//10.78.115.149
        private static string _GB_SERVICE_LOCAL_IP;
        private const string GB_SERVER_CHANNEL_ADDRESS = "GB_SERVER_CHANNEL_ADDRESS";//10.77.38.86:5000
        private static string _GB_SERVER_CHANNEL_ADDRESS;
        public static string GbServiceLocalIp
        {
            get { return _GB_SERVICE_LOCAL_IP ?? Environment.GetEnvironmentVariable(GB_SERVICE_LOCAL_IP); }
            set { _GB_SERVICE_LOCAL_IP = value; }
        }        
        public static string GBServerChannelAddress
        {
            get { return _GB_SERVER_CHANNEL_ADDRESS ?? Environment.GetEnvironmentVariable(GB_SERVER_CHANNEL_ADDRESS); }
            set { _GB_SERVER_CHANNEL_ADDRESS = value; }
        }
        public static int GBServerGrpcPort
        {
            get { return 50051; }
        }
        public static int GbServiceLocalPort
        {
            get { return 5061; }
        }
    }
}

