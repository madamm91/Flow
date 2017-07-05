using Flow.Interfaces;

namespace Flow.BPMN.Reader.File
{
    public class StartEventItem : BaseObjectItem
    {

        internal override IExecutableElement Transform()
        {
            var newItem = new StartEvent()
            {
                Id = Id,
                Name = Name,
            };
            return newItem;
        }
    }
}
