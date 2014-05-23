using NoNameLib.Debug;
using NoNameLib.Logging;
using Server.Creatures;
using Server.Interfaces;
using Server.Logic.Exceptions;
using Server.Map;

namespace Server.Logic.Managers
{
    internal class MapManager : Manager
    {
        private enum MapManagerException
        {
            UnableToCreateMapProvider,
            FailedToLoadMaps
        }

        private IMapProvider mapProvider;

        public MapManager() : base("MapManager")
        {
        }

        public override void Initialize()
        {
            this.mapProvider = null;

            if (TestUtil.IsPcMrDark)
                this.mapProvider = new TestMapProvider();

            if (this.mapProvider == null)
                throw new StartupException(MapManagerException.UnableToCreateMapProvider, "Unable to load MapProvider, check configuration.");
        }

        public void LoadAllMaps()
        {
            Logger.Info(Name, "LoadAllMaps", "Start loading worldmap into memory");
            var loadingTimer = new System.Diagnostics.Stopwatch();
            loadingTimer.Start();

            string result;
            if (!this.mapProvider.LoadMaps(out result))
            {
                if (result == string.Empty)
                    result = "Unknown Error";

                loadingTimer.Stop();
                throw new StartupException(MapManagerException.FailedToLoadMaps, result);
            }

            loadingTimer.Stop();
            Logger.Info(Name, "LoadAppMaps", "Worldmap loaded in {0:mm\\:ss\\:fff}", loadingTimer.Elapsed);
        }

        public bool GetMap(int mapId, out MapBase mapBase)
        {
            return this.mapProvider.TryGetMap(mapId, out mapBase);
        }

        public void MoveCreatureToMap(Creature creature, int mapId)
        {
            if (creature.MapInstance != null && creature.MapInstance.Base.MapId == mapId)
                return;

            MapBase nextMap;
            if (!GetMap(mapId, out nextMap))
            {
                Logger.Warning("MapManager", "MoveCreatureToMap", "Could not find map with id '{0}'.", mapId);
                return;
            }

            MapInstance oldMapInstance = creature.MapInstance;

            // Get available map instance and add creature to it
            if (nextMap.GetAvailableInstance().AddCreature(creature))
            {
                if (oldMapInstance != null)
                    oldMapInstance.RemoveCreature(creature);
            }
        }
    }
}
