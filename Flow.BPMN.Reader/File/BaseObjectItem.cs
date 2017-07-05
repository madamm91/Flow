using Flow.Interfaces;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Flow.BPMN.Reader.File
{
    public abstract class BaseObjectItem
    {
        internal abstract IExecutableElement Transform();

        internal ProcessItem Process { get; set; }

        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlElement("incoming")]
        public List<string> IncomingFlows { get; set; }

        [XmlElement("outgoing")]
        public List<string> OutgoingFlows { get; set; }

    }
}