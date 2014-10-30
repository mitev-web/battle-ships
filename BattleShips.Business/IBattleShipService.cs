namespace BattleShips.Business
{
    using System.Collections.Generic;
    using BattleShips.Business.Models;

    public interface IBattleShipService
    {
        /// <summary>
        /// Gets the new game board.
        /// </summary>
        /// <param name="boardSize">Size of the board.</param>
        /// <returns>the game board</returns>
        char[,] GetNewGameBoard(int boardSize);

        /// <summary>
        /// Gets all ships for placement.
        /// (usually this should be taken from DAL)
        /// </summary>
        /// <returns>the ships</returns>
        Stack<Ship> GetAllShipsForPlacement();

        /// <summary>
        /// Places the ships.
        /// </summary>
        /// <param name="shipsToBePlaced">Ships that need to be placed</param>
        void PlaceShips(Stack<Ship> shipsToBePlaced);

        /// <summary>
        /// Gets the game board. (CHEAT CODE!!)
        /// </summary>
        /// <returns>The current gameboard</returns>
        char[,] GetGameBoard();

        /// <summary>
        /// Shoots at position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>Result of the last shot</returns>
        BoardResult ShootAtPosition(Position position);

        /// <summary>
        /// Determines whether [the ship is destroyed at] [the specified position].
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        bool IsShipDestroyedAtPosition(Position position);

        /// <summary>
        /// Determines whether the game is won
        /// </summary>
        /// <returns>bool result</returns>
        bool GameIsWon();
    }
}
