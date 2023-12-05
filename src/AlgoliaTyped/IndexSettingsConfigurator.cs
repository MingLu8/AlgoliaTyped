using Algolia.Search.Models.Settings;

namespace AlgoliaTyped
{
    public interface IIndexSettingsConfigurator<T>
    {
        IndexSettingsConfigurator<T> SetAllFieldsSearchable();
        IndexSettings GetIndexSettings();
    }

    public class IndexSettingsConfigurator<T> : IIndexSettingsConfigurator<T>
    {
        private IndexSettings _indexSettings = new IndexSettings();

        public IndexSettingsConfigurator(IndexSettings indexSettings)
        {
            
        }

        public IndexSettingsConfigurator<T> SetAllFieldsSearchable()
        {
            var propertyNames = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                                         .Select(a => a.Name[0].ToString().ToLowerInvariant() + a.Name.Substring(1)).ToList();
            _indexSettings.SearchableAttributes = propertyNames;
            return this;
        }

        public IndexSettings GetIndexSettings() => _indexSettings;
    }
}
