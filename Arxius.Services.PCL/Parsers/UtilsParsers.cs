using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace Arxius.Services.PCL.Parsers
{
    public static class UtilsParsers
    {
        public static List<News> GetFeedElementsContent(string page)
        {
            page = page.Replace("\n", string.Empty);
            var matches = Regex.Matches(page, @"<h3>(.*?)<\/h3>(.*?)\/div><\/div>", RegexOptions.Multiline).Cast<Match>().ToList();

            var list = new List<News>();
            foreach (var match in matches)
            {
                var _match = match as Match;
                list.Add(new News() { Title = _match.Groups[1].ToString().ToUpper()[0]+_match.Groups[1].ToString().Substring(1), RestToParse= _match.Groups[2].ToString() });
            }
            return list;
        }
        public static News GetNewsDetails(News news)
        {
            var newsDetailsMatch = Regex.Match(news.RestToParse, @"<span class=""od-news-date"">(.*?)<\/span><\/div><div class=""od-news-body""><p>(.*?)<\/p><\/div><div class=""od-news-footer"">(.*?)<");
            news.Date = newsDetailsMatch.Groups[1].ToString().Trim(' ');
            news.Content = Regex.Replace(newsDetailsMatch.Groups[2].ToString().Trim(' '), @"<.*?>|&nbsp;", "").Replace("&oacute;", "ó"); 
            news.Author= newsDetailsMatch.Groups[3].ToString().Trim(' ');

            return news;
        }
        public static UserPage GetUserPage(string page)
        {
            var ectsMatch = Regex.Matches(page, @"<tr><th>Punkty ECTS<\/th><td>(\d*)<\/td>", RegexOptions.Multiline);
            var ects = ectsMatch.Count != 0 ? Convert.ToInt32(ectsMatch[0].Groups[1].ToString()) : 0;

            var dict = new List<RegistrationTimesCollection>();
            var votedCoursesMatch = Regex.Matches(page, @"<tr><th>Czas:<\/th><td>(.*?)<\/td><\/tr><tr><td><\/td><td><ul class=""voted-courses"">(.*?)<\/td>", RegexOptions.Multiline);
            foreach (var match in votedCoursesMatch)
            {
                var courseList = new List<Course>();
                var _match = match as Match;
                var time = _match.Groups[1].ToString();
                var courses = Regex.Matches(_match.Groups[2].ToString(), @"<li><a href=""(.*?)"">(.*?)<\/a><\/li>", RegexOptions.Multiline);
                foreach (var course in courses)
                {
                    var _course = course as Match;
                    courseList.Add(new Course() { Url = _course.Groups[1].ToString(), Name = _course.Groups[2].ToString() });
                }
                var x = new RegistrationTimesCollection(Convert.ToDateTime(time));
                x.AddRange(courseList);
                dict.Add(x);
            }
            var userPage = new UserPage() { Ects = ects, RegistrationTimes = dict };
            var registrationMatch = Regex.Matches(page, @"<th>Zniesienie limitu 35 ECTS<\/th><td>(.*?)<\/td><\/tr><tr><th>Koniec zapisów<\/th><td>(.*?)<\/td>", RegexOptions.Multiline);
            if (registrationMatch.Count != 0)
            {
                userPage.EndTime = registrationMatch[0].Groups[2].ToString();
                userPage.LimitRemovalTime = registrationMatch[0].Groups[1].ToString();
            }
            else
            {
                userPage.EndTime = userPage.LimitRemovalTime = null;
            }
            return userPage;
        }

        public static List<Employee> GetEmployeesList(string page)
        {
            var employeesMatch = Regex.Matches(page, @"<li><a href=\""(.*?)\"" class=\""employee-profile-link\"">(.*?)<\/a><input type='hidden' name='employee-id' value='(.*?)' \/><input type='hidden' name='employee-email' value='(.*?)' \/><input type='hidden' name='employee-short_old' value='(.*?)' \/><input type='hidden' name='employee-short_new' value='(.*?)' \/><\/li>", RegexOptions.Multiline);
            var employees = new List<Employee>();
            foreach (var employeeMatch in employeesMatch)
            {
                var employee = employeeMatch as Match;
                employees.Add(new Employee() { Name = employee.Groups[2].ToString(), Url = string.Format(Properties.Resources.baseUri,employee.Groups[1].ToString()), Email = employee.Groups[4].ToString() });
            }
            return employees;
        }

        public static void GetImportantDates(string page)
        {
            var m1 = Regex.Matches(page, @"<h4>(S|s)(.*?)<\/h4><p><strong>(.*?)<\/strong>(.*?)<(.*?)\/p>", RegexOptions.Multiline);
            var m1a = Regex.Matches(page, @"<h4>(.*?)<\/h4><p><strong>(.*?)<\/strong>(.*?)<(.*?)\/p>", RegexOptions.Multiline);
            var m2 = Regex.Matches(page, @"<strong>Dni rektorskie: <\/strong>(.*?)<br>", RegexOptions.Multiline);
            var m3a= Regex.Matches(page, @"<strong>Przerwa świąteczna:</strong>(.*?)<br>", RegexOptions.Multiline);
            var m4 = Regex.Matches(page, @"<strong>Sesja egzaminacyjna:</strong>(.*?)<br>", RegexOptions.Multiline);
            var m5 = Regex.Matches(page, @"<strong>Sesja poprawkowa:</strong>(.*?)</p>", RegexOptions.Multiline);
        }



    }
}
