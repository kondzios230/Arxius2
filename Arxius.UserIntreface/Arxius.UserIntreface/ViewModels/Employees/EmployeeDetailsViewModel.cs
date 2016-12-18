using Arxius.Services.PCL;
using Arxius.Services.PCL.Entities;
using Arxius.Services.PCL.Interfaces_and_mocks;
using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;

namespace Arxius.UserIntreface.ViewModels
{
    class EmployeeDetailsViewModel : AbstractViewModel
    {
        public ICommand SendEmail { private set; get; }
        public ICommand OpenPage { private set; get; }
        private Employee emptyEmployee;

        public EmployeeDetailsViewModel(INavigation navi, Employee employee, Page page)
        {
            emptyEmployee = employee;
            uService = new UtilsService();
            GetEmployeDetailsAsync();
            _page = page;
            Navigation = navi;
            Refresh = new Command(() => GetEmployeDetailsAsync(true));
            SendEmail = new Command(ExecuteSendEmail,()=> { return Employee != null && Employee.Email.Length != 0; });
            OpenPage = new Command(ExecuteOpenPage, () => { return Employee != null && Employee.Url.Length != 0; });
        }
        private async void GetEmployeDetailsAsync(bool clear = false)
        {
           
            try
            {
                IsAIRunning = true;
                Employee = await uService.GetEmployeeDetails(emptyEmployee, clear);
            }
            catch (ArxiusException e)
            {
                MessagingCenter.Send(this, Properties.Resources.MsgNetworkError, e.Message);
            }
            IsAIRunning = false;
        }
        #region Bindable properties
        private Employee _employee;
        public Employee Employee
        {
            get { return _employee; }
            set
            {
                if (_employee != value)
                {
                    _employee = value;
                    OnPropertyChanged("Employee");
                    if (SendEmail != null)
                        ((Command)SendEmail).ChangeCanExecute();
                    if (OpenPage != null)
                        ((Command)OpenPage).ChangeCanExecute();
                }
            }
        }


        #endregion
       
        void ExecuteSendEmail()
        {
            var eMailService = DependencyService.Get<IOpenMailer>();
            if (eMailService != null&& Employee.Email!=null && Employee.Email.Length != 0)
                eMailService.SendMail(Employee.Email);
        }
        void ExecuteOpenPage()
        {
            if (Employee == null || Employee.Url == null) return;
            Device.OpenUri(new Uri(Employee.Url));
        }

    }
}