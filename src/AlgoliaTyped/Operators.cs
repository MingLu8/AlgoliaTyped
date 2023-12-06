using System.Linq.Expressions;

namespace AlgoliaTyped
{
    public static class Operators
    {
        public const string Equal = ":";
        public const string NotEqual = "NOT";
        public const string GreaterThan = ">";
        public const string GreaterThanOrEqual = ">=";
        public const string LessThan = "<";
        public const string LessThanOrEqual = "<=";
        public const string And = "AND";
        public const string Or = "OR";

        public static string GetOperator(this ExpressionType opType)
        {
            switch (opType)
            {
                case ExpressionType.Equal: return Equal;
                case ExpressionType.NotEqual: return NotEqual;
                case ExpressionType.GreaterThan: return GreaterThan;
                case ExpressionType.GreaterThanOrEqual: return GreaterThanOrEqual;
                case ExpressionType.LessThan: return LessThan;
                case ExpressionType.LessThanOrEqual: return LessThanOrEqual;
                case ExpressionType.AndAlso: return And;
                case ExpressionType.OrElse: return Or;
                default: throw new InvalidSearchExpressionException();
            }
        }
    }
}