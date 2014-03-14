using System.Threading;

namespace Server.Logic
{
    public class ObjectBase
    {
        private static long uniqueId;

        public ObjectBase()
        {
            // Use atomic operation to increment object unique id
            Interlocked.Increment(ref uniqueId);
            UniqueId = uniqueId;
        }

        #region Properties

        public long UniqueId { get; private set; }

        public string Name { get; set; }

        #endregion
    }
}
