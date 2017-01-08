
using System.Collections.Generic;
namespace Arxius.Services.PCL.Entities
{
    public class Course
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string CourseID { get; set; }
        public string Kind { get; set; }
        public string GroupOfEffects { get; set; }
        public string Semester { get; set; }

        public List<_Class> Classes { get; set; }

        public Dictionary<string, int> HoursSchema { get; set; }

        public int Type { get; set; }
        public int Ects { get; set; }

        public bool WasEnrolled { get; set; }
        public bool IsEnglish { get; set; }
        public bool IsExam { get; set; }
        public bool SugestedFor1stYear { get; set; }

        public string Notes { get; set; }
        public Course()
        {
            Classes = new List<_Class>();
            HoursSchema = new Dictionary<string, int>();
        }
     
    }


    public class CourseNameComparer : IEqualityComparer<Course>
    {

        public int GetHashCode(Course obj)
        {
            return 1;
        }

        bool IEqualityComparer<Course>.Equals(Course x, Course y)
        {
            return string.Equals(x.Name, y.Name);
        }
    }


}
