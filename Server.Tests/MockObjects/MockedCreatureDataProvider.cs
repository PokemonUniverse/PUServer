using Server.Creatures;
using Server.Interfaces;
using Server.Logic.Enums;
using Server.Networking.Messages;

namespace Server.Tests.MockObjects
{
    public class MockedCreatureDataProvider : ICreatureDataProvider
    {
        public AuthenticateResult Authenticate(Player player, AuthenticateMessage message)
        {
            throw new System.NotImplementedException();
        }

        public bool LoadPlayerData(Player player)
        {
            player.Name = "MockedPlayer";

            return true;
        }
    }
}
