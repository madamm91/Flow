using Flow.Interfaces;
using System;

namespace Flow.Engine
{
    public class ProcessEntryPointMissingException : Exception
    {
        // ReSharper disable once UseStringInterpolation
        public ProcessEntryPointMissingException(IProcess process) : base(string.Format("Process {0} has no entry point.", process.Id))
        {
        }
    }
}
