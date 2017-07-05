using System.Linq;
using Flow.BPMN.Reader.File;
using System.Xml.Serialization;
using System.IO;
using Flow.Interfaces;
using System.Collections.Generic;

namespace Flow.BPMN.Reader
{
    public class BpmnFileReader
    {
        public Definitions Definitions { get; set; }

        private readonly IEvaluableExpressionConverter converter;

        public BpmnFileReader(IEvaluableExpressionConverter converter)
        {
            this.converter = converter;
        }

        //public BpmnFileReader() { }

        public Process Load(string path)
        {
            var xmlSerializer = new XmlSerializer(typeof(Definitions));
            StreamReader reader = new StreamReader(path);
            Definitions = (Definitions)xmlSerializer.Deserialize(reader);
            Definitions.Process.SetChildrensProcessItem();
            InjectConverter(Definitions.Process.Objects.OfType<IHasEvaluableExpressionConverter>());
            var process = Transform(Definitions.Process);
            return process;
        }

        private void InjectConverter(IEnumerable<IHasEvaluableExpressionConverter> toInject)
        {
            foreach (var item in toInject)
            {
                item.Converter = converter;
            }
        }

        private readonly Dictionary<BaseObjectItem, IExecutableElement> cache = new Dictionary<BaseObjectItem, IExecutableElement>();

        private IExecutableElement GetTransformed(BaseObjectItem item)
        {
            if (cache.ContainsKey(item))
            {
                return cache[item];
            }
            else
            {
                var transformedItem = item.Transform();
                cache.Add(item, transformedItem);
                return transformedItem;
            }
        }

        public Process Transform(ProcessItem process)
        {
            var result = new Process();
            foreach (var item in process.Objects)
            {

                var transformedItem = GetTransformed(item);
                //extract:

                if (transformedItem is IEntryPoint)
                {
                    result.DefaultEntryPoint = transformedItem;
                }

                result.Add(transformedItem);

                foreach (string outgoingId in item.OutgoingFlows)
                {
                    var outgoingItem = process.Get(outgoingId);
                    transformedItem.DestinationElements.Add(GetTransformed(outgoingItem));
                }


                foreach (string incomingId in item.IncomingFlows)
                {
                    var incomingItem = process.Get(incomingId);
                    transformedItem.SourceElements.Add(GetTransformed(incomingItem));
                }


            }
            return result;
        }
    }
}
