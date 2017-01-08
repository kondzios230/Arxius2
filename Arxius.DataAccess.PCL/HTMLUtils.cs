using System;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using Arxius.CrossLayer.PCL;
namespace Arxius.DataAccess.PCL
{
    public static class HTMLUtils
    {

        private static HttpClientHandler handler = new HttpClientHandler { UseCookies = false };
        private static HttpClient client = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, 30), BaseAddress =
            new Uri(CrossLayerData.BaseAddressShort) };
        public static string csrfToken;
        public static string cookie;
       
        public static async Task<string> GetPage(string uri)
        {
            try
            {
                var message = new HttpRequestMessage(HttpMethod.Get, uri);
                message.Headers.Add("Cookie", cookie);
                var result = await client.SendAsync(message);
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                if (e is WebException)
                    throw new ArxiusDataException("Wystąpił problem z siecią, sprawdź swoje połączenie", e);
                if (e is TaskCanceledException)
                    throw new ArxiusDataException("Przekroczono czas oczekiwania", e);
                throw new ArxiusDataException("Wystąpił nieznany problem", e);
            }
        }
        public static async Task<string> GetPageUnAuthorised(string uri)
        {
            try
            {
                return await client.GetStringAsync(uri);
            }
            catch (Exception e)
            {
                if (e is WebException)
                    throw new ArxiusDataException("Wystąpił problem z siecią, sprawdź swoje połączenie", e);
                if (e is TaskCanceledException)
                    throw new ArxiusDataException("Przekroczono czas oczekiwania", e);
                throw new ArxiusDataException("Wystąpił nieznany problem", e);
            }
        }
        public static async Task<string> PostString(string uri, string postData)
        {
            try
            {
                var message = new HttpRequestMessage(HttpMethod.Post, uri);
                message.Headers.Add("Cookie", cookie);
                message.Content = new StringContent(postData);
                var result = await client.SendAsync(message);
                result.EnsureSuccessStatusCode();
                return await result.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                if (e is WebException)
                    throw new ArxiusDataException("Wystąpił problem z siecią, sprawdź swoje połączenie", e);
                if (e is TaskCanceledException)
                    throw new ArxiusDataException("Przekroczono czas oczekiwania", e);
                throw new ArxiusDataException("Wystąpił nieznany problem", e);
            }
        }
    }
}
