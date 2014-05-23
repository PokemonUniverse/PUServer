using NoNameLib.Extension;
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
            Direction = packet.ReadInt().ToEnum<Direction>();
        }
    }
}
