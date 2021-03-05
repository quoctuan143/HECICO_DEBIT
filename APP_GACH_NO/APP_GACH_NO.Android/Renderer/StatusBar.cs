using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using APP_GACH_NO.Droid.Renderer;
using APP_GACH_NO.Interface;
using Xamarin.Forms;
[assembly: Dependency( typeof(StatusBar))]
namespace APP_GACH_NO.Droid.Renderer
{
    public class StatusBar : IStatusBar
    {
        private WindowManagerFlags windowManager;

        [Obsolete]
        public void HideStatusBar() 
        {
            var activity = (Activity) Forms.Context;
            var attibutes = activity.Window.Attributes;
            windowManager = attibutes.Flags;
            attibutes.Flags |= WindowManagerFlags.Fullscreen | WindowManagerFlags.TranslucentStatus;
            activity.Window.Attributes = attibutes;

        }

        [Obsolete]
        public void ShowStatusBar()
        {
            var activity = (Activity) Forms.Context;
            var attibutes = activity.Window.Attributes;
            attibutes.Flags = windowManager;
            activity.Window.Attributes = attibutes;

        }
    }
}