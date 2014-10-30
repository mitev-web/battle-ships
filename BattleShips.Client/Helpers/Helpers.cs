namespace BattleShips.Console.Helpers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using BattleShips.Business.Models;
    using BattleShips.Client.Models;

    public static class Helpers
    {
        public static char[] boardLetters = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J' };

        /// <summary>
        /// Determines whether [is valid input] [the specified input].
        /// </summary>
        /// <param name="input">The client input.</param>
        /// <returns></returns>
        public static ShootPositionRequest IsValidInput(string input)
        {
            int posX = 0;
            var posY = 0;

            var isValid = input != null && input.Length == 2 && boardLetters.Contains(char.ToUpper(input[0])) && int.TryParse(input[1].ToString(CultureInfo.InvariantCulture), out posX);
            
            for (var i = 0; i < boardLetters.Length; i++)
            {
                if (boardLetters[i] == char.ToUpper(input[0]))
                {
                    posY = i;
                    break;
                }
            }

            posX = posX == 0 ? 9 : --posX;

            return new ShootPositionRequest
            {
                IsValidRequest = isValid,
                RequestPosition = new Position { Y = posY, X = posX }
            };
        }
    }
}
