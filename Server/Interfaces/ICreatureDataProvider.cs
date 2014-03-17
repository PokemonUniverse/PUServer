using Server.Creatures;
using Server.Logic.Enums;
using Server.Networking.Messages;

namespace Server.Interfaces
{
    public interface ICreatureDataProvider
    {
        AuthenticateResult Authenticate(Player player, AuthenticateMessage message);

        bool LoadPlayerData(Player player);
    }
}
