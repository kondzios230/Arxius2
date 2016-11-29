﻿using Arxius.Services.PCL.Entities;
using Arxius.UserIntreface.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Arxius.UserIntreface
{
    public partial class NewsDetailsPage : ContentPage
    {
        public NewsDetailsPage(INavigation _navi,News _news)
        {
            InitializeComponent();
            BindingContext = new NewsDetailsViewModel(_navi,_news);
          
        }
        
    }
}