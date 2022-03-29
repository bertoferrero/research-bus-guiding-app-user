using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(BusGuiding.AppShell), typeof(BusGuiding.Droid.DependencyService.Shell.MyShellRenderer))]
namespace BusGuiding.Droid.DependencyService.Shell
{
    class MyShellRenderer : ShellRenderer
    {
        public MyShellRenderer(Context context) : base(context)
        {
        }

        protected override IShellToolbarAppearanceTracker CreateToolbarAppearanceTracker()
        {
            return new MyShellToolbarAppearanceTracker(this);
        }
    }
}