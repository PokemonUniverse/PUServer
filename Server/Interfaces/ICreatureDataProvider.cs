using Server.Creatures;

namespace Server.Interfaces
{
    public interface ICreatureDataProvider
    {
        bool LoadPlayerData(Player player);
    }
}
