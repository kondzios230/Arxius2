using System;
using System.Collections.Generic;

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
