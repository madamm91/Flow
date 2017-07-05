using System.Collections.Generic;

namespace Flow.Interfaces
{
    public interface IExecutableElement : IHasToken
    {
        string Id { get; }
        IList<IExecutableElement> SourceElements { get; }

        IList<IExecutableElement> DestinationElements { get; }
        IEnumerable<IExecutableElement> Execute();
    }
}