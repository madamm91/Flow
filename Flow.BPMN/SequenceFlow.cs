using System;
using System.Collections.Generic;
using Flow.Interfaces;

namespace Flow.BPMN
{
    public class SequenceFlow : BaseObject, IEvaluable
    {
        public Func<bool> Expression { get; set; } = () => true;
     
        public SequenceFlow(IExecutableElement from, IExecutableElement to)
        {
            SourceElements.Add(from);
            DestinationElements.Add(to);
        }

        public SequenceFlow() { }

        public override IEnumerable<IExecutableElement> Execute()
        {
            return DestinationElements;
        }
    }
}