using Algolia.Search.Models.Settings;
using System.Linq.Expressions;
using System.Reflection;

namespace AlgoliaTyped
{
    public interface IIndexSettingsConfigurator<T> where T : class
    {
        IndexSettingsConfigurator<T> SetAllAttributesSearchable(bool ordered = true);
        IndexSettings Configure();
    }

    public enum FacetType
    {
        Searchable, FilterOnly
    }

    public class IndexSettingsConfigurator<T> : IIndexSettingsConfigurator<T> where T : class
    {
        private Dictionary<string, bool> _searchableAttributes = new Dictionary<string, bool>();
        private Dictionary<string, FacetType> _facetedAttributes = new Dictionary<string, FacetType>();
        private IndexSettings _indexSettings = new IndexSettings();
        public List<string> Attributes = typeof(T).GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public)
                                        .Select(a => a.Name.ToAttributeName()).ToList();
        public IndexSettingsConfigurator(IndexSettings indexSettings)
        {
            
        }

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

    public static class ExtensionMethods
    {
        public static string ToAttributeName(this string name)
        {
            var subNames = name.Split('.').Select(a => a[0].ToString().ToLowerInvariant() + a.Substring(1));
            return string.Join(".", subNames);
        }

        public static IEnumerable<string> GetPropertyNames<T>(this Expression<Func<T, object>>[] expressions) where T : class
        {
            return expressions.Select(a => GetPropertyName<T>(a)).Distinct();
        }


        public static IEnumerable<string> GetAttributeNames<T>(this Expression<Func<T, object>>[] expressions) where T : class
        {
            return expressions.GetPropertyNames<T>().Select(a => a.ToAttributeName());
        }

        public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            if (expression.Body is MemberExpression memberExpression)
                return GetPropertyName(memberExpression);

            //this is needed for numeric fields
            if (expression.Body is UnaryExpression body) return (body.Operand as MemberExpression)?.Member.Name;

            throw new InvalidPropertyExpressionException("Could not get property name from expression.");
        }

        public static string GetPropertyName<T, K>(this Expression<Func<T, K>> expression) where K : new()
        {
            if (expression.Body is MemberExpression memberExpression)
                return GetPropertyName(memberExpression);

            if (expression.Body is MethodCallExpression methodCallExpression)
            {
                memberExpression = methodCallExpression.GetParentMemberNameForArray();
                return GetPropertyName(memberExpression);
            }

            throw new InvalidPropertyExpressionException("Could not get property name from expression.");
        }

        public static string GetPropertyName(this MemberExpression memberExpression)
        {
            var name = memberExpression.Member.Name;

            var parentExpression = memberExpression.Expression as MemberExpression;
            parentExpression ??= GetParentMemberNameForArray(memberExpression);
            while (parentExpression != null)
            {
                name = $"{parentExpression.Member.Name}.{name}";
                parentExpression = parentExpression.Expression as MemberExpression;
            }

            return name;
        }

        public static T? Element<T>(this IEnumerable<T> enumerable)
        {
            return default(T);
        }

        public static MemberExpression? GetParentMemberNameForArray(MemberExpression memberExpression)
        {
            if (memberExpression.Expression is MethodCallExpression methodCallExpression)
                return methodCallExpression.GetParentMemberNameForArray();
            return null;
        }

        public static MemberExpression? GetParentMemberNameForArray(this MethodCallExpression methodCallExpression)
        {
            if (methodCallExpression.Method.DeclaringType == typeof(ExtensionMethods) &&
                methodCallExpression.Method.Name == nameof(Element))
                return methodCallExpression.Arguments[0] as MemberExpression;

            return null;
        }

        public static IEnumerable<string> GetPropertyNames(this Type type, IEnumerable<string> exclusionList)
            => type.GetPropertyNames().Where(a => !exclusionList.Contains(a)).ToList();

        public static IEnumerable<string> GetPropertyNames(this Type type)
            => type.GetProperties(BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.Public).Select(a => a.Name).ToList();

        public static IEnumerable<string> GetDistinct(this IEnumerable<string> newNames, IEnumerable<string>? existing)
        {
            var list = newNames.ToList();
            if (existing == null || !existing.Any()) return list.Distinct();

            list.AddRange(existing);

            return list.Distinct();
        }


        public static T[] MergeParameters<T>(this T item, params T[]? items)
        {
            var list = new List<T> { item };
            if (items == null) return list.ToArray();

            list.AddRange(items);
            return list.ToArray();
        }
    }

    public class InvalidPropertyExpressionException : Exception
    {
        public InvalidPropertyExpressionException(string message) : base(message)
        {

        }
    }
}
