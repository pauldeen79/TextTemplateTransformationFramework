using System;
using System.Collections.Generic;
using System.Linq;

namespace Utilities.Extensions
{
    /// <summary>
    /// Class which contains extension methods for the generic IEnumerable class.
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Determines whether the specified instance contains the specified value.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        public static bool Contains(this IEnumerable<string> instance, string value, StringComparison comparisonType)
            => instance.Any(s => s.Equals(value, comparisonType));

        /// <summary>
        /// Fixes null reference on this enumerable instance, and optionally applies a filter.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> instance, Func<T, bool> predicate = null)
        {
            var notNull = instance ?? Array.Empty<T>();
            return predicate == null
                ? notNull
                : notNull.Where(predicate);
        }

        /// <summary>
        /// Gets distinct values.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="keySelector">The key selector used to group by.</param>
        /// <returns>
        /// Distinct values.
        /// </returns>
        public static IEnumerable<TKey> Distinct<TSource, TKey>(this IEnumerable<TSource> instance, Func<TSource, TKey> keySelector)
            => instance
                .GroupBy(keySelector)
                .Select(t => t.Key);

#if NETFRAMEWORK
        /// <summary>
        /// Gets distinct values by the specified expression.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TKey">The type of the key.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="keySelector">The key selector.</param>
        /// <returns>
        /// Distinct values.
        /// </returns>
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> instance, Func<TSource, TKey> keySelector)
            => instance
                .GroupBy(keySelector)
                .Select(t => t.First());
#endif

        /// <summary>
        /// Excludes the specified type on this enumerable.
        /// </summary>
        /// <typeparam name="TSource">The enumerable type.</typeparam>
        /// <typeparam name="TExclude">The type to exclude.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IEnumerable<TSource> Exclude<TSource, TExclude>(this IEnumerable<TSource> instance)
            => instance.Where(t => t is not TExclude);

        public static IEnumerable<T> Concat<T>(this IEnumerable<T> instance, T additionalItem)
            => instance.Concat(new[] { additionalItem });

        public static string LastOrDefaultWhenEmpty(this IEnumerable<string> instance, string defaultValue)
            => instance.LastOrDefault().WhenNullOrEmpty(defaultValue);

        public static string LastOrDefaultWhenEmpty(this IEnumerable<string> instance, Func<string> defaultValueDelegate)
            => instance.LastOrDefault().WhenNullOrEmpty(defaultValueDelegate);

        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> instance, Action<T> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            foreach (var item in instance)
            {
                action(item);
            }

            return instance;
        }

        public static bool ContainsAny<T>(this IEnumerable<T> instance, params T[] conditions)
            => instance.ContainsAny((IEnumerable<T>)conditions);

        public static bool ContainsAny<T>(this IEnumerable<T> instance, IEnumerable<T> conditions)
            => conditions.Any(instance.Contains);

        public static bool ContainsAny(this IEnumerable<string> instance, StringComparison stringComparison, params string[] conditions)
            => instance.ContainsAny(stringComparison, (IEnumerable<string>)conditions);

        public static bool ContainsAny(this IEnumerable<string> instance, StringComparison stringComparison, IEnumerable<string> conditions)
            => conditions.Any(s => instance.Contains(s, stringComparison));
    }
}
