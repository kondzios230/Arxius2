﻿using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class EmployeeListPage : ContentPage
    {
        public EmployeeListPage(INavigation _navi)
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new EmployeeListViewModel(_navi);
            EmployeeList.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };
        }
        
    }
}
