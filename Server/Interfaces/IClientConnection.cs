using System;
using NoNameLib.Logic.Position;
using Server.Creatures;
using Server.Networking.Messages;

namespace Server.Interfaces
{
    public interface IClientConnection
    {
        event EventHandler<MessageBase> OnMessageReceived;

        long OwnerId { get; internal set; }

        void Disconnect();

        void SendMessage();

        void CreatureVisibleAdd(Creature creature);
        void CreatureVisibleRemove(Creature creature);
        void CreatureMoved(Creature creature, Position from, Position to, bool isTeleport);

        void MoveFailed(Position position);
    }
}
