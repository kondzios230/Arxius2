using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Entities
{
    public class EnrollmentClass : _Class
    {
        public string csrfToken { get; set; }
        public string enrollmentUri { get; set; }
        public string enrollmentId { get; set; }
        public string buttonEnrollText { get; set; }
        public string buttonListText { get; set; }
        public EnrollmentClass(_Class _class)
        {
            ClassType = _class.ClassType;
            //StartTime = _class.StartTime;
            //EndTime = _class.EndTime;
            //Day = _class.Day;
            //Classroom = _class.Classroom;
            Teacher = _class.Teacher;
            Total = _class.Total;
            SignedIn = _class.SignedIn;
            Queued = _class.Queued;
        }
        public EnrollmentClass()
        {
        }
        //public string FriendlyStartTime { get { return StartTime.ToString("HH:mm"); } }
        //public string FriendlyEndTime { get { return EndTime.ToString("HH:mm"); } }
        //public string FriendlyTimeSpan { get { return FriendlyStartTime + "-" + FriendlyEndTime; } }
      
    
    }
    }
