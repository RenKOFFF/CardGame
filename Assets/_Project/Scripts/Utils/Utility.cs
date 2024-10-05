
using System;
using System.ComponentModel;

namespace CardGame.Utils
{
    public static class Utility
    {
        public static TEnum GetRandomOf<TEnum>(int min = 0, int max = int.MaxValue) where TEnum : Enum
        {
            var values = Enum.GetValues(typeof(TEnum));

            if (values.Length == 0)
                return default;

            var minRange = Math.Max(min, 0);
            var maxRange = Math.Min(max, values.Length);

            if (minRange >= maxRange)
            {
                throw new InvalidEnumArgumentException("Min value must be less than max value");
            }
            
            var randomValue = UnityEngine.Random.Range(minRange, maxRange);
            return (TEnum)values.GetValue(randomValue);
        }
    }
}