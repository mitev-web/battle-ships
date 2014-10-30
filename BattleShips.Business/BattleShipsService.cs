namespace BattleShips.Business
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BattleShips.Business.Helpers;
    using BattleShips.Business.Models;

    /// <summary>
    /// Handles Business logic for BattleShip game
    /// </summary>
    public class BattleShipsService : IBattleShipService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BattleShipsService"/> class.
        /// </summary>
        public BattleShipsService()
        {
            // generate some ships, usually these ships should be taken from DAL
            // if client want -> they can pass different ships to the board via Placeships() member
            this.ShipsToBePlaced = new Stack<Ship>();

            this.ShipsToBePlaced.Push(new BattleShip());
            this.ShipsToBePlaced.Push(new Destroyer());
            this.ShipsToBePlaced.Push(new BattleShip());
        }

        public List<Ship> CurrentShipsOnBoard { get; set; }

        /// <summary> After ships are placed this space is allocated </summary>
        /// <value> The ships allocated space. </value>
        public Dictionary<Position, List<Position>> ShipsAllocatedSpace { get; set; }

        private char[,] ActualBoard { get; set; }

        private Stack<Ship> ShipsToBePlaced { get; set; }

        #region Public Methods
        /// <summary>
        /// Populates the board.
        /// </summary>
        public char[,] GetNewGameBoard(int boardSize)
        {
            this.ActualBoard = new char[boardSize, boardSize];

            var posX = 0;
            var posY = 0;

            var direction = Direction.Vertical;

            var length = Math.Pow(boardSize, 2) + 1;

            for (var i = 1; i < length; i++)
            {
                this.ActualBoard[posY, posX] = '.';

                if (i % boardSize == 0)
                    direction = direction.Next();

                switch (direction)
                {
                    case Direction.Vertical:
                        posY++;
                        break;

                    case Direction.Horizontal:
                        posY = 0;
                        posX++;
                        direction = Direction.Vertical;
                        break;
                }
            }

            return this.ActualBoard;
        }

        /// <summary>
        /// Places the ships.
        /// </summary>
        public void PlaceShips(Stack<Ship> shipsToBePlaced)
        {
            // saves the collection to member, before its empty
            this.CurrentShipsOnBoard = this.ShipsToBePlaced.ToList();

            // the cicle continues untill we place all the ships randomly
            while (shipsToBePlaced.Count > 0)
            {
                var posX = 0;
                var posY = 0;

                var direction = Direction.Vertical;
                var boardSize = this.ActualBoard.GetLength(0);

                var length = Math.Pow(boardSize, 2) + 1;

                for (var i = 1; i < length; i++)
                {
                    var currentShip = shipsToBePlaced.Peek();

                    if (BattleShipsHelpers.ShouldPlaceShip())
                    {
                        // verify if ship placement is possible
                        var placement = BattleShipsHelpers.ShipPlacementIsPossible(new Position { X = posX, Y = posY }, currentShip.Size, this.ActualBoard);

                        if (placement.IsPossiblePlacement)
                        {
                            var placingShip = shipsToBePlaced.Pop();

                            placingShip.SetPosition(new Position { X = posX, Y = posY });

                            placingShip.SetDirection(placement.PlacementDirection);

                            this.PlaceShip(placingShip);

                            if (shipsToBePlaced.Count == 0)
                            {
                                // after placing all ships - populate ships allocated space
                                this.PopulateShipsAllocatedSpace();

                                return;
                            }
                        }
                    }

                    if (i % boardSize == 0)
                        direction = direction.Next();

                    switch (direction)
                    {
                        case Direction.Vertical:
                            posY++;
                            break;

                        case Direction.Horizontal:
                            posY = 0;
                            posX++;
                            direction = Direction.Vertical;
                            break;
                    }
                }
            }
        }

        public Stack<Ship> GetAllShipsForPlacement()
        {
            return this.ShipsToBePlaced;
        }

        /// <summary>
        /// Shoots at position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>board result</returns>
        public BoardResult ShootAtPosition(Position position)
        {
            var targetField = this.ActualBoard[position.Y, position.X];

            var shootResult = ShootResult.NoShot;

            switch (targetField)
            {
                case '-':
                case '+':
                    shootResult = ShootResult.AlreadyShootAtLocation;
                    break;

                case '.':
                    shootResult = ShootResult.Missed;
                    this.ActualBoard[position.Y, position.X] = '-';
                    break;

                case 'X':
                    // with + - we mark successful hit on the target
                    this.ActualBoard[position.Y, position.X] = '+';
                    shootResult = ShootResult.SuccessfulHit;

                    // check if ship is destroyed and update its status
                    var shipIsDestroyed = this.IsShipDestroyedAtPosition(position);

                    if (shipIsDestroyed)
                        this.GameIsWon();

                    break;
            }

            var result = new BoardResult
                             {
                                 ShootResult = shootResult,
                                 LastShootPosition = position
                             };

            return result;
        }

        public char[,] GetGameBoard()
        {
            return this.ActualBoard;
        }

        public bool GameIsWon()
        {
            return this.AreAllShipsDestroyed();
        }

        /// <summary>
        /// Determines whether [is ship destroyed at position] [the specified position].
        /// and updates its status
        /// </summary>
        public bool IsShipDestroyedAtPosition(Position position)
        {
            var isDestroyed = true;

            Ship targetShip = null;

            foreach (var shipAllocatedSpace in this.ShipsAllocatedSpace)
            {
                foreach (var currentPosition in shipAllocatedSpace.Value)
                {
                    if (position.Equals(currentPosition))
                    {
                        targetShip = this.CurrentShipsOnBoard.Single(x => x.StartPosition == shipAllocatedSpace.Key);
                        break;
                    }
                }
            }

            if (targetShip == null)
                return false;

            if (targetShip.Direction == Direction.Horizontal)
            {
                for (int i = targetShip.StartPosition.X; i < targetShip.StartPosition.X + targetShip.Size; i++)
                {
                    if (this.ActualBoard[targetShip.StartPosition.Y, i] == '+')
                        continue;
                    else
                        return false;
                }
            }
            else
            {
                for (int i = targetShip.StartPosition.Y; i < targetShip.StartPosition.Y + targetShip.Size; i++)
                {
                    if (this.ActualBoard[i, targetShip.StartPosition.X] == '+')
                        continue;
                    else
                        return false;
                }
            }

            if (isDestroyed)
                targetShip.IsSunk = true;

            return isDestroyed;
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Populates the allocated space by the ships.
        /// </summary>
        private void PopulateShipsAllocatedSpace()
        {
            this.ShipsAllocatedSpace = new Dictionary<Position, List<Position>>();

            foreach (var ship in this.CurrentShipsOnBoard)
            {
                var space = new List<Position>();

                if (ship.Direction == Direction.Horizontal)
                {
                    for (int i = ship.StartPosition.X; i < ship.StartPosition.X + ship.Size; i++)
                        space.Add(new Position(y: ship.StartPosition.Y, x: i));
                }
                else
                {
                    for (int i = ship.StartPosition.Y; i < ship.StartPosition.Y + ship.Size; i++)
                        space.Add(new Position(y: i, x: ship.StartPosition.X));
                }

                this.ShipsAllocatedSpace[ship.StartPosition] = space;
            }
        }

        /// <summary>
        /// Places the ship.
        /// </summary>
        /// <param name="placingShip">The placing ship.</param>
        private void PlaceShip(Ship placingShip)
        {
            if (placingShip.Direction == Direction.Horizontal)
            {
                for (var i = 0; i < placingShip.Size; i++)
                    this.ActualBoard[placingShip.StartPosition.Y, placingShip.StartPosition.X + i] = 'X';
            }
            else
            {
                for (var i = 0; i < placingShip.Size; i++)
                    this.ActualBoard[placingShip.StartPosition.Y + i, placingShip.StartPosition.X] = 'X';
            }
        }

        /// <summary> Determines whether all ships destroyed. </summary>
        private bool AreAllShipsDestroyed()
        {
            return this.CurrentShipsOnBoard.All(x => x.IsSunk);
        }

        #endregion
    }
}