using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using APP_GACH_NO.iOS;
using APP_GACH_NO.Renderer;
using CoreAnimation;
using CoreGraphics;
using Foundation;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
[assembly: ExportRenderer(typeof(NavigationPageGradientHeader), typeof(NavigationPageGradientHeaderRenderer))]
namespace APP_GACH_NO.iOS
{
    public class NavigationPageGradientHeaderRenderer : ShellRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
        }

        protected override IShellNavBarAppearanceTracker CreateNavBarAppearanceTracker()
        {
            return new CustomNaviBarAppearance();
        }
    }
    public class CustomNaviBarAppearance : IShellNavBarAppearanceTracker
    {
        public void Dispose()
        {

        }

        public void ResetAppearance(UINavigationController controller)
        {

        }

        public void SetAppearance(UINavigationController controller, ShellAppearance appearance)
        {
            CAGradientLayer gradient = new CAGradientLayer();
            gradient.Frame = controller.NavigationBar.Bounds;
            gradient.Colors = new CoreGraphics.CGColor[] { UIColor.FromName("36ED81").CGColor, UIColor.FromName("26C986").CGColor, UIColor.FromName("109F8D").CGColor };
            gradient.StartPoint = new CoreGraphics.CGPoint(0, 0);
            gradient.EndPoint = new CoreGraphics.CGPoint(1, 0);

            controller.NavigationBar.SetBackgroundImage(imageFromLayer(gradient), UIBarMetrics.Default);
        }

        public void SetHasShadow(UINavigationController controller, bool hasShadow)
        {
            throw new NotImplementedException();
        }

        public void UpdateLayout(UINavigationController controller)
        {

        }

        UIImage imageFromLayer(CALayer layer)
        {
            UIGraphics.BeginImageContext(layer.Frame.Size);
            layer.RenderInContext(UIGraphics.GetCurrentContext());

            UIImage outputImage = UIGraphics.GetImageFromCurrentImageContext();

            UIGraphics.EndImageContext();

            return outputImage;
        }
    }
}