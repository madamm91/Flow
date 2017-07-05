using System.Collections.Generic;

namespace Flow.Interfaces
{
    public interface IToken
    {
        void Put(IEnumerable<IExecutableElement> to);
        void Put(IExecutableElement to);
    }
}