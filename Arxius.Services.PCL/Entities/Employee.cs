

using Arxius.CrossLayer.PCL;
using System.Collections.Generic;
using System.ComponentModel;

namespace Arxius.Services.PCL.Entities
{
    public class Employee : INotifyPropertyChanged
    {
        public string Name { get; set; }
        public string FirstLetterOfName { get; set; }
        public string Url { get; set; }
        public string Email { get; set; }
        public string Room { get; set; }
        public string Consults { get; set; }
        private List<GenericGroupedCollection<string, string>> _weekPlan;

        public List<GenericGroupedCollection<string, string>> WeekPlan
        {
            get { return _weekPlan; }
            set
            {
                _weekPlan = value;
                if (PropertyChanged != null)
                    PropertyChanged(this, new PropertyChangedEventArgs("WeekPlan"));
            }
        }


        public Employee()
        {

        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
