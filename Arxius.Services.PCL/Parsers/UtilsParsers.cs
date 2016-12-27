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
                list.Add(new News() { Title = _match.Groups[1].ToString().ToUpper()[0] + _match.Groups[1].ToString().Substring(1), RestToParse = _match.Groups[2].ToString() });
            }
            return list;
        }

        public static News GetNewsDetails(News news)
        {
            var newsDetailsMatch = Regex.Match(news.RestToParse, @"<span class=""od-news-date"">(.*?)<\/span><\/div><div class=""od-news-body""><p>(.*?)<\/p><\/div><div class=""od-news-footer"">(.*?)<");
            news.Date = newsDetailsMatch.Groups[1].ToString().Trim(' ');
            news.Content = Regex.Replace(newsDetailsMatch.Groups[2].ToString().Trim(' '), @"<.*?>|&nbsp;", "").Replace("&oacute;", "ó");
            news.Author = newsDetailsMatch.Groups[3].ToString().Trim(' ');

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
                var x = new RegistrationTimesCollection(time);
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
                employees.Add(new Employee() { Name = employee.Groups[2].ToString(), Url = string.Format(Properties.Resources.baseUri, employee.Groups[1].ToString()), Email = employee.Groups[4].ToString() });
            }
            return employees;
        }

        public static List<StringGroup> GetImportantDates(string page)
        {
            var headerMatches = Regex.Matches(page, @"<h4>(S|s)(.*?)<\/h4><p><strong>(.*?)<\/strong>(.*?)<(.*?)\/p>", RegexOptions.Multiline);
            string[] tableHeaders = { "Dni rektorskie: ", "Przerwa świąteczna:", "Sesja egzaminacyjna:", "Sesja poprawkowa:" };
            var dic = new List<StringGroup>();
            if (headerMatches.Count == 2)
            {
                
                var semsters = new List<string>() { page.Substring(headerMatches[0].Index, headerMatches[1].Index - headerMatches[0].Index), page.Substring(headerMatches[1].Index) };
               
                for (int i = 0; i < 2; i++)
                {
                    var result= new StringGroup(headerMatches[i].Groups[1].ToString()+ headerMatches[i].Groups[2].ToString());
                    foreach (var head in tableHeaders)
                    {
                        var match = Regex.Match(semsters[i], string.Format(@"<strong>{0}<\/strong>(.*?)<", head), RegexOptions.Multiline);
                        if (match != null)
                            result.Add(string.Format("{0} - {1}", head, match.Groups[1].ToString().Replace("r.",string.Empty).Replace(" r.", string.Empty)));
                    }
                    dic.Add(result);
                }
            }
            return dic;
        }

        public static Employee GetEmployeeDetails(string page, Employee employee)
        {
            page = page.Replace("\n", string.Empty);
            var employeeDetailsMatch = Regex.Match(page, @"<tr><th>pokój<\/th><td>(.*?)<\/td><\/tr>(.*?)<h3>Konsultacje:<\/h3><p>(.*?)<\/p>(.*?)<div class=""byWeekDays"">(.*?)<\/div>", RegexOptions.Multiline);
            if (employeeDetailsMatch == null) throw new Exception();
            employee.Room = "pok. " + employeeDetailsMatch.Groups[1].ToString().Trim(' ').Replace("\t", string.Empty);
            employee.Consults = employeeDetailsMatch.Groups[3].ToString().Trim(' ').Replace("\t", string.Empty);
            var weekByDays = employeeDetailsMatch.Groups[5].ToString();
            var daysMatch = Regex.Matches(weekByDays, @"<h3>(.*?)<\/h3><ul>(.*?)<\/ul>");
            var dict = new List<StringGroup>();
            foreach (Match match in daysMatch)
            {
                var day = match.Groups[1].ToString();
                if (day == "Poniedzialek")
                    day = "Poniedziałek";
                if (day == "Sroda")
                    day = "Środa";
                if (day == "Piatek")
                    day = "Piątek";
                var x = new StringGroup(day);
                var classesMatch = Regex.Matches(match.Groups[2].ToString(), @"<li><span class=""time"">(.*?)</span><span class=""name"">(.*?)</span><span class=""type"">(.*?)</span><span class=""classroom"">(.*?)</span></li>", RegexOptions.Multiline);
                foreach (Match classMatch in classesMatch)
                {
                    var hours = classMatch.Groups[1].ToString().Trim(' ').Replace("\t", string.Empty);
                    var name = classMatch.Groups[2].ToString().Trim(' ').Replace("\t", string.Empty);
                    var type = classMatch.Groups[3].ToString().Trim(' ').Replace("\t", string.Empty).Replace("&ndash;", string.Empty);
                    var classRoom = classMatch.Groups[4].ToString().Trim(' ').Replace("\t", string.Empty);
                    x.Add(string.Format("{0} {1}, {2} s:{3}", hours, name, type.ToString().ToUpper()[0] + type.ToString().Substring(1), classRoom));
                }
                dict.Add(x);
            }
            employee.WeekPlan = dict;
            return employee;
        }


    }
}
