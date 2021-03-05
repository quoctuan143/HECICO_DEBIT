using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_GACH_NO.Services;
using APP_GACH_NO.Views;
using APP_GACH_NO.Renderer;
using APP_GACH_NO.Global;
using APP_GACH_NO.Interface;
using Plugin.Connectivity;
using Plugin.Connectivity.Abstractions;
using APP_GACH_NO.Dialog;
using System.Timers;
using System.Linq;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace APP_GACH_NO
{
    public partial class App : Application
    {
        public static Timer timer;
        public static int DemNguoc = 0;
        public static MessageTimeOut showTimeout;
        public App()
        {

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MzA4MTIyQDMxMzgyZTMyMmUzME4rVWJvRGdVY0ZibWlYbUFBN1dyNVFjemJ5djZ5dWQzZzdMaDNEQ1hBN3M9");
            InitializeComponent();

            timer = new Timer();
            timer.Enabled = true;
            timer.Interval = 1000;
            timer.Elapsed += Timer_Elapsed;
            DependencyService.Register<MockDataStore>();
            DependencyService.Register<IProcessLoader>();
            Config.client = new System.Net.Http.HttpClient();
            showTimeout = new MessageTimeOut();
            MainPage = new StartPage();
        }

        private async void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            DemNguoc += 1;
            if (DemNguoc > 120)
            {
                var page = Shell.Current.CurrentItem;
                if (page.Route.Contains("Login") == true) return;
                if (page.Route.Contains("Register") == true) return;
                if (showTimeout.isShow == false)
                    await showTimeout.Show();
            }
        }

        protected override void OnStart()
        {


        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
            if (DemNguoc < 120)
            {
                DemNguoc = 0;
            }
        }
    }
}
