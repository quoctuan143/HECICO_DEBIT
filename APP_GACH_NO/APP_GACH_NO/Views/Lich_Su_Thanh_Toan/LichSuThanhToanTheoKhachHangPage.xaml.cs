using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using APP_GACH_NO.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using APP_GACH_NO.Models;
namespace APP_GACH_NO.Views.Lich_Su_Thanh_Toan
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LichSuThanhToanTheoKhachHangPage : ContentPage
    {
        LichSuThanhToanTheoKhachHangViewModel viewModel;
        TONG_HOP_DA_THU_KHACH_HANG congno;
        public LichSuThanhToanTheoKhachHangPage(TONG_HOP_DA_THU_KHACH_HANG cn)
        {
            InitializeComponent();
            congno = cn;
            viewModel = new LichSuThanhToanTheoKhachHangViewModel(cn);            
            BindingContext = viewModel;
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListDaThu.Count == 0)
                viewModel.LoadDanhSachDaThuCommand.Execute(null);
        }
    }
}