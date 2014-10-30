namespace BattleShips.Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var board = new ClientGameBoard();

            board.PlayGame();
        }
    }
}
