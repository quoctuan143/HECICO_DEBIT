using APP_GACH_NO.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace APP_GACH_NO.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectTramLo : ContentPage , INotifyPropertyChanged
    {
        ObservableCollection<TRAM> ListData;
        string Loai;
        public SelectTramLo(ObservableCollection<TRAM> _list,string _loai)
        {
            InitializeComponent();
            ListData = _list;
            Loai = _loai;
            lstView.ItemsSource = ListData;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                TRAM item = lstView.SelectedItem as TRAM;
                if (item != null )
                {
                    if (Loai == "tram")
                    {
                        MessagingCenter.Send<SelectTramLo, TRAM>(this, "tram", item);
                        Navigation.PopModalAsync();
                    }    
                       
                    else
                    {
                        MessagingCenter.Send<SelectTramLo, TRAM>(this, "lo", item);
                        Navigation.PopModalAsync();
                    }    
                        
                }    
            }
            catch (Exception)
            {

               
            }
        }
    }
}