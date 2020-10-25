using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace GhostCore.Extensions
{
    public static class EntityFrameworkExtensions
    {
        /// <summary>
        /// Order a query by one or multiple properties by subsequent calls.
        /// </summary>
        /// <typeparam name="T">the object type</typeparam>
        /// <param name="source">the source collection</param>
        /// <param name="prop">the property name to sort by</param>
        /// <param name="order">the order - Ascending/Descending</param>
        /// <returns>a sorted collection by the requested property</returns>
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string prop, ListSortDirection order)
        {
            var isOrdered = source.Expression is MethodCallExpression mce &&
                            (mce.Method.Name == "OrderBy" ||
                             mce.Method.Name == "OrderByDescending" ||
                             mce.Method.Name == "ThenBy" ||
                             mce.Method.Name == "ThenByDescending");

            var type = typeof(T);
            var property = type.GetProperty(prop);
            var parameter = Expression.Parameter(type, "p");
            var access = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(access, parameter);
            var typeArguments = new Type[] { type, property.PropertyType };
            var methodName = isOrdered ? (order == ListSortDirection.Ascending ? "ThenBy" : "ThenByDescending") : (order == ListSortDirection.Ascending ? "OrderBy" : "OrderByDescending");
            var resultExp = Expression.Call(typeof(Queryable), methodName, typeArguments, source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
