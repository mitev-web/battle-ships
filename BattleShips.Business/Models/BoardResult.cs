namespace BattleShips.Business.Models
{
    /// <summary>
    /// Result from shooting the board sent to client
    /// </summary>
    public class BoardResult
    {
        public ShootResult ShootResult { get; set; }

        public Position LastShootPosition { get; set; }
    }
}
