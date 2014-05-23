using System;
using System.Collections.Concurrent;
using NoNameLib.Logging;
using NoNameLib.Logic.Enums;
using NoNameLib.TileEditor.Collections;
using NoNameLib.TileEditor.Enums;
using Server.Creatures;
using Server.Logic.Enums;

namespace Server.Map
{
    public class MapInstance
    {
        private const string TAG = "MapInstance";

        private readonly ConcurrentDictionary<long, Creature> creatures;
        private readonly ConcurrentDictionary<long, Player> players;

        internal MapInstance(MapBase map, int instanceNumber, int maxPlayers, bool isMain = false)
        {
            creatures = new ConcurrentDictionary<long, Creature>(2, maxPlayers);
            players = new ConcurrentDictionary<long, Player>(2, maxPlayers);

            Base = map;

            InstanceNumber = instanceNumber;
            IsMain = isMain;

            Name = string.Format("{0}-{1}", Base.Name, InstanceNumber);
        }

        #region Properties

        public MapBase Base { get; private set; }

        public int InstanceNumber { get; private set; }

        /// <summary>
        /// Name of the instance formatted as MapName-InstanceNumber.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Number of players active in this instance.
        ///  </summary>
        public int PlayerCount { get { return players.Count; } }

        /// <summary>
        /// Boolean whether is the main instance. If it is, this instance will never be destroyed.
        /// </summary>
        public bool IsMain { get; private set; }

        #endregion

        #region Public Methods

        public bool HasAvailableSpot()
        {
            return PlayerCount < Base.MaxPlayersPerInstance;
        }

        public bool AddCreature(Creature creature)
        {
            if (creatures.ContainsKey(creature.UniqueId))
            {
                Logger.Debug(TAG, "AddCreature", "Creature with unique id '{0}' already exists in instance '{1}'.", creature.UniqueId, Name);
                return true;
            }
            
            if (!creatures.TryAdd(creature.UniqueId, creature))
            {
                Logger.Warning(TAG, "AddCreature", "Failed to add creature '{0}' to instance '{1}'.", creature.UniqueId, Name);                
                return false;
            }

            if (creature.Type == ObjectType.Player)
            {
                if (!AddPlayer((Player)creature))
                    return false;
            }

            creature.MapInstance = this;
            creature.CreatureOnMove += OnCreatureMove;

            return true;
        }

        public bool RemoveCreature(Creature creature)
        {
            if (!creatures.ContainsKey(creature.UniqueId))
            {
                Logger.Debug(TAG, "RemoveCreature", "Creature with unique id '{0}' does not exists in instance '{1}'.", creature.UniqueId, Name);
                return true;
            }

            if (!creatures.TryRemove(creature.UniqueId, out creature))
            {
                Logger.Warning(TAG, "RemoveCreature", "Failed to remove creature '{0}' from instance '{1}'.", creature.UniqueId, Name);
                return false;
            }

            if (creature.Type == ObjectType.Player)
            {
                if (!RemovePlayer((Player)creature))
                    return false;
            }

            creature.CreatureOnMove -= OnCreatureMove;

            if (creature.MapInstance.Base.MapId == Base.MapId)
                creature.MapInstance = null;

            return true;
        }

        #endregion

        #region Private Methods

        private bool AddPlayer(Player player)
        {
            if (players.ContainsKey(player.UniqueId))
            {
                Logger.Debug(TAG, "AddPlayer", "Player with unique id '{0}' already exists in instance '{1}'.", player.UniqueId, Name);
                return true;
            }

            if (!players.TryAdd(player.UniqueId, player))
            {
                Logger.Warning(TAG, "AddCreature", "Failed to add player '{0}' to instance '{1}'.", player.UniqueId, Name);
                return false;
            }

            return true;
        }

        private bool RemovePlayer(Player player)
        {
            if (!players.ContainsKey(player.UniqueId))
            {
                Logger.Debug(TAG, "AddPlayer", "Player with unique id '{0}' does not exists in instance '{1}'.", player.UniqueId, Name);
                return true;
            }

            if (!players.TryRemove(player.UniqueId, out player))
            {
                Logger.Warning(TAG, "AddCreature", "Failed to remove player '{0}' from instance '{1}'.", player.UniqueId, Name);
                return false;
            }

            return true;
        }

        private bool InternalCreatureMove(Creature creature, Direction direction)
        {
            var currentPosition = creature.Position;
            var newPosition = creature.Position;

            // Calculate next position based on move direciton
            switch (direction)
            {
                case Direction.North:
                    newPosition.Y--;
                    break;
                case Direction.East:
                    newPosition.X++;
                    break;
                case Direction.South:
                    newPosition.Y++;
                    break;
                case Direction.West:
                    newPosition.X--;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("direction");
            }

            // Get TilePointLayer for new position
            var newTilePointLayer = this.Base.GetTilePointLayer(newPosition);
            if (newTilePointLayer == null)
                return false;

            // Check if creature can move to new position
            if (!CanCreatureMove(creature.Movement, direction, newTilePointLayer))
                return false;

            // Loop all creatures in this instance to check if they can see each other
            foreach (var c in creatures.Values)
            {
                c.OnCreatureMoved(creature, currentPosition, newPosition, false);
            }

            return true;
        }

        private bool CanCreatureMove(MovementTypes movementType, Direction direction, TilePointLayer to)
        {
            if (to.Movement == MovementTypes.Walk)
                return true;

            if (to.Movement == MovementTypes.Blocked)
                return false;
            if (direction == Direction.East && to.Movement.HasFlag(MovementTypes.BlockedWest))
                return false;
            if (direction == Direction.West && to.Movement.HasFlag(MovementTypes.BlockedEast))
                return false;
            if (direction == Direction.North && to.Movement.HasFlag(MovementTypes.BlockedSouth))
                return false;
            if (direction == Direction.South && to.Movement.HasFlag(MovementTypes.BlockedNorth))
                return false;

            if (!to.Movement.HasFlag(movementType))
                return false;

            return true;
        }

        #endregion

        #region Event Handlers

        private void OnCreatureMove(object sender, Direction direction)
        {
            var creature = (Creature)sender;
            if (InternalCreatureMove(creature, direction))
            {
                creature.MoveSuccess(direction);
            }
            else
            {
                creature.MoveFailed();
            }
        }

        #endregion
    }
}
