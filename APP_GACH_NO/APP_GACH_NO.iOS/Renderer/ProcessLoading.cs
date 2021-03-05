using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using APP_GACH_NO.Interface;
using APP_GACH_NO.iOS.Renderer;
using BigTed;
using Foundation;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(ProcessLoading))]

namespace APP_GACH_NO.iOS.Renderer
{
    public class ProcessLoading : IProcessLoader
    {
        public void Hide()
        {
            BTProgressHUD.Dismiss();           
        }

        public void Show(string title = "Loading")
        {
            BTProgressHUD.Show(title, maskType: ProgressHUD.MaskType.Black);
        }
    }
}