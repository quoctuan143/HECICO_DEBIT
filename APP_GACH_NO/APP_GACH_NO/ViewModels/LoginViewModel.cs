using APP_GACH_NO.Dialog;
using APP_GACH_NO.Global;
using APP_GACH_NO.Interface;
using APP_GACH_NO.Models;
using APP_GACH_NO.Views;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.DeviceInfo;
using Plugin.Fingerprint;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace APP_GACH_NO.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private bool _isShowHidePassword;
        string _phonenumber;
        string _fullname;
        string _password;
        ObservableCollection<User> _user;
        public Command LoginCommand { get; }
        public Command FingerComand { get; }
        public Command ShowHideTapCommand { get; }
        public ObservableCollection<User> Users { get => _user; set { SetProperty(ref _user, value); } }
        public ObservableCollection<String> ListImage { get; set; }
        public bool IsPasswordShow { get => _isShowHidePassword; set { SetProperty(ref _isShowHidePassword, value); } }
        public string PhoneNumber
        {
            get => _phonenumber;
            set
            {
                SetProperty(ref _phonenumber, value);
                if (_phonenumber.Length >= 10)
                {
                    string old = _phonenumber.Substring(3, 5);
                    _phonenumber = _phonenumber.Replace(old, "*****");
                }

            }
        }
        public string Password { get => _password; set { SetProperty(ref _password, value); } }
        public string FullName { get => _fullname; set { SetProperty(ref _fullname, value); } }
        public LoginViewModel()
        {
            FullName = Preferences.Get(Config.FullName, "");
            PhoneNumber = Preferences.Get(Config.PhoneNumber, "");
            IsPasswordShow = true;
            LoginCommand = new Command(OnLoginClicked, ValidateLogin);
            this.PropertyChanged +=
                                  (_, __) => LoginCommand.ChangeCanExecute();
            FingerComand = new Command(OnFingerCLicked);
            ShowHideTapCommand = new Command(OnShowHidePassClicked);
            ListImage = new ObservableCollection<string> { "hecico.jpg", "hecico1.jpg", "hecico2.jpg" };
            OnPropertyChanged(nameof(ListImage));
        }

        private bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(_password);
        }
        private async void OnFingerCLicked(object obj)
        {
            if (!CheckInternet())
            {
                return;
            }
            var result = await CrossFingerprint.Current.AuthenticateAsync("Vui lòng quét vân tay");
            if (result.Authenticated)
            {
                try
                {
                    ShowLoading("Đang kiểm tra vui lòng đợi");
                    await Task.Delay(2000);
                    string str = Config.Url + "api/Hecico/Login_VanTay?phonenumber=" + Preferences.Get(Config.PhoneNumber, "") + "&imei=" + CrossDevice.Device.DeviceId;
                    var _json = Config.client.GetStringAsync(str).Result;
                    _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                    if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                    {
                        Int32 from = _json.IndexOf("[");
                        Int32 to = _json.IndexOf("]");
                        string kq = _json.Substring(from, to - from + 1);
                        Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(kq);
                        if (Users[0].NGAYHHLUC >= DateTime.Now)
                            Config.IsGachNo = true;
                        else Config.IsGachNo = false;
                        Preferences.Set(Config.Token, Users[0].TOKEN);
                        HideLoading();
                        App.DemNguoc = 0;
                        await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                        DependencyService.Get<IStatusBar>().ShowStatusBar();
                    }
                    else
                    {
                        HideLoading();
                        await new MessageBox("User của bạn chưa kích hoạt / sai mật khẩu . Vui lòng thử lại").Show();
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

        private void OnShowHidePassClicked(object obj)
        {
            IsPasswordShow = !IsPasswordShow;

        }

        private async void OnLoginClicked()
        {
            // Prefixing with `//` switches to a different navigation stack instead of pushing to the active one
            if (!CheckInternet())
            {
                return;
            }
            try
            {
                ShowLoading("Đang kiểm tra vui lòng đợi");
                await Task.Delay(2000);
                string str = Config.Url + "api/Hecico/Login?phonenumber=" + Preferences.Get(Config.PhoneNumber, "") + "&password=" + _password + "&imei=" + CrossDevice.Device.DeviceId;
                var _json = Config.client.GetStringAsync(str).Result;
                _json = _json.Replace("\\r\\n", "").Replace("\\", "");
                if (_json.Contains("Không Tìm Thấy Dữ Liệu") == false && _json.Contains("[]") == false)
                {
                    Int32 from = _json.IndexOf("[");
                    Int32 to = _json.IndexOf("]");
                    string kq = _json.Substring(from, to - from + 1);
                    Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(kq);
                    if (Users[0].NGAYHHLUC >= DateTime.Now)
                        Config.IsGachNo = true;
                    else Config.IsGachNo = false;
                    Preferences.Set(Config.Token, Users[0].TOKEN);
                    HideLoading();
                    App.DemNguoc = 0;
                    await Shell.Current.GoToAsync($"//{nameof(HomePage)}");
                    DependencyService.Get<IStatusBar>().ShowStatusBar();
                }
                else
                {
                    HideLoading();
                    await new MessageBox("User của bạn chưa kích hoạt / sai mật khẩu. Vui lòng thử lại").Show();
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
