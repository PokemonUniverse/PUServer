using System.Collections.Concurrent;
using NoNameLib.Logging;
using NoNameLib.Logic.Position;
using NoNameLib.TileEditor.Collections;

namespace Server.Map
{
    public class MapBase
    {
        private const string TAG = "MapBase";

        private readonly ConcurrentDictionary<int, MapInstance> availableInstances = new ConcurrentDictionary<int, MapInstance>();

        private readonly TilePointTable tiles;

        public MapBase(string name, TilePointTable tiles)
        {
            this.tiles = tiles;

            Name = name;
            MaxPlayersPerInstance = 0;
        }

        #region Properties

        /// <summary>
        /// Name of the map
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Max players before a new instance is created (0 = unlimited).
        /// </summary>
        public int MaxPlayersPerInstance { get; private set; }

        #endregion

        #region Instances

        public MapInstance GetAvailableInstance()
        {
            foreach (var availableInstance in availableInstances)
            {
                if (availableInstance.Value.HasAvailableSpot())
                    return availableInstance.Value;
            }

            return CreateInstance();
        }

        /// <summary>
        /// Create a new instance for this map
        /// </summary>
        /// <returns>MapInstance which has been created</returns>
        public MapInstance CreateInstance()
        {
            var instance = new MapInstance(this, 0, 0);
            if (!availableInstances.TryAdd(0, instance))
            {
                Logger.Warning(TAG, "CreateInstance", "Unable to save instance to list");
            }
            return instance;
        }

        /// <summary>
        /// Attempt to remove the MapInstance from the server
        /// </summary>
        /// <param name="instance">MapInstance object to be deleted</param>
        /// <returns>True if MapInstance has been deleted, otherwise false</returns>
        public bool DeleteInstance(MapInstance instance)
        {
            return availableInstances.TryRemove(instance.InstanceNumber, out instance);
        }

        #endregion

        #region Tiles

        /// <summary>
        /// Gets a TilePoint object from TilePointTable. Return null if there is no TilePoint on this position
        /// </summary>
        /// <param name="position"></param>
        /// <returns>TilePoint object if exists, otherwise null</returns>
        public TilePoint GetTilePoint(Position position)
        {
            return tiles.GetTilePoint(position.X, position.Y, false);
        }

        /// <summary>
        /// Get TilePointLayer from position parameter
        /// </summary>
        /// <param name="position"></param>
        /// <returns>TilePointLayer object if exists otherwise null</returns>
        public TilePointLayer GetTilePointLayer(Position position)
        {
            TilePointLayer tilePointLayer = null;
            var tilePoint = tiles.GetTilePoint(position.X, position.Y, false);
            if (tilePoint != null)
            {
                tilePointLayer = tilePoint.GetLayer(position.Z);
            }

            return tilePointLayer;
        }

        #endregion
    }
}