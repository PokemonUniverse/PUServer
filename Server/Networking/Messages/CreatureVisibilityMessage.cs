using NoNameLib.Net.Packet;
using Server.Creatures;
using Server.Logic.Enums;

namespace Server.Networking.Messages
{
    public class CreatureVisibilityMessage : MessageBase
    {
        public bool IsVisible { get; set; }
        public Creature Creature { get; set; }

        public CreatureVisibilityMessage(Creature creature, bool isVisible)
            : base(MessageType.CreatureVisibility)
        {
            IsVisible = isVisible;
            Creature = creature;
        }

        public override Packet ToPacket()
        {
            var packet = new Packet();
            packet.WriteByte((byte)GetMessageType());
            packet.WriteBool(IsVisible);
            packet.WriteLong(Creature.UniqueId);

            if (IsVisible)
            {
                // Creature became visible so we also add creature info to packet
                packet.WriteString(Creature.Name);
                packet.WriteLong(Creature.Position.GetHash());

                // TODO: Send outfit data
            }

            return packet;
        }
    }
}
