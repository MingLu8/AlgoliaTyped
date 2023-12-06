using Algolia.Search.Models.Settings;

namespace AlgoliaTyped
{
    public interface IIndexSettingsConfigurator<T> where T : class
    {
        IndexSettingsConfigurator<T> SetAllAttributesSearchable(bool ordered = true);
        IndexSettings Configure();
    }
}
