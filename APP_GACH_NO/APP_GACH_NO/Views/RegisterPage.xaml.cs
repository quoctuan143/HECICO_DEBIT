using APP_GACH_NO.Interface;
using APP_GACH_NO.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_GACH_NO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegisterPage : ContentPage
    {
        public RegisterPage()
        {
            InitializeComponent();
            this.BindingContext = new RegisterViewModel();
            DependencyService.Get<IStatusBar>().ShowStatusBar();
            SetValue(NavigationPage.HasNavigationBarProperty, false);
            DependencyService.Get<IStatusBar>().HideStatusBar();
        }
    }
}