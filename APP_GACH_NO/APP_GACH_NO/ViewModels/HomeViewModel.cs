using APP_GACH_NO.Dialog;
using APP_GACH_NO.Global;
using APP_GACH_NO.Models;
using APP_GACH_NO.Views;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace APP_GACH_NO.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        public INavigation Navigation;
        bool _isShowAmount = false;
        public bool IsShowAmount { get => _isShowAmount; set { SetProperty(ref _isShowAmount, value); } }
        string _totalAmount = "****", _monthAmount = "****";
        User _user;
        public Command ScanCommand { get; }
        public Command QuitCommand { get; }
        public Command SelectDebitCommand { get; }
        public Command TongHopDaThuCommand { get; }
        public Command SettingCommand { get; }
        public Command ChangePasswordCommand { get; }
        public Command ShowHideTapCommand { get; }
        public User Users { get => _user; set { SetProperty(ref _user, value); } }
        public string TotalAmount { get => _totalAmount; set => SetProperty(ref _totalAmount, value); }
        public string MonthAmount { get => _monthAmount; set => SetProperty(ref _monthAmount, value); }
        public ObservableCollection<String> ListImage { get; set; }
        public HomeViewModel()
        {
            ListImage = new ObservableCollection<string> { "http://smart.cpc.vn/HecicoApi/image/hinh1.jpg" };
            Users = new User { THUNGAN = Preferences.Get(Config.FullName, ""), MATNGAN = Preferences.Get(Config.PhoneNumber, "") };
            SelectDebitCommand = new Command(OnSelectDebitClicked);
            ScanCommand = new Command(OnScanClicked);
            QuitCommand = new Command(OnQuitCLicked);
            TongHopDaThuCommand = new Command(OnTongHopDaThuClicked);
            ShowHideTapCommand = new Command(OnShowHideAmountClicked);
            SettingCommand = new Command(OnSettingClicked);
            ChangePasswordCommand = new Command(OnChangePasswordClicked);
        }

        private async void OnChangePasswordClicked(object obj)
        {
            try
            {
                var ok = await new MessageChangePassword().Show();
                if (ok == DialogReturn.OK)
                {
                    Shell.Current.CurrentItem = new LoginPage();
                }
            }
            catch (Exception)
            {

                
            }
        }

        private async void OnSettingClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(SettingPage)}");
        }

        private void OnShowHideAmountClicked(object obj)
        {
            if (!CheckInternet())
            {
                return;
            }
            try
            {
                IsShowAmount = !IsShowAmount;
                if (_isShowAmount == true)
                {
                    ShowLoading("Đang kiểm tra vui lòng đợi");
                    string str = Config.Url + "api/Hecico/ThongKeTrongThang?mathungan=" + _user.MATNGAN;
                    var _json = Config.client.GetStringAsync(str).Result;
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("error") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string result = _json.Substring(from, to - from + 1);

                        ObservableCollection<TONG_HOP_NHANH_MODEL> dt = JsonConvert.DeserializeObject<ObservableCollection<TONG_HOP_NHANH_MODEL>>(result);
                        if (dt.Count > 0)
                        {
                            TotalAmount = string.Format("{0:#,##0}", Convert.ToDecimal(dt[0].TRONG_NGAY)) + " đ";
                            MonthAmount = string.Format("{0:#,###}", Convert.ToDecimal(dt[0].TRONG_THANG)) + " đ";
                            HideLoading();
                        }

                    }
                    else
                    {
                        HideLoading();
                    }
                }
                else
                {
                    TotalAmount = "****";
                    MonthAmount = "****";
                }


            }
            catch (Exception ex)
            {
                HideLoading();
            }
        }

        private async void OnTongHopDaThuClicked(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(ThongKeDaThuPage)}");
        }

        private async void OnQuitCLicked(object obj)
        {
            var Ok = await new MessageYesNo("Quý khách có muốn thoát ứng dụng không?").Show();
            if (Ok == DialogReturn.OK)
            {
                Shell.Current.CurrentItem = new LoginPage();
            }

        }

        private async void OnScanClicked(object obj)
        {
            try
            {
                if (!CheckInternet())
                {
                    return;
                }
                var scan = new ZXingScannerPage();
                scan.Title = "Quét mã vạch khách hàng";
                await Navigation.PushAsync(scan);
                scan.OnScanResult += (ketqua) =>
                {
                    Device.BeginInvokeOnMainThread(async () => {
                        await Navigation.PopAsync();
                        string str = Config.Url + "api/hecico/TimKhachHang?makhachhang=" + ketqua.Text;
                        var _json = Config.client.GetStringAsync(str).Result;
                        _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                        if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                        {
                            Int32 from = _json.IndexOf("[");
                            Int32 to = _json.IndexOf("]");
                            string result = _json.Substring(from, to - from + 1);
                            ObservableCollection<CONG_NO_KHACH_HANG> CongNo = JsonConvert.DeserializeObject<ObservableCollection<CONG_NO_KHACH_HANG>>(result);
                            if (CongNo.Count > 0)
                            {
                                await Navigation.PushAsync(new ThongTinThanhToanPage(CongNo[0]));
                            }
                        }
                        else
                        {
                            await new MessageBox("Không tìm thấy kết quả").Show();
                            return;
                        }

                    });

                };
            }
            catch (Exception ex)
            {
                await new MessageBox("Lỗi hệ thống").Show();

            }
        }

        private async void OnSelectDebitClicked(object obj)
        {
            if (!CheckInternet())
            {
                return;
            }
            await Shell.Current.GoToAsync($"{nameof(ListDebitPage)}");
        }
    }
}
