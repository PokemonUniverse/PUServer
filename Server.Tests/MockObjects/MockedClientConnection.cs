using System;
using NoNameLib.Logic.Position;
using Server.Creatures;
using Server.Interfaces;
using Server.Logic.Enums;
using Server.Networking.Messages;

namespace Server.Tests.MockObjects
{
    public class MockedClientConnection : IClientConnection
    {
        public event EventHandler<MessageBase> OnMessageReceived;
        public long OwnerId { get; set; }
        public void Disconnect(string reason)
        {
            throw new NotImplementedException();
        }

        public void SendMessage(MessageBase message)
        {
            throw new NotImplementedException();
        }

        public void AuthenticateResult(AuthenticateResult result)
        {
            throw new NotImplementedException();
        }

        public void CreatureVisibleAdd(Creature creature)
        {
            throw new NotImplementedException();
        }

        public void CreatureVisibleRemove(Creature creature)
        {
            throw new NotImplementedException();
        }

        public void MoveFailed(Position position)
        {
            throw new NotImplementedException();
        }

        public void CreatureMoved(Creature creature, Position @from, Position to, bool isTeleport)
        {
            throw new NotImplementedException();
        }

        public void SendMessage()
        {
            throw new NotImplementedException();
        }
    }
}
