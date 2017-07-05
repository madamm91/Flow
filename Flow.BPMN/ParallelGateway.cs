using System.Collections.Generic;
using System.Linq;
using Flow.Interfaces;

namespace Flow.BPMN
{
    public class ParallelGateway : Gateway
    {
        protected override bool Merge()
        {
            bool allIncomingHasToken = SourceElements.All(x => x.Token is PositiveToken);
            if (allIncomingHasToken)
            {
                foreach (var source in SourceElements)
                {
                    source.Token = null;
                }
                return true;
            }
            return false;
        }

        protected override IEnumerable<IExecutableElement> Split()
        {
            return DestinationElements;
        }

    }
}
