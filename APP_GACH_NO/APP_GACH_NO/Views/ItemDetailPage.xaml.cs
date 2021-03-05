using System.ComponentModel;
using Xamarin.Forms;
using APP_GACH_NO.ViewModels;

namespace APP_GACH_NO.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}