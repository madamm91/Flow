using Microsoft.VisualStudio.TestTools.UnitTesting;
using Flow.BPMN.Reader;
using System.Linq;
using Flow.BPMN.Reader.File;
using Flow.BPMN;
using Flow.Language;

namespace FlowTest
{
    [TestClass]
    public class BpmnFileTests
    {

        [TestMethod]
        public void LoadSimple()
        {
            string path = TestExtensions.File("simple.bpmn");
            var file = new BpmnFileReader(new SimpleExpressionConverter());
            var process = file.Load(path);
            var definitions = file.Definitions;

            ProcessExists(definitions, pid: "sid-e138ad92-53db-4474-a117-cf3a5074182e");

            Assert.AreNotEqual(null, definitions.Process.Objects);
            Assert.IsTrue(definitions.Process.Objects.Any());


            var start = definitions.Process.Get<StartEventItem>("sid-477D1DF3-C5FC-460F-8AD1-03D4B7C26FB6");
            var end = definitions.Process.Get<EndEventItem>("sid-08B606A8-2F7C-4DFD-BEA8-A0F4694AA576");
            var flow = definitions.Process.Get<SequenceFlowItem>("sid-6FD4FFD3-5784-4D33-9509-234EAB886930");

            Assert.AreNotEqual(null, flow);
            Assert.AreNotEqual(null, start);
            Assert.AreNotEqual(null, end);

            Assert.AreEqual("sid-6FD4FFD3-5784-4D33-9509-234EAB886930", start.OutgoingFlows.First());
            Assert.AreEqual(0, start.IncomingFlows.Count);

            Assert.AreEqual("sid-6FD4FFD3-5784-4D33-9509-234EAB886930", end.IncomingFlows.First());
            Assert.AreEqual(0, end.OutgoingFlows.Count);

            Assert.AreEqual(1, flow.IncomingFlows.Count);
            Assert.AreEqual(1, flow.OutgoingFlows.Count);
        }

        private static void ProcessExists(Definitions definitions, string pid)
        {
            Assert.AreNotEqual(null, definitions);
            Assert.AreNotEqual(null, definitions.Process);
            Assert.AreEqual(pid, definitions.Process.Id);
        }

        [TestMethod]
        [Timeout(260)]
        public void LoadSimpleWithTaskAndTransformAndRun()
        {
            string path = TestExtensions.File("simpleWithTask.bpmn");
            var file = new BpmnFileReader(new SimpleExpressionConverter());
            var process = file.Load(path);

            var startEvent = process.Elements.OfType<StartEvent>().Single();
            var flows = process.Elements.OfType<SequenceFlow>().ToArray();
            var task = process.Elements.OfType<Task>().Single();
            var endEvent = process.Elements.OfType<EndEvent>().Single();

            Assert.AreEqual(2, flows.Length);

            startEvent.IsNotNull();
            task.IsNotNull();
            flows[0].IsNotNull();
            flows[1].IsNotNull();
            endEvent.IsNotNull();

            var engine = new Flow.Engine.TrackedEngine();
            engine.Run(process);
            engine.ExecutedList.ShouldBe(startEvent, flows[0], task);
            engine.Done(task);
            engine.DoneList.ShouldBe(task);
            engine.ExecutedList.ShouldBe(startEvent, flows[0], task, flows[1], endEvent);

        }

        [TestMethod]
        [Timeout(500)]
        public void TransformSimple()
        {
            string path = TestExtensions.File("simple.bpmn");
            var file = new BpmnFileReader(new SimpleExpressionConverter());
            var process = file.Load(path);

            var startEvent = process.Elements.OfType<StartEvent>().Single();
            var sequenceFlow = process.Elements.OfType<SequenceFlow>().Single();
            var endEvent = process.Elements.OfType<EndEvent>().Single();

            startEvent.IsNotNull();
            sequenceFlow.IsNotNull();
            endEvent.IsNotNull();

            var engine = new Flow.Engine.TrackedEngine();
            engine.Run(process);
            engine.ExecutedList.ShouldBe(startEvent, sequenceFlow, endEvent);
        }

        [TestMethod]
        public void ParallelSplit()
        {
            string path = TestExtensions.File("parallelSplit.bpmn");
            var file = new BpmnFileReader(new SimpleExpressionConverter());
            var process = file.Load(path);

            var startEvent = process.Elements.OfType<StartEvent>().Single();
            var endEvent1 = process.Elements.OfType<EndEvent>().First();
            var endEvent2 = process.Elements.OfType<EndEvent>().Last();
            var flows = process.Elements.OfType<SequenceFlow>().ToArray();
            Assert.AreNotEqual(endEvent1, endEvent2);
            Assert.AreEqual(3, flows.Length);
            var gw = process.Elements.OfType<ParallelGateway>().Single();
            Assert.AreNotEqual(null, gw);
            startEvent.IsNotNull();
            endEvent1.IsNotNull();
            endEvent2.IsNotNull();
            gw.IsNotNull();

            Assert.AreEqual(gw, flows[0].DestinationElements.Single());
            Assert.AreEqual(gw, flows[1].SourceElements.Single());
            Assert.AreEqual(gw, flows[2].SourceElements.Single());

            var engine = new Flow.Engine.TrackedEngine();
            engine.Run(process);
            engine.ExecutedList.ShouldContainAll(process.Elements.ToArray());

        }

        [TestMethod]
        public void DoubleParallelGateway()
        {
            string path = TestExtensions.File("doubleParallelGateway.bpmn");
            var file = new BpmnFileReader(new SimpleExpressionConverter());
            var process = file.Load(path);
            var engine = new Flow.Engine.TrackedEngine();
            engine.Run(process);

            engine.ExecutedList.ShouldContainAll(process.Elements.ToArray());
            engine.DoneList.ShouldBe();
        }

        [TestMethod]
        public void ExclusiveGatewayWithExpression()
        {
            string path = TestExtensions.File("exclusiveGatewayWithExpression.bpmn");
            var file = new BpmnFileReader(new SimpleExpressionConverter());
            var process = file.Load(path);

            var startEvent = process.Elements.OfType<StartEvent>().Single();
            var endEvent1 = process.Elements.OfType<EndEvent>().First();
            var endEvent2 = process.Elements.OfType<EndEvent>().Last();
            var flows = process.Elements.OfType<SequenceFlow>().ToArray();
            var gw = process.Elements.OfType<ExclusiveGateway>().Single();

            Assert.AreEqual(flows[1].Expression(), true);

            startEvent.IsNotNull();
            endEvent1.IsNotNull();
            endEvent2.IsNotNull();
            gw.IsNotNull();
            Assert.AreNotEqual(endEvent1, endEvent2);


            var engine = new Flow.Engine.TrackedEngine();
            engine.Run(process);

            engine.ExecutedList.ShouldBe(startEvent, flows[0], gw, flows[1], endEvent1);
            var sb = TestExtensions.File("exclusiveGatewayWithExpression.executedList.shouldBe");
            engine.ExecutedList.ShouldBe(sb);

        }
    }
}
