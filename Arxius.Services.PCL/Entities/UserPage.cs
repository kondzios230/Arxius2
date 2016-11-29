using Arxius.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Entities
{
    public  class UserPage
    {        
        public int Ects { get; set; }
        public List<RegistrationTimesCollection> RegistrationTimes { get; set; }
        public string LimitRemovalTime { get; set; }
        public string EndTime { get; set; }
        public UserPage()
        {
            RegistrationTimes = new List<RegistrationTimesCollection>();
        }
    }

   
    
}
