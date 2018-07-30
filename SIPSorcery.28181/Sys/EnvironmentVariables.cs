using System;
using System.Collections.Generic;
using System.Text;

namespace SIPSorcery.GB28181.Sys
{
    public class EnvironmentVariables
    {
        private const string MICRO_REGISTRY_ADDRESS = "MICRO_REGISTRY_ADDRESS";
        private static string _MICRO_REGISTRY_ADDRESS;
        private const string GB_SERVICE_LOCAL_IP = "GB_SERVICE_LOCAL_IP";//10.78.115.152
        private static string _GB_SERVICE_LOCAL_IP;
        private const string GB_CAMERA_REMOTE_IP = "GB_CAMERA_REMOTE_IP";//10.78.115.155
        private static string _GB_CAMERA_REMOTE_IP;
        private const string GB_CAMERA_REMOTE_PORT = "GB_CAMERA_REMOTE_PORT";//5060
        private static string _GB_CAMERA_REMOTE_PORT;
        private const string GB_SERVER_CHANNEL_ADDRESS = "GB_SERVER_CHANNEL_ADDRESS";//10.77.38.86:5000
        private static string _GB_SERVER_CHANNEL_ADDRESS;
        public static string MicroRegistryAddress
        {
            get { return _MICRO_REGISTRY_ADDRESS ?? Environment.GetEnvironmentVariable(MICRO_REGISTRY_ADDRESS); }
            set { _MICRO_REGISTRY_ADDRESS = value; }
        }
        public static string GbServiceLocalIp
        {
            get { return _GB_SERVICE_LOCAL_IP ?? Environment.GetEnvironmentVariable(GB_SERVICE_LOCAL_IP); }
            set { _GB_SERVICE_LOCAL_IP = value; }
        }
        
        public static string GbCameraRemoteIp
        {
            get { return _GB_CAMERA_REMOTE_IP ?? Environment.GetEnvironmentVariable(GB_CAMERA_REMOTE_IP); }
            set { _GB_CAMERA_REMOTE_IP = value; }
        }
        public static string GbCameraRemotePort
        {
            get { return _GB_CAMERA_REMOTE_PORT ?? Environment.GetEnvironmentVariable(GB_CAMERA_REMOTE_PORT); }
            set { _GB_CAMERA_REMOTE_PORT = value; }
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

