using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using APP_GACH_NO.Models;
using System.Net.Http;
using APP_GACH_NO.Dialog;
using APP_GACH_NO.Global;
using Xamarin.Essentials;
using Newtonsoft.Json;
using APP_GACH_NO.Views.Lich_Su_Thanh_Toan;
using Plugin.Fingerprint;
using Plugin.DeviceInfo;
using System.Threading.Tasks;

namespace APP_GACH_NO.ViewModels
{
    public class ThongKeDaThuViewModel : BaseViewModel
    {
        DateTime _fromDate, _toDate;
        public DateTime FromDate { get => _fromDate; set { SetProperty(ref _fromDate, value); LoadItemExcuted(null); } }
        public DateTime ToDate { get => _toDate; set { SetProperty(ref _toDate, value); LoadItemExcuted(null); } }
        ObservableCollection<TONG_HOP_DA_THU_KHACH_HANG> _listDaThu;
        public ObservableCollection<TONG_HOP_DA_THU_KHACH_HANG> ListDaThu { get => _listDaThu; set => SetProperty(ref _listDaThu, value); }
        public TONG_HOP_DA_THU_KHACH_HANG _selectItem;
        public TONG_HOP_DA_THU_KHACH_HANG SelectItem { get => _selectItem; set => SetProperty(ref _selectItem, value); }
        public INavigation navigation;
        public ThongKeDaThuViewModel()
        {
            _fromDate = DateTime.Now.Date;
            _toDate = DateTime.Now.Date;
            ListDaThu = new ObservableCollection<TONG_HOP_DA_THU_KHACH_HANG>();
            LoadItemCommand = new Command(LoadItemExcuted);
            XemChiTietCommand = new Command(OnXemChiTietClicked);
            HuyGachNoCommand = new Command(OnHuyGachNoClicked);
        }

        private async void OnHuyGachNoClicked(object obj)
        {
            try
            {
                if (Config.IsGachNo == false)
                {
                    await new MessageBox("Bạn đã hết thời hạn thu tiền của khách hàng.").Show();
                    return;
                }
                if (!CheckInternet())
                {
                    return;
                }
                if (SelectItem == null)
                {
                    await new MessageBox("Chọn khách hàng muốn hủy gạch nợ").Show();
                    return;
                }
                if (Preferences.Get(Config.AprrovePassword, false) == true)
                {
                    var ok = await new MessageXacThucMatKhau().Show();
                    if (ok == Global.DialogReturn.OK)
                    {
                        ShowLoading("Đang thực hiện giao dịch");
                        await Task.Delay(2000);
                        using (HttpClient client = new HttpClient())
                        {
                            client.BaseAddress = new Uri(Config.Url);
                            SelectItem.TOKEN = Preferences.Get(Config.Token, "");
                            var xoano = client.PostAsJsonAsync("api/hecico/HuyXoaNo?SerialID=" + CrossDevice.Device.DeviceId, SelectItem);
                            HideLoading();
                            await new MessageBox(xoano.Result.Content.ReadAsStringAsync().Result.Replace("\"", "")).Show();
                            if (xoano.Result.Content.ReadAsStringAsync().Result.Contains("Hủy thanh toán thành công"))
                                ListDaThu.Remove(SelectItem);
                        }

                    }
                }
                else
                {
                    var result = await CrossFingerprint.Current.AuthenticateAsync("Vui lòng quét vân tay");
                    if (result.Authenticated)
                    {
                        try
                        {
                            ShowLoading("Đang thực hiện giao dịch");
                            await Task.Delay(2000);
                            string str = Config.Url + "api/Hecico/Check_VanTay?phonenumber=" + Preferences.Get(Config.PhoneNumber, "") + "&imei=" + CrossDevice.Device.DeviceId;
                            var _json = Config.client.GetStringAsync(str).Result;
                            _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                            if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                            {
                                using (HttpClient client = new HttpClient())
                                {
                                    client.BaseAddress = new Uri(Config.Url);
                                    //string a = JsonConvert.SerializeObject(SelectItem);
                                    SelectItem.TOKEN = Preferences.Get(Config.Token, "");
                                    var xoano = client.PostAsJsonAsync("api/hecico/HuyXoaNo?SerialID=" + CrossDevice.Device.DeviceId, SelectItem);
                                    HideLoading();
                                    await new MessageBox(xoano.Result.Content.ReadAsStringAsync().Result.Replace("\"", "")).Show();
                                    if (xoano.Result.Content.ReadAsStringAsync().Result.Contains("Hủy thanh toán thành công"))
                                        ListDaThu.Remove(SelectItem);
                                    // LoadItemExcuted(null);
                                }
                            }
                            else
                            {
                                HideLoading();
                                await new MessageBox("User của bạn đã hết hiệu lực / chưa đăng ký. Vui lòng thử lại").Show();
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            HideLoading();
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox(ex.Message).Show();

            }
        }

        private async void OnXemChiTietClicked(object obj)
        {
            if (_selectItem != null)
                await navigation.PushAsync(new LichSuThanhToanTheoKhachHangPage(_selectItem));
        }

        public Command XemChiTietCommand { get; }
        public Command HuyGachNoCommand { get; }
        public Command LoadItemCommand { get; }
        private async void LoadItemExcuted(object obj)
        {
            if (!CheckInternet())
            {
                return;
            }
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                await Task.Delay(2000);
                string str = Config.Url + "api/Hecico/ThongKeDaThuTheoNgay?mathungan=" + Preferences.Get(Config.PhoneNumber, "") + "&tungay=" + string.Format("{0:yyyy-MM-dd}", _fromDate) + "&denngay=" + string.Format("{0:yyyy-MM-dd}", _toDate);
                var _json = Config.client.GetStringAsync(str).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListDaThu.Clear();
                    ListDaThu = JsonConvert.DeserializeObject<ObservableCollection<TONG_HOP_DA_THU_KHACH_HANG>>(result);

                }
                else
                {
                    ListDaThu.Clear();
                    return;
                }


            }
            catch (Exception ex)
            {
                await new MessageBox(ex.Message).Show();
            }
            finally
            {
                IsBusy = false;
            }
        }



    }
}
