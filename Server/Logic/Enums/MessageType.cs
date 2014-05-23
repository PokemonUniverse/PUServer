namespace Server.Logic.Enums
{
    public enum MessageType : byte
    {
        Authenticate = 0x01, // C -> S

        Walk = 0x10, // C -> S
        CreatureVisibility = 0x11, // S -> C
        CreatureMove = 0x12, // S -> C
    }
}
