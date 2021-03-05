using APP_GACH_NO.Views;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_GACH_NO.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageInternetProblem : PopupPage
    {
        public MessageInternetProblem()
        {
            InitializeComponent();
        }
        private async void btnOK_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopAllPopupAsync(true);
        }
        public async Task Show()
        {
            await Navigation.PushPopupAsync(this);

        }
    }
}