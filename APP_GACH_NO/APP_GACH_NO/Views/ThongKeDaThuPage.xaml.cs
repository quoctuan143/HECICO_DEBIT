using APP_GACH_NO.Interface;
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
    public partial class ThongKeDaThuPage : ContentPage
    {
        ThongKeDaThuViewModel viewModel;
        public ThongKeDaThuPage()
        {
            InitializeComponent();
            viewModel = new ThongKeDaThuViewModel();
            viewModel.navigation = Navigation;
            BindingContext = viewModel;
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListDaThu.Count == 0)
            {
                viewModel.LoadItemCommand.Execute(null);
            }

        }
        public bool FilterRecords(object o)
        {

            var item = o as TONG_HOP_DA_THU_KHACH_HANG;

            if (item != null)
            {

                if (item.TEN_KHANG.ToLower().Contains(filterText) || item.DTHOAI_KHANG.ToLower().Contains(filterText))
                    return true;
            }
            return false;
        }
        string filterText = "";
        private void search_TextChanged(object sender, TextChangedEventArgs e)
        {
            filterText = e.NewTextValue;
            gridThongKe.View.Filter = FilterRecords;
            gridThongKe.View.RefreshFilter();
            viewModel.SelectItem = null; //reset lai mục chọn
        }
    }
}