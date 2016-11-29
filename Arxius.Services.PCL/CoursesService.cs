using Arxius.DataAccess;
using Arxius.DataAccess.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Parsers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL
{
    public class CoursesService
    {
        public async Task<Course> GetCourseDetails(Course course)
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri,course.Url));
            CoursesParsers.GetCourseDetails(page, course);
            return course;
        }
        public async Task<List<Course>> GetAllUserCoursesWithDetails()
        {            
            var courses = await GetAllUserCourses();
            foreach (var course in courses)
            {
                await GetCourseDetails(course);
            }
            return courses;
        }
        public async Task<Dictionary<string, int>> SumAllECTSPoints()
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
        }

        public async Task<List<Course>> GetUserPlanForCurrentSemester()
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri,"/records/schedule/"));
            return CoursesParsers.GetUserPlanForCurrentSemester(page);
        }
        public async Task<List<Course>> GetAllUserCourses()
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/courses/"));
            return CoursesParsers.GetAllUserCourses(page);
        }
        public async Task<List<Course>> GetAllCourses()
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, "/courses/"));
            return CoursesParsers.GetAllCourses(page);
        }
        public async Task<Course> GetCourseWideDetails(Course course)
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, course.Url));
            CoursesParsers.GetCourseWideDetails(page, course);
            return course;
        }
        public async Task<Tuple<int,int, List<Student>>> GetStudentsList(_Class _class)
        {
            var page = await HTMLUtils.GetPage(string.Format(Properties.Resources.baseUri, _class.ListUrl));
            return  CoursesParsers.GetStudentsList(page); 
        }
        public void EnrollOrUnroll(EnrollmentClass _class)
        {
            if (Properties.Resources.baseUri.Contains("zapisy")) return; //safety first
            HTMLUtils.PostString(string.Format(Properties.Resources.baseUri,"/records/set-enrolled"), string.Format("csrfmiddlewaretoken={0}&group={1}&enroll={2}",HTMLUtils.csrfToken,_class.enrollmentId,!_class.IsSignedIn));
        }
    }
}
 