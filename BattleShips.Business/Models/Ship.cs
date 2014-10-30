namespace BattleShips.Business.Models
{
    /// <summary>
    /// Abstract ship class
    /// </summary>
    public abstract class Ship
    {
        private Direction direction;

        private Position startPosition;

        protected Ship()
        {
        }

        protected Ship(Direction direction, Position position)
        {
            this.direction = direction;
            this.startPosition = position;
        }

        public abstract int Size { get; }

        public bool IsSunk { get; set; }

        /// <summary>
        /// Starting position on the board
        /// </summary>
        /// <value>
        /// The start position.
        /// </value>
        public Position StartPosition
        {
            get
            {
                return this.startPosition;
            }
        }

        /// <summary>
        /// Direction on which the ship is positioned
        /// </summary>
        /// <value>
        /// The direction.
        /// </value>
        public Direction Direction
        {
            get
            {
                return this.direction;
            }
        }

        public void SetPosition(Position position)
        {
            this.startPosition = position;
        }

        public void SetDirection(Direction positionDirection)
        {
            this.direction = positionDirection;
        }
    }
}
