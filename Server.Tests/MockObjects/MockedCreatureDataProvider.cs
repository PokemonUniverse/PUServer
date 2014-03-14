using Server.Creatures;
using Server.Interfaces;

namespace Server.Tests.MockObjects
{
    public class MockedCreatureDataProvider : ICreatureDataProvider
    {
        public bool LoadPlayerData(Player player)
        {
            player.Name = "MockedPlayer";

            return true;
        }
    }
}
