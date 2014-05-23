using System;
using System.Collections.Concurrent;
using Server.Creatures;

namespace Server.Logic.Managers
{
    public class CreatureManager : Manager
    {
        //private readonly ConcurrentDictionary<long, Creature> creatures = new ConcurrentDictionary<long, Creature>(2, 1000);
        private readonly ConcurrentDictionary<long, Player> players = new ConcurrentDictionary<long, Player>(2, 500);

        public CreatureManager() 
            : base("CreatureManager")
        {
        }

        public void AddPlayer(Player player)
        {
            players.TryAdd(player.UniqueId, player);
        }

        public void RemovePlayer(Player player)
        {
            players.TryRemove(player.UniqueId, out player);
        }

        public void CleanupInactivePlayers()
        {
            var inactiveTimeout = DateTime.UtcNow.AddMinutes(-15);

            foreach (var entry in players)
            {
                if (entry.Value.LastActivity <= inactiveTimeout)
                {
                    entry.Value.Destroy("Inactive timeout");
                }
            }
        }
    }
}
