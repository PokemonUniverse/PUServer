using System;
using NoNameLib.Net.Packet;
using Server.Interfaces;
using Server.Networking.Messages;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Server.Networking.WebSocket
{
    public class WebSocketServerConnection : AppServer<WebSocketClientConnection, BinaryRequestInfo>, IServerConnection
    {
        #region IServerConnection

        public event EventHandler<IClientConnection> OnClientConnected;

        public void AcceptConnections(int port)
        {
            this.Setup(9001);
            this.Start();
        }

        public void StopListening()
        {
            this.Stop();
        }

        #endregion

        protected override void OnNewSessionConnected(WebSocketClientConnection session)
        {
            if (OnClientConnected != null)
                OnClientConnected(this, session);
        }

        protected override void ExecuteCommand(WebSocketClientConnection session, BinaryRequestInfo requestInfo)
        {
            var message = MessageFactory.CreateMessage(new Packet(requestInfo.Body));
            if (message != null)
            {
                session.MessageReceived(message);
            }

            base.ExecuteCommand(session, requestInfo);
        }
    }

}
