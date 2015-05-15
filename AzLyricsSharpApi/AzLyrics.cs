using System;
using System.Net;
using System.Text;
//using System.Web;

namespace AzLyricsSharpApi
{
    public class AzLyrics
    {
        const string _url = "http://www.azlyrics.com/lyrics/";
        Uri _uri;
        public AzLyrics(string artist, string title)
        {
            artist = FixInput(artist.ToLowerInvariant());
            title = FixInput(title.ToLowerInvariant());
            _uri = new Uri(_url + artist + "/" + title + ".html", UriKind.Absolute);
        }


        private string FixInput(string input)
        {
            return input = input.Replace(" ", string.Empty);
        }

        public string GetLyris()
        {
            string lyrics = string.Empty;
            using (var webClient = new AzLyricsWebClient())
            {
                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36");
                webClient.Encoding = Encoding.UTF8;
                // If file name or title is invalid this will throw
                try
                {
                    var date = webClient.DownloadString(_uri);
                    lyrics = ExtractLyricsFromHtml(date);
                }
                catch (WebException ex) { }
            }
            return lyrics;
        }

        private string ExtractLyricsFromHtml(string htmlPage)
        {
            const string find1 = "<!-- AddThis Button END -->";
            var idx = htmlPage.IndexOf(find1, StringComparison.Ordinal);
            if (idx > 0)
            {
                htmlPage = htmlPage.Remove(0, idx + find1.Length).TrimStart();
                idx = htmlPage.IndexOf("<!-- MxM banner -->", StringComparison.Ordinal);
                if (idx > 0)
                {
                    htmlPage = htmlPage.Remove(idx).TrimEnd();
                    htmlPage = WebUtility.HtmlDecode(htmlPage);
                }
            }
            htmlPage = RemoveAllHtmlTags(htmlPage).Trim();
            return htmlPage;
        }

        private string RemoveAllHtmlTags(string html)
        {
            html = html.Replace("<i>", string.Empty);
            html = html.Replace("</i>", string.Empty);
            html = html.Replace("<h1>", string.Empty);
            html = html.Replace("</h1>", string.Empty);
            html = html.Replace("<div>", string.Empty);
            html = html.Replace("</div>", string.Empty);
            html = html.Replace("<br>", string.Empty);
            html = html.Replace("<b>", string.Empty);
            html = html.Replace("</b>", string.Empty);

            var idx = html.IndexOf('<');
            while (idx >= 0)
            {
                var endIdx = html.IndexOf('>', idx + 1);
                if (endIdx > idx)
                {
                    /*var tag = html.Substring(idx, endIdx - idx + 1);
                    if (tag == "<i>" || tag == "</i>" || tag == "<div>" || tag == "</div>" || tag == "<br>" || tag == "<br>" || tag == "<b>" || tag == "</b>")
                    {
                        html = html.Remove(idx, endIdx - idx + 1);
                    }*/
                    html = html.Remove(idx, endIdx - idx + 1);
                }
                idx = html.IndexOf('<', idx);
            }
            while (html.Contains("  "))
            {
                html = html.Replace("  ", " ");
            }
            while (html.Contains("\r\n\r\n\r\n"))
                html = html.Replace("\r\n\r\n\r\n", "\r\n\r\n");
            return html;
        }

        private bool IsValidUri()
        {
            if (_uri == null)
                return false;
            return true;
        }
    }
}
