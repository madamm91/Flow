using Flow.Interfaces;

namespace Flow.BPMN.Reader.File
{
    public class EndEventItem : BaseObjectItem
    {
        internal override IExecutableElement Transform()
        {
            return new EndEvent()
            {
                Id = Id,
                Name = Name,
            };
        }
    }
}
