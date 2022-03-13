using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Helper
{
    public static class Sorting
    {
        public static void InsertionSort<TSource, TKey>(IList<TSource> list, Func<TSource,TKey> keySelector) where TKey : IComparable
        {
            int i = 1;
            int j;
            TSource xSource;
            TKey x;
            int length = list.Count;
            while(i < length)
            {
                xSource = list[i];
                x = keySelector(xSource);
                j = i - 1;
                while(j >= 0 && keySelector(list[j]).CompareTo(x) > 0)
                {
                    list[j + 1] = list[j];
                    j -= 1;
                }
                list[j + 1] = xSource;
                i += 1;
            }
        }
    }
}
