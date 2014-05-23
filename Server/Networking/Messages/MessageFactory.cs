using System;
using NoNameLib.Extension;
using NoNameLib.Logging;
using NoNameLib.Net.Packet;
using Server.Logic.Enums;

namespace Server.Networking.Messages
{
    public static class MessageFactory
    {
        /// <summary>
        /// Create message object from packet.
        /// </summary>
        /// <param name="packet">Packet with data to read from</param>
        /// <returns>MessageBase with data from packet. Null if message doesn't exists</returns>
        public static MessageBase CreateMessage(Packet packet)
        {
            MessageBase message;

            try
            {
                var header = EnumUtil.ToEnum<MessageType>(packet.ReadByte());
                switch (header)
                {
                    case MessageType.Authenticate:
                        message = new AuthenticateMessage(packet);
                        break;
                    case MessageType.Walk:
                        message = new WalkMessage(packet);
                        break;
#if DEBUG
                    default:
                        throw new NotImplementedException("Netmessage not implemented for header: " + header);
#endif
                }
            }
            catch (Exception e)
            {
                Logger.Error("MessageFectory", "CreateMessage", "{0}\n{1}", e.Message, e.StackTrace);
#if DEBUG
                throw;
#endif
            }

            return message;
        }
    }
}
