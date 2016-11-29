using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arxius.Services.PCL.Entities
{
    public class _Class
    {
        public ClassTypeEnum ClassType { get; set; }
        public List<Lesson> Lessons { get; set; }
       
        public Employee Teacher { get; set; }
        public string Total { get; set; }
        public string SignedIn { get; set; }
        public string Queued { get; set; }
        public bool IsSignedIn { get; set; }
        public string ListUrl { get; set; }
        public string Priority { get; set; }
        public _Class()
        {
            Lessons = new List<Lesson>();
        }

        public string FriendlyLessons
        {
            get
            {
                var s = "";
                foreach (var l in Lessons)
                    s += l.Print + '\n';
                return s;
            }
        }
        public string FriendlyClassType { get { return ClassTypeEnums.ToFriendlyString(ClassType); } }
    }
}
