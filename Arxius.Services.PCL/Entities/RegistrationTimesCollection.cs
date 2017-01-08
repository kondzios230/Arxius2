using System;
using System.Collections.Generic;

namespace Arxius.Services.PCL.Entities
{
    public class CourseGroupedCollection : List<Course>
    {
        public string Key { get; set; }
        public CourseGroupedCollection(string time)
        {
            Key = time;
        }

        public static IList<Course> All { private set; get; }
    }
}
