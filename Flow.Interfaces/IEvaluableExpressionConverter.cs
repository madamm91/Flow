using System;

namespace Flow.Interfaces
{
    public interface IEvaluableExpressionConverter
    {
        Func<bool> Convert(object expression);
    }
}
