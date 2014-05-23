using Server.Creatures;
using Server.Interfaces;
using Server.Logic.Enums;
using Server.Networking.Messages;

namespace Server.Data
{
    public class DummyCreatureDataProvider : ICreatureDataProvider
    {
        public AuthenticateResult Authenticate(Player player, AuthenticateMessage message)
        {
            return AuthenticateResult.Success;
        }

        public bool LoadPlayerData(Player player)
        {
            player.Name = "Player-" + player.UniqueId;

            return true;
        }
    }
}
