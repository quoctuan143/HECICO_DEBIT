using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

using Xamarin.Forms;

using APP_GACH_NO.Models;
using APP_GACH_NO.Services;
using APP_GACH_NO.Interface;
using APP_GACH_NO.Popup;
using Plugin.Connectivity;
using APP_GACH_NO.Dialog;
using System.Threading.Tasks;

namespace APP_GACH_NO.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        public BaseViewModel()
        {
            CrossConnectivity.Current.ConnectivityChanged += Current_ConnectivityChanged1;

        }
        public bool CheckInternet()
        {
            App.DemNguoc = 0;
            if (!CrossConnectivity.Current.IsConnected)
            {
                Task.Run(() => new MessageInternetProblem().Show());
            }
            return CrossConnectivity.Current.IsConnected;
        }
        private async void Current_ConnectivityChanged1(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            if (e.IsConnected == false)
            {
                await new MessageInternetProblem().Show();
            }
        }
        public IDataStore<Item> DataStore => DependencyService.Get<IDataStore<Item>>();

        bool isBusy = false;
        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }
        public void ShowLoading(string title)
        {
            DependencyService.Get<IProcessLoader>().Show(Title);
        }
        public void HideLoading()
        {
            DependencyService.Get<IProcessLoader>().Hide();
        }
        public void ShortAlert(string title)
        {
            DependencyService.Get<IMessage>().ShortAlert(title);
        }
        public void LongAlert(string title)
        {
            DependencyService.Get<IMessage>().LongAlert(title);
        }
        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }


        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
