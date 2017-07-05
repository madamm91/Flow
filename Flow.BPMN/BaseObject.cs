using Flow.Interfaces;
using System.Collections.Generic;

namespace Flow.BPMN
{
    public abstract class BaseObject : IExecutableElement
    {
        private readonly List<IExecutableElement> sourceElements = new List<IExecutableElement>();
        private readonly List<IExecutableElement> destinationElements = new List<IExecutableElement>();
        public IList<IExecutableElement> DestinationElements => destinationElements;

        public IList<IExecutableElement> SourceElements => sourceElements;

        public IToken Token { get; set; }

        public string Id { get; set; }
        public string Name { get; set; }

        public abstract IEnumerable<IExecutableElement> Execute();
    }
}
