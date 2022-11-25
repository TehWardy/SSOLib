using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SharedObjects.Extensions
{
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// No idea why this isn't in .Net already to be honest but never mind ...
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="action"></param>
        /// 

        public static void For<T>(this IEnumerable<T> source, Action<T, int> action)
        {
            if (source != null)
            {
                for (int i = 0; i < source.Count(); i++)
                {
                    action(source.ElementAt(i), i);
                }
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            if (source != null)
                foreach (T i in source)
                    action(i);
        }

        public static int IndexOf<T>(this IList<T> source, Func<T, bool> condition)
        {
            for (int i = 0; i < source.Count; i++)
                if (condition(source[i]))
                    return i;

            return -1;
        }

        /// <summary>
        /// Breaks a collection of items in to sub collections / batches.
        /// </summary>
        /// <typeparam name="T">Type of the collection to break up</typeparam>
        /// <param name="source">the collection</param>
        /// <param name="chunkSize">The size of the collections / batches that will be returned</param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> BatchesOf<T>(this IEnumerable<T> source, int chunkSize) => source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / chunkSize)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();

        public static string DisplayNameFor(this IEnumerable<IResource> source, string resourceName) 
            => source.FirstOrDefault(r => r.Name == resourceName)?.DisplayName ?? $"[{resourceName}_DisplayName]";

        public static string ShortDisplayNameFor(this IEnumerable<IResource> source, string resourceName) 
            => source.FirstOrDefault(r => r.Name == resourceName).ShortDisplayName ?? $"[{resourceName}_ShortDisplayName]";

        public static string DescriptionFor(this IEnumerable<IResource> source, string resourceName) 
            => source.FirstOrDefault(r => r.Name == resourceName)?.Description ?? $"[{resourceName}_Description]";

        public static async ValueTask Match<T, T2>(this IEnumerable<T> source, IEnumerable<T2> possibles, Func<T, T2, bool> with, Func<T, T2, ValueTask> then)
        {
            foreach (T i in source)
                await then(i, possibles.FirstOrDefault(j => with(i, j)));
        }
    }
}