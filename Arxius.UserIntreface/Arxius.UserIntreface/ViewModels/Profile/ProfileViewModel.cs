using Arxius.Services.PCL;
using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class EctsViewModel : AbstractViewModel
    {
        public EctsViewModel(INavigation navi,Page page)
        {
            _page = page;
            Navigation = navi;
            GetProfileAsync();
            EctsPoints = new List<string>() { "To może trochę potrwać...." };
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
                    OnPropertyChanged("EctsPoints");
                }
            }
        }


    }
}