using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using Flow.BPMN;
using Flow.BPMN.Reader;
using Flow.Interfaces;
using Flow.Language;

namespace FlowTest
{
    [TestClass]
    public class AutomatedTests
    {
        [TestMethod]
        public void Run()
        {
            var files = Directory.EnumerateFiles(TestExtensions.AutoTestFolder, "*.bpmn", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                Console.WriteLine("File: {0}", file);
                var fileReader = new BpmnFileReader(new SimpleExpressionConverter());
                var process = fileReader.Load(file);

                var processElementsShouldContainAll = Path.ChangeExtension(file, ".processElements.shouldContainAll");
                if(File.Exists(processElementsShouldContainAll))
                {
                    ProcessElementsCheck(process, processElementsShouldContainAll);
                }

                var executedListShouldBe = Path.ChangeExtension(file, ".executedList.shouldBe");
                if (File.Exists(executedListShouldBe))
                {
                    ExecutedListCheck(process, executedListShouldBe);
                }

                Console.WriteLine();
            }
        }

        private static void ExecutedListCheck(IProcess process, string executedListShouldBe)
        {
            var engine = new Flow.Engine.TrackedEngine();
            engine.Run(process);
            Console.Write("Executed list check ");
            engine.ExecutedList.ShouldBe(executedListShouldBe);
            Console.WriteLine("OK!");
        }

        private static void ProcessElementsCheck(Process process, string processElementsShouldContainAll)
        {
            Console.Write("ProcessElements check ");
            process.Elements.ShouldContainAll(processElementsShouldContainAll);
            Console.WriteLine("OK!");
        }
    }
}
