using System;
using System.Collections.Concurrent;
using NoNameLib.Logging;
using NoNameLib.Logic.Enums;
using NoNameLib.TileEditor.Collections;
using NoNameLib.TileEditor.Enums;
using Server.Creatures;

namespace Server.Map
{
    public class MapInstance
    {
        private const string TAG = "MapInstance";

        private readonly ConcurrentDictionary<long, Creature> creatures;
        private readonly ConcurrentDictionary<long, Player> players;
        private readonly MapBase mapBase;

        internal MapInstance(MapBase map, int instanceNumber, int maxPlayers, bool isMain = false)
        {
            creatures = new ConcurrentDictionary<long, Creature>(2, maxPlayers);
            players = new ConcurrentDictionary<long, Player>(2, maxPlayers);

            this.mapBase = map;

            InstanceNumber = instanceNumber;
            IsMain = isMain;
        }

        #region Properties

        public int InstanceNumber { get; private set; }

        /// <summary>
        /// Name of the instance formatted as MapName-InstanceNumber.
        /// </summary>
        public string Name
        {
            get { return string.Format("{0}-{1}", this.mapBase.Name, InstanceNumber); }
        }

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
            return PlayerCount < this.mapBase.MaxPlayersPerInstance;
        }

        public bool AddPlayer(Player player)
        {
            if (!AddCreature(player))
            {
                return false;
            }

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

        public bool AddCreature(Creature creature)
        {
            if (creatures.ContainsKey(creature.UniqueId))
            {
                Logger.Debug(TAG, "AddCreature", "Creature with unique id '{0}' already exists in instance '{1}'.", creature.UniqueId, Name);
                return true;
            }
            
            if (creatures.TryAdd(creature.UniqueId, creature))
            {
                creature.CreatureOnMove += OnCreatureMove;

                return true;
            }
            
            Logger.Warning(TAG, "AddCreature", "Failed to add creature '{0}' to instance '{1}'.", creature.UniqueId, Name);

            return false;
        }

        public bool RemoveCreature(Creature creature)
        {
            if (!creatures.ContainsKey(creature.UniqueId))
            {
                Logger.Debug(TAG, "RemoveCreature", "Creature with unique id '{0}' does not exists in disctict '{1}'.", creature.UniqueId, Name);
                return true;
            }

            if (creatures.TryRemove(creature.UniqueId, out creature))
            {
                creature.CreatureOnMove -= OnCreatureMove;

                return true;
            }
            
            Logger.Warning(TAG, "RemoveCreature", "Failed to remove creature '{0}' from district '{1}'.", creature.UniqueId, Name);

            return false;
        }

        #endregion

        #region Private Methods

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
            var newTilePointLayer = this.mapBase.GetTilePointLayer(newPosition);
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

        private void OnCreatureMove(object sender, Direction moveDirection)
        {
            var creature = (Creature)sender;
            if (!InternalCreatureMove(creature, moveDirection))
            {
                creature.MoveFailed();
            }
        }

        #endregion
    }
}
