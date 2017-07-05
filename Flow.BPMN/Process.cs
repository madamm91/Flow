using Flow.Interfaces;
using System.Collections.Generic;

namespace Flow.BPMN
{
    public class Process : BaseObject, IProcess
    {
        private IExecutableElement defaultEntryPoint;
        public IExecutableElement DefaultEntryPoint
        {
            get
            {
                return defaultEntryPoint;
            }
            set
            {
                defaultEntryPoint = value;
                if (defaultEntryPoint != null)
                {
                    PositiveToken.PutNew(defaultEntryPoint);
                }
            }
        }
        public List<IExecutableElement> Elements { get; protected set; } = new List<IExecutableElement>();

        public void Add(IExecutableElement element)
        {
            Elements.Add(element);
        }

        public void AddRange(IEnumerable<IExecutableElement> transformed)
        {
            Elements.AddRange(transformed);
        }

        public override IEnumerable<IExecutableElement> Execute()
        {
            yield return DefaultEntryPoint;
        }
    }
}