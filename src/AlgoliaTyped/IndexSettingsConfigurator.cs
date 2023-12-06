using Algolia.Search.Models.Settings;
using System.Linq.Expressions;

namespace AlgoliaTyped
{
    public class IndexSettingsConfigurator<T> : IIndexSettingsConfigurator<T> where T : class
    {
        private Dictionary<string, bool> _searchableAttributes = new Dictionary<string, bool>();
        private Dictionary<string, FacetType> _facetedAttributes = new Dictionary<string, FacetType>();
        public List<string> Attributes = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                                        .Select(a => a.Name.ToAttributeName()).ToList();

        public IndexSettingsConfigurator<T> SetAllAttributesFaceted(FacetType facetType)
        {
            _facetedAttributes.Clear();
            _facetedAttributes = Attributes.ToDictionary(a => a, _ => facetType);
            return this;
        }

        public IndexSettingsConfigurator<T> RemoveFacetedAttributes(params Expression<Func<T, object>>[] expressions)
        {
            var attributeNames = expressions.GetAttributeNames().ToList();
            attributeNames.ForEach(a => _facetedAttributes.Remove(a));
            return this;
        }

        public IndexSettingsConfigurator<T> AddFacetedAttributes(FacetType facetType, params Expression<Func<T, object>>[] expressions)
        {
            var attributeNames = expressions.GetAttributeNames().ToList();
            attributeNames.ForEach(a =>
            {
                _facetedAttributes.Remove(a);
                _facetedAttributes.Add(a, facetType);
            });
            return this;
        }

        public IndexSettingsConfigurator<T> SetAllAttributesSearchable(bool ordered = true)
        {            
            _searchableAttributes.Clear();
            _searchableAttributes = Attributes.ToDictionary(a => a, _ => ordered);
            return this;
        }

        public IndexSettingsConfigurator<T> RemoveSearchableAttributes(params Expression<Func<T, object>>[] expressions)
        {
            var attributeNames = expressions.GetAttributeNames().ToList();
            attributeNames.ForEach(a => _searchableAttributes.Remove(a));
            return this;
        }

        public IndexSettingsConfigurator<T> AddSearchableAttributes(bool ordered, params Expression<Func<T, object>>[] expressions)
        {
            var attributeNames = expressions.GetAttributeNames().ToList();
            attributeNames.ForEach(a =>
            {
                _searchableAttributes.Remove(a);
                _searchableAttributes.Add(a, ordered);
            });
            return this;
        }

        public IndexSettings Configure()
        {
            var indexSettings = new IndexSettings
            {
                SearchableAttributes = _searchableAttributes.Select(a => a.Value ? a.Key : $"unordered({a.Key})").ToList(),
                AttributesForFaceting = _facetedAttributes.Select(a => a.Value == FacetType.Searchable ? $"searchable({a.Key})" : $"filterOnly({a.Key})").ToList(),
            };

            return indexSettings;
        }
    }
}
