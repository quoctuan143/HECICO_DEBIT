using APP_GACH_NO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using APP_GACH_NO.Global;
using Newtonsoft.Json;

namespace APP_GACH_NO.ViewModels
{
    public class LichSuThanhToanTheoKhachHangViewModel : BaseViewModel
    {
        ObservableCollection<TONG_HOP_DA_THU_KHACH_HANG> _listDaThu;
        public ObservableCollection<TONG_HOP_DA_THU_KHACH_HANG> ListDaThu { get => _listDaThu; set => SetProperty(ref _listDaThu, value); }
        TONG_HOP_DA_THU_KHACH_HANG _tonghop;
        public TONG_HOP_DA_THU_KHACH_HANG TongHop { get => _tonghop; set => SetProperty(ref _tonghop, value); }
        public LichSuThanhToanTheoKhachHangViewModel(TONG_HOP_DA_THU_KHACH_HANG cn)
        {
            _tonghop = cn;
            ListDaThu = new ObservableCollection<TONG_HOP_DA_THU_KHACH_HANG>();
            LoadDanhSachDaThuCommand = new Command(OnLoadDataDaThuExcute);
        }

        private async void OnLoadDataDaThuExcute(object obj)
        {
            if (!CheckInternet())
            {
                return;
            }
            try
            {
                if (IsBusy == true) return;
                IsBusy = true;
                string str = Config.Url + "api/Hecico/ThongKeDaThuTheoKhachHang?makhachhang=" + _tonghop.MA_KHANG;
                var _json = Config.client.GetStringAsync(str).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    ListDaThu = JsonConvert.DeserializeObject<ObservableCollection<TONG_HOP_DA_THU_KHACH_HANG>>(result);

                }
                else
                {
                    // This will pop the current page off the navigation stack
                    await Shell.Current.GoToAsync("..");
                    return;
                }


            }
            catch (Exception)
            {

            }
            finally
            {
                IsBusy = false;
            }
        }

        public Command LoadDanhSachDaThuCommand { get; }
    }
}
