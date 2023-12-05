using Algolia.Search.Clients;
using Algolia.Search.Models.Settings;
using System.Runtime.InteropServices;

namespace AlgoliaTyped
{
    public class TypedIndex<T> where T : class
    {
        private readonly SearchIndex _index;
        private readonly ISearchClient _searchClient;
        private readonly IndexSettings _indexSettings = new IndexSettings();
       
        public virtual string IndexName { get; } = typeof(T).Name;

        public TypedIndex(ISearchClient searchClient, Action<IndexSettings>? indexSettingsConfigurator = null)
        {
            _index = searchClient.InitIndex(IndexName);
            _searchClient = searchClient;
            if(indexSettingsConfigurator != null)
                indexSettingsConfigurator(_indexSettings);
        }

        public void Create()
        {
            _index.SetSettings(_indexSettings).Wait();
        }

        public void Delete()
        {
            _index.Delete().Wait();
        }
    }
}
