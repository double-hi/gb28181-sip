using Logger4Net;
using SIPSorcery.GB28181.Servers.SIPMessage;
using SIPSorcery.GB28181.Sys;
using SIPSorcery.GB28181.Sys.Config;
using SIPSorcery.GB28181.Sys.XML;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using GrpcAgent;
using GrpcAgent.WebsocketRpcServer;
using MediaContract;
using SIPSorcery.GB28181.Servers;
using System.IO;
using Microsoft.Extensions.Configuration;
using SIPSorcery.GB28181.SIP;
using SIPSorcery.GB28181.Servers.SIPMonitor;
using SIPSorcery.GB28181.Sys.Cache;
using SIPSorcery.GB28181.Sys.Model;
using GrpcPtzControl;
using GrpcDeviceCatalog;
using Grpc.Core;
using GrpcGb28181Config;

namespace GB28181Service
{
    public class MainProcess : IDisposable
    {
        private static ILog logger = AppState.GetLogger("Startup");
        //interface IDisposable implementation
        private bool _already_disposed = false;

        // Thread signal for stop work.
        private readonly ManualResetEvent _eventStopService = new ManualResetEvent(false);

        // Thread signal for infor thread is over.
        private readonly ManualResetEvent _eventThreadExit = new ManualResetEvent(false);

        //signal to exit the main Process
        private readonly AutoResetEvent _eventExitMainProcess = new AutoResetEvent(false);

        private Task _mainTask = null;
        private Task _sipTask = null;
        private Task _registerTask = null;

        private Task _mainWebSocketRpcTask = null;

        private DateTime _keepaliveTime;
        private Queue<HeartBeatEndPoint> _keepAliveQueue = new Queue<HeartBeatEndPoint>();

        private Queue<Catalog> _catalogQueue = new Queue<Catalog>();

        private readonly ServiceCollection servicesContainer = new ServiceCollection();

        private ServiceProvider _serviceProvider = null;
        public MainProcess()
        {

        }

        #region IDisposable interface

        public void Dispose()
        {
            // tell the GC that the Finalize process no longer needs to be run for this object.
            GC.SuppressFinalize(this);
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (_already_disposed)
                    return;

                if (disposing)
                {
                    _eventStopService.Close();
                    _eventThreadExit.Close();
                }
            }

            _already_disposed = true;
        }

        #endregion

        public void Run()
        {
            _eventStopService.Reset();
            _eventThreadExit.Reset();

            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

            // config info for .net core https://www.cnblogs.com/Leo_wl/p/5745772.html#_label3
            var builder = new ConfigurationBuilder();
                //.SetBasePath(Directory.GetCurrentDirectory())
                //.AddXmlFile("Config/gb28181.xml", false, reloadOnChange: true);
            var config = builder.Build();//Console.WriteLine(config["sipaccount:ID"]);
            //var sect = config.GetSection("sipaccounts");

            //InitServer
            SipAccountStorage.RPCGBServerConfigReceived += SipAccountStorage_RPCGBServerConfigReceived;

            //Config Service & and run
            ConfigServices(config);

            //Start the main serice
            _mainTask = Task.Factory.StartNew(() => MainServiceProcessing());

            //wait the process exit of main
            _eventExitMainProcess.WaitOne();
        }
        
        public void Stop()
        {
            // signal main service exit
            _eventStopService.Set();
            _eventThreadExit.WaitOne();
        }

        private void ConfigServices(IConfigurationRoot configuration)
        {
            //we should initialize resource here then use them.
            servicesContainer.AddSingleton(configuration)  // add configuration 
                            .AddSingleton<ILog, Logger>()
                            .AddSingleton<ISipAccountStorage, SipAccountStorage>()
                            .AddSingleton<MediaEventSource>()
                            .AddScoped<VideoSession.VideoSessionBase, SSMediaSessionImpl>()
                            .AddScoped<ISIPServiceDirector, SIPServiceDirector>()
                            .AddSingleton<IRpcService, RpcServer>()
                            .AddTransient<ISIPTransactionEngine, SIPTransactionEngine>()
                            .AddTransient<ISIPMonitorCore, SIPMonitorCoreService>()
                            .AddScoped<PtzControl.PtzControlBase, PtzControlImpl>()
                            .AddScoped<DeviceCatalog.DeviceCatalogBase, DeviceCatalogImpl>()
                            .AddScoped<Manage.Manage.ManageBase, DeviceManageImpl>()
                            .AddSingleton<ISIPTransport, SIPTransport>()
                            .AddSingleton<ISIPRegistrarCore, SIPRegistrarCoreService>()
                            .AddSingleton<ISipMessageCore, SIPMessageCoreService>()
                            .AddSingleton<MessageCenter>()
                            .AddSingleton<IMemoCache<Camera>, DeviceObjectCache>()
                            .AddSingleton<IServiceCollection>(servicesContainer); // add itself 
            _serviceProvider = servicesContainer.BuildServiceProvider();
        }
        
        private void MainServiceProcessing()
        {
            _keepaliveTime = DateTime.Now;
            try
            {
                var _mainSipService = _serviceProvider.GetRequiredService<ISipMessageCore>();
                //Get meassage Handler
                var messageHandler = _serviceProvider.GetRequiredService<MessageCenter>();                
                // start the Listening SipService in main Service
                _sipTask = Task.Factory.StartNew(() =>
                {
                    _mainSipService.OnKeepaliveReceived += messageHandler.OnKeepaliveReceived;
                    _mainSipService.OnServiceChanged += messageHandler.OnServiceChanged;
                    _mainSipService.OnCatalogReceived += messageHandler.OnCatalogReceived;
                    _mainSipService.OnNotifyCatalogReceived += messageHandler.OnNotifyCatalogReceived;
                    _mainSipService.OnAlarmReceived += messageHandler.OnAlarmReceived;
                    _mainSipService.OnRecordInfoReceived += messageHandler.OnRecordInfoReceived;
                    _mainSipService.OnDeviceStatusReceived += messageHandler.OnDeviceStatusReceived;
                    _mainSipService.OnDeviceInfoReceived += messageHandler.OnDeviceInfoReceived;
                    _mainSipService.OnMediaStatusReceived += messageHandler.OnMediaStatusReceived;
                    _mainSipService.OnPresetQueryReceived += messageHandler.OnPresetQueryReceived;
                    _mainSipService.OnDeviceConfigDownloadReceived += messageHandler.OnDeviceConfigDownloadReceived;
                    _mainSipService.Start();

                });

                // run the register service
                var _registrarCore = _serviceProvider.GetRequiredService<ISIPRegistrarCore>();
                _registerTask = Task.Factory.StartNew(() =>
                {
                    _registrarCore.ProcessRegisterRequest();
                });

                //Run the Rpc Server End
                var _mainWebSocketRpcServer = _serviceProvider.GetRequiredService<IRpcService>();
                _mainWebSocketRpcTask = Task.Factory.StartNew(() =>
                {
                    _mainWebSocketRpcServer.AddIPAdress("0.0.0.0");
                    _mainWebSocketRpcServer.AddPort(EnvironmentVariables.GBServerGrpcPort);//50051
                    _mainWebSocketRpcServer.Run();
                });

                ////test code will be removed
                //var abc = WaitUserCmd();
                //abc.Wait();

                //wait main service exit
                _eventStopService.WaitOne();

                //signal main process exit
                _eventExitMainProcess.Set();
            }
            catch (Exception exMsg)
            {
                logger.Error(exMsg.Message);
                _eventExitMainProcess.Set();
            }
            finally
            {

            }
        }

        private List<SIPSorcery.GB28181.SIP.App.SIPAccount> SipAccountStorage_RPCGBServerConfigReceived()
        {
            try
            {
                string GBServerChannelAddress = EnvironmentVariables.GBServerChannelAddress ?? "10.78.115.149:5000";
                Channel channel = new Channel(GBServerChannelAddress, ChannelCredentials.Insecure);
                var client = new Gb28181Config.Gb28181ConfigClient(channel);
                //GbConfigRequest _GbConfigRequest = new GbConfigRequest();
                GbConfigReply _GbConfigReply = new GbConfigReply();
                _GbConfigReply = client.GbConfig(new GbConfigRequest() { });

                List<SIPSorcery.GB28181.SIP.App.SIPAccount> _lstSIPAccount = new List<SIPSorcery.GB28181.SIP.App.SIPAccount>();
                foreach (SIPAccount item in _GbConfigReply.Sipaccount)
                {
                    SIPSorcery.GB28181.SIP.App.SIPAccount obj = new SIPSorcery.GB28181.SIP.App.SIPAccount();
                    obj.Id = Guid.NewGuid();
                    //obj.Owner = item.Name;
                    obj.GbVersion = string.IsNullOrEmpty(item.GbVersion) ? obj.GbVersion : item.GbVersion;
                    obj.LocalID = string.IsNullOrEmpty(item.LocalID) ? obj.LocalID : item.LocalID;
                    obj.LocalIP = string.IsNullOrEmpty(item.LocalIP) ? obj.LocalIP : System.Net.IPAddress.Parse(item.LocalIP);
                    obj.LocalPort = string.IsNullOrEmpty(item.LocalPort) ? obj.LocalPort : Convert.ToUInt16(item.LocalPort);
                    obj.RemotePort = string.IsNullOrEmpty(item.RemotePort) ? obj.RemotePort : Convert.ToUInt16(item.RemotePort);
                    obj.Authentication = string.IsNullOrEmpty(item.Authentication) ? obj.Authentication : Boolean.Parse(item.Authentication);
                    obj.SIPUsername = string.IsNullOrEmpty(item.SIPUsername) ? obj.SIPUsername : item.SIPUsername;
                    obj.SIPPassword = string.IsNullOrEmpty(item.SIPPassword) ? obj.SIPPassword : item.SIPPassword;
                    obj.MsgProtocol = string.IsNullOrEmpty(item.MsgProtocol) ? obj.MsgProtocol : System.Net.Sockets.ProtocolType.Udp;
                    obj.StreamProtocol = string.IsNullOrEmpty(item.StreamProtocol) ? obj.StreamProtocol : System.Net.Sockets.ProtocolType.Udp;
                    obj.TcpMode = string.IsNullOrEmpty(item.TcpMode) ? obj.TcpMode : SIPSorcery.GB28181.Net.RTP.TcpConnectMode.passive;
                    obj.MsgEncode = string.IsNullOrEmpty(item.MsgEncode) ? obj.MsgEncode : item.MsgEncode;
                    obj.PacketOutOrder = string.IsNullOrEmpty(item.PacketOutOrder) ? obj.PacketOutOrder : Boolean.Parse(item.PacketOutOrder);
                    obj.KeepaliveInterval = string.IsNullOrEmpty(item.KeepaliveInterval) ? obj.KeepaliveInterval : Convert.ToUInt16(item.KeepaliveInterval);
                    obj.KeepaliveNumber = string.IsNullOrEmpty(item.KeepaliveNumber) ? obj.KeepaliveNumber : Convert.ToByte(item.KeepaliveNumber);
                    _lstSIPAccount.Add(obj);
                }
                return _lstSIPAccount;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //private async Task WaitUserCmd()
        //{
        //    await Task.Run(() =>
        //     {
        //         while (true)
        //         {
        //             Console.WriteLine("\ninput command : I -Invite, E -Exit");
        //             var inputkey = Console.ReadKey();
        //             switch (inputkey.Key)
        //             {
        //                 case ConsoleKey.I:
        //                     {
        //                         var mockCaller = _serviceProvider.GetService<ISIPServiceDirector>();
        //                         mockCaller.MakeVideoRequest("42010000001310000184", new int[] { 5060 }, EnvironmentVariables.LocalIp);
        //                     }
        //                     break;
        //                 case ConsoleKey.E:
        //                     Console.WriteLine("\nexit Process!");
        //                     break;
        //                 default:
        //                     break;
        //             }
        //             if (inputkey.Key == ConsoleKey.E)
        //             {
        //                 return 0;
        //             }
        //             else
        //             {
        //                 continue;
        //             }
        //         }
        //     });
        //}

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject != null)
            {
                if (e.ExceptionObject is Exception exceptionObj)
                {
                    throw exceptionObj;
                }
            }
        }
    }
}
