using Algolia.Search.Clients;
using Algolia.Search.Http;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Settings;
using System.Runtime.InteropServices;

namespace AlgoliaTyped
{
    public class TypedIndex<T> where T : class
    {
        protected readonly SearchIndex InternalSearchIndex;
        private readonly ISearchClient _searchClient;

        public virtual string IndexName { get; } = typeof(T).Name;

        public virtual IndexSettings ConfigureIndexSettings(IIndexSettingsConfigurator<T> indexSettingsConfigurator) { return new IndexSettings(); }

        public TypedIndex(ISearchClient searchClient)
        {
            InternalSearchIndex = searchClient.InitIndex(IndexName);
            _searchClient = searchClient;
        }

        /// <summary>
        /// Setting index with index settings defined by the ConfigureIndexSettings method.
        /// If the index already exists in algolia, then the index setting is not changed.
        /// </summary>
        public virtual void ConfigureIfNotExists()
        {
            if (InternalSearchIndex.Exists()) return;

            var indexSettings = ConfigureIndexSettings(new IndexSettingsConfigurator<T>(new IndexSettings()));
            InternalSearchIndex.SetSettings(indexSettings).Wait();
        }

        /// <summary>
        /// Forwards call to index.SetSettings(settings, requestOptions, forwardToReplicas).Wait();
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestOptions"></param>
        /// <param name="forwardToReplicas"></param>
        public virtual void Configure(IndexSettings settings, RequestOptions requestOptions = null, bool forwardToReplicas = false)
        {
            InternalSearchIndex.SetSettings(settings, requestOptions, forwardToReplicas).Wait();
        }

        public virtual BatchIndexingResponse SaveObjects(IEnumerable<T> documents, RequestOptions? requestOptions = null, bool waitForCompletion = false)
        {
            var response = InternalSearchIndex.SaveObjects(documents, requestOptions);
            if(waitForCompletion)
                response.Wait();
            return response;
        }

        public virtual void Delete()
        {
            InternalSearchIndex.Delete();
        }
    }
}
