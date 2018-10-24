
using NUnit.Framework;
namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net.Mime;
    using System.Reflection;

    [TestFixture]
    public class HikerTest
    {
        [Test]
        public void CanGetFileName()
        {
            var fileNameWithPath = "foobar.txt";
            var converter = new UnicodeFileToHtmlTextConverter(fileNameWithPath, new TestTextReader(new List<string>{"whatever"}));
            Assert.AreEqual(fileNameWithPath, converter.GetFilename());
        }

        [Test]
        public void CanConvertEmptyTextFileToHtml()
        {

            var converter = new UnicodeFileToHtmlTextConverter("doesn't matter", new TestTextReader(new List<string>{""}));
            Assert.AreEqual(string.Empty, converter.ConvertToHtml());
        }

        [Test]
        public void ReplacesSpecialCharacters()
        {
            var converter = new UnicodeFileToHtmlTextConverter("doesn't matter", new TestTextReader(new List<string> { "<>&\\\"\\\'" }));
            Assert.AreEqual("&lt;&gt;&amp;&quot;&quot;<br />", converter.ConvertToHtml());
        }

        [Test]
        public void DoesNotReplaceRegularText()
        {
            var converter = new UnicodeFileToHtmlTextConverter("doesn't matter", new TestTextReader(new List<string> { "regulartext" }));
            Assert.AreEqual("regulartext<br />", converter.ConvertToHtml());
        }

        [Test]
        public void MixOfSpecialCharactersAndRegularText()
        {
            var converter = new UnicodeFileToHtmlTextConverter("doesn't matter", new TestTextReader(new List<string> { "<a href=\"somelink\">Go here</>" }));
            Assert.AreEqual("&lt;a href=\"somelink\"&gt;Go here&lt;/&gt;<br />", converter.ConvertToHtml());
        }

        [Test]
        public void MultipleLines()
        {
            var converter = new UnicodeFileToHtmlTextConverter("doesn't matter", new TestTextReader(new List<string> { "<a href=\"somelink\">Go here</>", "<a href=\"somelink\">Go here</>" }));
            Assert.AreEqual("&lt;a href=\"somelink\"&gt;Go here&lt;/&gt;<br />&lt;a href=\"somelink\"&gt;Go here&lt;/&gt;<br />", converter.ConvertToHtml());
        }
    }

    public class TestTextReader : ITextReader
    {
        private readonly List<string> _value;

        public TestTextReader(List<string> value)
        {
            _value = value;
        }

        public void Dispose()
        {
            
        }

        public string ReadLine()
        {
            string toReturn = null;

            if (_value.Count(v => !string.IsNullOrWhiteSpace(v)) != 0)
            {
                toReturn = _value.First();
                _value.RemoveAt(0);
            }

            return toReturn;
        }
    }
}
