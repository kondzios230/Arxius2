using Arxius.DataAccess.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using Arxius.Services.PCL.Parsers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.Services.PCL
{
    public class CoursesService : ICourseService
    {
        public async Task<Dictionary<string, int>> SumAllECTSPoints(bool clean = false)
        {
            return await Cache.Get("SumAllECTSPoints", async () =>
            {
                var ret = new Dictionary<string, int>();
                var courses = await GetAllUserCoursesWithDetails();
                var groupedCourses = courses.GroupBy(c => c.Kind);
                foreach (var group in groupedCourses)
                {
                    int ects = 0;
                    foreach (var course in group.ToList())
                        ects += course.Ects;
                    ret.Add(group.Key, ects);
                }
                return ret;
            }, clean);
        }
        public async Task<List<Course>> GetUserPlanForCurrentSemester(bool clean = false)
        {
            return await Cache.Get("GetUserPlanForCurrentSemester", async () =>
            {
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/records/schedule/"));
                return CoursesParsers.GetUserPlanForCurrentSemester(page);
            }, clean);
        }
        public async Task<List<Course>> GetAllUserCourses(bool clean = false)
        {
            return await Cache.Get("GetAllUserCourses", async () =>
            {
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/courses/"));
                return CoursesParsers.GetAllUserCourses(page);
            }, clean);
        }
        public async Task<List<Course>> GetAllCourses(bool clean = false)
        {
            return await Cache.Get("GetAllCourses", async () =>
            {
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/courses/"));
                return CoursesParsers.GetAllCourses(page);
            }, clean);
        }
        public async Task<Course> GetCourseWideDetails(Course course, bool clean = false)
        {
            var ret = await Cache.Get(new { c = course, a = "GetCourseWideDetails" }, async () =>
            {
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, course.Url));
                CoursesParsers.GetCourseWideDetails(page, course);

                return course;
            }, clean);
            var fileService = DependencyService.Get<ISaveAndLoad>();
            if (fileService != null && fileService.FileExists(string.Format(Properties.Resources.FileName, course.CourseID)))
                ret.Notes = await fileService.LoadTextAsync(string.Format(Properties.Resources.FileName, course.CourseID));
            return ret;

            //var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, course.Url));
            //CoursesParsers.GetCourseWideDetails(page, course);            
            //return course;
        }
        public async Task<Tuple<int, int, List<Student>>> GetStudentsList(_Class _class, bool clean = false)
        {
            return await Cache.Get(new { c = _class, a = "GetStudentsList" }, async () =>
            {
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, _class.ListUrl));
                return CoursesParsers.GetStudentsList(page);
            }, clean);
        }
        public async Task<Tuple<bool, string, List<string>>> EnrollOrUnroll(_Class _class, bool clean = false)
        {
            if (Properties.Resources.baseUri.Contains("zapisy")) throw new Exception(); //safety first
            var response = await HTMLUtils.PostString(string.Format(Properties.Resources.baseUri, "/records/set-enrolled"), string.Format("csrfmiddlewaretoken={0}&group={1}&enroll={2}", HTMLUtils.csrfToken, _class.enrollmentId, (!_class.IsSignedIn).ToString().ToLower()));
            var sigingResult = CoursesParsers.IsSignedIn(response, _class);
            return Tuple.Create(sigingResult.Item1 != _class.IsSignedIn, sigingResult.Item2, sigingResult.Item3); //if differs, then some error must have occured
        }

        private async Task<List<Course>> GetAllUserCoursesWithDetails(bool clean = false)
        {
            return await Cache.Get("GetCourseWideDetails", async () =>
            {
                var courses = await GetAllUserCourses();
                foreach (var course in courses)
                {
                    await GetCourseDetails(course);
                }
                return courses;
            }, clean);
        }
        private async Task<Course> GetCourseDetails(Course course, bool clean = false)
        {
            return await Cache.Get(new { c = course, a = "GetCourseDetails" }, async () =>
            {
                var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, course.Url));
                CoursesParsers.GetCourseECTSandGroup(page, course);
                return course;
            }, clean);
        }
    }
}
