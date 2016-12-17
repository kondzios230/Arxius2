using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class EctsViewModel : AbstractViewModel
    {
        public EctsViewModel(INavigation navi,Page page)
        {
            _page = page;
            Navigation = navi;
            Refresh = new Command(ExecuteRefresh);
            EctsPoints = new List<string>() { "To może trochę potrwać...." };
            GetProfileAsync();
        }
        private async void GetProfileAsync()
        {
            try
            {
                var s = new CoursesService();
                var ects = await s.SumAllECTSPoints();
                var l = new List<string>();
                foreach (var val in ects.Keys)
                    l.Add(string.Format("{0}: {1}", val, ects[val]));
                EctsPoints = l;
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
           
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
        public ICommand Refresh { private set; get; }
        async void ExecuteRefresh()
        {
            try
            {
                (_page as EctsPage).SetRefreshImage("refresh2.jpg");
                var s = new CoursesService();
                var ects = await s.SumAllECTSPoints(true);
                var l = new List<string>();
                foreach (var val in ects.Keys)
                    l.Add(string.Format("{0}: {1}", val, ects[val]));
                EctsPoints = l;
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            
            (_page as EctsPage).SetRefreshImage("refresh.jpg");
        }


    }
}