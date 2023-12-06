using Algolia.Search.Models.Search;
using System.Linq.Expressions;

namespace AlgoliaTyped
{
    public interface IQueryDescriptor<T> where T : class
    {
        IQueryDescriptor<T> AllFacets();
        IQueryDescriptor<T> Facets(IEnumerable<string> settingValue);
        IQueryDescriptor<T> Facets(params Expression<Func<T, object>>[] propertyExpressions);
        IQueryDescriptor<T> Filters(Expression<Func<T, object>> expression, bool combineExistingFilterWithOrOperator = false);
        Query GetQuery();
        IQueryDescriptor<T> HitsPerPage(int itemCount);
        IQueryDescriptor<T> Page(int pageNo);
        IQueryDescriptor<T> RestrictSearchableAttributes(params Expression<Func<T, object>>[] attributes);
        IQueryDescriptor<T> SearchQuery(string settingValue);
    }
}