using System.Threading;
using Server.Logic.Enums;

namespace Server.Logic
{
    public class ObjectBase
    {
        private static long uniqueId;

        public ObjectBase(ObjectType objectType)
        {
            // Use atomic operation to increment object unique id
            Interlocked.Increment(ref uniqueId);
            UniqueId = uniqueId;
            Type = objectType;
        }

        #region Properties

        public long UniqueId { get; private set; }

        public string Name { get; set; }

        public ObjectType Type { get; protected set; }

        #endregion
    }
}
