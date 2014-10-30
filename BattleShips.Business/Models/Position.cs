namespace BattleShips.Business.Models
{
    public class Position
    {
        public Position()
        {
        }

        public Position(int y, int x)
        {
            this.Y = y;
            this.X = x;
        }

        public int X { get; set; }

        public int Y { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            Position p = obj as Position;

            return (this.X == p.X) && (this.Y == p.Y);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;

                hash = hash * 23 + X.GetHashCode();
                hash = hash * 23 + Y.GetHashCode();
                return hash;
            }
        }
    }
}
