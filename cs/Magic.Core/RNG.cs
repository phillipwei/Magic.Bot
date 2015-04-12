using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public static class RNG
    {
        private static Random _random = new Random();

        public static int Next()
        {
            return _random.Next();
        }

        public static int Next(int max)
        {
            return _random.Next(max);
        }

        public static HeadsOrTails Flip()
        {
            return (HeadsOrTails) RNG.Next(2);
        }

        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = RNG.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }
}
