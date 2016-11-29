using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class ProfileViewModel : INotifyPropertyChanged
    {
      
        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public ProfileViewModel(INavigation navi)
        {
            _navigation = navi;
            GetProfileAsync();
            EctsPoints = new List<string>() { "To może trochę potrwać" };
           // ShowCourse = new Command(ExecuteShowPlan);
        }
        private async void GetProfileAsync()
        {
            var s = new CoursesService();
            var ects= await s.SumAllECTSPoints();
            EctsPoints = new List<string>();
            foreach (var val in ects.Keys)
                EctsPoints.Add(string.Format("{0}: {1}", val, ects[val]));
        }

        private List<string> _ectsPoints;

        public List<string> EctsPoints
        {
            get { return _ectsPoints; }
            set
            {
                if (_ectsPoints != value)
                {
                    _ectsPoints = value;

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("EctsPoints"));
                    }
                }
            }
        }


    }
}