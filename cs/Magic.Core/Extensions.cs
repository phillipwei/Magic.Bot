using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Core
{
    public static class Extensions
    {
        // http://stackoverflow.com/questions/1577822/passing-a-single-item-as-ienumerablet
        /// <summary>
        /// Wraps this object instance into an IEnumerable&lt;T&gt;
        /// consisting of a single item.
        /// </summary>
        /// <typeparam name="T"> Type of the wrapped object.</typeparam>
        /// <param name="item"> The object to wrap.</param>
        /// <returns>
        /// An IEnumerable&lt;T&gt; consisting of a single item.
        /// </returns>
        public static IEnumerable<T> Yield<T>(this T item)
        {
            yield return item;
        }

        public static IEnumerable<int> IndicesWhere<T>(this List<T> list, Func<T,bool> find)
        {
            for(var i = 0; i<list.Count; i++)
            {
                if(find(list[i]))
                {
                    yield return i;
                }
            }
        }

        public static IEnumerable<T> WithIndices<T>(this List<T> list, List<int> indices)
        {
            foreach(int i in indices)
            {
                yield return list[i];
            }
        }

        public static bool None<T>(this IEnumerable<T> enumerable, Func<T,bool> predicate)
        {
            return !enumerable.All(predicate);
        }

        public static List<T> Duplicate<T>(this List<T> list) where T : IDuplicatable
        {
            return list.Select(x => x.Duplicate()).Cast<T>().ToList();
        }

        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }

        public static IEnumerable<TResult> Zip<TFirst, TSecond, TThird, TResult>(
            this IEnumerable<TFirst> first,
            IEnumerable<TSecond> second,
            IEnumerable<TThird> third,
            Func<TFirst, TSecond, TThird, TResult> resultSelector)
        {
            using (var enum1 = first.GetEnumerator())
            using (var enum2 = second.GetEnumerator())
            using (var enum3 = third.GetEnumerator())
            {
                while (enum1.MoveNext() && enum2.MoveNext() && enum3.MoveNext())
                {
                    yield return resultSelector(
                        enum1.Current,
                        enum2.Current,
                        enum3.Current);
                }
            }
        }

        public static void ZipDo<T1, T2, T3>(this IEnumerable<T1> first,
            IEnumerable<T2> second, IEnumerable<T3> third,
            Action<T1, T2, T3> action)
        {
            first.Zip(second, third, (a, b, c) => { 
                action(a, b, c); 
                return 0; 
            }).ToList();
        }

        public static string ToDisplayString(this IEnumerable<CardDefinition> list)
        {
            var sb = new StringBuilder();
            var group = list.GroupBy(c => c).OrderBy(g => g.Key, Comparer<CardDefinition>.Create((a, b) => { return a.DisplayCompare(b); }));
            var max = group.Max(g => g.Count()).ToString().Length;
            group.Select(g => string.Format("x{0," + max + "} {1}", g.Count(), g.Key)).ForEach(s => sb.AppendLine(s));
            return sb.ToString();
        }

        private static List<CardType> _formatOrder = new List<CardType>() { 
            CardType.Creature,
            CardType.Instant,
            CardType.Sorcery,
            CardType.Enchantment,
            CardType.Planeswalker,
            CardType.Artifact,
            CardType.Land,
            CardType.Tribal
        };

        public static int DisplayIndex(this CardDefinition a)
        {
            return a.Types.Select(t => _formatOrder.IndexOf(t)).Min();
        }

        public static int DisplayCompare(this CardDefinition a, CardDefinition b)
        {
            return a.DisplayIndex().CompareTo(b.DisplayIndex());
        }

        public static IEnumerable<string> SplitIntoLines(this string s, StringSplitOptions options = StringSplitOptions.None)
        {
            return s.Split(new string[] { Environment.NewLine }, options);
        }
    }
}
