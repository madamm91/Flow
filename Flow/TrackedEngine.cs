using Flow.Interfaces;
using System.Collections.Generic;
using System.Diagnostics;

namespace Flow.Engine
{
    public class TrackedEngine : Engine
    {
        public List<IExecutableElement> ExecutedList { get; } = new List<IExecutableElement>();
        public List<IDoneableElement> DoneList { get; } = new List<IDoneableElement>();

        protected override IEnumerable<IExecutableElement> ExecuteIt(IExecutableElement element)
        {
            var result = base.ExecuteIt(element);
            ExecutedList.Add(element);
            
            // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
            if (!string.IsNullOrEmpty(element.Id))
            {
                Debug.WriteLine($"An instance of {element.GetType()} with id '{element.Id}' was executed.");
            }
            else
            {
                Debug.WriteLine($"An instance of {element.GetType()} without id was executed.");
            }
            return result;
        }

        protected override IEnumerable<IExecutableElement> DoneIt(IDoneableElement doneable)
        {
            var result = base.DoneIt(doneable);
            DoneList.Add(doneable);
            Debug.WriteLine($"An instance of {doneable.GetType()} is done.");
            return result;
        }
    }
}