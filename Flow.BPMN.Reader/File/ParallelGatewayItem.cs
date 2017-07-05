using Flow.Interfaces;

namespace Flow.BPMN.Reader.File
{
    public class ParallelGatewayItem : GatewayItem
    {

        internal override IExecutableElement Transform()
        { 
                var newItem = new ParallelGateway()
                {
                    Id = Id,
                    Name = Name,
                };
                return newItem;
        }
    }
}
