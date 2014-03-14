using System.ServiceProcess;
using NoNameLib.Logging;
using Server.Interfaces;
using Server.Logic.Exceptions;
using Server.Logic.Managers;
using Server.Networking;

namespace Server
{
    public class Startup : ServiceBase
    {
        public enum StartupExceptions
        {
            NoServerConnection
        }

        private const string TAG = "Startup";

        private IServerConnection serverConnection;

        #region Server control

        internal void StartServer()
        {
            InitializeManagers();

            // Load worldmap
            GlobalManager.GetManager<MapManager>().LoadAllMaps();

            // Start accepting connections
            StartConnection();
        }

        internal void StopServer()
        {
            Logger.Verbose(TAG, "StopServer", "Stopping server");
            GlobalManager.Destroy();
        }

        #endregion

        #region Server initialization

        private void InitializeManagers()
        {
            Logger.Verbose(TAG, "StartServer", "Initializing managers");
            GlobalManager.Initialize();
        }

        #endregion

        private void StartConnection()
        {
           
        }

        #region Service method overrides

        protected override void OnStart(string[] args)
        {
            StartServer();

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            StopServer();

            base.OnStop();
        }

        #endregion
    }
}
