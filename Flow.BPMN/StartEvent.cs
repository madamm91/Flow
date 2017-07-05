using Flow.Interfaces;
using System.Collections.Generic;

namespace Flow.BPMN
{
    public class StartEvent : BaseObject, IEntryPoint
    {
        public override IEnumerable<IExecutableElement> Execute()
        {
            return DestinationElements;
        }

    }
}