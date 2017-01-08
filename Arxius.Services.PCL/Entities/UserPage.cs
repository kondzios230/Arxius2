using System.Collections.Generic;
using Arxius.CrossLayer.PCL;
namespace Arxius.Services.PCL.Entities
{
    public  class UserPage
    {        
        public int Ects { get; set; }
        public List<GenericGroupedCollection<string,Course>> RegistrationTimes { get; set; }
        public string LimitRemovalTime { get; set; }
        public string EndTime { get; set; }
        public UserPage()
        {
            RegistrationTimes = new List<GenericGroupedCollection<string, Course>>();
        }
    }

   
    
}
