
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Arxius.Services.PCL.Parsers
{
    public static class CoursesParsers
    {
        public static void GetCourseDetails(string page, Course course)
        {
            var start = page.IndexOf(@"enr-course-view");
            var end = page.IndexOf(@"<div class=""span5 columns"">");
            var courseDetailsSubstring = page.Substring(start, end);

            var kindMatch = Regex.Matches(courseDetailsSubstring, @"""table-info""><tr><th>Rodzaj<\/th><td>(.*?)<\/td>", RegexOptions.Multiline);
            if (kindMatch.Count != 0)
                course.Kind = kindMatch[0].Groups[1].ToString();
            var ectsMatch = Regex.Matches(courseDetailsSubstring, @"<th>Punkty ECTS<\/th><td>(.*?)<\/td>", RegexOptions.Multiline);
            if (ectsMatch.Count != 0)
                course.Ects = Convert.ToInt32(ectsMatch[0].Groups[1].ToString());


            var secondPartOfTable = Regex.Matches(courseDetailsSubstring, @"<\/td><\/tr><tr><th>", RegexOptions.Multiline);
            if (secondPartOfTable.Count != 0)
            {
                var dict = new Dictionary<string, int>();
                var hoursPart = courseDetailsSubstring.Substring(ectsMatch[0].Index + ectsMatch[0].Length, secondPartOfTable[0].Index + 100).Replace(" ", "").Replace("\n", "");
                var table = Regex.Matches(hoursPart, @"(\d{1,3})\((\w*)\)", RegexOptions.Multiline);
                if (table.Count != 0)
                {
                    foreach (var match in table)
                    {
                        var matchx = match as Match;
                        dict.Add(matchx.Groups[2].ToString(), Convert.ToInt32(matchx.Groups[1].ToString()));
                    }
                    course.HoursSchema = dict;
                }
            }

        }
        public static void GetCourseWideDetails(string page, Course course)
        {
            course.Classes.Clear();
            var start = page.IndexOf(@"enr-course-view");
            var end = page.IndexOf(@"<div class=""span5 columns"">");
            var courseDetailsSubstring = page.Substring(start, end);
            var ectsMatch = Regex.Matches(courseDetailsSubstring, @"<th>Punkty ECTS<\/th><td>(.*?)<\/td>", RegexOptions.Multiline);
            var secondPartOfTable = Regex.Matches(courseDetailsSubstring, @"<\/td><\/tr><tr><th>", RegexOptions.Multiline);
            if (secondPartOfTable.Count != 0 && ectsMatch.Count != 0)
            {
                var dict = new Dictionary<string, int>();
                var hoursPart = courseDetailsSubstring.Substring(ectsMatch[0].Index + ectsMatch[0].Length, secondPartOfTable[0].Index + 100).Replace(" ", "").Replace("\n", "");
                var table = Regex.Matches(hoursPart, @"(\d{1,3})\((\w*)\)", RegexOptions.Multiline);
                if (table.Count != 0)
                {
                    foreach (var match in table)
                    {
                        var matchx = match as Match;
                        dict.Add(matchx.Groups[2].ToString(), Convert.ToInt32(matchx.Groups[1].ToString()));
                    }
                    course.HoursSchema = dict;
                }
            }
            var tutorialKindListMatch = Regex.Matches(page.Replace("\n", string.Empty), @"""tutorial""(.*?)<\/strong", RegexOptions.Multiline);
            foreach (var tutorialKindMatchSingle in tutorialKindListMatch)
            {
                var tutorialGroupString = (tutorialKindMatchSingle as Match).Groups[1].ToString();
                var enrollmentCourseMatch = Regex.Matches(tutorialGroupString, @"><a href=""(.*?)"" class=""person"">(.*?)<\/a><\/td><td class=""term""><span(.*?)<\/span><\/td><td class=""number termLimit"">(.*?)<\/td><td class=""number termEnrolledCount"">(.*?)<\/td><td class=""number termQueuedCount"">(.*?)<\/td><td class=""controls""><input type=""hidden"" name=""group-id"" value=""(.*?)""\/><input type=""hidden"" name=(.*?)}'\/><input type=""hidden"" name=""is-signed(.*?)value=""(.*?)""\/><form action=""(.*?)"" method=""post"" class=""setEnrolled""><div style='display:none'><input type='hidden' name='csrfmiddlewaretoken' value='(.*?)' \/><\/div><div><input type=""hidden"" name=""group"" value=""(.*?)""\/><input type=""hidden"" name=""enroll"" value=""(.*?)""\/><(.*?)setEnrolledButton"">(.*?)<\/button><\/div><\/form><a href=""(.*?)"">(.*?)<\/a><\/td><td class=""priority"">(.*?)<\/td><\/tr>", RegexOptions.Multiline);
                if (enrollmentCourseMatch.Count != 0)
                {
                    foreach (Match courseMatch in enrollmentCourseMatch)
                    {
                        var _class = new _Class();
                        _class.Teacher = new Employee() { Name = courseMatch.Groups[2].ToString().Trim(' '), Url = courseMatch.Groups[1].ToString() };
                        var lessonMatches = Regex.Matches(courseMatch.Groups[3].ToString(), @">(.*?)\((.*?)\)", RegexOptions.Multiline);
                        foreach (Match lessonMatch in lessonMatches)
                        {
                            var l = parseToLesson(lessonMatch);
                            l.CourseName = course.Name;
                            _class.Lessons.Add(l);
                        }

                        _class.TotalPeople = courseMatch.Groups[4].ToString().Trim(' ');
                        _class.SignedInPeople = courseMatch.Groups[5].ToString().Trim(' ');
                        _class.QueuedPeople = courseMatch.Groups[6].ToString().Trim(' ');
                        _class.IsSignedIn = courseMatch.Groups[10].ToString().Trim(' ') == "true";
                        _class.enrollmentUri = courseMatch.Groups[11].ToString().Trim(' ');
                        _class.csrfToken = courseMatch.Groups[12].ToString().Trim(' ');
                        _class.enrollmentId = courseMatch.Groups[13].ToString().Trim(' ');
                        _class.ButtonEnrollText = courseMatch.Groups[16].ToString().Trim(' ');
                        _class.ListUrl = courseMatch.Groups[17].ToString().Trim(' ');
                        _class.buttonListText = courseMatch.Groups[18].ToString().Trim(' ');
                        _class.Priority = courseMatch.Groups[19].ToString().Trim(' ');
                        course.Classes.Add(_class);
                    }
                }
                var nonEnrollmentCourseMatch = Regex.Matches(tutorialGroupString, @"><a href=""(.*?)"" class=""person"">(.*?)<\/a><\/td><td class=""term""><span(.*?)<\/span><\/td><td class=""number termLimit"">(.*?)<\/td><td class=""number termEnrolledCount"">(.*?)<\/td><td class=""number termQueuedCount"">(.*?)<\/td><td class=""controls""><input type=""hidden"" name=""group-id"" value=""(.*?)""\/><input type=""hidden"" name=""(.*?)}'\/><input type=""hidden"" name=""is(.*?)value=""(.*?)""\/><a href=""(.*?)"">(.*?)<\/a><\/td><\/tr>", RegexOptions.Multiline);
                if (nonEnrollmentCourseMatch.Count != 0)
                {
                    foreach (Match courseMatch in nonEnrollmentCourseMatch)
                    {
                        var _class = new _Class();
                        _class.Teacher = new Employee() { Name = courseMatch.Groups[2].ToString().Trim(' '), Url = courseMatch.Groups[1].ToString() };
                        var lessonMatches = Regex.Matches(courseMatch.Groups[3].ToString(), @">(.*?)\((.*?)\)", RegexOptions.Multiline);
                        foreach (Match lessonMatch in lessonMatches)
                        {
                            var l = parseToLesson(lessonMatch);
                            l.CourseName = course.Name;
                            _class.Lessons.Add(l);
                        }
                        _class.TotalPeople = courseMatch.Groups[4].ToString().Trim(' ');
                        _class.SignedInPeople = courseMatch.Groups[5].ToString().Trim(' ');
                        _class.QueuedPeople = courseMatch.Groups[6].ToString().Trim(' ');
                        _class.IsSignedIn = courseMatch.Groups[10].ToString() == "true".Trim(' ');
                        _class.ListUrl = courseMatch.Groups[11].ToString().Trim(' ').Trim('\\');
                        _class.buttonListText = courseMatch.Groups[12].ToString().Trim(' ');
                        course.Classes.Add(_class);
                    }
                }

                var enrolledCourseMatch = Regex.Matches(tutorialGroupString, @"><a href=""(.*?)"" class=""person"">(.*?)<\/a><\/td><td class=""term""><span>(.*?)\((.*?)\)<\/span><\/td><td class=""number termLimit"">(.*?)<\/td><td class=""number termEnrolledCount"">(.*?)<\/td><td class=""number termQueuedCount"">(.*?)<\/td><td class=""controls""><input type=""hidden"" name=""group-id"" value=""(.*?)""\/><input type=""hidden"" name=""(.*?)}'\/><input type=""hidden"" name=""is-(.*?)value=""(.*?)""\/><form><div><button (.*?)setEnrolledButton"">(.*?)<\/button><\/div><\/form><a href=""(.*?)"">(.*?)<\/a><\/td><\/tr><\/tbody><\/table><p><strong>", RegexOptions.Multiline);
                if (enrolledCourseMatch.Count != 0)
                {
                    foreach (Match courseMatch in enrolledCourseMatch)
                    {
                        var _class = new _Class();
                        _class.Teacher = new Employee() { Name = courseMatch.Groups[2].ToString().Trim(' '), Url = courseMatch.Groups[1].ToString() };
                        var lessonMatches = Regex.Matches(courseMatch.Groups[3].ToString(), @">(.*?)\((.*?)\)", RegexOptions.Multiline);
                        foreach (Match lessonMatch in lessonMatches)
                        {
                            var l = parseToLesson(lessonMatch);
                            l.CourseName = course.Name;
                            _class.Lessons.Add(l);
                        }
                        _class.TotalPeople = courseMatch.Groups[4].ToString().Trim(' ');
                        _class.SignedInPeople = courseMatch.Groups[5].ToString().Trim(' ');
                        _class.QueuedPeople = courseMatch.Groups[6].ToString().Trim(' ');
                        _class.IsSignedIn = courseMatch.Groups[10].ToString().Trim(' ') == "true";
                        _class.ListUrl = courseMatch.Groups[13].ToString().Trim(' ');
                        course.Classes.Add(_class);
                    }
                }


            }

        }
        public static List<Course> GetUserPlanForCurrentSemester(string page)
        {
            var courseList = new List<Course>();
            var headersOfCoursesCollection = Regex.Matches(page, @"<td class=""name""><a href=""(.*?)"">(.*?)<\/a><\/td><td rowspan=""2"" class=""ects"">(.*?)<\/td><\/tr><tr class=""courseDetails""><td><ul>(.*?)<\/ul", RegexOptions.Singleline);
            foreach (Match match in headersOfCoursesCollection)
            {
                var course = new Course();
                course.Url = match.Groups[1].ToString().Replace("\n", string.Empty).Trim(' ');
                course.Name = match.Groups[2].ToString().Replace("\n", string.Empty).Trim(' ');
                course.Ects = Convert.ToInt32(match.Groups[3].ToString().Replace("\n", string.Empty).Trim(' '));
                course.Classes = parseToClasses(match.Groups[4].ToString());
                courseList.Add(course);
            }
            return courseList;
        }
        public static List<Course> GetAllUserCourses(string page)
        {
            var allCoursesHtmlElements = Regex.Matches(page, Properties.Regex.RegexForListElement, RegexOptions.Multiline);
            var allEnrolledCourses = allCoursesHtmlElements.Cast<Match>().ToList().Select(c => c.ToString()).ToList().FindAll(c => c.Contains("name=\"wasEnrolled\" value=\"True\"")).ToList();
            var listOfCourses = new List<Course>();
            foreach (var courseString in allEnrolledCourses)
            {
                var parsedCourseGroupsMatch = Regex.Matches(courseString, @"<li><a href=\""(.*?)"" id=\""(.*?)"">(.*?)<\/a><(.*?)value=\""(.*?)"" \/><(.*?)value=\""(.*?)"" \/><(.*?)value=\""(.*?)"" \/><(.*?) value=\""(.*?)"" \/><(.*?) value=\""(.*?)"" \/><\/li>", RegexOptions.Multiline);
                if (parsedCourseGroupsMatch.Count != 0)
                {
                    var parsedCourseGroups = parsedCourseGroupsMatch[0];
                    listOfCourses.Add(new Course()
                    {
                        Url = parsedCourseGroups.Groups[1].ToString(),
                        CourseID = parsedCourseGroups.Groups[2].ToString(),
                        Name = parsedCourseGroups.Groups[3].ToString(),
                        Type = Convert.ToInt32(parsedCourseGroups.Groups[5].ToString()),
                        WasEnrolled = parsedCourseGroups.Groups[7].ToString() == "True",
                        IsEnglish = parsedCourseGroups.Groups[9].ToString() == "True",
                        IsExam = parsedCourseGroups.Groups[11].ToString() == "True",
                        SugestedFor1stYear = parsedCourseGroups.Groups[12].ToString() == "True"
                    });
                }
            }
            return listOfCourses.Distinct(new CourseNameComparer()).ToList();
        }
        public static List<Course> GetAllCourses(string page)
        {
            var allCoursesHtmlElements = Regex.Matches(page, Properties.Regex.RegexForListElement, RegexOptions.Multiline);
            var listOfCourses = new List<Course>();
            foreach (var courseMatch in allCoursesHtmlElements)
            {
                var courseString = (courseMatch as Match).ToString();
                var parsedCourseGroupsMatch = Regex.Matches(courseString, @"<li><a href=\""(.*?)"" id=\""(.*?)"">(.*?)<\/a><(.*?)value=\""(.*?)"" \/><(.*?)value=\""(.*?)"" \/><(.*?)value=\""(.*?)"" \/><(.*?) value=\""(.*?)"" \/><(.*?) value=\""(.*?)"" \/><\/li>", RegexOptions.Multiline);
                if (parsedCourseGroupsMatch.Count != 0)
                {
                    var parsedCourseGroups = parsedCourseGroupsMatch[0];
                    listOfCourses.Add(new Course()
                    {
                        Url = parsedCourseGroups.Groups[1].ToString(),
                        CourseID = parsedCourseGroups.Groups[2].ToString(),
                        Name = parsedCourseGroups.Groups[3].ToString(),
                        Type = Convert.ToInt32(parsedCourseGroups.Groups[5].ToString()),
                        WasEnrolled = parsedCourseGroups.Groups[7].ToString() == "True",
                        IsEnglish = parsedCourseGroups.Groups[9].ToString() == "True",
                        IsExam = parsedCourseGroups.Groups[11].ToString() == "True",
                        SugestedFor1stYear = parsedCourseGroups.Groups[12].ToString() == "True"
                    });
                }
            }
            return listOfCourses.Distinct(new CourseNameComparer()).ToList();
        }
        public static Tuple<int, int, List<Student>> GetStudentsList(string page)
        {
            var item1 = 0;
            var item2 = 0;
            var item3 = new List<Student>();
            page = page.Replace("\t", string.Empty).Replace("\n", string.Empty);
            var headerMatch = Regex.Match(page, @"<hr><h3>(.*?)<\/h3><p>(.*?):(.*?)\/(.*?)<\/p><table", RegexOptions.Multiline);
            var studentsMatch = Regex.Matches(page, @"<tr><td><a href=""(.*?)"" class=""person"">(.*?)<\/a><\/td><td><a href=""(.*?)"" class=""person"">(.*?)<\/a><\/td><td>(.*?)<\/td><td>(.*?)<\/td><td>(.*?)<\/td><\/tr>", RegexOptions.Multiline);

            if (headerMatch != null)
            {
                item1 = Convert.ToInt32(headerMatch.Groups[3].ToString().Trim(' '));
                item2 = Convert.ToInt32(headerMatch.Groups[4].ToString().Trim(' '));
            }
            foreach (Match student in studentsMatch)
            {
                var s = new Student();
                s.Name = student.Groups[2].ToString().Trim(' ');
                s.Url = student.Groups[3].ToString().Trim(' ');
                s.Surname = student.Groups[4].ToString().Trim(' ');
                s.Index = student.Groups[5].ToString().Trim(' ');
                s.StudiesKind = student.Groups[6].ToString().Trim(' ');
                s.Semester = student.Groups[7].ToString().Trim(' ');
                item3.Add(s);
            }
            return new Tuple<int, int, List<Student>>(item1, item2, item3);
        }
        public static Tuple<bool, string, List<string>> IsSignedIn(string response, _Class _class)
        {
            response = response.Replace("\n", string.Empty);
            var match = Regex.Match(response, string.Format(@"<div class=""alert-message info""(.*?)<div id=""enr-coursesList-top-bar""(.*?)<input type=""hidden"" name=""is-signed-in""(.*?)value=""(.*?)""\/>(.*?)<input type=""hidden"" name=""group"" value=""{0}""\/>(.*?)<button type=""(.*?)Button"">(.*?)<\/button>", _class.enrollmentId), RegexOptions.Multiline);
            if (match == null) throw new Exception();
            var messages = Regex.Matches(match.Groups[1].ToString(), @">(.*?)</div>", RegexOptions.Multiline);
            var listOfMessages = new List<string>();
            foreach (Match messageMatch in messages)
                listOfMessages.Add(messageMatch.Groups[1].ToString().Trim(' '));
   
            return Tuple.Create(match.Groups[4].ToString() == "true", match.Groups[8].ToString().Trim(' '),listOfMessages);
        }
        #region Private Methods
        private static List<_Class> parseToClasses(string dMatch)
        {
            var forbiddenChars = new char[] { ' ', '\n', '>', '\\', '"' };
            var list = new List<_Class>();
            var typeStringMatchList = Regex.Matches(dMatch.Replace("\n", string.Empty), @"<span class=""type"">(.*?)<\/span>(.*?)<\/li", RegexOptions.Multiline);
            foreach (Match typeStringMatch in typeStringMatchList)
            {
                var _class = new _Class();
                var type = ClassTypeEnum.Other;
                var typeString = typeStringMatch.Groups[1].ToString().Trim(forbiddenChars);
                switch (typeString[0])
                {
                    case 'w':
                        type = ClassTypeEnum.Lecture;
                        break;
                    case 'p':
                        type = ClassTypeEnum.Lab;
                        break;
                    case 'c':
                    case 'ć':
                        type = ClassTypeEnum.Excercise;
                        break;
                }
                _class.ClassType = type;
                var lessonsMatch = Regex.Matches(typeStringMatch.Groups[2].ToString(), @"<span class=""term"">\s*?(.*?)\s*?(.*?)-(.*?)\s*?<\/span><span class=""classroom"">(.*?):(.*?)<\/span>", RegexOptions.Multiline);
                foreach (Match lessonMatch in lessonsMatch)
                {
                    var lesson = new Lesson();
                    lesson.Classroom = lessonMatch.Groups[5].ToString().Trim(' ');
                    var split = lessonMatch.Groups[2].ToString().Trim(' ').Split(' ');
                    switch (split[0])
                    {
                        case "poniedzialek":
                        case "poniedziałek":
                            lesson.Day = DayOfWeek.Monday;
                            break;
                        case "wtorek":
                            lesson.Day = DayOfWeek.Tuesday;
                            break;
                        case "sroda":
                        case "środa":
                            lesson.Day = DayOfWeek.Wednesday;
                            break;
                        case "czwartek":
                            lesson.Day = DayOfWeek.Thursday;
                            break;
                        case "piątek":
                        case "piatek":
                            lesson.Day = DayOfWeek.Friday;
                            break;
                        default:
                            lesson.Day = DayOfWeek.Monday;
                            break;
                    }
                    lesson.StartTime = Convert.ToDateTime(split[split.Count() - 1]);
                    lesson.EndTime = Convert.ToDateTime(lessonMatch.Groups[3].ToString());
                    _class.Lessons.Add(lesson);
                }
                list.Add(_class);
            }

            return list;
        }
        private static Lesson parseToLesson(Match lessonMatch)
        {
            var lesson = new Lesson();
            var split = lessonMatch.Groups[1].ToString().Trim(' ').Split(' ');
            var classRoomSplit = split[0].Split('>');
            var times = split[1].Split('-');
            lesson.StartTime = Convert.ToDateTime(times[0]);
            lesson.EndTime = Convert.ToDateTime(times[1]);
            switch (classRoomSplit[classRoomSplit.Count() - 1])
            {
                case "pon":
                    lesson.Day = DayOfWeek.Monday;
                    break;
                case "wt":
                    lesson.Day = DayOfWeek.Tuesday;
                    break;
                case "śr":
                    lesson.Day = DayOfWeek.Wednesday;
                    break;
                case "cz":
                case "czw":
                    lesson.Day = DayOfWeek.Thursday;
                    break;
                case "pt":
                    lesson.Day = DayOfWeek.Friday;
                    break;
                default:
                    lesson.Day = DayOfWeek.Monday;
                    break;
            }
            lesson.Classroom = lessonMatch.Groups[2].ToString().Trim(' ');
            return lesson;
        }
        #endregion

    }
}
