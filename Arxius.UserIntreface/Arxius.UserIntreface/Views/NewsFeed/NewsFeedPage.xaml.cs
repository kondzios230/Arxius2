﻿using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class NewsFeedPage : ContentPage
    {
        public NewsFeedPage(INavigation _navi)
        {
            Title = Properties.Resources.PageNameNewsFeed;
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            BindingContext = new NewsFeedViewModel(_navi,this);
            NewsList.ItemSelected += (sender, e) => {
                ((ListView)sender).SelectedItem = null;
            };

        }
        public void SetRefreshImage(string imagePath)
        {
            RefreshButton.Source = imagePath;
        }

    }
}
