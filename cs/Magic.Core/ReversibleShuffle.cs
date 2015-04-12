using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Magic.Core
{
    public class ReversibleShuffle
    {
        List<int> _swapList = new List<int>();

        public ReversibleShuffle(int size)
        {
            // Creates a list of indices each item would swap with when reverse-iterating
            // over the list.  Do not swap at 0th index.
            int n = size;
            while (n > 1)
            {
                n--;
                _swapList.Add(RNG.Next(n + 1));
            }
        }

        public void Apply<T>(IList<T> list)
        {
            if(list.Count != _swapList.Count + 1)
            {
                throw new ArgumentException("Wrong sized list.");
            }

            int n = list.Count;
            foreach(var k in this._swapList)
            {
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public void Undo<T>(IList<T> list)
        {
            if (list.Count != _swapList.Count + 1)
            {
                throw new ArgumentException("Wrong sized list.");
            }

            int n = 1;
            foreach(var k in this._swapList.Reverse<int>())
            {
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
                n++;
            }
        }
    }
}
