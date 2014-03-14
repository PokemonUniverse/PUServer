using NoNameLib.Logic.Enums;
using NoNameLib.Net.Packet;
using Server.Logic.Enums;

namespace Server.Networking.Messages
{
    public class WalkMessage : MessageBase
    {
        public Direction Direction { get; set; }

        public WalkMessage()
            : base(MessageType.Walk)
        {
            
        }

        public WalkMessage(Packet packet) 
            : this()
        {
            // TODO: Read packet into message
        }
    }
}
