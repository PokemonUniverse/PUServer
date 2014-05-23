using NoNameLib.Logic.Position;
using NoNameLib.Net.Packet;
using Server.Creatures;
using Server.Logic.Enums;

namespace Server.Networking.Messages
{
    public class CreatureMoveMessage : MessageBase
    {
        public Creature Creature { get; set; }
        public Position From { get; set; }
        public Position To { get; set; }
        public bool IsTeleport { get; set; }

        public CreatureMoveMessage(Creature creature, Position from, Position to, bool isTeleport)
            : base(MessageType.CreatureMove)
        {
            Creature = creature;
            From = from;
            To = to;
            IsTeleport = isTeleport;
        }

        public override Packet ToPacket()
        {
            var packet = new Packet();
            packet.WriteByte((byte)GetMessageType());

            packet.WriteLong(Creature.UniqueId);
            packet.WriteLong(From.GetHash());
            packet.WriteLong(To.GetHash());
            packet.WriteBool(IsTeleport);

            return packet;
        }
    }
}
