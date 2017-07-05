using Flow.BPMN.Reader;
using Flow.Language;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace FlowTest
{
    [TestClass]
    public class ElementTests
    {
        [TestMethod]
        public void Elements()
        {
            bool allElementsCanBeTransformed = true;
            var files = Directory.EnumerateFiles(TestExtensions.ElementsFolder).ToArray();

            var all = files.Length;
            int transformableCount = 0;

            foreach (var file in files)
            {
                var reader = new BpmnFileReader(new SimpleExpressionConverter());
                var process = reader.Load(file);

                var name = Path.GetFileName(file);
                var type = process?.Elements?.SingleOrDefault()?.GetType().ToString() ?? "X";
                var count = process?.Elements?.Count;
                Console.WriteLine($"{name}\t{type}\t{count}");
                if (count != 1)
                {
                    allElementsCanBeTransformed = false;
                }
                else
                {
                    transformableCount++;
                }
            }

            Console.WriteLine();
            Console.WriteLine("Transformable: {0:0.0%}\n{1}/{2}",
                (double)transformableCount / all, transformableCount, all);

            Assert.IsTrue(allElementsCanBeTransformed);
        }
    }
}
