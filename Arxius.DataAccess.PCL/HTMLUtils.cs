using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arxius.DataAccess.PCL
{
    public static class HTMLUtils
    {
        public static HttpClient client = new HttpClient() { Timeout = new TimeSpan(0, 0, 10) };
        public static string csrfToken;
        public async static Task<bool> Login(string baseUri, string username, string password)
        {
            try
            {
                var response = await client.GetAsync(string.Format(baseUri, "/users/login/"));
                var cookieValue = Regex.Matches(response.Headers.ToString(), @"Set-Cookie: csrftoken=(.*?);");
                csrfToken = cookieValue[0].Groups[1].ToString();
                var postData = string.Format(@"csrfmiddlewaretoken={0}&next=%2Frecords%2Fschedule%2F&username={1}&password={2}", cookieValue[0].Groups[1], username, password);
                return (await client.PostAsync(string.Format(baseUri, "/users/login/"), new StringContent(postData))).IsSuccessStatusCode;
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
        public static async Task<string> GetPage(string uri)
        {
            try
            {
                var s = new Stopwatch();
               
                Debug.WriteLine("GetPage({0})", uri);
                s.Start();
                var x = await client.GetStringAsync(uri);
                s.Stop();
                Debug.WriteLine("Download - {0}", s.Elapsed);
                return x;
            }
            catch (Exception e)
            {
                if (e is WebException)
                    throw new ArxiusDataException("Wystąpił problem z siecią, sprawdź swoje połączenie", e);
                if(e is TaskCanceledException)
                    throw new ArxiusDataException("Przekroczono czas oczekiwania", e);
                throw new ArxiusDataException("Wystąpił nieznany problem", e);
            }
        }

        public static async Task<string> PostString(string uri, string postData)
        {
            try
            {
                var response = await client.PostAsync(uri, new StringContent(postData));
                return await response.Content.ReadAsStringAsync();
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
