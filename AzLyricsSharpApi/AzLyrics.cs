using System;
using System.Net;
using System.Text;

namespace AzLyricsSharpApi
{
    public class AzLyrics
    {
        private readonly Uri _baseUri;

        /// <summary>
        /// Has the error count of the operation
        /// </summary>
        public int Error { get; private set; }

        private Uri _uri;

        public AzLyrics(string endpoint = "http://www.azlyrics.com/lyrics/")
            : this(new Uri(endpoint))
        {

        }

        public AzLyrics(Uri endpoint)
        {
            _baseUri = endpoint;

            // todo: init the http web-client here
        }

        public string Search(string title, string artist)
        {
            // http://www.azlyrics.com/lyrics/youngthug/richniggashit.htm
            artist = StringUtils.StripWhiteSpaces(artist.ToLowerInvariant());
            title = StringUtils.StripWhiteSpaces(title.ToLowerInvariant());

            //_uri = new Uri(_url + artist + "/" + title + ".html", UriKind.Absolute);

            new Uri(_baseUri, $"{artist}/{title}.html");

            return null;
        }

        public string GetLyris()
        {
            string lyrics = string.Empty;
            using (var webClient = new AzLyricsWebClient())
            {
                webClient.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 6.3) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/35.0.1916.114 Safari/537.36");
                webClient.Encoding = Encoding.UTF8;
                try
                {
                    var date = webClient.DownloadString(_uri);
                    //var date = Encoding.UTF8.GetString(webClient.DownloadData(_uri));
                    lyrics = ExtractLyricsFromHtml(date);
                }
                catch (WebException ex)
                {
                    Error++;
                }
            }
            return lyrics;
        }

        private string ExtractLyricsFromHtml(string htmlPage)
        {
            const string find1 = "<!-- AddThis Button END -->";
            var idx = htmlPage.IndexOf(find1, StringComparison.Ordinal);
            if (idx > 0)
            {
                // Remove from start to "<!-- AddThis Button END -->" length
                htmlPage = htmlPage.Remove(0, idx + find1.Length).TrimStart();
                idx = htmlPage.IndexOf("<!-- MxM banner -->", StringComparison.Ordinal);
                if (idx > 0)
                {
                    htmlPage = htmlPage.Remove(idx).TrimEnd();
                    htmlPage = WebUtility.HtmlDecode(htmlPage);
                }
            }
            return RemoveAllHtmlTags(htmlPage).Trim();
        }

        private string RemoveAllHtmlTags(string html)
        {
            var idx = html.IndexOf("<form id=\"addsong\"");
            if (idx > 20)
            {
                html = html.Substring(0, idx);
            }

            html = StringUtils.RemoveHtmlTags(html);

            // fix recursive white-spaces
            while (html.Contains("  "))
            {
                html = html.Replace("  ", " ");
            }

            // fix recursive line-break
            while (html.Contains("\r\n\r\n\r\n"))
            {
                html = html.Replace("\r\n\r\n\r\n", "\r\n\r\n");
            }

            return html;
        }

        private bool IsValidUri(string url) => !(string.IsNullOrWhiteSpace(url) && (!Uri.TryCreate(url, UriKind.Absolute, out _uri)));

    }
}
