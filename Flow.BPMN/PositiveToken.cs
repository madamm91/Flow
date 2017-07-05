using System.Collections.Generic;
using Flow.Interfaces;

namespace Flow.BPMN
{
    public class PositiveToken : IToken
    {
        public static void PutNew(IExecutableElement element)
        {
            element.Token = new PositiveToken();
        }

        public void Put(IEnumerable<IExecutableElement> to)
        {
            foreach (var item in to)
            {
                PutNew(item);
            }
        }

        public void Put(IExecutableElement to)
        {
            PutNew(to);
        }
    }
}