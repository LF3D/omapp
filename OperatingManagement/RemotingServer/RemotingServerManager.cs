using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Lifetime;
using System.Configuration;

using OperatingManagement.RemotingServer.Configuration;

namespace OperatingManagement.RemotingServer
{
    /// <summary>
    /// Contains methods to manage the remoting operations.
    /// </summary>
    public class RemotingServerManager
    {
        /// <summary>
        /// (Privacy) Create a new instance of <see cref="RemotingServerManager"/> class.
        /// </summary>
        private RemotingServerManager()
        {
            _config = (RemotingServerSection)ConfigurationManager.GetSection("remotingServices");
        }

        #region -Properties-
        private static RemotingServerManager _instance = null;
        private static object _locker = new object();

        private RemotingServerSection _config;

        private TcpServerChannel _tcpChannel;
        
        /// <summary>
        /// Gets the instance of <see cref="RemotingServerManager"/> class.
        /// </summary>
        public static RemotingServerManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_locker)
                    {
                        if (_instance == null)
                        {
                            _instance = new RemotingServerManager();
                        }
                    }
                }
                return _instance;
            }
        }
        #endregion

        /// <summary>
        /// Starts the remoting service.
        /// </summary>
        public void Start()
        {
            if (!_config.IsAuto)
            {
                return;
            }

            _tcpChannel = new TcpServerChannel("TcpRemotingServices", _config.Port);
            ChannelServices.RegisterChannel(_tcpChannel, false);
            foreach (RemotingServerElement service in _config.Services)
            {
                RemotingConfiguration.RegisterWellKnownServiceType(Type.GetType(service.Type),
                                                                   service.ObjectUri,
                                                                   (WellKnownObjectMode)Enum.Parse(typeof(WellKnownObjectMode), service.Mode));
            }

            LifetimeServices.LeaseTime = new TimeSpan(0, 30, 0);
            LifetimeServices.RenewOnCallTime = new TimeSpan(0, 20, 0);
        }

        /// <summary>
        /// Stops the remoting service.
        /// </summary>
        public void Stop()
        {
            if (!_config.IsAuto)
            {
                return;
            }
            if (_tcpChannel != null)
            {
                _tcpChannel.StopListening(null);
                ChannelServices.UnregisterChannel(_tcpChannel);
                _tcpChannel = null;
            }
        }
    }
}