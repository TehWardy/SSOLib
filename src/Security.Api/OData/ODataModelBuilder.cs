using Microsoft.OData.ModelBuilder;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Security.Api.OData
{
    /// <summary>
    /// Base model builder class for all OData model builders
    /// </summary>
    public abstract class ODataModelBuilder
    {
        protected ODataConventionModelBuilder Builder = new();

        /// <summary>
        /// Derived types implement this to setup the OData Model information
        /// </summary>
        /// <returns></returns>
        public abstract ODataModel Build();

        protected virtual EntitySetConfiguration<T> AddSet<T, TKey>(bool enableBatchingToo = false, string setName = null) where T : class
        {
            setName ??= typeof(T).Name;
            EntitySetConfiguration<T> setConfig = Builder.EntitySet<T>(setName);

            return setConfig;
        }

        protected virtual EntitySetConfiguration<T> AddJoinSet<T, TKey>(Expression<Func<T, TKey>> key) where T : class
        {

            string setName = typeof(T).Name;
            // register basic CRUD endpoint
            EntitySetConfiguration<T> setConfig = Builder.EntitySet<T>(setName);

            // register base OData controller defined functions

            _ = Builder.EntityType<T>().HasKey(key);

            return setConfig;
        }
    }
}