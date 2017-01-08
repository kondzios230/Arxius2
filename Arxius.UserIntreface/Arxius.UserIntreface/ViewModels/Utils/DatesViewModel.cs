using Arxius.CrossLayer.PCL;
using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class DatesViewModel : AbstractViewModel
    {
        public DatesViewModel(INavigation navi,Page page)
        {
            uService = new UtilsService();
            GetDatesAsync();
            _page = page;
            Navigation = navi;
            Refresh = new Command(() => GetDatesAsync(true));
        }
        private async void GetDatesAsync(bool clear = false)
        {           
            try
            {
                IsAIRunning = true;
                Dates = await uService.GetImportantDates(clear);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
        }
        #region Bindable properties
        private List<GenericGroupedCollection<string, string>> _dates;
        public List<GenericGroupedCollection<string, string>> Dates
        {
            get { return _dates; }
            set
            {
                if (_dates != value)
                {
                    _dates = value;
                    OnPropertyChanged("Dates");
                }
            }
        }


        #endregion
      
    }
}