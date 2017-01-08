using System.Collections.Generic;
namespace Arxius.Services.PCL.Entities
{
    public  class UserPage
    {        
        public int Ects { get; set; }
        public List<CourseGroupedCollection> RegistrationTimes { get; set; }
        public string LimitRemovalTime { get; set; }
        public string EndTime { get; set; }
        public UserPage()
        {
            RegistrationTimes = new List<CourseGroupedCollection>();
        }
    }

   
    
}
