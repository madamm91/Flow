using System;

namespace Flow.Interfaces
{
    public interface IEvaluable
    {
        Func<bool> Expression { get; set; }
    }
}
