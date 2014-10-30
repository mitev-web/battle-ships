namespace BattleShips.Client
{
    using System;
    using BattleShips.Business;
    using BattleShips.Business.Models;
    using BattleShips.Console.Helpers;

    public delegate BoardResult ShootOnBoard(Position position);

    /// <summary>
    /// GameBoard for BattleShips.
    /// </summary>
    public class ClientGameBoard : IClientGameBoard
    {
        /// <summary>
        /// The board size.
        /// </summary>
        private const int BoardSize = 10;

        /// <summary>
        /// The play board that is visible for players.
        /// </summary>
        private readonly char[,] playBoard = new char[BoardSize, BoardSize];

        private readonly IBattleShipService service = new BattleShipsService();

        /// <summary>
        /// The number of shots used in the game.
        /// </summary>
        private uint numberOfShots;

        private ShootOnBoard shootAtPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientGameBoard"/> class.
        /// </summary>
        public ClientGameBoard()
        {
            // create new gameboard
            this.GetNewGameBoard();

            // place some ships on board
            this.GetAndPlaceShips();

            // show the board to player
            this.PrintBoard(this.playBoard);

            // set reference for delegate
            this.shootAtPosition = this.service.ShootAtPosition;
        }

        /// <summary>
        /// Gets a new game, dependant on the size of the board
        /// </summary>
        public void GetNewGameBoard()
        {
            var gameBoard = this.service.GetNewGameBoard(BoardSize);

            // copy the board to the client
            Array.Copy(gameBoard, 0, this.playBoard, 0, gameBoard.Length);
        }

        /// <summary>
        /// Gets the and place ships.
        /// </summary>
        public void GetAndPlaceShips()
        {
            // get some ships
            var ships = this.service.GetAllShipsForPlacement();

            // place ships on the ActualBoard
            this.service.PlaceShips(ships);
        }

        /// <summary>
        /// Play the game.
        /// </summary>
        public void PlayGame()
        {
            // We take input until all ships are destroyed
            while (!this.service.GameIsWon())
            {
                var input = Console.ReadLine();

                // CHEAT CODE !!
                if (input == "show")
                {
                    this.PrintBoard(this.service.GetGameBoard());
                }
                else
                {
                    var request = Helpers.IsValidInput(input);

                    if (request.IsValidRequest)
                    {
                        // shoot at the board
                        var boardResult = this.shootAtPosition(request.RequestPosition);

                        // increase number of shots
                        this.numberOfShots++;

                        // send response to client and update board
                        this.SendResponseToClientAndUpdateBoard(boardResult);
                    }
                    else
                    {
                        Console.WriteLine("Input Not Valid");
                    }
                }
            }

            this.WinTheGame();
        }

        /// <summary> Checks if ship is destroyed at position in order to notify the client </summary>
        /// <param name="position">The position.</param>
        public void CheckIfShipIsDestroyedAtPosition(Position position)
        {
            var sheepIsDestroyed = this.service.IsShipDestroyedAtPosition(position);

            if (sheepIsDestroyed)
                Console.WriteLine("The ship has sunk!!");
        }

        /// <summary> Prints a given board to the console. </summary>
        /// <param name="board">The board. </param>
        public void PrintBoard(char[,] board)
        {
            var n = board.GetLength(0);

            Console.WriteLine("     1  2  3  4  5  6  7  8  9  0");
            for (var posY = 0; posY < n; posY++)
            {
                for (var posX = 0; posX < n; posX++)
                {
                    if (posX == 0)
                        Console.Write("{0,3}", Helpers.boardLetters[posY]);

                    Console.Write("{0,3}", board[posY, posX]);
                }

                Console.WriteLine("\n");
            }

            if (!this.service.GameIsWon())
                Console.WriteLine("Enter coordinates (row, col), e.g. A5 =");
        }

        /// <summary> Sends the response to client and update board.</summary>
        /// <param name="boardResult">The board result.</param>
        public void SendResponseToClientAndUpdateBoard(BoardResult boardResult)
        {
            // send text response to client about last shoot
            Console.WriteLine(boardResult.ShootResult.GetDescription());

            switch (boardResult.ShootResult)
            {
                case ShootResult.Missed:
                    // we mark with '-' symbol the missed target
                    this.UpdateBoardAtPosition(boardResult.LastShootPosition, '-');
                    break;
                case ShootResult.SuccessfulHit:
                    // we mark with 'X' symbol successful hit on the target
                    this.UpdateBoardAtPosition(boardResult.LastShootPosition, 'X');

                    // check if ship is destroyed at this position and notify client
                    this.CheckIfShipIsDestroyedAtPosition(boardResult.LastShootPosition);
                    break;
            }

            // after the shoot we draw again the board
            this.PrintBoard(this.playBoard);
        }

        /// <summary>
        /// Helper method to update the board quicker
        /// </summary>
        /// <param name="position">The position.</param>
        /// <param name="value">The value.</param>
        private void UpdateBoardAtPosition(Position position, char value)
        {
            this.playBoard[position.Y, position.X] = value;
        }

        private void WinTheGame()
        {
            Console.WriteLine("\n\n");
            Console.WriteLine("Congratulations you sunk all the ships!!");
            Console.WriteLine("You completed the game in {0} shots", this.numberOfShots);
            Console.WriteLine("You are the winner!!!");

            Console.ReadLine();
        }
    }
}