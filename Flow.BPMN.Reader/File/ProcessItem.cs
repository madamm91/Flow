using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Flow.BPMN.Reader.File
{
    public class ProcessItem
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("sequenceFlow", typeof(SequenceFlowItem))]
        [XmlElement("startEvent", typeof(StartEventItem))]
        [XmlElement("endEvent", typeof(EndEventItem))]
        [XmlElement("task", typeof(TaskItem))]
        [XmlElement("parallelGateway", typeof(ParallelGatewayItem))]
        [XmlElement("exclusiveGateway", typeof(ExclusiveGatewayItem))]
        [XmlElement("complexGateway", typeof(ComplexGatewayItem))]
        public List<BaseObjectItem> Objects { get; set; }

        public BaseObjectItem Get(string id)
        {
            var objects = from o in Objects
                          where o.Id == id
                          select o;

            return objects.SingleOrDefault();
        }

        internal void SetChildrensProcessItem()
        {
            foreach (var item in Objects)
            {
                item.Process = this;
            }
        }

        public T Get<T>(string id) where T : class
        {
            var got = Get(id);
            return got as T;
        }
    }



}
