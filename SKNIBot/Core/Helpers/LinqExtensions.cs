﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace SKNIBot.Core.Helpers
{
    public static class LinqExtensions
    {
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> whereClause)
        {
            if (condition)
            {
                return query.Where(whereClause);
            }

            return query;
        }
    }
}
