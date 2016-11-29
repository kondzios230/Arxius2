using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arxius.DataAccess.PCL
{
    public static class HTMLUtils
    {
        public static HttpClient client = new HttpClient();
        public static string csrfToken;
        public async static Task<bool> Login(string baseUri,string username, string password)
        {
           //var baseUri = "http://192.168.0.16:8002{0}";
           // var baseUri = "http://localhost:8002{0}";
           // var baseUri = "http://192.168.0.16:8002{0}";
          //  var baseUri = "https://zapisy.ii.uni.wroc.pl{0}";
            var response = await client.GetAsync(string.Format(baseUri,"/users/login/"));
            var cookieValue = Regex.Matches(response.Headers.ToString(), @"Set-Cookie: csrftoken=(.*?);");
            csrfToken = cookieValue[0].Groups[1].ToString();
            var postData = String.Format(@"csrfmiddlewaretoken={0}&next=%2Frecords%2Fschedule%2F&username={1}&password={2}", cookieValue[0].Groups[1], username, password);
            return (await client.PostAsync(string.Format(baseUri, "/users/login/"), new StringContent(postData))).IsSuccessStatusCode;
        }
        public static async Task<string> GetPage(string uri)
        {
            return await client.GetStringAsync(uri);
        }

        public static async void PostString(string uri,string postData)
        {
           var x = await client.PostAsync(uri, new StringContent(postData));
        }
    }
}
