﻿
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Parsers
{
    public static class CoursesParsers
    {
        public static void GetCourseECTSandGroup(string page, Course course)
        {
            var kindMatch = Regex.Matches(page, @"<tr><th>Rodzaj<\/th><td>(.*?)<\/td>", RegexOptions.Multiline);
            if (kindMatch.Count != 0)
                course.Kind = kindMatch[0].Groups[1].ToString();
            var ectsMatch = Regex.Matches(page, @"<th>Punkty ECTS<\/th><td>(.*?)<\/td>", RegexOptions.Multiline);
            if (ectsMatch.Count != 0)
                course.Ects = Convert.ToInt32(ectsMatch[0].Groups[1].ToString());


        }
        public static void GetCourseWideDetails(string page, Course course)
        {
            page = page.Replace("\n", string.Empty);
            course.Classes.Clear();

            var detailsTableMatch = Regex.Match(page, @"<table class=""table-info"">(.*?)<\/table>");
            if (detailsTableMatch != null)
                parseDetailsTable(course, detailsTableMatch.Groups[1].ToString());

            var tutorialKindListMatch = Regex.Matches(page.Replace("\t",string.Empty), @"""tutorial""(.*?)<\/strong");
            foreach (var tutorialKindMatchSingle in tutorialKindListMatch)
            {
                var tutorialGroupString = (tutorialKindMatchSingle as Match).Groups[1].ToString();
                var courseTypeMatch = Regex.Match(tutorialGroupString, @"<h2>(.*?)<\/h2>", RegexOptions.Multiline);
                if (courseTypeMatch == null) continue;
                var typeSign = courseTypeMatch.Groups[1].ToString().Trim(' ')[0];
                var type = ClassTypeEnum.Other;
                switch (typeSign)
                {
                    case 'W':
                        type = ClassTypeEnum.Lecture;
                        break;
                    case 'P':
                        type = ClassTypeEnum.Lab;
                        break;
                    case 'C':
                    case 'Ć':
                        type = ClassTypeEnum.Excercise;
                        break;
                    case 'R':
                        type = ClassTypeEnum.Repetitory;
                        break;
                }
                var globalCourseMatches = Regex.Matches(tutorialGroupString, @"<tr (.*?)><td><a href=""(.*?)"" class=""person"">(.*?)<\/a><\/td><td class=""term"">(.*?)<\/td><td class=""number termLimit"">(.*?)<\/td><td class=""number termEnrolledCount"">(.*?)<\/td><td class=""number termQueuedCount"">(.*?)<\/td>(.*?)<\/tr>",RegexOptions.Multiline).Cast<Match>().ToList();
                var listOfEnrollmentMatches =new List<List<Group>>();
                var listOfEnrolledMatches =new List<List<Group>>();
                var listOfNotEnrolledMatches =new List<List<Group>>();
                foreach(var courseMatch in globalCourseMatches)
                {
                    var enrollmentMatch = Regex.Match(courseMatch.Groups[8].ToString(), @"<td class=""controls""><input type=""hidden"" name=""group-id"" value=""(.*?)""\/><input type=""hidden"" name=(.*?)}'\/><input type=""hidden"" name=""is-signed(.*?)value=""(.*?)""\/><form action=""(.*?)"" method=""post"" class=""setEnrolled""><div style='display:none'><input type='hidden' name='csrfmiddlewaretoken' value='(.*?)' \/><\/div><div><input type=""hidden"" name=""group"" value=""(.*?)""\/><input type=""hidden"" name=""enroll"" value=""(.*?)""\/><(.*?)setEnrolledButton"">(.*?)<\/button><\/div><\/form><a href=""(.*?)"">(.*?)<\/a><\/td><td class=""priority"">(.*?)<\/td>");
                    if(enrollmentMatch != null && enrollmentMatch.Groups.Count>1)
                    {
                        var head = courseMatch.Groups.Cast<Group>().ToList();
                        head.RemoveAt(head.Count - 1);
                        var tail = enrollmentMatch.Groups.Cast<Group>().ToList();
                        tail.RemoveAt(0);
                        head.AddRange(tail);
                        listOfEnrollmentMatches.Add(head);
                        continue;
                    }
                    var notEnrolledMatch = Regex.Match(courseMatch.Groups[8].ToString(), @"<td class=""controls""><input type=""hidden"" name=""group-id"" value=""(.*?)""\/><input type=""hidden"" name=""(.*?)}'\/><input type=""hidden"" name=""is(.*?)value=""false""\/><a href=""(.*?)"">(.*?)<\/a><\/td>");
                    if (notEnrolledMatch != null && notEnrolledMatch.Groups.Count > 1)
                    {
                        var head = courseMatch.Groups.Cast<Group>().ToList();
                        head.RemoveAt(head.Count - 1);
                        var tail = notEnrolledMatch.Groups.Cast<Group>().ToList();
                        tail.RemoveAt(0);
                        head.AddRange(tail);
                        listOfNotEnrolledMatches.Add(head);
                    }
                    var enrolledMatch = Regex.Match(courseMatch.Groups[8].ToString(),   @"<td class=""controls""><input type=""hidden"" name=""group-id"" value=""(.*?)""\/><input type=""hidden"" name=""(.*?)}'\/><input type=""hidden"" name=""is-(.*?)value=""true""\/><form><div><button (.*?)setEnrolledButton"">(.*?)<\/button><\/div><\/form><a href=""(.*?)"">(.*?)<\/a><\/td>");
                    if (enrolledMatch != null && enrolledMatch.Groups.Count > 1)
                    {
                        var head = courseMatch.Groups.Cast<Group>().ToList();
                        head.RemoveAt(head.Count - 1);
                        var tail = enrolledMatch.Groups.Cast<Group>().ToList();
                        tail.RemoveAt(0);
                        head.AddRange(tail);
                        listOfEnrolledMatches.Add(head);
                    }
                }
                if (listOfEnrollmentMatches.Count != 0)
                {
                    parseInEnrollmentClasses(listOfEnrollmentMatches, course, type);
                    continue;
                }
                if (listOfNotEnrolledMatches.Count != 0)
                {
                    parseNonEnrolledClasses(listOfNotEnrolledMatches, course, type);
                }
                if (listOfEnrolledMatches.Count != 0)
                {
                    parseEnrolledClasses(listOfEnrolledMatches, course, type);
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
                course.Classes = parseScheduleToClasses(match.Groups[4].ToString(), course);
                courseList.Add(course);
            }
            return courseList;
        }
        public static List<Course> GetAllCourses(string page, bool onlyUser)
        {
            var semesterMatches = Regex.Matches(page.Replace("\n",string.Empty), @"<div class=""semester""><h3>(.*?)<\/h3>(.*?)<\/div>");
            var listOfCourses = new List<Course>();
            foreach (Match semester in semesterMatches)
            {
                var allCoursesHtmlElements = Regex.Matches(semester.Groups[2].ToString(), @"<li>(.*?)<\/li>", RegexOptions.Multiline);
                var allCoursesStrings = new List<string>();
                if (onlyUser)
                    allCoursesStrings = allCoursesHtmlElements.Cast<Match>().ToList().Select(c => c.ToString().Replace("\n", string.Empty)).ToList().FindAll(c => c.Contains("name=\"wasEnrolled\" value=\"True\"")).ToList();
                else
                    allCoursesStrings = allCoursesHtmlElements.Cast<Match>().ToList().Select(c => c.ToString()).ToList();
                
                foreach (var courseString in allCoursesStrings)
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
                            SugestedFor1stYear = parsedCourseGroups.Groups[12].ToString() == "True",
                            Semester = Regex.Replace(semester.Groups[1].ToString(), "<.*?>", string.Empty)                            
                        });
                    }
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
            var messagesMatches = Regex.Matches(response, @"<div class=""alert-message info"">(.*?)<\/div>", RegexOptions.Multiline);            
            var listOfMessages = new List<string>();
            foreach (Match messageMatch in messagesMatches)
                listOfMessages.Add(messageMatch.Groups[1].ToString().Trim(' ')+".");
            var classesMatch = Regex.Matches(response, @"<tr(.*?)<\/tr>", RegexOptions.Multiline).Cast<Match>().ToList();
            var classMatch = classesMatch.FirstOrDefault(c => c.ToString().Contains(_class.enrollmentId));
            if (classMatch == null) throw new Exception();
            var r = Regex.Match(classMatch.ToString(), @"<input type=""hidden"" name=""is-signed-in""(.*?)value=""(.*?)""\/>(.*?)<button type=""(.*?)Button"">(.*?)<\/button>");
            return Tuple.Create(r.Groups[2].ToString() == "true", r.Groups[5].ToString().Trim(' '), listOfMessages);
        }
        #region Private Methods
        private static void parseInEnrollmentClasses(List<List<Group>> enrollmentCourseMatch, Course course, ClassTypeEnum type)
        {
            foreach (var courseMatch in enrollmentCourseMatch)
            {
                var _class = new _Class();
                _class.ClassType = type;
                _class.Teacher = new Employee() { Name = courseMatch[3].ToString().Trim(' '), Url = courseMatch[2].ToString() };
                var lessonMatches = Regex.Matches(courseMatch[4].ToString(), @">(.*?)\((.*?)\)", RegexOptions.Multiline);
                foreach (Match lessonMatch in lessonMatches)
                {
                    var l = parseToLesson(lessonMatch);
                    l.Course = course;
                    l.Type = _class.ClassType;
                    _class.Lessons.Add(l);
                }

                _class.TotalPeople = courseMatch[5].ToString().Trim(' ');
                _class.SignedInPeople = courseMatch[6].ToString().Trim(' ');
                _class.QueuedPeople = courseMatch[7].ToString().Trim(' ');
                _class.IsSignedIn = courseMatch[11].ToString().Trim(' ') == "true";
                _class.enrollmentUri = courseMatch[12].ToString().Trim(' ');
                _class.csrfToken = courseMatch[13].ToString().Trim(' ');
                _class.enrollmentId = courseMatch[14].ToString().Trim(' ');
                _class.ButtonEnrollText = courseMatch[17].ToString().Trim(' ').ToUpper();
                _class.ListUrl = courseMatch[18].ToString().Trim(' ');
                _class.IsEnrollment = true;
                _class.buttonListText = courseMatch[19].ToString().Trim(' ').ToUpper();
                _class.Priority = courseMatch[20].ToString().Trim(' ');
                course.Classes.Add(_class);
            }
        }
        private static void parseNonEnrolledClasses(List<List<Group>> nonEnrolledCourseMatch, Course course, ClassTypeEnum type)
        {
            foreach (var courseMatch in nonEnrolledCourseMatch)
            {
                var _class = new _Class();
                _class.ClassType = type;
                _class.Teacher = new Employee() { Name = courseMatch[3].ToString().Trim(' '), Url = courseMatch[2].ToString() };
                var lessonMatches = Regex.Matches(courseMatch[4].ToString(), @">(.*?)\((.*?)\)", RegexOptions.Multiline);
                foreach (Match lessonMatch in lessonMatches)
                {
                    var l = parseToLesson(lessonMatch);
                    l.Course = course;
                    l.Type = _class.ClassType;
                    _class.Lessons.Add(l);
                }
                _class.TotalPeople = courseMatch[5].ToString().Trim(' ');
                _class.SignedInPeople = courseMatch[6].ToString().Trim(' ');
                _class.QueuedPeople = courseMatch[7].ToString().Trim(' ');
                _class.IsSignedIn = false;
                _class.IsEnrollment = false;
                _class.ListUrl = courseMatch[11].ToString().Trim(' ').Trim('\\');
                _class.ButtonEnrollText = "ZAPISY";
                _class.buttonListText = courseMatch[12].ToString().Trim(' ').ToUpper();
                course.Classes.Add(_class);
            }
        }
        private static void parseEnrolledClasses(List<List<Group>> enrolledCourseMatch, Course course, ClassTypeEnum type)
        {
            foreach (var courseMatch in enrolledCourseMatch)
            {
                var _class = new _Class();
                _class.ClassType = type;
                _class.Teacher = new Employee() { Name = courseMatch[3].ToString().Trim(' '), Url = courseMatch[2].ToString() };
                var lessonMatches = Regex.Matches(courseMatch[4].ToString(), @">(.*?)\((.*?)\)", RegexOptions.Multiline);
                foreach (Match lessonMatch in lessonMatches)
                {
                    var l = parseToLesson(lessonMatch);
                    l.Course = course;
                    l.Type = _class.ClassType;
                    _class.Lessons.Add(l);
                }
                _class.TotalPeople = courseMatch[5].ToString().Trim(' ');
                _class.SignedInPeople = courseMatch[6].ToString().Trim(' ');
                _class.QueuedPeople = courseMatch[7].ToString().Trim(' ');
                _class.IsSignedIn = true;
                _class.ListUrl = courseMatch[13].ToString().Trim(' ');
                _class.IsEnrollment = !courseMatch[11].ToString().Contains("disabled");
                _class.ButtonEnrollText = courseMatch[12].ToString().Trim(' ').ToUpper();
                _class.buttonListText = courseMatch[14].ToString().Trim(' ').ToUpper();
                course.Classes.Add(_class);
            }
        }
        private static List<_Class> parseScheduleToClasses(string dMatch, Course course)
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
                    case 'r':
                        type = ClassTypeEnum.Repetitory;
                        break;
                }
                _class.ClassType = type;
                var lessonsMatch = Regex.Matches(typeStringMatch.Groups[2].ToString(), @"<span class=""term"">\s*?(.*?)\s*?(.*?)-(.*?)\s*?<\/span><span class=""classroom"">(.*?):(.*?)<\/span>", RegexOptions.Multiline);
                foreach (Match lessonMatch in lessonsMatch)
                {
                    var lesson = new Lesson();
                    lesson.Classroom = lessonMatch.Groups[5].ToString().Trim(' ');
                    var temp = lesson.Classroom.Split(' ').Where(s => !string.IsNullOrWhiteSpace(s));
                    lesson.Classroom = string.Join(" ", temp);
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
                    lesson.Type = _class.ClassType;
                    lesson.Course = course;
                    _class.Lessons.Add(lesson);
                }
                _class.IsSignedIn = true;
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
        private static void parseDetailsTable(Course course, string tableDetails)
        {
            var detailsTable = tableDetails;
            var allMatches = Regex.Matches(detailsTable, @"<th>(.*?)</th><td>(.*?)</td>");
            foreach (Match detailMatch in allMatches)
            {
                switch (detailMatch.Groups[1].ToString())
                {
                    case "Rodzaj":
                        {
                            course.Kind = detailMatch.Groups[2].ToString().Trim(' ');
                            break;
                        }
                    case "Punkty ECTS":
                        {
                            course.Ects = Convert.ToInt32(detailMatch.Groups[2].ToString().Trim(' '));
                            break;
                        }
                    case "Przedmiot przyjazny dla I roku":
                        {
                            course.SugestedFor1stYear = detailMatch.Groups[2].ToString().Trim(' ') == "Tak";
                            break;
                        }
                    case "Egzamin":
                        {
                            course.IsExam = detailMatch.Groups[2].ToString().Trim(' ') == "Tak";
                            break;
                        }
                    case "Grupa efektów kształcenia":
                        {
                            var match = Regex.Match(detailMatch.Groups[2].ToString(), @"<ul style=""list-style: none;""><li><span class=""label success"">(.*?)</span></li></ul>");
                            if (match != null)
                                course.GroupOfEffects = match.Groups[1].ToString().Trim(' ');
                            break;
                        }
                    case "Liczba godzin":
                        {
                            var dict = new Dictionary<string, int>();
                            var table = Regex.Matches(detailMatch.Groups[2].ToString().Replace(" ", string.Empty), @"(\d{1,3})\((.*?)\)", RegexOptions.Multiline);
                            if (table.Count != 0)
                            {
                                foreach (var match in table)
                                {
                                    var matchx = match as Match;
                                    dict.Add(matchx.Groups[2].ToString(), Convert.ToInt32(matchx.Groups[1].ToString()));
                                }
                                course.HoursSchema = dict;
                            }
                            break;
                        }
                }

            }
        }
        #endregion

    }
}
