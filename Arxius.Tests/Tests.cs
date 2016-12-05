using Arxius.DataAccess;
using Arxius.DataAccess.PCL;
using Arxius.Services;
using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Arxius.Services.PCL.Entities.Lesson;

namespace Arxius.Tests
{
    public class Tests
    {
        public async Task<bool> IsAuthenticatedSuccessfully()
        {
            return await AuthDoNotSync.Login();
        }
        public async Task<bool> IsCorrectNumberOfCoursesInCurrentSemesterReturned()
        {
            var sService = new CoursesService();
            var x = await sService.GetUserPlanForCurrentSemester();
            Foo(x);
            return (x.Count == 2);
        }

        private void Foo(List<Course> _Schedule)
        {
            var lStart = new List<Lesson>();
            var lEnd = new List<Lesson>();
            foreach (var course in _Schedule)
            {
                foreach (var c in course.Classes)
                {
                    lStart.AddRange(c.Lessons);
                    lEnd.AddRange(c.Lessons);
                }
            }
            lStart.Sort(new LessonStartTimeComparer());
            lEnd.Sort(new LessonStartTimeComparer());
            var firstTime = lStart[0].StartTime;
            var lastTime = lEnd[lEnd.Count - 1].EndTime;
            var hours = lastTime.Hour - firstTime.Hour;
            //GenerateGrid(hours, firstTime.Hour);
            MapLessons(lStart,hours,firstTime.Hour);
        }
        public void MapLessons(List<Lesson> lessons, int hours, int begingHour)
        {
            var listOfStudyHours = new List<StudyHour>();
            foreach (var lesson in lessons)
            {
                var d = (int)lesson.Day;
                var deltaStart = lesson.StartTime.Hour - begingHour;
                var deltaEnd = lesson.EndTime.Hour - begingHour;
              
                for (var dT = deltaStart; dT < deltaEnd; dT++)
                {
                    listOfStudyHours.Add(new StudyHour() { Lesson = lesson, Hour = dT });
                }
                
            }
            var lessonsGroups = listOfStudyHours.GroupBy(c => new { c.Hour, c.Lesson.Day });
        }


        public async Task<bool> IsCorrectNumberOfAllTimeCourses()
        {
            var sService = new CoursesService();
            var x = await sService.GetAllUserCourses();
            return (x.Count == 29);
        }
        public async Task<bool> IsNewsfeedDownloadSuccesfull()
        {
            var uService = new UtilsService();
            var x = await uService.GetFeedPage(2);
            return (!x.Any(n=>n.Content==null));
        }
        public async Task<bool> IsEmployeeListDownloaded()
        {
            var uService = new UtilsService();
            var x = await uService.GetEmployees();
            return (x.Count ==142);
        }
        public async Task<bool> IsUserPageDownloadSuccesfull()
        {
            var uService = new UtilsService();
            var x = await uService.GetUserPage();
            int ct = 1;
            foreach(var i in x.RegistrationTimes)
            {
                if (ct == 1 && i.Count != 13)
                    return false;
                if (ct == 2 && i.Count != 42)
                    return false;
                ct++;
            }
            return true;
        }
    }
}
