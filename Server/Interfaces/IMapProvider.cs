using NoNameLib.TileEditor.Collections;
using Server.Map;

namespace Server.Interfaces
{
    public interface IMapProvider
    {
        bool LoadMaps(out string errors);

        bool TryGetMap(int mapId, out MapBase mapBase);

        bool TryGetTilePoint(int mapId, int x, int y, out TilePoint tilePoint);
    }
}
