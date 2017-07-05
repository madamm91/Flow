using Flow.Interfaces;

namespace Flow.BPMN.Reader.File
{
    public class TaskItem : BaseObjectItem
    {
        internal override IExecutableElement Transform()
        {
            return new Task()
            {
                Id = Id,
                Name = Name,
            }; 
        }
    }
}
