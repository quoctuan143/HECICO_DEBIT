using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APP_GACH_NO.Interface;
using APP_GACH_NO.iOS;
using Foundation;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(StatusBar))]
namespace APP_GACH_NO.iOS
{ 
   public class StatusBar : IStatusBar
    {
        public void HideStatusBar()
        {
           
        }

        public void ShowStatusBar()
        {
           
        }
    }
}