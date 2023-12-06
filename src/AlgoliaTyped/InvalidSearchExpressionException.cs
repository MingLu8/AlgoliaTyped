using System.Linq.Expressions;

namespace AlgoliaTyped
{
    public class InvalidSearchExpressionException : Exception
    {
        public InvalidSearchExpressionException(Expression expression) : base($"Expression {expression} Operation not supported exception, only '&&', '||', '==' and '!=' are supported, ")
        {

        }

        public InvalidSearchExpressionException() : base($"Operation not supported exception, only '&&', '||', '==' and '!=' are supported, ")
        {

        }
    }
}