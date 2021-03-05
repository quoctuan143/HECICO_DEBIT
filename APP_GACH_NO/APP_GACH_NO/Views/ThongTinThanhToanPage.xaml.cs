using APP_GACH_NO.Models;
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
    public partial class ThongTinThanhToanPage : ContentPage
    {
        ThongTinThanhToanViewModel viewModel;
        public ThongTinThanhToanPage(CONG_NO_KHACH_HANG cn)
        {
            InitializeComponent();
            viewModel = new ThongTinThanhToanViewModel(cn);
            viewModel.navigation = Navigation;
            BindingContext = viewModel;


        }
    }
}