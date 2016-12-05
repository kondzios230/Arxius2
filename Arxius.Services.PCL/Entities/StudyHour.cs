using System;
using System.Collections.Generic;

namespace Arxius.Services.PCL.Entities
{
    public class StudyHour
    {
        public Lesson Lesson { get; set; }
        public int Hour { get; set; }
    }
}
