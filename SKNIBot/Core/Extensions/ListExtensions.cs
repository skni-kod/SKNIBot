using System;
using System.Collections.Generic;

namespace SKNIBot.Core.Extensions
{
    public static class ListExtensions
    {
        public static int RandomIndex<T>(this List<T> list)
        {
            var rng = new Random();

            return rng.Next(0, list.Count);
        }

        public static T RandomItem<T>(this List<T> list)
        {
            var index = list.RandomIndex();
            return list[index];
        }
    }
}