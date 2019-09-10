using System;

namespace Engine
{
    public class RandomGenerator
    {
        private static Random _generator = new Random();

        public static int NumberBetween(int minValue, int maxValue)
        {
            return _generator.Next(minValue, maxValue + 1);
        }
    }
}