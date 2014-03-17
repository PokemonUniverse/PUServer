using System.ServiceProcess;
using NoNameLib.Logging;
using Server.Logic.Managers;

namespace Server
{
    public class Startup : ServiceBase
    {
        public enum StartupExceptions
        {
            NoServerConnection,
            InvalidPortNumber
        }

        private const string TAG = "Startup";

        private Networking.Server server;

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
            server = new Networking.Server();
            server.Start();
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
