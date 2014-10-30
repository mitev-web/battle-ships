namespace BattleShips.Business
{
    using BattleShips.Business.Models;

    public static class Extensions
    {
        /// <summary>
        /// Returns next direction
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <returns></returns>
        public static Direction Next(this Direction direction)
        {
            return direction == Direction.Horizontal ? Direction.Vertical : Direction.Horizontal;
        }

        /// <summary>
        /// Gets the description from ComponentModel Description attribute (used on enums)
        /// </summary>
        /// <param name="obj">The object.</param>
        /// <returns></returns>
        public static string GetDescription(this object obj)
        {
            var type = obj.GetType();

            var memberInfo = type.GetMember(obj.ToString());

            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(System.ComponentModel.DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((System.ComponentModel.DescriptionAttribute)attrs[0]).Description;
                }
            }

            return obj.ToString();
        }
    }
}
