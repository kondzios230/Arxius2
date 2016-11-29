using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Entities
{
    public class RegistrationTimesCollection : List<Course>
    {
        public string Time { get; set; }
        public RegistrationTimesCollection(DateTime time)
        {
            Time = time.ToString();
        }

        public static IList<Course> All { private set; get; }
    }
}
