using System;
using System.Collections.Generic;

namespace Arxius.Services.PCL.Entities
{
    public class Lesson
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DayOfWeek Day { get; set; }
        public string CourseName { get; set; }
        public string Classroom { get; set; }
        public string Print { get { return string.Format("{0}-{1} {2} {3}", StartTime.ToString("HH:mm"), EndTime.ToString("HH:mm"), Day, Classroom); } }

        public class LessonStartTimeComparer : IComparer<Lesson>
        {
            public int Compare(Lesson x, Lesson y)
            {
                return DateTime.Compare(x.StartTime, y.StartTime);
            }

            public int GetHashCode(Lesson obj)
            {
                return 1;
            }
            
        }
        public class LessonEndTimeComparer : IComparer<Lesson>
        {

            public int Compare(Lesson x, Lesson y)
            {
                return DateTime.Compare(x.EndTime, y.EndTime);
            }
            public int GetHashCode(Lesson obj)
            {
                return 1;
            }
        }
    }
}
