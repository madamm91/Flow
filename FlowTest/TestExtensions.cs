using Flow.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace FlowTest
{
    public static class TestExtensions
    {
        public static string TestFolder { get; set; } = @"..\..\TestData\";
        public static string AutoTestFolder { get; internal set; } = @"..\..\AutoTestData\";
        public static string ElementsFolder { get; internal set; } = @"..\..\Elements\";

        public static string File(string filename)
        {
            return System.IO.Path.Combine(TestFolder, filename);
        }

        public static void ShouldBe<T>(this List<T> l1, params object[] l2)
        {
            Assert.AreEqual(l2.Length, l1.Count);
            for (int i = 0; i < l1.Count; i++)
            {
                Assert.AreEqual(l2[i], l1[i]);
            }
        }

        public static void ShouldBe(this List<IExecutableElement> l1, string filename)
        {
            //filename = File(filename);
            Assert.IsTrue(System.IO.File.Exists(filename), $"The specified file doesn't exists! Filename: {filename}");
            Assert.AreNotEqual(l1, null, "The input list is null.");
            string[] ids = System.IO.File.ReadAllLines(filename);
            Assert.AreEqual(l1.Count, ids.Length, "The executed items count and the specified count are different.");
            for (int i = 0; i < ids.Length; i++)
            {
                Assert.AreEqual(ids[i], l1[i].Id);
            }
        }
        public static void ShouldContainAll(this List<IExecutableElement> l1, string filename)
        {
            Assert.IsTrue(System.IO.File.Exists(filename), $"The specified file doesn't exists! Filename: {filename}");
            Assert.AreNotEqual(l1, null, "The input list is null.");
            string[] ids = System.IO.File.ReadAllLines(filename);
            foreach (var id in ids)
            {
                bool contains = l1.Any(x => x.Id == id);
                if(!contains) Assert.Fail("Should contain all elements, but {0} is not contained.", id);
            }

        }
        public static void ShouldContainAll<T>(this List<T> l1, params T[] l2)
        {
            foreach (var item in l2)
            {
                if (!l1.Contains(item)) Assert.Fail("Should contain all elements, but {0} is not contained.", item);
            }
        }

        public static T IsNotNull<T>(this T o)
        {
            Assert.IsNotNull(o);
            return o;
        }

        public static void EnsureAllHasTokens<T>(this List<T> list)
        {
            foreach (var item in list)
            {
                if ((item is IExecutableElement) && ((item as IExecutableElement).Token == null))
                {
                    Assert.Fail($"{item} has no tokens.");
                }
            }
        }
    }
}