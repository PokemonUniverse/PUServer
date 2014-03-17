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
        
        public abstract void Disconnect();

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
            throw new NotImplementedException();
        }

        public void CreatureVisibleRemove(Creature creature)
        {
            throw new NotImplementedException();
        }

        public void CreatureMoved(Creature creature, Position @from, Position to, bool isTeleport)
        {
            throw new NotImplementedException();
        }

        public void MoveFailed(Position position)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
