using System;
using System.Collections.Generic;
using APP_GACH_NO.ViewModels;
using APP_GACH_NO.Views;
using Xamarin.Forms;
using Xamarin.Essentials;
using APP_GACH_NO.Global;

namespace APP_GACH_NO
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
            Routing.RegisterRoute(nameof(ListDebitPage), typeof(ListDebitPage));
            Routing.RegisterRoute(nameof(ThongTinThanhToanPage), typeof(ThongTinThanhToanPage));
            Routing.RegisterRoute(nameof(ThongKeDaThuPage), typeof(ThongKeDaThuPage));
            Routing.RegisterRoute(nameof(SettingPage ), typeof(SettingPage));
            if (Preferences.Get(Config.PhoneNumber, "") != "")
                CurrentItem = Login; 
            else
                CurrentItem = register ;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
           
        }
    }
}
