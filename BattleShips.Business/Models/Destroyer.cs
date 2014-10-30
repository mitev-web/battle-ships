namespace BattleShips.Business.Models
{
    /// <summary>
    /// Destroyer ship
    /// </summary>
    public class Destroyer : Ship
    {
        public Destroyer()
        {
        }

        public Destroyer(Direction direction, Position position)
            : base(direction, position)
        {
        }

        public override int Size
        {
            get
            {
                return 5;
            }
        }
    }
}
