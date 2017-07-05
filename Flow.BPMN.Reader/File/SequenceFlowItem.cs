using Flow.Interfaces;
using System.Xml.Serialization;

namespace Flow.BPMN.Reader.File
{
    public class SequenceFlowItem : BaseObjectItem, IHasEvaluableExpressionConverter
    {
        [XmlIgnore]
        public IEvaluableExpressionConverter Converter { get; set; }

        [XmlAttribute("sourceRef")]
        public string Source { get; set; }
        [XmlAttribute("targetRef")]
        public string Target { get; set; }

        internal override IExecutableElement Transform()
        {
            OutgoingFlows.Add(Target);
            IncomingFlows.Add(Source);
            var flow = new SequenceFlow
            {
                Id = Id,
                Name = Name,
                //TODO: maybe not from name...
                Expression = Converter.Convert(Name ?? string.Empty),
            };
            return flow;
        }

    }

}
