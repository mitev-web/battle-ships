namespace BattleShips.Business.Models
{
    /// <summary>
    /// Determines whether is ok to place the ship on the board
    /// </summary>
    public class ShipPlacement
    {
        public bool IsPossiblePlacement { get; set; }

        public Direction PlacementDirection { get; set; }
    }
}
