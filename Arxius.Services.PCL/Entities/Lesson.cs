using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Entities
{
    public class Lesson
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DayOfWeek Day { get; set; }
        public string Classroom { get; set; }
        public string Print { get { return string.Format("{0}-{1} {2} {3}", StartTime.ToString("HH:mm"), EndTime.ToString("HH:mm"), Day, Classroom); } }
    }
}
