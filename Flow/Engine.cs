using Flow.Interfaces;
using Flow.Engine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Flow.Engine
{
    public class Engine
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        private Queue<IExecutableElement> Queue { get; } = new Queue<IExecutableElement>();

        public void Run(IExecutableElement element)
        {
            Queue.Enqueue(element);
            Run();
        }

        public void Run(IProcess process)
        {
            if (process == null) throw new ArgumentNullException(nameof(process));
            if (process.DefaultEntryPoint == null)
            {
                throw new ProcessEntryPointMissingException(process);
            }
            Run(process.DefaultEntryPoint);
        }

        private void Run()
        {
            while (Queue.Any())
            {
                var element = Queue.Dequeue();
                var result = ExecuteIt(element).ToArray();
                element.Token.Put(result);
                Queue.Enqueue(result);
            }
        }

        protected virtual IEnumerable<IExecutableElement> ExecuteIt(IExecutableElement element)
        {
            return element.Execute();
        }

        protected virtual IEnumerable<IExecutableElement> DoneIt(IDoneableElement doneable)
        {
            return doneable.Done();
        }

        public void Done(IDoneableElement doneable)
        {
            var done = DoneIt(doneable).ToArray();
            doneable.Token.Put(done);
            Queue.Enqueue(done);
            Run();
        }
    }
}