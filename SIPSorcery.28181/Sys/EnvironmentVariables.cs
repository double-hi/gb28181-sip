using System;
using System.Collections.Generic;
using System.Text;

namespace SIPSorcery.GB28181.Sys
{
    public class EnvironmentVariables
    {
        private const string MICRO_REGISTRY_ADDRESS = "MICRO_REGISTRY_ADDRESS";//10.78.115.124:8500
        private static string _MICRO_REGISTRY_ADDRESS;
        private const string GB_SERVICE_LOCAL_ID = "GB_SERVICE_LOCAL_ID";//42010000002100000002
        private static string _GB_SERVICE_LOCAL_ID;
        private const string GB_SERVICE_LOCAL_IP = "GB_SERVICE_LOCAL_IP";//10.78.115.149
        private static string _GB_SERVICE_LOCAL_IP;
        private const string GB_SERVER_CHANNEL_ADDRESS = "GB_SERVER_CHANNEL_ADDRESS";//localhost:8080
        private static string _GB_SERVER_CHANNEL_ADDRESS;
        private const string GB_NATS_CHANNEL_ADDRESS = "GB_NATS_CHANNEL_ADDRESS";//nats://10.78.115.149:4222
        private static string _GB_NATS_CHANNEL_ADDRESS;
        public static string MicroRegistryAddress
        {
            get { return _MICRO_REGISTRY_ADDRESS ?? Environment.GetEnvironmentVariable(MICRO_REGISTRY_ADDRESS); }
            set { _MICRO_REGISTRY_ADDRESS = value; }
        }
        public static string GbServiceLocalId
        {
            get { return _GB_SERVICE_LOCAL_ID ?? Environment.GetEnvironmentVariable(GB_SERVICE_LOCAL_ID); }
            set { _GB_SERVICE_LOCAL_ID = value; }
        }
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
        public static string GBNatsChannelAddress
        {
            get { return _GB_NATS_CHANNEL_ADDRESS ?? Environment.GetEnvironmentVariable(GB_NATS_CHANNEL_ADDRESS); }
            set { _GB_NATS_CHANNEL_ADDRESS = value; }
        }
        public static int GBServerGrpcPort
        {
            get { return 50051; }
        }
        public static int GbServiceLocalPort
        {
            get { return 5060; }
        }
    }
}

