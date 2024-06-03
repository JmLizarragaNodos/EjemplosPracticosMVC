using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Helpers
{
    public static class LinqHelper
    {
        public static List<TSource> DistinctBy<TSource, TKey>(
            this IEnumerable<TSource> source, Func<TSource, TKey> keySelector
        )
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            return source.Where(element => seenKeys.Add(keySelector(element))).ToList();
        }
    }
}