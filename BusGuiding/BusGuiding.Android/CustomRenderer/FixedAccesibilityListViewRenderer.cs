using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidX.SwipeRefreshLayout.Widget;
using BusGuiding.Droid.CustomRenderer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using AListView = Android.Widget.ListView;
using System.ComponentModel;
#if __ANDROID_29__
using AndroidX.Core.Widget;
#else
using Android.Support.V4.Widget;
#endif
using AView = Android.Views.View;
using Xamarin.Forms.Internals;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

[assembly: ExportRenderer(typeof(BusGuiding.CustomViews.FixedAccesibilityListView), typeof(FixedAccesibilityListViewRenderer))]
namespace BusGuiding.Droid.CustomRenderer
{
    class FixedAccesibilityListViewRenderer : ListViewRenderer
    {
		public FixedAccesibilityListViewRenderer(Context context) : base(context)
        {
        }
		protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.ListView> e)
		{
            base.OnElementChanged(e);
			if (e.NewElement != null)
			{
				AListView nativeListView = Control;
				if (nativeListView != null)
				{
					var wrappedAdapter = (nativeListView.Adapter as Android.Widget.HeaderViewListAdapter).WrappedAdapter;
					if (nativeListView.FooterViewsCount > 0)
					{
						Android.Views.View footerView = (wrappedAdapter.GetType().GetProperty("FooterView").GetValue(wrappedAdapter, null) as Android.Views.View);
						nativeListView.RemoveFooterView(footerView);
					}
					/*if (nativeListView.HeaderViewsCount > 0)
					{
						Android.Views.View headerView = (wrappedAdapter.GetType().GetProperty("HeaderView").GetValue(wrappedAdapter, null) as Android.Views.View);
						nativeListView.RemoveHeaderView(headerView);
					}*/
				}
			}
		}
	}

}