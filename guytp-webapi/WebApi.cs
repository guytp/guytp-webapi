using System;
using System.Threading.Tasks;
using Guytp.Logging;
using System.Reflection;
using System.IO;
using System.Net.Http;
using Newtonsoft.Json.Converters;
using System.Runtime.Loader;
using System.Threading;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
#if DEBUG
#endif

namespace Guytp.WebApi
{
    /// <summary>
    /// This class provides the entry point to the service logic for the web API and allows the service to be started or stopped.
    /// </summary>
    public class WebApi
    {
        #region Declarations
        /// <summary>
        /// Defines the logger to use.
        /// </summary>
        private static readonly Logger Logger = Logger.ApplicationInstance;

        /// <summary>
        /// Defines whether we've received an OS termination signal.
        /// </summary>
        private bool _termSignalReceived;

        private IWebHost _webApp;
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of this class.
        /// </summary>
        public WebApi()
        {
            AssemblyLoadContext.Default.Unloading += OnTerminationSignal;
            Console.CancelKeyPress += OnCancelPress;
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Handle a termination signal from the OS.
        /// </summary>
        /// <param name="obj">
        /// The current context.
        /// </param>
        private void OnTerminationSignal(AssemblyLoadContext obj)
        {
            _termSignalReceived = true;
            Stop();
        }

        /// <summary>
        /// Handle the user closing the service with CTRL+C or equivalent.
        /// </summary>
        /// <param name="sender">
        /// The event sender.
        /// </param>
        /// <param name="e">
        /// The event arguments.
        /// </param>
        private void OnCancelPress(object sender, ConsoleCancelEventArgs e)
        {
            _termSignalReceived = true;
            Stop();
        }
        #endregion

        #region Bootstrapping
        /// <summary>
        /// Bootstraps the service.  If this is running as a console application it is loaded and waits for the user to press "Q" to terminate, otherwise it runs as a service
        /// and immediately returns.
        /// </summary>
        public void Bootstrap()
        {
            Start();
            while (!_termSignalReceived)
                Thread.Sleep(100);
            Stop();
        }
        #endregion

        #region Lifecycle
        /// <summary>
        /// Starts the service.
        /// </summary>
        /// <param name="args">
        /// The optional command line arguments that have been supplied to the service.
        /// </param>
        public void Start()
        {
            if (_termSignalReceived)
                return;
            Logger.Debug("Starting Web API");
            _webApp = WebHost.CreateDefaultBuilder().UseUrls(AppConfig.ApplicationInstance.Api.BindingUri).UseStartup<Startup>().Build();
            _webApp.Start();
            Logger.Info("Successfully started web API");
        }

        /// <summary>
        /// Stops the service.
        /// </summary>
        public void Stop()
        {
            Logger.Debug("Stopping Web API");
            _webApp?.StopAsync();
            _webApp?.Dispose();
            _webApp = null;
            Logger.Info("Successfully stopped web API");
        }
        #endregion
    }
}