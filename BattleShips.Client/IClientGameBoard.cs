namespace BattleShips.Client
{
    using BattleShips.Business.Models;

    /// <summary>
    /// Client interface for game board
    /// </summary>
    public interface IClientGameBoard
    {
        /// <summary>
        /// Starts new game
        /// </summary>
        void PlayGame();

        /// <summary>
        /// Gets a new game, dependant on the size of the board
        /// </summary>
        void GetNewGameBoard();

        /// <summary>
        /// Prints the board.
        /// </summary>
        /// <param name="board">The board.</param>
        void PrintBoard(char[,] board);

        /// <summary>
        /// Sends the response to client and updates board after shoot
        /// </summary>
        /// <param name="shootResult">The shoot result.</param>
        void SendResponseToClientAndUpdateBoard(BoardResult shootResult);

        /// <summary>
        /// Checks if ship is destroyed at position in order to notify the client
        /// </summary>
        /// <param name="position">The position.</param>
        void CheckIfShipIsDestroyedAtPosition(Position position);
    }
}
