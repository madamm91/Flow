namespace Flow.Interfaces
{
    public interface IProcess : IExecutableElement
    {
        IExecutableElement DefaultEntryPoint { get; set; }
    }
}