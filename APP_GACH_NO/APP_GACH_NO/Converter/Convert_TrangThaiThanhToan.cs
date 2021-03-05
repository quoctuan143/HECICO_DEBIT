using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
namespace APP_GACH_NO.Converter
{
    public class Convert_TrangThaiThanhToan : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string loai = parameter as string;
            if (loai == "text")
            {
                bool _value = (bool)value;
                if (_value == false)
                    return "Chưa thanh toán";
                return "Đã thanh toán";
            }    
            else
            {
                bool _value = (bool)value;
                if (_value == false)
                    return Color.Red ;
                return Color.Blue; ;
            }    
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
