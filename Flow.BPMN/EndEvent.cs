using Flow.Interfaces;
using System.Collections.Generic;

namespace Flow.BPMN
{
    public class EndEvent : BaseObject
    {
        public override IEnumerable<IExecutableElement> Execute()
        {
            yield break;
        }
    }
}