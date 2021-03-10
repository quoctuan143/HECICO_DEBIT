using APP_GACH_NO.Dialog;
using APP_GACH_NO.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using APP_GACH_NO.Global;
using Plugin.Fingerprint;
using System.Net.Http;
using Plugin.DeviceInfo;
using System.Threading.Tasks;

namespace APP_GACH_NO.ViewModels
{
    public class ThongTinThanhToanViewModel : BaseViewModel
    {

        CONG_NO_KHACH_HANG _congNoKhachHang;
        public INavigation navigation;
        public CONG_NO_KHACH_HANG CongNoKhachHang
        {
            get => _congNoKhachHang;
            set
            {
                SetProperty(ref _congNoKhachHang, value);

            }
        }
        public Command ThanhToanCommand { get; }
        public ThongTinThanhToanViewModel(CONG_NO_KHACH_HANG Cn)
        {
            CongNoKhachHang = Cn;
            CongNoKhachHang.TOKEN = Preferences.Get(Config.Token, "");
            CongNoKhachHang.TIEN_BANG_CHU = "Bằng chữ : " + Config.DocTien(Convert.ToDouble(Cn.TONG_TIEN));
            ThanhToanCommand = new Command(OnThanhToanClicked);
        }

        private async void OnThanhToanClicked(object obj)
        {
            try
            {
                if (!CheckInternet())
                {
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
                            var xoano = client.PostAsJsonAsync("api/hecico/XoaNo?SerialID=" + CrossDevice.Device.DeviceId, CongNoKhachHang);
                            HideLoading();
                            await new MessageBox(xoano.Result.Content.ReadAsStringAsync().Result.Replace("\"", "")).Show();
                            //gọi trạng thái đã thanh toán true về form chính
                            if (xoano.Result.Content.ReadAsStringAsync().Result.Contains("Thanh toán thành công"))
                                CongNoKhachHang.TRANG_THAI = true;
                            MessagingCenter.Send<ThongTinThanhToanViewModel, CONG_NO_KHACH_HANG>(this, "XacThucThanhToan", CongNoKhachHang);
                            await navigation.PopAsync();
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
                                    string json = JsonConvert.SerializeObject(CongNoKhachHang);
                                    var xoano = client.PostAsJsonAsync("api/hecico/XoaNo?SerialID=" + CrossDevice.Device.DeviceId, CongNoKhachHang);
                                    HideLoading();
                                    await new MessageBox(xoano.Result.Content.ReadAsStringAsync().Result.Replace("\"", "")).Show();
                                    //gọi trạng thái đã thanh toán true về form chính
                                    if (xoano.Result.Content.ReadAsStringAsync().Result.Contains("Thanh toán thành công"))
                                        CongNoKhachHang.TRANG_THAI = true;
                                    MessagingCenter.Send<ThongTinThanhToanViewModel, CONG_NO_KHACH_HANG>(this, "XacThucThanhToan", CongNoKhachHang);
                                    await navigation.PopAsync();
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
                    else
                    {
                        await new MessageBox("Vân tay không đúng").Show();
                    }
                }
            }
            catch (Exception ex)
            {
                HideLoading();
                await new MessageBox(ex.Message).Show();

            }


        }
    }
}
