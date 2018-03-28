using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace SKNIBot.Core.Database.Helpers
{
    public static class RandomRowHelper
    {
        public static T Random<T>(this IEnumerable<T> dbSet) where T:class
        {
            return dbSet.OrderBy(p => Guid.NewGuid()).FirstOrDefault();
        }
    }
}
