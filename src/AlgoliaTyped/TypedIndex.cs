using Algolia.Search.Clients;
using Algolia.Search.Http;
using Algolia.Search.Models.Common;
using Algolia.Search.Models.Settings;
using System.Runtime.InteropServices;

namespace AlgoliaTyped
{
    public class TypedIndex<T> where T : class
    {
        public readonly SearchIndex InternalSearchIndex;
        private readonly ISearchClient _searchClient;

        public string IndexName { get; private set; }

        public virtual IndexSettings ConfigureIndexSettings(IIndexSettingsConfigurator<T> indexSettingsConfigurator) { return new IndexSettings(); }

        public TypedIndex(ISearchClient searchClient, string? indexName = null)
        {
            IndexName = indexName ?? typeof(T).Name;
            InternalSearchIndex = searchClient.InitIndex(IndexName);
            _searchClient = searchClient;
        }

        /// <summary>
        /// Setting index with index settings defined by the ConfigureIndexSettings method.
        /// If the index already exists in algolia, then the index setting is not changed.
        /// </summary>
        public virtual void Configure(bool skipIfExists = true)
        {
            if (skipIfExists && InternalSearchIndex.Exists()) return;

            var indexSettings = ConfigureIndexSettings(new IndexSettingsConfigurator<T>());
            InternalSearchIndex.SetSettings(indexSettings).Wait();
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
            InternalSearchIndex.Delete().Wait();           
        }
    }
}
