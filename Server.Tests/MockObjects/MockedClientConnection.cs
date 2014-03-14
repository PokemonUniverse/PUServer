using System;
using Server.Interfaces;
using Server.Networking.Messages;

namespace Server.Tests.MockObjects
{
    public class MockedClientConnection : IClientConnection
    {
        public event EventHandler<MessageBase> OnMessageReceived;

        public void SendMessage()
        {
            throw new NotImplementedException();
        }
    }
}
