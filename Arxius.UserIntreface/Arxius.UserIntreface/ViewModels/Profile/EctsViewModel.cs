using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class EctsViewModel : AbstractViewModel
    {
        public CancellationTokenSource cT;
        public EctsViewModel(INavigation navi,Page page)
        {
            cT = new CancellationTokenSource();
            cService = new CoursesService();  
            _page = page;
            Navigation = navi;
            Refresh = new Command(()=> GetProfileAsync(true));
            EctsPoints = new List<string>();
            GetProfileAsync();
        }
        private async void GetProfileAsync(bool clear = false)
        {
            try
            {
                IsAIRunning = true;
                var ects = await cService.SumAllECTSPoints(cT.Token, (d) => Progress = (Progress+d)%1,  clear);
                var l = new List<string>();
                foreach (var val in ects.Keys)
                    l.Add(string.Format("{0}: {1}", val, ects[val]));
                EctsPoints = l;
                Progress = 1;
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            catch (Exception)
            {
            }
            IsAIRunning = false;
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

        private double _progress;
        public double Progress
        {
            get { return _progress; }
            set
            {
                if (_progress != value)
                {
                    _progress = value;
                    OnPropertyChanged("Progress");
                }
            }
        }


        


    }
}