using System;
using NoNameLib.Logic.Position;
using Server.Creatures;
using Server.Interfaces;
using Server.Networking.Messages;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Protocol;

namespace Server.Networking.WebSocket
{
    public class WebSocketClientConnection : AppSession<WebSocketClientConnection, BinaryRequestInfo>, IClientConnection
    {
        #region IClientConnection

        public event EventHandler<MessageBase> OnMessageReceived;

        public long OwnerId { get; internal set; }

        public void Disconnect()
        {
        
        }

        public void SendMessage()
        {
            //this.Send();
            
        }

        public void CreatureVisibleAdd(Creature creature)
        {
            
        }

        public void CreatureVisibleRemove(Creature creature)
        {
            
        }

        public void CreatureMoved(Creature creature, Position from, Position to, bool isTeleport)
        {
            
        }

        public void MoveFailed(Position position)
        {

        }

        #endregion

        internal void MessageReceived(MessageBase message)
        {
            if (OnMessageReceived != null)
                OnMessageReceived(this, message);
        }
    }
}
