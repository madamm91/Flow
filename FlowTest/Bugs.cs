using Flow.BPMN.Reader;
using Flow.Language;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FlowTest
{
    [TestClass]
    public class Bugs
    {
        [TestMethod]
        public void Bug1()
        {
            string path = TestExtensions.File("empty.bpmn");
            var file = new BpmnFileReader(new SimpleExpressionConverter());
            var process = file.Load(path);
            var definitions = file.Definitions;
            Assert.AreNotEqual(null, definitions.Process.Objects);
        }
    }
}
