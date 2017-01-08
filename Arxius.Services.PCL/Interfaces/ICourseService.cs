using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces
{
    public interface ICourseService
    {
        Task<Dictionary<string, int>> SumAllECTSPoints(CancellationToken cT, Func<double,double> a, bool clean=false);
        Task<List<Course>> GetUserPlanForCurrentSemester(bool isOffline, bool clean = false);
        Task<List<Course>> GetAllUserCourses(bool clean = false);
        Task<List<Course>> GetAllCourses(bool clean = false);
        Task<Course> GetCourseWideDetails(Course course, bool clean = false);
        Task<Tuple<int, int, List<Student>>> GetStudentsList(_Class _class, bool clean = false);
        Task<bool> EnrollOrUnroll(_Class _class, bool clean = false);
    }
}
