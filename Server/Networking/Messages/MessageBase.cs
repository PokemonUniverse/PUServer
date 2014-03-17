using NoNameLib.Net.Packet;
using Server.Logic.Enums;

namespace Server.Networking.Messages
{
    public abstract class MessageBase
    {
        private readonly MessageType messageType;

        public bool RestrictedAccess { get; protected set; }

        protected MessageBase(MessageType messageType)
        {
            RestrictedAccess = true;

            this.messageType = messageType;
        }

        public MessageType GetMessageType()
        {
            return this.messageType;
        }

        public virtual Packet ToPacket()
        {
            return null;
        }
    }
}
