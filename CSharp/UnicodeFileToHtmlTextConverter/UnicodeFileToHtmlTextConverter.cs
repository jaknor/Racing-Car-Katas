using System.IO;
using System.Web;

namespace TDDMicroExercises.UnicodeFileToHtmlTextConverter
{
    using System;
    using System.Collections.Generic;

    public class UnicodeFileToHtmlTextConverter
    {
        private readonly string _fullFilenameWithPath;
        private readonly ITextReader _textReader;
        private readonly List<IHttpUtility> _httpUtility;

        public UnicodeFileToHtmlTextConverter(string fullFilenameWithPath) : this(fullFilenameWithPath, new TextReader(fullFilenameWithPath))
        {
        }

        public UnicodeFileToHtmlTextConverter(string fullFilenameWithPath, ITextReader textReader) : this(fullFilenameWithPath, textReader, new List<IHttpUtility>() { new HttpUtility() })
        {
        }

        public UnicodeFileToHtmlTextConverter(string fullFilenameWithPath, ITextReader textReader, List<IHttpUtility> httpUtility)
        {
            _fullFilenameWithPath = fullFilenameWithPath;
            _textReader = textReader;
            _httpUtility = httpUtility;
        }

        public string GetFilename()
        {
            return _fullFilenameWithPath;
        }

        public string ConvertToHtml()
        {
            string html = string.Empty;

            string line = _textReader.ReadLine();
            while (line != null)
            {
                foreach (var httpUtility in _httpUtility)
                {
                    html += httpUtility.HtmlEncode(line);
                }
                
                html += "<br />";
                line = _textReader.ReadLine();
            }

            return html;
        }
    }

    public interface IHttpUtility
    {
        string HtmlEncode(string line);
    } 

    public class HttpUtility : IHttpUtility
    {
        public string HtmlEncode(string line)
        {
            line = line.Replace("&", "&amp;");
            line = line.Replace("<", "&lt;");
            line = line.Replace(">", "&gt;");
            line = line.Replace("\\\"", "&quot;");
            line = line.Replace("\\\'", "&quot;");
            return line;
        }
    }

    public interface ITextReader : IDisposable
    {
        string ReadLine();
    }

    class TextReader : ITextReader
    {
        private readonly StreamReader _textReader;

        public TextReader(string fullFilenameWithPath)
        {
            _textReader = File.OpenText(fullFilenameWithPath);
        }

        public string ReadLine()
        {
            return _textReader.ReadLine();
        }

        public void Dispose()
        {
            _textReader.Dispose();
        }
    }
}
