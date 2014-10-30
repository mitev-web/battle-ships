namespace BattleShips.Business.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BattleShips.Business.Models;

    public static class BattleShipsHelpers
    {
        /// <summary>
        /// Check if ship placement is possible.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="shipSize">Size of the ship.</param>
        /// <param name="actualBoard">The actual board.</param>
        /// <returns>
        /// ShipPlacement result.
        /// </returns>
        public static ShipPlacement ShipPlacementIsPossible(Position pos, int shipSize, char[,] actualBoard)
        {
            var horizontalAvailable = HorizontalPlacementIsPossible(pos, shipSize, actualBoard);

            var verticalAvailable = VerticalPlacementIsPossible(pos, shipSize, actualBoard);

            var returnResult = new ShipPlacement();

            if (horizontalAvailable || verticalAvailable)
            {
                returnResult.IsPossiblePlacement = true;
            }
            else
            {
                returnResult.IsPossiblePlacement = false;
            }

            if (horizontalAvailable && !verticalAvailable)
            {
                returnResult.PlacementDirection = Direction.Horizontal;
            }
            else if (!horizontalAvailable && verticalAvailable)
            {
                returnResult.PlacementDirection = Direction.Vertical;
            }
            else
            {
                // If placement for both directions is avalable just choose one of the directions
                // randomly
                if (new Random().Next(10, 50) % 2 == 1)
                    returnResult.PlacementDirection = Direction.Horizontal;
                else
                    returnResult.PlacementDirection = Direction.Vertical;
            }

            return returnResult;
        }

        /// <summary>
        /// 1/25 (4%) chance that it should place the ship at the current field.
        /// </summary>
        /// <returns>Whether ship should be placed or not.</returns>
        public static bool ShouldPlaceShip()
        {
            var rand1 = new Random(Guid.NewGuid().GetHashCode()).Next(1, 25);
            var rand2 = new Random(Guid.NewGuid().GetHashCode()).Next(1, 25);

            return rand1 == rand2;
        }

        /// <summary>
        /// Check if horizontal placement of the ship is possible.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <param name="shipSize">Size of the ship.</param>
        /// <param name="actualBoard">The actual board.</param>
        /// <returns>
        /// Whether placement is possible or not.
        /// </returns>
        private static bool HorizontalPlacementIsPossible(Position pos, int shipSize, char[,] actualBoard)
        {
            for (var i = pos.X; i < pos.X + shipSize; i++)
            {
                try
                {
                    if (actualBoard[pos.X, i] == '.')
                        continue;

                    if (actualBoard[pos.Y, i] == 'X')
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Check if vertical placement of the ship is possible.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="shipSize">Size of the ship.</param>
        /// <param name="actualBoard">The actual board.</param>
        /// <returns>
        /// Whether placement is possible or not.
        /// </returns>
        private static bool VerticalPlacementIsPossible(Position position, int shipSize, char[,] actualBoard)
        {
            for (var i = position.Y; i < position.Y + shipSize; i++)
            {
                try
                {
                    if (actualBoard[i, position.X] == '-')
                        continue;

                    if (actualBoard[i, position.X] == 'X')
                        return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
