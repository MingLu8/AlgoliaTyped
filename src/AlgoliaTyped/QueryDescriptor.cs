using Algolia.Search.Models.Search;
using System.Linq.Expressions;
using System.Reflection;

namespace AlgoliaTyped
{
    public class QueryDescriptor<T> : IQueryDescriptor<T> where T : class
    {
        private readonly Query _query = new Query();

        public virtual Query GetQuery()
        {
            return new Query
            {
                SearchQuery = _query.SearchQuery,
                Facets = _query.Facets?.ToList(),
                Filters = _query.Filters,
                Page = _query.Page,
                HitsPerPage = _query.HitsPerPage,
                RestrictSearchableAttributes = _query.RestrictSearchableAttributes?.ToList(),
            };
        }

        /// <summary>
        /// Supported operators are '==', '!=', '&&', '||', '>', '<', '>=', '<='
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="combineExistingFilterWithOrOperator"></param>
        /// <returns></returns>
        /// <exception cref="InvalidSearchExpressionException"></exception>
        /// <exception cref="InvalidFilterExpressionException"></exception>
        public virtual IQueryDescriptor<T> Filters(Expression<Func<T, object>> expression, bool combineExistingFilterWithOrOperator = false)
        {
            var expressionBody = expression.Body as UnaryExpression;
            if (expressionBody == null) throw new InvalidSearchExpressionException(expression);

            if (expressionBody.Operand is not BinaryExpression operand)
                throw new InvalidFilterExpressionException("only binary expressions are supported.");

            var filter = GetFilter(operand);
            if (_query.Filters == null)
                _query.Filters = filter;
            else
            {
                var op = combineExistingFilterWithOrOperator ? Operators.Or : Operators.And;
                _query.Filters += $" {op} {filter}";
            }

            return this;
        }

        public virtual IQueryDescriptor<T> SearchQuery(string settingValue)
        {
            _query.SearchQuery = settingValue;
            return this;
        }

        public virtual IQueryDescriptor<T> Page(int pageNo)
        {
            _query.Page = pageNo;
            return this;
        }

        public virtual IQueryDescriptor<T> HitsPerPage(int itemCount)
        {
            _query.HitsPerPage = itemCount;
            return this;
        }

        public IQueryDescriptor<T> Facets(IEnumerable<string> settingValue)
        {
            if (!settingValue.Any()) return this;

            if (_query.Facets == null)
                _query.Facets = settingValue;
            else
            {
                var list = settingValue.ToList();
                list.AddRange(_query.Facets);
                _query.Facets = list;
            }

            return this;
        }

        public virtual IQueryDescriptor<T> Facets(params Expression<Func<T, object>>[] propertyExpressions)
        {
            if (propertyExpressions.Length == 0) return this;

            var attributes = propertyExpressions.Select(a => a.GetPropertyName().ToAttributeName()).ToList();
            if (_query.Facets == null)
                _query.Facets = attributes;
            else
            {
                attributes.AddRange(_query.Facets);
                _query.Facets = attributes;
            }

            return this;
        }

        public virtual IQueryDescriptor<T> AllFacets()
        {
            _query.Facets = new[] { "*" };
            return this;
        }

        public virtual IQueryDescriptor<T> RestrictSearchableAttributes(params Expression<Func<T, object>>[] attributes)
        {
            var names = attributes.ToList().Select(a => a.GetPropertyName().ToAttributeName()).ToList();
            if (_query.RestrictSearchableAttributes == null)
                _query.RestrictSearchableAttributes = names;
            else
                names.ForEach(a =>
                {
                    if (!_query.RestrictSearchableAttributes.Contains(a))
                        _query.RestrictSearchableAttributes.Add(a);
                });

            return this;
        }

        private string GetFilter(BinaryExpression operand, bool addGrouping = false)
        {
            if (operand == null) throw new InvalidSearchExpressionException(operand);

            var nodeType = operand.NodeType;
            if (IsEqualityNode(operand.NodeType))
                return GetEqualityFilter(operand);

            if (!IsAndOrNode(operand.NodeType)) throw new InvalidSearchExpressionException(operand);

            var leftOperand = operand.Left as BinaryExpression;
            var rightOperand = operand.Right as BinaryExpression;

            var op = nodeType.GetOperator();
            var leftFilter = GetFilter(leftOperand, true);
            var rightFilter = GetFilter(rightOperand, true);

            return addGrouping ? $"({leftFilter} {op} {rightFilter})" : $"{leftFilter} {op} {rightFilter}";
        }

        private string GetEqualityFilter(BinaryExpression operand)
        {
            MemberExpression? memberExpression = null;
            if (operand.Left is MethodCallExpression methodCallExpression)
                memberExpression = methodCallExpression.GetParentMemberNameForArray();

            memberExpression ??= (MemberExpression)operand.Left;
            var propType = ((PropertyInfo)memberExpression.Member).PropertyType;

            var constant = (ConstantExpression)operand.Right;
            var value = propType == typeof(string) ? $"'{constant.Value}'" : $"{constant.Value}";
            var attributeName = memberExpression.GetPropertyName().ToAttributeName();

            var op = operand.NodeType.GetOperator();
            if (operand.NodeType == ExpressionType.NotEqual)
                return $"({op} {attributeName}{Operators.Equal}{value})";

            return $"({attributeName}{op}{value})";
        }

        private bool IsEqualityNode(ExpressionType nodeType) =>
           nodeType == ExpressionType.Equal ||
           nodeType == ExpressionType.NotEqual ||
           nodeType == ExpressionType.GreaterThan ||
           nodeType == ExpressionType.GreaterThanOrEqual ||
           nodeType == ExpressionType.LessThan ||
           nodeType == ExpressionType.LessThanOrEqual;

        private bool IsAndOrNode(ExpressionType nodeType) => nodeType == ExpressionType.AndAlso || nodeType == ExpressionType.OrElse;
    }
}