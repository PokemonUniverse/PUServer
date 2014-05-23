using System;
using System.Collections.Concurrent;
using NoNameLib.Logic.Enums;
using NoNameLib.Logic.Position;
using NoNameLib.TileEditor.Enums;
using Server.Interfaces;
using Server.Logic;
using Server.Map;
using Server.Logic.Enums;

namespace Server.Creatures
{
    public abstract class Creature : ObjectBase
    {
        private readonly ConcurrentDictionary<long, Creature> visibleCreatures = new ConcurrentDictionary<long, Creature>();

        protected Creature(ObjectType objectType, ICreatureDataProvider dataProvider)
            : base(objectType)
        {
            DataProvider = dataProvider;
        }

        #region Events

        /// <summary>
        /// Subscribe to this event to receive when Player wants to move around
        /// </summary>
        public event EventHandler<Direction> CreatureOnMove;

        #endregion

        #region Properties

        protected ICreatureDataProvider DataProvider { get; private set; }

        /// <summary>
        /// Current movement type (eg. Walk, Surf, Bike)
        /// </summary>
        public MovementTypes Movement { get; set; }

        /// <summary>
        /// Map instance in which the creature currently is
        /// </summary>
        public MapInstance MapInstance { get; set; }

        /// <summary>
        /// Current position on the map
        /// </summary>
        public Position Position { get; set; }

        /// <summary>
        /// Direction which the character is facing
        /// </summary>
        public Direction Direction { get; set; }

        #endregion

        /// <summary>
        /// Move creature to supplied direction
        /// </summary>
        /// <param name="direction">Direction to which the player should move</param>
        public void Move(Direction direction)
        {
            Direction = direction;

            if (CreatureOnMove != null)
                CreatureOnMove(this, direction);
        }

        public abstract void MoveSuccess(Direction direction);

        public abstract void MoveFailed();

        /// <summary>
        /// Checks if the moved creature can be seen on our screen and will add/remove the creature when entering/leaving.
        /// If the move creature is itself, the position will be updated.
        /// </summary>
        /// <param name="creature">Creature which moved</param>
        /// <param name="oldPosition">From location</param>
        /// <param name="newPosition">To location</param>
        /// <param name="isTeleport">True if creature teleported to this location (eg, logged in)</param>
        /// <returns>True if creature moved inside our viewport and creature is not self, otherwise false</returns>
        public virtual bool OnCreatureMoved(Creature creature, Position oldPosition, Position newPosition, bool isTeleport)
        {
            // Update self
            if (creature.UniqueId == this.UniqueId)
            {
                this.Position = newPosition;
                return true;
            }

            var canSeeOldPosition = this.Position.InRange(oldPosition, ServerConstants.CLIENT_VIEWPORT_CENTER_WIDTH, ServerConstants.CLIENT_VIEWPORT_CENTER_HEIGHT);
            var canSeeNewPosition = this.Position.InRange(newPosition, ServerConstants.CLIENT_VIEWPORT_CENTER_WIDTH, ServerConstants.CLIENT_VIEWPORT_CENTER_HEIGHT);

            if (!canSeeOldPosition && canSeeNewPosition)
                AddVisibleCreature(creature);
            else if (canSeeOldPosition && !canSeeNewPosition)
                RemoveVisibleCreature(creature);

            return (canSeeOldPosition || canSeeNewPosition);
        }

        /// <summary>
        /// Add creature to visible creature list. Doesn't matter if the creature already exists in the list.
        /// </summary>
        /// <param name="creature">Creature to be added to visible creatures list</param>
        /// <returns>True if the creature was added</returns>
        public virtual bool AddVisibleCreature(Creature creature)
        {
            return visibleCreatures.TryAdd(creature.UniqueId, creature);
        }

        /// <summary>
        /// Remove creature from visible creatures list. Doesn't matter if the creature exists or not in the list.
        /// </summary>
        /// <param name="creature">Creature to be removed</param>
        /// <returns>True if creature was removed from the list</returns>
        public virtual bool RemoveVisibleCreature(Creature creature)
        {
            Creature dummy;
            return visibleCreatures.TryRemove(creature.UniqueId, out dummy);
        }
    }
}
