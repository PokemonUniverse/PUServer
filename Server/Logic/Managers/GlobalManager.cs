using System;
using System.Collections.Generic;
using NoNameLib.Logging;
using Server.Logic.Exceptions;

namespace Server.Logic.Managers
{
    public static class GlobalManager
    {
        private enum GlobalManagerException
        {
            ManagerNonExistent
        }

        private readonly static Dictionary<Type, Manager> managers = new Dictionary<Type, Manager>();
        private static bool isInitialized;

        public static void Initialize()
        {
            if (isInitialized)
            {
                Logger.Warning("GlobalManager", "Initialize", "Managers are already initialized.");
                return;
            }
            isInitialized = true;
            
            managers.Add(typeof(CreatureManager), new CreatureManager());
            managers.Add(typeof(MapManager), new MapManager());

            foreach (var manager in managers.Values)
            {
                manager.Initialize();
            }
        }

        public static void Destroy()
        {
            foreach (var manager in managers.Values)
            {
                manager.Destroy();
            }

            isInitialized = false;
        }

        public static T GetManager<T>() where T : Manager
        {
            Manager m;
            if (managers.TryGetValue(typeof(T), out m))
            {
                return (T)m;
            }

            throw new ManagerException(GlobalManagerException.ManagerNonExistent, "The manager '{0}' does not exists. Are you sure the manager gets initialized in GlobalManager.Initialize?", typeof(T));
        }
    }
}
