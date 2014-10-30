namespace BattleShips.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BattleShips.Business.Models;

    /// <summary>
    /// Client request to shoot at position
    /// </summary>
    public class ShootPositionRequest
    {
        /// <summary>
        /// Gets or sets a value indicating whether [is valid request].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [is valid request]; otherwise, <c>false</c>.
        /// </value>
        public bool IsValidRequest { get; set; }

        /// <summary>
        /// Gets or sets the request position.
        /// </summary>
        /// <value>
        /// The request position.
        /// </value>
        public Position RequestPosition { get; set; }
    }
}
