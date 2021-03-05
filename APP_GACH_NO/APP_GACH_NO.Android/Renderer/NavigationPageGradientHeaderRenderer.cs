using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using APP_GACH_NO;
using APP_GACH_NO.Droid.Renderer;
using APP_GACH_NO.Renderer;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;


[assembly: ExportRenderer(typeof(AppShell), typeof(NavigationPageGradientHeaderRenderer))]
namespace APP_GACH_NO.Droid.Renderer
{
    [Obsolete]
    public  class NavigationPageGradientHeaderRenderer : ShellRenderer 
    {
        public NavigationPageGradientHeaderRenderer(Context context) : base(context)
        {
        }

        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new CustomToolbarAppearanceTracker(); 
        }
    }
    public class CustomToolbarAppearanceTracker : IShellToolbarAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker)
        {

        }

        public void SetAppearance(Android.Support.V7.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            toolbar.SetBackgroundResource(Resource.Drawable.gradient);
        }
    }
}