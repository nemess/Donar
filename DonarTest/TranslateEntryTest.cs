using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DonarTest
{
    [TestClass]
    public class TranslateEntryTest
    {
        [TestMethod]
        public void LinuxNoEmptyEnd()
        {
            string testText = "first\nsecond\n\nfourth";
            DonarDatabase.TranslateEntry entry = new DonarDatabase.TranslateEntry(testText);

            Assert.AreEqual(entry.Lines.Length, 4);
            Assert.AreEqual(entry.Paragraphs.Length, 3);

            Assert.AreEqual(entry.Lines[0], "first");
            Assert.AreEqual(entry.Lines[1], "second");
            Assert.AreEqual(entry.Lines[2], "");
            Assert.AreEqual(entry.Lines[3], "fourth");

            Assert.AreEqual(entry.Paragraphs[0], "first");
            Assert.AreEqual(entry.Paragraphs[1], "second");
            Assert.AreEqual(entry.Paragraphs[2], "fourth");

            Assert.AreEqual(DonarDatabase.TranslateEntry.ToString("\n", entry.Lines), testText);
        }

        [TestMethod]
        public void LinuxEmptyEnd()
        {
            string testText = "first\nsecond\n\nfourth\n";
            DonarDatabase.TranslateEntry entry = new DonarDatabase.TranslateEntry(testText);

            Assert.AreEqual(entry.Lines.Length, 5);
            Assert.AreEqual(entry.Paragraphs.Length, 3);

            Assert.AreEqual(entry.Lines[0], "first");
            Assert.AreEqual(entry.Lines[1], "second");
            Assert.AreEqual(entry.Lines[2], "");
            Assert.AreEqual(entry.Lines[3], "fourth");
            Assert.AreEqual(entry.Lines[4], "");

            Assert.AreEqual(entry.Paragraphs[0], "first");
            Assert.AreEqual(entry.Paragraphs[1], "second");
            Assert.AreEqual(entry.Paragraphs[2], "fourth");

            Assert.AreEqual(DonarDatabase.TranslateEntry.ToString("\n", entry.Lines), testText);
        }
        [TestMethod]
        public void WindowsNoEmptyEnd()
        {
            string testText = "first\r\nsecond\r\n\r\nfourth";
            DonarDatabase.TranslateEntry entry = new DonarDatabase.TranslateEntry(testText);

            Assert.AreEqual(entry.Lines.Length, 4);
            Assert.AreEqual(entry.Paragraphs.Length, 3);

            Assert.AreEqual(entry.Lines[0], "first");
            Assert.AreEqual(entry.Lines[1], "second");
            Assert.AreEqual(entry.Lines[2], "");
            Assert.AreEqual(entry.Lines[3], "fourth");

            Assert.AreEqual(entry.Paragraphs[0], "first");
            Assert.AreEqual(entry.Paragraphs[1], "second");
            Assert.AreEqual(entry.Paragraphs[2], "fourth");

            Assert.AreEqual(DonarDatabase.TranslateEntry.ToString("\r\n", entry.Lines), testText);
        }

        [TestMethod]
        public void WindowsEmptyEnd()
        {
            string testText = "first\r\nsecond\r\n\r\nfourth\r\n";
            DonarDatabase.TranslateEntry entry = new DonarDatabase.TranslateEntry(testText);

            Assert.AreEqual(entry.Lines.Length, 5);
            Assert.AreEqual(entry.Paragraphs.Length, 3);

            Assert.AreEqual(entry.Lines[0], "first");
            Assert.AreEqual(entry.Lines[1], "second");
            Assert.AreEqual(entry.Lines[2], "");
            Assert.AreEqual(entry.Lines[3], "fourth");
            Assert.AreEqual(entry.Lines[4], "");

            Assert.AreEqual(entry.Paragraphs[0], "first");
            Assert.AreEqual(entry.Paragraphs[1], "second");
            Assert.AreEqual(entry.Paragraphs[2], "fourth");

            Assert.AreEqual(DonarDatabase.TranslateEntry.ToString("\r\n", entry.Lines), testText);
        }
    }
}
