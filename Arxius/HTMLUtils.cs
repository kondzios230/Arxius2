//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Text;
//using System.Text.RegularExpressions;
//using System.Threading.Tasks;

//namespace Arxius.DataAccess
//{
//    public static  class HTMLUtils
//    {
//        public static CookieAwareWebClient client = new CookieAwareWebClient(new CookieContainer());
    
//        public static string Login(string username,string password)
//        { 
//            var response = client.DownloadString("https://zapisy.ii.uni.wroc.pl/users/login/");
//            var cookieValue = Regex.Matches(client.ResponseHeaders[HttpResponseHeader.SetCookie], @"=(.*?);");
//            var token = cookieValue[0].ToString().Substring(1, cookieValue[0].ToString().Length - 2);
//            var postData = String.Format("csrfmiddlewaretoken={0}&next=%2Frecords%2Fschedule%2F&username={1}&password={2}", token,username,password);
            
//            client.IsPOST = true;
//            return client.UploadString("https://zapisy.ii.uni.wroc.pl/users/login/", postData);
//        }
//        public static async Task<string> GetPage(string uri)
//        {
//            var c = new CookieAwareWebClient(client.CookieContainer);
//            return await c.DownloadStringTaskAsync(uri);
//        }
//    }
//}
