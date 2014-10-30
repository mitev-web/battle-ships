namespace BattleShips.Business.Models
{
    /// <summary>
    /// Battle ship
    /// </summary>
    public class BattleShip : Ship
    {
        public BattleShip()
        {
        }

        public BattleShip(Direction direction, Position position)
            : base(direction, position)
        {
        }

        public override int Size
        {
            get
            {
                return 4;
            }
        }
    }
}
