using Flow.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Flow.BPMN
{
    public abstract class Gateway : BaseObject
    {
        protected abstract bool Merge();
        protected abstract IEnumerable<IExecutableElement> Split();
        public override IEnumerable<IExecutableElement> Execute()
        {
            if (Merge())
            {
                return Split();
            }
            return Enumerable.Empty<IExecutableElement>();
        }
    }
}
