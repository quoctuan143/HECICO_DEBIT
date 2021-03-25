using APP_GACH_NO.Dialog;
using APP_GACH_NO.Global;
using APP_GACH_NO.Interface;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_GACH_NO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            SetValue(NavigationPage.HasNavigationBarProperty, false);
            DependencyService.Get<IStatusBar>().HideStatusBar();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
           
            if (!CrossConnectivity.Current.IsConnected)
            {
                await new MessageInternetProblem().Show();
                System.Diagnostics.Process.GetCurrentProcess().CloseMainWindow();
                return;
            }
            
            await Task.Delay(2000);
            App.Current.MainPage = new AppShell();   
        }

        

    }
}