using Server.Logic.Enums;

namespace Server.Networking.Messages
{
    public abstract class MessageBase
    {
        private readonly MessageType messageType;

        protected MessageBase(MessageType messageType)
        {
            this.messageType = messageType;
        }

        public MessageType GetMessageType()
        {
            return this.messageType;
        }
    }
}
