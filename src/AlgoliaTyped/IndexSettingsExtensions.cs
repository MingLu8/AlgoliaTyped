using Algolia.Search.Models.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AlgoliaTyped
{
    public static class IndexSettingsExtensions
    {
        public static IndexSettings SetAllFieldsSearchable<T>(this IndexSettings indexSettings)
        {
            var propertyNames = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                                         .Select(x => x.Name).ToList();
            indexSettings.SearchableAttributes = propertyNames;
            return indexSettings;
        }
    }
}
