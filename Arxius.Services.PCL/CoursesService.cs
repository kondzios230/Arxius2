﻿using Arxius.DataAccess.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces;
using Arxius.Services.PCL.Parsers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.Services.PCL
{
    public class CoursesService : ICourseService
    {
        public async Task<Dictionary<string, int>> SumAllECTSPoints(CancellationToken cT, Func<double, double> a, bool clean = false)
        {
            return await Cache.Get("SumAllECTSPoints", async () =>
            {
                var ret = new Dictionary<string, int>();
                var courses = await GetAllUserCoursesWithDetails(cT, a);
                if (cT.IsCancellationRequested)
                    throw new Exception();

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
        public async Task<List<Course>> GetUserPlanForCurrentSemester(bool IsOffline,bool clean = false)
        {
            if(IsOffline)
            {
                var fileService = DependencyService.Get<ISaveAndLoad>();
                if (fileService == null || !fileService.FileExists("Schedule1.txt"))
                    throw new ArxiusFileException("Brak zapisanego offline planu, aby go wygenerować, wejdź na widok planu zajęć");
                else
                {
                    var page = await fileService.LoadTextAsync("Schedule1.txt");
                    if (page == null || page.Length == 0)
                        throw new ArxiusException();
                   return CoursesParsers.GetUserPlanForCurrentSemester(page);
                }
            }
            return await Cache.Get("GetUserPlanForCurrentSemester", async () =>
            {
                var page = await HTMLUtils.GetPage("/records/schedule/");
                var fileService = DependencyService.Get<ISaveAndLoad>();
                try
                {
                    await fileService.SaveTextAsync("Schedule1.txt", page);
                }
                catch
                {
                }
                return CoursesParsers.GetUserPlanForCurrentSemester(page);
            }, clean);
        }
        public async Task<List<Course>> GetAllUserCourses(bool clean = false)
        {
            return await Cache.Get("GetAllUserCourses", async () =>
            {
                var page = await HTMLUtils.GetPage("/courses/");
                return CoursesParsers.GetAllCourses(page, true);
            }, clean);
        }
        public async Task<List<Course>> GetAllCourses(bool clean = false)
        {
            return await Cache.Get("GetAllCourses", async () =>
            {
                var page = await HTMLUtils.GetPage("/courses/");
                return CoursesParsers.GetAllCourses(page, false);
            }, clean);
        }
        public async Task<Course> GetCourseWideDetails(Course course, bool clean = false)
        {
            var ret = await Cache.Get(new { c = course, a = "GetCourseWideDetails" }, async () =>
            {
                var s = new Stopwatch();
                var page = await HTMLUtils.GetPage(course.Url);
                s.Start();
                CoursesParsers.GetCourseWideDetails(page, course);
                s.Stop();
                Debug.WriteLine("Parsing - {0}", s.Elapsed);

                return course;
            }, clean);
            var fileService = DependencyService.Get<ISaveAndLoad>();
            if (fileService != null && fileService.FileExists(string.Format(Properties.Resources.FileName, course.Name)))
                ret.Notes = await fileService.LoadTextAsync(string.Format(Properties.Resources.FileName, course.Name));
            return ret;
        }
        public async Task<Tuple<int, int, List<Student>>> GetStudentsList(_Class _class, bool clean = false)
        {
            return await Cache.Get(new { c = _class, a = "GetStudentsList" }, async () =>
            {
                var page = await HTMLUtils.GetPage(_class.ListUrl);
                return CoursesParsers.GetStudentsList(page);
            }, clean);
        }
        public async Task<bool> EnrollOrUnroll(_Class _class, bool clean = false)
        {
            
            var response = await HTMLUtils.PostString("/records/set-enrolled", string.Format("csrfmiddlewaretoken={0}&group={1}&enroll={2}", HTMLUtils.csrfToken, _class.enrollmentId, (!_class.IsSignedIn).ToString().ToLower()));
            var sigingResult = CoursesParsers.IsSignedIn(response, _class);
            return sigingResult.Item1 != _class.IsSignedIn; //if differs, then some error must have occured
        }

        private async Task<List<Course>> GetAllUserCoursesWithDetails(CancellationToken cT, Func<double, double> a)
        {
            var courses = await GetAllUserCourses();
            var diff = 1d / (courses.Count + 1d);
            a(diff);
            foreach (var course in courses)
            {
                if (cT.IsCancellationRequested)
                    break;
                await GetCourseECTSPoints(course);
                a(diff);
            }
            return courses;
        }
        private async Task<Course> GetCourseECTSPoints(Course course, bool clean = false)
        {
            var page = await HTMLUtils.GetPage(course.Url);
            CoursesParsers.GetCourseECTSandGroup(page, course);
            return course;
        }
    }
}
