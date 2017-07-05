using Flow.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace Flow.BPMN
{
    public class ExclusiveGateway : Gateway
    {
        protected override bool Merge()
        {
            return true;
        }

        protected override IEnumerable<IExecutableElement> Split()
        {
            var evaluatedTrue = from s in DestinationElements
                                where s is IEvaluable
                                let evaluable = (IEvaluable)s
                                where evaluable.Expression()
                                select s;
            //to avoid multiple enumeration
            var evaluatedTrueArray = evaluatedTrue.ToArray();
            if (evaluatedTrueArray.Length != 1) throw new NonExclusiveConditionsException();

            var selected = evaluatedTrueArray[0];
            yield return selected;

        }
    }
}