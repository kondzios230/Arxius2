using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class EmployeeListViewModel : AbstractViewModel
    {
        public ICommand ShowEmployee { private set; get; }
        public EmployeeListViewModel(INavigation navi,Page page)
        {
            uService = new UtilsService();
            _page = page;
            Navigation = navi;
            GetEmployeeListAsync();
            ShowEmployee = new Command(async () => await Navigation.PushAsync(new EmployeeDetailsPage(Navigation, SelectedEmployee)));
            Refresh = new Command(() => GetEmployeeListAsync(true));
        }
        private async void GetEmployeeListAsync(bool clear = false)
        {
            try
            {
                IsAIRunning = true;
                EmployeeList = await uService.GetEmployees(clear);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
        }
        #region Bindable properties

        private List<Employee> _employeeList;

        public List<Employee> EmployeeList
        {
            get { return _employeeList; }
            set
            {
                if (_employeeList != value)
                {
                    _employeeList = value;
                    OnPropertyChanged("EmployeeList");
                }
            }
        }
        private Employee _selectedEmployee;
        public Employee SelectedEmployee
        {
            get
            {
                return _selectedEmployee;
            }
            set
            {
                _selectedEmployee = value;

                if (_selectedEmployee == null)
                    return;

                ShowEmployee.Execute(SelectedEmployee);
                SelectedEmployee = null;
            }
        }

        #endregion
       
      
    }
}