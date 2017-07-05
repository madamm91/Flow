using Flow.Interfaces;
using System.Collections.Generic;

namespace Flow.BPMN
{
    public class Task : BaseObject, IDoneableElement
    {

        public IEnumerable<IExecutableElement> Done()
        {
            return DestinationElements;
        }

        public override IEnumerable<IExecutableElement> Execute()
        {
            yield break;
        }
    }
}