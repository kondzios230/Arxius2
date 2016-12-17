﻿using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arxius.DataAccess.PCL
{
    public static class HTMLUtils
    {
        public static HttpClient client = new HttpClient() { Timeout = new TimeSpan(0, 0, 5) };
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
                if(e is WebException || e is TaskCanceledException)
                    throw new ArxiusDataException("Wystąpił problem z siecią, sprawdź swoje połączenie", e);
                throw new ArxiusDataException("Wystąpił nieznany problem", e);
            }
        }
        public static async Task<string> GetPage(string uri)
        {
            try
            {
                Debug.WriteLine("\n\n\n                       GetPage({0})\n\n\n", uri);
                return await client.GetStringAsync(uri);
            }
            catch (Exception e)
            {
                if (e is WebException || e is TaskCanceledException)
                    throw new ArxiusDataException("Wystąpił problem z siecią, sprawdź swoje połączenie", e);
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
                if (e is WebException || e is TaskCanceledException)
                    throw new ArxiusDataException("Wystąpił problem z siecią, sprawdź swoje połączenie", e);
                throw new ArxiusDataException("Wystąpił nieznany problem", e);
            }
        }
    }
}
