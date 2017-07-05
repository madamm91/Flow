using System.Xml.Serialization;
using Flow.Interfaces;

namespace Flow.BPMN.Reader.File
{
    public class ComplexGatewayItem : GatewayItem, IHasEvaluableExpressionConverter
    {
        [XmlIgnore]
        public IEvaluableExpressionConverter Converter { get; set; }

        internal override IExecutableElement Transform()
        {
            return new ComplexGateway()
            {
                Id = Id,
                Name = Name,
                //TODO: Not from name
                MergeExpression = Converter.Convert(Name ?? string.Empty),
            };
        }
    }
}
