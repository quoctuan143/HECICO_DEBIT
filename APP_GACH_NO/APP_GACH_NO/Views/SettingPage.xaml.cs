using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_GACH_NO.Global;
namespace APP_GACH_NO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        public SettingPage() 
        {
            InitializeComponent();
            radMatKhau.IsChecked = Preferences.Get(Config.AprrovePassword, false );
            radVanTay.IsChecked = Preferences.Get(Config.AprroveFinger, false);
        }

        private void SfRadioGroup_CheckedChanged(object sender, Syncfusion.XForms.Buttons.CheckedChangedEventArgs e)
        {
            if(radMatKhau.IsChecked == true )
            {
                Preferences.Set(Config.AprrovePassword, true);
                Preferences.Set(Config.AprroveFinger, false );
            }   
            else
            {
                Preferences.Set(Config.AprrovePassword, false );
                Preferences.Set(Config.AprroveFinger, true );
            }    
        }
    }
}