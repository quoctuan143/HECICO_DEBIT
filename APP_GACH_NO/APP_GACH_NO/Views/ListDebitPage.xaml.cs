using APP_GACH_NO.Dialog;
using APP_GACH_NO.Interface;
using APP_GACH_NO.Models;
using APP_GACH_NO.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_GACH_NO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListDebitPage : ContentPage
    {
        ListDebitViewModel viewModel;
        public ListDebitPage()
        {
            InitializeComponent();
            viewModel = new ListDebitViewModel();
            viewModel.Navigation = Navigation;
            BindingContext = viewModel;
            MessagingCenter.Subscribe<ThongTinThanhToanViewModel, CONG_NO_KHACH_HANG>(this, "XacThucThanhToan", (sender, item) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    //viewModel.LoadDataCommand.Execute(null);
                    ListDebitRefresh();
                    lblSoLuongChuaThu.Text = viewModel.ListCongNo.Where(p => p.TRANG_THAI == false).ToList().Count.ToString();
                });
            });
            MessagingCenter.Subscribe<SelectTramLo, TRAM>(this, "tram", (sender, item) => {
                txtTram.Text = item.TEN_TRAM ;
                 matram = item.TEN_TRAM  ;
                if (matram == "") matram = "Tất Cả";
                viewModel.LoadDsLo(item.MA_TRAM);
                txtLo.Text = "Tất Cả";
                malo = "Tất Cả";
                ListDebitRefresh();
            });
            MessagingCenter.Subscribe<SelectTramLo, TRAM>(this, "lo", (sender, item) => {
                txtLo.Text = item.TEN_TRAM;
                malo = item.TEN_TRAM ;
                if (malo == "") malo = "Tất Cả";
                ListDebitRefresh();
            });
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (viewModel.ListCongNo.Count == 0)
            {
                try
                {
                    viewModel.LoadDataCommand.Execute(null);
                    listDebit.ItemsSource = viewModel.ListCongNo.Take(20);
                }
                catch (Exception)
                {


                }

            }

        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ListDebitRefresh();
        }
        string matram = "Tất Cả", malo = "Tất Cả";
        void ListDebitRefresh()
        {
            if (viewModel.ListCongNo.Count == 0) return;
            string text = search.Text.ToLower();
            if (matram == "Tất Cả" || matram == "")
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    listDebit.ItemsSource = viewModel.ListCongNo.Where(p => (p.TEN_KHANG.ToLower().Contains(text) || p.DTHOAI_KHANG.ToLower().Contains(text)
                || p.HOADON_ID.ToLower().Contains(text)) && p.TRANG_THAI == false);
                }
                else
                {
                    listDebit.ItemsSource = viewModel.ListCongNo.Where(p => p.TRANG_THAI == false);
                }
            }
            else if (matram != "Tất Cả" && malo == "Tất Cả")
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    listDebit.ItemsSource = viewModel.ListCongNo.Where(p => (p.TEN_KHANG.ToLower().Contains(text) || p.DTHOAI_KHANG.ToLower().Contains(text)
                || p.HOADON_ID.ToLower().Contains(text)) && p.TRANG_THAI == false && p.TENTRAM == matram);
                }
                else
                {
                    listDebit.ItemsSource = viewModel.ListCongNo.Where(p => p.TRANG_THAI == false && p.TENTRAM == matram);
                }
            }
            else if (matram != "Tất Cả" && malo != "Tất Cả")
            {
                if (!string.IsNullOrWhiteSpace(text))
                {
                    listDebit.ItemsSource = viewModel.ListCongNo.Where(p => (p.TEN_KHANG.ToLower().Contains(text) || p.DTHOAI_KHANG.ToLower().Contains(text)
                || p.HOADON_ID.ToLower().Contains(text)) && p.TRANG_THAI == false && p.TENTRAM == matram && p.TENLO == malo);
                }
                else
                {
                    listDebit.ItemsSource = viewModel.ListCongNo.Where(p => p.TRANG_THAI == false && p.TENTRAM == matram && p.TENLO == malo);
                }
            }


        }

        private void cbLo_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            try
            {
                //malo = cbLo.Text;
                if (malo == "") malo = "Tất Cả";
                ListDebitRefresh();
            }
            catch (Exception)
            {
            }
        }

        private async void btnChonTram_Tapped(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushModalAsync(new SelectTramLo(viewModel.ListTram, "tram")); ;

            }
            catch (Exception)
            {

                throw;
            }
        }

        private async void btnChonLo_Tapped(object sender, EventArgs e)
        {
            ObservableCollection<TRAM> tram = new ObservableCollection<TRAM>();
            foreach (LO lo in viewModel.ListLo )
            {
                TRAM tr = new TRAM { MA_TRAM = lo.MA_LO, TEN_TRAM = lo.TEN_LO };
                tram.Add(tr);
            }    
            await Navigation.PushModalAsync(new SelectTramLo(tram , "lo")); ;
        }

        private void cbTram_SelectionChanged(object sender, Syncfusion.XForms.ComboBox.SelectionChangedEventArgs e)
        {
            try
            {
               // matram = cbTram.Text;
                if (matram == "") matram = "Tất Cả";
                //viewModel.LoadDsLo(cbTram.SelectedValue.ToString());
              //  cbLo.SelectedIndex = 0;
                ListDebitRefresh();
            }
            catch (Exception)
            {
            }
        }
    }
}