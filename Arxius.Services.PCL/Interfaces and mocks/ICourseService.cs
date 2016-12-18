using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces_and_mocks
{
    public interface ICourseService
    {
        Task<Dictionary<string, int>> SumAllECTSPoints(CancellationToken cT, bool clean=false);
        Task<List<Course>> GetUserPlanForCurrentSemester(bool clean = false);
        Task<List<Course>> GetAllUserCourses(bool clean = false);
        Task<List<Course>> GetAllCourses(bool clean = false);
        Task<Course> GetCourseWideDetails(Course course, bool clean = false);
        Task<Tuple<int, int, List<Student>>> GetStudentsList(_Class _class, bool clean = false);
        Task<Tuple<bool, string, List<string>>> EnrollOrUnroll(_Class _class, bool clean = false);
    }
}
