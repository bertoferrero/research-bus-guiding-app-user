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
using AndroidX.AppCompat.Widget;
using Xamarin.Essentials;
using AndroidX.Navigation;

namespace BusGuiding.Droid.DependencyService.Shell
{
    class MyShellToolbarAppearanceTracker : ShellToolbarAppearanceTracker
    {
        public MyShellToolbarAppearanceTracker(IShellContext context) : base(context)
        {
        }

        protected override void SetColors(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, Color foreground, Color background, Color title)
        {
            base.SetColors(toolbar, toolbarTracker, foreground, background, title);
            var role = Preferences.Get(Constants.PreferenceKeys.UserRole, "");
            if (role.Equals(Constants.UserRoles.Rider))
            {
                var stack = Xamarin.Forms.Application.Current.MainPage.Navigation.NavigationStack.Count();
                if (stack == 1)
                {
                    toolbar.NavigationContentDescription = "Menú";
                }
                else
                {
                    toolbar.NavigationContentDescription = "Atrás";
                }
            }
        }

        public override void ResetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker)
        {
            base.ResetAppearance(toolbar, toolbarTracker);
        }

        public override void SetAppearance(AndroidX.AppCompat.Widget.Toolbar toolbar, IShellToolbarTracker toolbarTracker, ShellAppearance appearance)
        {
            base.SetAppearance(toolbar, toolbarTracker, appearance);
        }
    }
}