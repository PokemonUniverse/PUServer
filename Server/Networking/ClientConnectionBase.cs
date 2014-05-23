using System;
using NoNameLib.Logic.Position;
using Server.Creatures;
using Server.Interfaces;
using Server.Logic.Enums;
using Server.Networking.Messages;

namespace Server.Networking
{
    public abstract class ClientConnectionBase : IClientConnection
    {
        public event EventHandler<MessageBase> OnMessageReceived;

        public long OwnerId { get; set; }
        
        public abstract void Disconnect(string reason);

        public abstract void SendMessage(MessageBase message);

        internal void MessageReceived(MessageBase message)
        {
            if (OnMessageReceived != null)
                OnMessageReceived(this, message);
        }

        #region Helper Methods

        public void AuthenticateResult(AuthenticateResult result)
        {
            SendMessage(new AuthenticateMessage(result));
        }

        public void CreatureVisibleAdd(Creature creature)
        {
            SendMessage(new CreatureVisibilityMessage(creature, true));
        }

        public void CreatureVisibleRemove(Creature creature)
        {
            SendMessage(new CreatureVisibilityMessage(creature, false));
        }

        public void CreatureMoved(Creature creature, Position from, Position to, bool isTeleport)
        {
            SendMessage(new CreatureMoveMessage(creature, from, to , isTeleport));
        }

        public void MoveFailed(Position position, string message = "")
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
