using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Threading.Tasks;
using System.Timers;
using APP_GACH_NO.Interface;
using APP_GACH_NO.ViewModels;
using Plugin.Connectivity;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_GACH_NO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {

        public LoginPage()
        {
            InitializeComponent();
            this.BindingContext = new LoginViewModel();
            // NavigationPage.SetHasNavigationBar(this, false);
            DependencyService.Get<IStatusBar>().ShowStatusBar();
            SetValue(NavigationPage.HasNavigationBarProperty, false);
            DependencyService.Get<IStatusBar>().HideStatusBar();
        }
        protected override void OnAppearing()
        {

            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {

            base.OnDisappearing();
        }
    }

}