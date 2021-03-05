using APP_GACH_NO.Global;
using APP_GACH_NO.Interface;
using APP_GACH_NO.Models;
using Plugin.DeviceInfo;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_GACH_NO.Dialog
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MessageXacThucMatKhau : PopupPage, INotifyPropertyChanged
    {
        bool isBusy;
        TaskCompletionSource<DialogReturn> _tsk = null;
        public CONG_NO_KHACH_HANG CongNo;
        string _password;
        int _demSaiPass = 0;
        public int DemSaiPass { get => _demSaiPass; set { _demSaiPass = value; OnPropertyChanged("DemSaiPass"); } }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        private bool _isShowHidePassword;
        public bool IsPasswordShow
        {
            get => _isShowHidePassword;
            set
            {
                _isShowHidePassword = value;
                OnPropertyChanged("IsPasswordShow");
            }
        }
        public Command ShowHideTapCommand { get; }
        public Command XacThucCommand { get; }
        public Command CloseCommand { get; }
        public MessageXacThucMatKhau()
        {
            InitializeComponent();
            _isShowHidePassword = true;
            XacThucCommand = new Command(OnXacThucClicked, ValidateLogin);
            this.PropertyChanged +=
                                  (_, __) => XacThucCommand.ChangeCanExecute();
            CloseCommand = new Command(OnCloseClicked);
            BindingContext = this;
        }

        private async void OnCloseClicked(object obj)
        {
            await Navigation.PopPopupAsync();
            _tsk.SetResult(DialogReturn.Cancel);
        }

        private async void OnXacThucClicked()
        {

            try
            {
                if (isBusy == true) return;
                isBusy = true;
                //xác thực mật khẩu
                using (HttpClient client = new HttpClient())
                {
                    User user = new User { MATNGAN = Preferences.Get(Config.PhoneNumber, ""), Password = _password };
                    client.BaseAddress = new Uri(Config.Url);
                    var ok = client.PostAsJsonAsync("api/hecico/XacThucMatKhau", user);
                    if (ok.Result.Content.ReadAsStringAsync().Result.ToLower().Contains("success"))
                    {
                        await Navigation.PopPopupAsync();
                        _tsk.SetResult(DialogReturn.OK);
                    }
                    else
                    {
                        //Nhập sai pass
                        DemSaiPass++;
                        await new MessageBox("Bạn đã nhập sai mật khẩu " + DemSaiPass.ToString() + " lần. Nếu sai quá 3 lần sẽ bị khóa").Show();
                    }

                }


            }
            catch (Exception ex)
            {

                await new MessageBox(ex.Message).Show();
            }
            finally
            {
                isBusy = false;
            }

        }
        private bool ValidateLogin()
        {
            return !String.IsNullOrWhiteSpace(_password) && _demSaiPass < 3;
        }



        public async Task<DialogReturn> Show()
        {
            _tsk = new TaskCompletionSource<DialogReturn>();
            await Navigation.PushPopupAsync(this);
            return await _tsk.Task;
        }

        private void imgShowPassword_Tapped(object sender, EventArgs e)
        {
            IsPasswordShow = !IsPasswordShow;
        }
    }
}