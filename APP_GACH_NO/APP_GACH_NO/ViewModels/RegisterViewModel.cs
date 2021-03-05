using APP_GACH_NO.Dialog;
using APP_GACH_NO.Global;
using APP_GACH_NO.Interface;
using APP_GACH_NO.Models;
using APP_GACH_NO.Popup;
using APP_GACH_NO.Views;
using Newtonsoft.Json;
using Plugin.DeviceInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_GACH_NO.ViewModels
{
    public class RegisterViewModel : BaseViewModel
    {
        string _phoneNumber;
        string _password;
        bool _isShowPassword;
        ObservableCollection<User> _user;
        public Command RegisterCommand { get; }
        public Command ShowHideTapCommand { get; }
        public string PhoneNumber { get => _phoneNumber; set { SetProperty(ref _phoneNumber, value); } }
        public string Password { get => _password; set { SetProperty(ref _password, value); } }
        public ObservableCollection<User> Users { get => _user; set { SetProperty(ref _user, value); } }
        public bool IsPasswordShow { get => _isShowPassword; set { SetProperty(ref _isShowPassword, value); } }

        public RegisterViewModel()
        {
            IsPasswordShow = true;
            RegisterCommand = new Command(OnRegisterClicked, ValidateRegister);
            this.PropertyChanged +=
                                    (_, __) => RegisterCommand.ChangeCanExecute();
            ShowHideTapCommand = new Command(OnShowPasswordClick);
        }

        private void OnShowPasswordClick(object obj)
        {
            IsPasswordShow = !IsPasswordShow;
        }

        private bool ValidateRegister()
        {
            return !String.IsNullOrWhiteSpace(_phoneNumber)
                && !String.IsNullOrWhiteSpace(_password);
        }
        private async void OnRegisterClicked()
        {
            if (!CheckInternet())
            {
                return;
            }
            try
            {
                ShowLoading("Đang kiểm tra vui lòng đợi");
                await Task.Delay(2000);
                string str = Config.Url + "api/Hecico/Register?phonenumber=" + _phoneNumber + "&password=" + _password + "&imei=" + CrossDevice.Device.DeviceId + "&name=" + CrossDevice.Device.Manufacturer;
                var _json = Config.client.GetStringAsync(str).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false && _json.Contains("AccoutExits") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string result = _json.Substring(from, to - from + 1);
                    Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(result);
                    Preferences.Set(Config.FullName, Users[0].THUNGAN);
                    Preferences.Set(Config.PhoneNumber, Users[0].MATNGAN);
                    HideLoading();
                    await new MessageBox("Đăng ký thiết bị thành công").Show();
                    await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
                    // DependencyService.Get<IStatusBar>().ShowStatusBar();

                }
                else
                {
                    HideLoading();
                    await new MessageBox("Vui lòng kiểm tra tên đăng nhập hoặc mật khẩu. Vui lòng thử lại").Show();
                    return;
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
