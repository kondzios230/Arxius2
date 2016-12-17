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
        public EmployeeListViewModel(INavigation navi,Page page)
        {
            uService = new UtilsService();
            _page = page;
            Navigation = navi;
            GetEmployeeListAsync();
            ShowEmployee = new Command(ExecuteShowEmployee);
            Refresh = new Command(ExecuteRefresh);
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
        public ICommand ShowEmployee { private set; get; }
        async void ExecuteShowEmployee()
        {
            await Navigation.PushAsync(new EmployeeDetailsPage(Navigation, SelectedEmployee  ));
        }
        public ICommand Refresh { private set; get; }
        void ExecuteRefresh()
        {
            GetEmployeeListAsync(true);
        }
    }
}