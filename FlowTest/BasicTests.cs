using Flow.BPMN;
using Flow.Engine;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace FlowTest
{
    [TestClass]
    public class BasicTests
    {
        [TestMethod]
        public void EntryPointTest()
        {
            var engine = new TrackedEngine();
            var process = new Process();
            var startEvent = new StartEvent();
            process.Add(startEvent);
            process.DefaultEntryPoint = startEvent;

            engine.Run(process);

            engine.ExecutedList.ShouldBe(startEvent);

            process.Elements.EnsureAllHasTokens();
        }

        [TestMethod]
        public void Hello()
        {
            var engine = new TrackedEngine();
            var process = new Process();
            var startEvent = new StartEvent();
            PositiveToken.PutNew(startEvent);

            var endEvent = new EndEvent();
            var flow = new SequenceFlow(startEvent, endEvent);
            startEvent.DestinationElements.Add(flow);
            endEvent.SourceElements.Add(flow);
            process.Add(startEvent);
            process.Add(endEvent);

            engine.Run(startEvent);

            engine.ExecutedList.ShouldBe(startEvent, flow, endEvent);
            process.Elements.EnsureAllHasTokens();
        }

        [TestMethod]
        public void Task()
        {
            var engine = new TrackedEngine();
            var process = new Process();

            var startEvent = new StartEvent();
            PositiveToken.PutNew(startEvent);

            var task = new Task();
            var endEvent = new EndEvent();
            var flow1 = new SequenceFlow(startEvent, task);
            var flow2 = new SequenceFlow(task, endEvent);
            startEvent.DestinationElements.Add(flow1);
            task.SourceElements.Add(flow1);
            task.DestinationElements.Add(flow2);
            endEvent.SourceElements.Add(flow2);

            process.Add(startEvent);
            process.Add(task);
            process.Add(endEvent);

            engine.Run(startEvent);

            Console.WriteLine("Wait here!");
            engine.Done(task);

            engine.ExecutedList.ShouldBe(startEvent, flow1, task, flow2, endEvent);
            engine.DoneList.ShouldBe(task);
            process.Elements.EnsureAllHasTokens();
        }

        [TestMethod]
        [ExpectedException(typeof(ProcessEntryPointMissingException))]
        public void NoEntryPoint()
        {
            var engine = new TrackedEngine();
            var process = new Process()
            {
                Id = Guid.NewGuid().ToString(),
            };
            engine.Run(process);
        }

        [TestMethod]
        public void TaskDoneOnDifferentEngine()
        {
            var engine = new TrackedEngine();
            var oldEngineId = engine.Id;
            var process = new Process();

            var startEvent = new StartEvent();
            PositiveToken.PutNew(startEvent);

            var task = new Task();
            var endEvent = new EndEvent();
            var flow1 = new SequenceFlow(startEvent, task);
            var flow2 = new SequenceFlow(task, endEvent);
            startEvent.DestinationElements.Add(flow1);
            task.SourceElements.Add(flow1);
            task.DestinationElements.Add(flow2);
            endEvent.SourceElements.Add(flow2);

            process.Add(startEvent);
            process.Add(task);
            process.Add(endEvent);

            engine.Run(startEvent);

            Console.WriteLine("Wait here!");
            engine = new TrackedEngine();
            var newEngineId = engine.Id;
            engine.Done(task);

            engine.ExecutedList.ShouldBe(flow2, endEvent);
            engine.DoneList.ShouldBe(task);
            Assert.AreNotEqual(oldEngineId, newEngineId);
            process.Elements.EnsureAllHasTokens();
        }

        [TestMethod]
        public void ExclusiveGatewayTest()
        {
            var engine = new TrackedEngine();
            var process = new Process();

            var startEvent = new StartEvent();
            PositiveToken.PutNew(startEvent);
            process.Add(startEvent);

            var gateway = new ExclusiveGateway();

            process.Add(gateway);

            var endEvent1 = new EndEvent();
            process.Add(endEvent1);

            var endEvent2 = new EndEvent();
            process.Add(endEvent2);

            var flow1 = new SequenceFlow(startEvent, gateway);
            flow1.Name = nameof(flow1);
            startEvent.DestinationElements.Add(flow1);

            var flow2 = new SequenceFlow(gateway, endEvent1);
            flow2.Name = nameof(flow2);
            flow2.Expression = () => true;
            gateway.DestinationElements.Add(flow2);

            var flow3 = new SequenceFlow(gateway, endEvent2);
            flow3.Name = nameof(flow3);
            flow3.Expression = () => false;
            gateway.DestinationElements.Add(flow3);

            //process.Elements.InjectEvaluator(new SimpleExpressionEvaluator());

            engine.Run(startEvent);

            engine.ExecutedList.ShouldBe(startEvent, flow1, gateway, flow2, endEvent1);
            engine.DoneList.ShouldBe();
            Assert.AreEqual(null, endEvent2.Token);
            Assert.AreNotEqual(null, endEvent1.Token);
        }
    }
}