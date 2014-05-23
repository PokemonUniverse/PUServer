using System;
using NoNameLib.Logic.Position;
using Server.Creatures;
using Server.Logic.Enums;
using Server.Networking.Messages;

namespace Server.Interfaces
{
    public interface IClientConnection
    {
        event EventHandler<MessageBase> OnMessageReceived;

        long OwnerId { get; set; }

        void Disconnect(string reason);

        void SendMessage(MessageBase message);

        void AuthenticateResult(AuthenticateResult result);

        void CreatureVisibleAdd(Creature creature);
        void CreatureVisibleRemove(Creature creature);
        void CreatureMoved(Creature creature, Position from, Position to, bool isTeleport);

        void MoveFailed(Position position, string message = "");
    }
}
