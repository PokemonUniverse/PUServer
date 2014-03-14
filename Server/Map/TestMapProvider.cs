using System.Collections.Generic;
using NoNameLib.Logging;
using NoNameLib.TileEditor.Collections;
using NoNameLib.TileEditor.Enums;
using Server.Interfaces;

namespace Server.Map
{
    internal class TestMapProvider : IMapProvider
    {
        private const string TAG = "TestMapProvider";

        readonly Dictionary<int, MapBase> maps = new Dictionary<int, MapBase>();

        public bool LoadMaps(out string errors)
        {
            errors = "";

            maps.Add(1, InitializeMap("TestMap1", 10, 5));
            Logger.Verbose(TAG, "LoadMaps", "TestMap1 Loaded");
            maps.Add(2, InitializeMap("TestMap2", 5, 10));
            Logger.Verbose(TAG, "LoadMaps", "TestMap2 Loaded");

            return true;
        }

        public bool TryGetMap(int mapId, out MapBase mapBase)
        {
            mapBase = null;
            return false;
        }

        public bool TryGetTilePoint(int mapId, int x, int y, out TilePoint tilePoint)
        {
            tilePoint = null;
            return false;
        }

        private MapBase InitializeMap(string name, int width, int height)
        {
            var tiles = new TilePointTable();
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var tp = new TilePoint(x, y);
                    var tpl = new TilePointLayer(tp, 1);
                    tpl.Movement = MovementTypes.Walk;
                    tpl.SetLayer(new TilePointTileLayer(1));

                    tp.SetLayer(tpl);
                }
            }

            return new MapBase(name, tiles);
        }
    }
}
