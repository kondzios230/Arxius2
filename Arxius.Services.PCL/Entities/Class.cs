using System.Collections.Generic;
using System.ComponentModel;

namespace Arxius.Services.PCL.Entities
{
    public class _Class : INotifyPropertyChanged
    {
        public ClassTypeEnum ClassType { get; set; }
        public List<Lesson> Lessons { get; set; }
       
        public Employee Teacher { get; set; }
        public string TotalPeople { get; set; }
        public string SignedInPeople { get; set; }
        public string QueuedPeople { get; set; }
        public bool IsSignedIn { get; set; }
        public string ListUrl { get; set; }
        public string Priority { get; set; }
        public string csrfToken { get; set; }
        public string enrollmentUri { get; set; }
        public string enrollmentId { get; set; }
        private string _buttonEnrollText { get; set; }
        public string ButtonEnrollText
        {
            set
            {
                _buttonEnrollText = value;

                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("ButtonEnrollText"));
                }
            }
            get
            {
                
                return _buttonEnrollText;
            }
        }
        public string buttonListText { get; set; }
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
