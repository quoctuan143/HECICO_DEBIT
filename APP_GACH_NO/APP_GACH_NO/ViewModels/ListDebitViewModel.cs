using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using APP_GACH_NO.Models;
using Xamarin.Forms;
using APP_GACH_NO.Views;
using Xamarin.Essentials;
using APP_GACH_NO.Global;
using Newtonsoft.Json;
using APP_GACH_NO.Dialog;
using System.Linq;
using System.Threading.Tasks;
using ZXing.Net.Mobile.Forms;

namespace APP_GACH_NO.ViewModels
{
    //danh sách công nợ khách hàng theo Thu ngân view
    public class ListDebitViewModel : BaseViewModel
    {
        string _filter = "";
        public string Filter
        {
            get => _filter;
            set
            {
                SetProperty(ref _filter, value);
            }
        }
        ObservableCollection<CONG_NO_KHACH_HANG> _listCongNo;
        ObservableCollection<TRAM> _lstTram;
        ObservableCollection<LO> _lstLo;
        CONG_NO_KHACH_HANG _selectKhachHang;
        int _demkhachhangchuathu;
        public int DemKhachHangChuaThu { get => _demkhachhangchuathu; set => SetProperty(ref _demkhachhangchuathu, value); }
        public ObservableCollection<CONG_NO_KHACH_HANG> ListCongNo
        {
            get
            {
                return _listCongNo;

            }

            set
            {
                SetProperty(ref _listCongNo, value);
                DemKhachHangChuaThu = ListCongNo.Where(p => p.TRANG_THAI == false).ToList().Count;
            }
        }
        public ObservableCollection<TRAM> ListTram
        {
            get
            {
                return _lstTram;
            }
            set
            {
                SetProperty(ref _lstTram, value);
            }
        }
        public ObservableCollection<LO> ListLo
        {
            get
            {
                return _lstLo;
            }
            set
            {
                SetProperty(ref _lstLo, value);
            }
        }
        public CONG_NO_KHACH_HANG SelectKhachHang
        {
            get => _selectKhachHang;
            set
            {
                SetProperty(ref _selectKhachHang, value);
                OnItemSelected(_selectKhachHang);
            }
        }
        public Command LoadDataCommand { get; }
        public Command ScanCommand { get; }
        public INavigation Navigation;

        public ListDebitViewModel()
        {
            ListCongNo = new ObservableCollection<CONG_NO_KHACH_HANG>();
            ListTram = new ObservableCollection<TRAM>();
            ListLo = new ObservableCollection<LO>();
            LoadDataCommand = new Command(LoadData);
            ScanCommand = new Command(OnScanClicked);
            LoadDsTram();
            LoadDsLo("-1");
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

        async void OnItemSelected(CONG_NO_KHACH_HANG item)
        {
            try
            {
                if (Config.IsGachNo == false)
                {
                    await new MessageBox("Bạn đã hết thời hạn thu tiền của khách hàng.").Show();
                    return;
                }
                if (item == null)
                    return;
                if (!CheckInternet())
                {
                    return;
                }
                if (item.TRANG_THAI == false)
                    await Navigation.PushAsync(new ThongTinThanhToanPage(item));
                else
                    await new MessageBox("Khách hàng này đã thanh toán rồi").Show();

            }
            catch (Exception ex)
            {


            }

            // This will push the ItemDetailPage onto the navigation stack
            //string _jsonCongno = JsonConvert.SerializeObject(item);
            // await Shell.Current.GoToAsync($"{nameof(ThongTinThanhToanPage)}?{nameof(ThongTinThanhToanViewModel.JsonCongNo)}={_jsonCongno}");
        }
        public async void LoadData(object obj)
        {
            if (!CheckInternet())
            {
                return;
            }
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                // ShowLoading("Đang kiểm tra vui lòng đợi");
                await Task.Delay(1000);
                string str = Config.Url + "api/Hecico/getDanhSachCongNoKhachHang?mathungan=" + Preferences.Get(Config.PhoneNumber, "");
                var _json = Config.client.GetStringAsync(str).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListCongNo = JsonConvert.DeserializeObject<ObservableCollection<CONG_NO_KHACH_HANG>>(result);
                    HideLoading();
                }
                else
                {
                    HideLoading();
                    await new MessageBox("Mã thu ngân này chưa được phân bổ khách hàng thu").Show();
                    // This will pop the current page off the navigation stack
                    await Shell.Current.GoToAsync("..");
                    return;
                }


            }
            catch (Exception)
            {
                HideLoading();
            }
            finally
            {
                IsBusy = false;
            }
        }
        public async void LoadDsTram()
        {
            if (!CheckInternet())
            {
                return;
            }
            try
            {
                // ShowLoading("Đang kiểm tra vui lòng đợi");
                await Task.Delay(1000);
                string str = Config.Url + "api/Hecico/GetDSTram?mathungan=" + Preferences.Get(Config.PhoneNumber, "");
                var _json = Config.client.GetStringAsync(str).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("error") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListTram = JsonConvert.DeserializeObject<ObservableCollection<TRAM>>(result);
                }
            }
            catch (Exception)
            {

            }

        }
        public async void LoadDsLo(string matram)
        {
            if (!CheckInternet())
            {
                return;
            }
            try
            {
                string str = Config.Url + "api/Hecico/GetDSLo?mathungan=" + Preferences.Get(Config.PhoneNumber, "") + "&matram=" + matram;
                var _json = Config.client.GetStringAsync(str).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("error") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListLo = JsonConvert.DeserializeObject<ObservableCollection<LO>>(result);
                }
            }
            catch (Exception)
            {

            }

        }
    }
}
