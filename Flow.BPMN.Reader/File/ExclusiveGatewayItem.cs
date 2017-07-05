using Flow.Interfaces;

namespace Flow.BPMN.Reader.File
{
    public class ExclusiveGatewayItem : GatewayItem
    {
        internal override IExecutableElement Transform()
        {
            return new ExclusiveGateway()
            {
                Id = Id,
                Name = Name,
            };
        }
    }
}
