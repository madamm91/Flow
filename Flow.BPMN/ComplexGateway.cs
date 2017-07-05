using System;
using System.Collections.Generic;
using System.Linq;
using Flow.Interfaces;

namespace Flow.BPMN
{
    public class ComplexGateway : Gateway
    {
        public Func<bool> MergeExpression { get; set; }

        protected override bool Merge()
        {
            return MergeExpression();
        }

        protected override IEnumerable<IExecutableElement> Split()
        {
            var evaluatedTrue = from s in DestinationElements
                                where s is IEvaluable
                                where ((IEvaluable)s).Expression()
                                select s;
            //to avoid multiple enumeration
            var evaluatedTrueArray = evaluatedTrue.ToArray();
            foreach (var selected in evaluatedTrueArray)
            {
                yield return selected;
            }


        }
    }
}
