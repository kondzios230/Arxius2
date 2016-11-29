using System;
using System.Net;

namespace Arxius.DataAccess
{
    public class CookieAwareWebClient : WebClient
    {
        public bool IsPOST;
        public CookieContainer CookieContainer { get; set; }
        public Uri Uri { get; set; }


        public CookieAwareWebClient(CookieContainer cookies)
        {
            CookieContainer = cookies;
            Encoding = System.Text.Encoding.UTF8;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            var httpWebRequest = (base.GetWebRequest(address) as HttpWebRequest);
            PrepareRequest(httpWebRequest);
            return httpWebRequest;
        }
        private void PrepareRequest(HttpWebRequest httpWebRequest)
        {
            if (httpWebRequest == null) return;

            httpWebRequest.CookieContainer = CookieContainer;
            httpWebRequest.ServicePoint.Expect100Continue = false;
            httpWebRequest.UserAgent = "Arxius";//"Mozilla/5.0 (Windows NT 6.1; WOW64; rv:18.0) Gecko/20100101 Firefox/18.0";
            httpWebRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
            httpWebRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, "pl-Pl");
            httpWebRequest.KeepAlive = true;
            httpWebRequest.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
            if (IsPOST)
            {
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            }
        }
        protected override WebResponse GetWebResponse(WebRequest request)
        {
            var response = base.GetWebResponse(request);
            var setCookieHeader = response.Headers[HttpResponseHeader.SetCookie];

            if (setCookieHeader != null)
            {
                try
                {
                    var cookie = new Cookie();
                    CookieContainer.Add(cookie);

                }
                catch (Exception)
                {

                }
            }
            return response;

        }
    }
}
