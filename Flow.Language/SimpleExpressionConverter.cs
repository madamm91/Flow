using System;
using Flow.Interfaces;

namespace Flow.Language
{
    public class SimpleExpressionConverter : IEvaluableExpressionConverter
    {
        public Func<bool> Convert(object expression)
        {
            if (expression == null) throw new ArgumentNullException(nameof(expression));
            var expressionString = (string)expression;
            if (expressionString == "true") return () => true;
            return () => false;
        }

    }
}
