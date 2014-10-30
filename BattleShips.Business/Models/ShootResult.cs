namespace BattleShips.Business.Models
{
    using System.ComponentModel;

    public enum ShootResult
    {
        [Description("You haven't shooted yet!!")]
        NoShot = 0,
        [Description("Alredy shoot at location!!")]
        AlreadyShootAtLocation = 1,
        [Description("Missed!!")]
        Missed,
        [Description("Successful Hit!!!")]
        SuccessfulHit
    }
}
