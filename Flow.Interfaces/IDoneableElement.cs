using System.Collections.Generic;

namespace Flow.Interfaces
{
    public interface IDoneableElement : IHasToken
    {
        IEnumerable<IExecutableElement> Done();
    }
}