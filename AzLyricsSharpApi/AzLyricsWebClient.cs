using System.Net;
using System;
namespace AzLyricsSharpApi
{
    class AzLyricsWebClient : WebClient
    {
        // http://stackoverflow.com/questions/15034771/cant-download-utf-8-web-content
        protected override WebRequest GetWebRequest(Uri address)
        {
            // this method GetWebRequest is only available form derived classes 'base.GetWebRequest()'
            HttpWebRequest req = base.GetWebRequest(address) as HttpWebRequest;
            if (req != null)
            {
                req.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            }
            return req;
        }
    }
}
