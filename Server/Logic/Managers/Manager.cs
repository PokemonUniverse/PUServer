using System;

namespace Server.Logic.Managers
{
    public class Manager
    {
        public string Name { get; protected set; }

        public Manager(String name)
        {
            this.Name = name;
        }

        public virtual void Initialize() { }

        public virtual void Deinitialize() { }

        public virtual void Destroy() { }
    }
}
