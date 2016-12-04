using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Interfaces_and_mocks
{
    public interface ICourseService
    {
        Task<Dictionary<string, int>> SumAllECTSPoints();
        Task<List<Course>> GetUserPlanForCurrentSemester();
        Task<List<Course>> GetAllUserCourses();
        Task<List<Course>> GetAllCourses();
        Task<Course> GetCourseWideDetails(Course course);
        Task<Tuple<int, int, List<Student>>> GetStudentsList(_Class _class);
        Task<Tuple<bool, string, List<string>>> EnrollOrUnroll(_Class _class);
    }
}
