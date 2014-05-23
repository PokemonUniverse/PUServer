using NoNameLib.Net.Packet;
using Server.Logic.Enums;

namespace Server.Networking.Messages
{
    public class AuthenticateMessage : MessageBase
    {
        public string Username { get; private set; }
        public string Password { get; private set; }
        public short Version { get; private set; }

        public AuthenticateResult Result { get; set; }

        public AuthenticateMessage() 
            : base(MessageType.Authenticate)
        {
            RestrictedAccess = false;
        }

        public AuthenticateMessage(Packet packet) 
            : this()
        {
            Username = packet.ReadString();
            Password = packet.ReadString();
            Version = packet.ReadShort();
        }

        public AuthenticateMessage(AuthenticateResult result) 
            : this()
        {
            Result = result;
        }

        public override Packet ToPacket()
        {
            var packet = new Packet();
            packet.WriteByte((byte)MessageType.Authenticate);
            packet.WriteByte((byte)Result);

            return packet;
        }
    }
}
