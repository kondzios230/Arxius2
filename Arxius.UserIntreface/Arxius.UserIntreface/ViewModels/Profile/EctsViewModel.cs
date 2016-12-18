﻿using Arxius.Services.PCL;
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
            cService = new CoursesService();  
            _page = page;
            Navigation = navi;
            Refresh = new Command(ExecuteRefresh);
            EctsPoints = new List<string>();
            GetProfileAsync();
        }
        private async void GetProfileAsync(bool clear = false)
        {
            try
            {
                IsAIRunning = true;
                var ects = await cService.SumAllECTSPoints(clear);
                var l = new List<string>();
                foreach (var val in ects.Keys)
                    l.Add(string.Format("{0}: {1}", val, ects[val]));
                EctsPoints = l;
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
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
        public ICommand Refresh { private set; get; }
        void ExecuteRefresh()
        {
            GetProfileAsync(true);
        }


    }
}