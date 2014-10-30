namespace BattleShips.Test
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using BattleShips.Business;
    using BattleShips.Business.Models;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Unit tests for BattleShip game
    /// </summary>
    [TestClass]
    public class BattleShipTests
    {
        /// <summary>
        /// Ensures that ships are correctly drawn correctly on the board 
        /// on the location they are suppose to occupy with 'X' symbol
        /// </summary>
        [TestMethod]
        [Description("Ensures that ship is drawn correctly on board")]
        public void Drawing_TheShips_IsCorrectlyOnBoard()
        {
            // Arrange
            var service = new BattleShipsService();
            service.GetNewGameBoard(10);

            var ships = service.GetAllShipsForPlacement();
            service.PlaceShips(ships);

            // Act
            var currentShips = service.CurrentShipsOnBoard;
            var board = service.GetGameBoard();

            // Assert
            foreach (var ship in currentShips)
            {
                if (ship.Direction == Direction.Horizontal)
                {
                    for (int x = ship.StartPosition.X; x < ship.StartPosition.X + ship.Size; x++)
                        Assert.AreEqual(board[ship.StartPosition.Y, x], 'X');
                }
                else
                {
                    for (int y = ship.StartPosition.Y; y < ship.StartPosition.Y + ship.Size; y++)
                        Assert.AreEqual(board[y, ship.StartPosition.X], 'X');
                }
            }
        }

        /// <summary>
        /// Ensures that the ships take the preserved space for them
        /// </summary>
        [TestMethod]
        [Description("Checks if the combined length of the ships is equal to the occupied space of the ships on bard")]
        public void Placing_TheShips_TakesCorrectSpaceOnBoard()
        {
            // Arrange
            var service = new BattleShipsService();
            service.GetNewGameBoard(10);

            var ships = service.GetAllShipsForPlacement();
            service.PlaceShips(ships);

            // Act
            // combined length of the ships on board
            var combinedShipsLength = service.CurrentShipsOnBoard.Sum(x => x.Size);

            var combinedShipsAllocatedSpaceOnBoard = 0;

            // space allocated for ships
            Dictionary<Position, List<Position>> shipsSpace = service.ShipsAllocatedSpace;

            foreach (var item in shipsSpace)
                combinedShipsAllocatedSpaceOnBoard += item.Value.Count;

            // Assert
            Assert.AreEqual(combinedShipsLength, combinedShipsAllocatedSpaceOnBoard);
        }

        /// <summary>
        /// Ensures that shooting at the ship hits it successfully
        /// </summary>
        [TestMethod]
        [Description("Checks if after shooting the ship's body - the hit is successful")]
        public void Shooting_AtTheShip_HitsItSuccessfully()
        {
            // Arrange
            var service = new BattleShipsService();
            service.GetNewGameBoard(10);

            var ships = service.GetAllShipsForPlacement();
            service.PlaceShips(ships);

            // Act
            var board = service.GetGameBoard();

            // take one random ship from the three on board
            Ship shipOnBoard = service.CurrentShipsOnBoard.Skip(new Random(Guid.NewGuid().GetHashCode()).Next(0, 2)).ToList().First();

            // shoot at the ship
            var shoot = service.ShootAtPosition(shipOnBoard.StartPosition);

            // Assert
            Assert.AreEqual(shoot.ShootResult, ShootResult.SuccessfulHit);

            Assert.AreEqual('+', board[shipOnBoard.StartPosition.Y, shipOnBoard.StartPosition.X]);
        }

        /// <summary>
        /// Ensures that shooting the whole ship sinks it successfully
        /// </summary>
        [TestMethod]
        [Description("Ensures that after shooting the ship's body - the ship gets destroyed")]
        public void Shooting_AtTheShip_SinksItSuccessfully()
        {
            // Arrange
            var service = new BattleShipsService();
            service.GetNewGameBoard(10);

            var ships = service.GetAllShipsForPlacement();
            service.PlaceShips(ships);

            // Act
            Ship shipOnBoard = service.CurrentShipsOnBoard.Skip(new Random(Guid.NewGuid().GetHashCode()).Next(0, 2)).ToList().First();

            // Assert
            if (shipOnBoard.Direction == Business.Models.Direction.Horizontal)
            {
                for (int x = shipOnBoard.StartPosition.X; x < shipOnBoard.StartPosition.X + shipOnBoard.Size; x++)
			    {
			        var shootResult = service.ShootAtPosition(new Position(y: shipOnBoard.StartPosition.Y, x: x));

                    Assert.AreEqual(shootResult.ShootResult, ShootResult.SuccessfulHit);
			    }
            }
            else
            {
                for (int y = shipOnBoard.StartPosition.Y; y < shipOnBoard.StartPosition.Y + shipOnBoard.Size; y++)
			    {
			        var shootResult = service.ShootAtPosition(new Position(y: y, x: shipOnBoard.StartPosition.X));

                    Assert.AreEqual(shootResult.ShootResult, ShootResult.SuccessfulHit);
			    }
            }

            Assert.IsTrue(service.IsShipDestroyedAtPosition(shipOnBoard.StartPosition));
            Assert.IsTrue(shipOnBoard.IsSunk);
        }

        /// <summary>
        /// Ensures that sinking all the ships wins the game
        /// </summary>
        [Description("Ensures that the game is won after sinking all the ships")]
        [TestMethod]
        public void Sinking_AllTheShips_WinsTheGame()
        {
            // Arrange
            var service = new BattleShipsService();

            // Act
            service.GetNewGameBoard(10);
            var ships = service.GetAllShipsForPlacement();
            service.PlaceShips(ships);

            var shipsOnBoard = service.CurrentShipsOnBoard;

            // Assert
            Assert.IsFalse(service.GameIsWon());

            foreach (var shipOnBoard in shipsOnBoard)
            {
                if (shipOnBoard.Direction == Business.Models.Direction.Horizontal)
                {
                    for (int x = shipOnBoard.StartPosition.X; x < shipOnBoard.StartPosition.X + shipOnBoard.Size; x++)
                    {
                        var shootResult = service.ShootAtPosition(new Position(y: shipOnBoard.StartPosition.Y, x: x));
                    }
                }
                else
                {
                    for (int y = shipOnBoard.StartPosition.Y; y < shipOnBoard.StartPosition.Y + shipOnBoard.Size; y++)
                    {
                        var shootResult = service.ShootAtPosition(new Position(y: y, x: shipOnBoard.StartPosition.X));
                    }
                }
            }

            Assert.IsTrue(service.GameIsWon());
        }
    }
}
