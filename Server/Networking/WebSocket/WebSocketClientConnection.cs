using Server.Networking.Messages;
using SuperWebSocket;

namespace Server.Networking.WebSocket
{
    public class WebSocketClientConnection : ClientConnectionBase
    {
        private ClientSession clientSession;

        public WebSocketClientConnection(ClientSession clientSession)
        {
            this.clientSession = clientSession;
        }

        #region IClientConnection

        public override void Disconnect(string reason)
        {
            clientSession.CloseWithHandshake(reason);
            clientSession = null;
        }

        public override void SendMessage(MessageBase message)
        {
            var packet = message.ToPacket();
            packet.Prepare();

            clientSession.Send(packet.GetBuffer(), 0, packet.Size);
        }

        #endregion
    }

    public class ClientSession : WebSocketSession<ClientSession>
    {
        public WebSocketClientConnection Client { get; private set; }

        public ClientSession()
        {
            Client = new WebSocketClientConnection(this);
        }

        public void MessageReceived(MessageBase message)
        {
            Client.MessageReceived(message);
        }
    }
}
