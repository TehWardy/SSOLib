using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SharedObjects.Extensions
{
    public static class ValueTaskExtensions
    {
        public static async ValueTask<T[]> WhenAll<T>(this IEnumerable<ValueTask<T>> tasks)
        {
            // We don't allocate the list if no task throws
            List<Exception> exceptions = null;

            var results = new List<T>();
            foreach(var t in tasks)
                try
                {
                    results.Add(await t);
                }
                catch (Exception ex)
                {
                    exceptions ??= new List<Exception>();
                    exceptions.Add(ex);
                }

            if(exceptions != null)
                throw new AggregateException(exceptions);

            return results.ToArray();
        }

        public static async ValueTask WhenAll(this IEnumerable<ValueTask> tasks)
        {
            // We don't allocate the list if no task throws
            List<Exception> exceptions = null;

            foreach (var t in tasks)
                try
                {
                    await t;
                }
                catch (Exception ex)
                {
                    exceptions ??= new List<Exception>();
                    exceptions.Add(ex);
                }

            if (exceptions != null)
                throw new AggregateException(exceptions);
        }
    }
}
