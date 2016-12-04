using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class EmployeeListViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;
        private INavigation _navigation;
        public EmployeeListViewModel(INavigation navi)
        {
            _navigation = navi;
            GetEmployeeListAsync();
            ShowEmployee = new Command(ExecuteShowEmployee);
        }
        private async void GetEmployeeListAsync()
        {
            var s = new UtilsService();
            EmployeeList = await s.GetEmployees();

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

                    if (PropertyChanged != null)
                    {
                        PropertyChanged(this, new PropertyChangedEventArgs("EmployeeList"));

                    }
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
            await _navigation.PushAsync(new EmployeeDetailsPage(_navigation, SelectedEmployee  ));
        }
    }
}