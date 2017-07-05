using System.Xml.Serialization;

namespace Flow.BPMN.Reader.File
{

    [XmlRoot("definitions", Namespace = Namespaces.Bpmn)]
    public class Definitions
    {
        [XmlElement("process")]
        public ProcessItem Process { get; set; }
    }
}
