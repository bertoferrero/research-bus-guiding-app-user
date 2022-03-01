using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace BusGuiding.Helpers
{
    public class GeoData
    {
        protected static bool currentlyRequesting = false;
        protected static CancellationTokenSource gpsCancellationToken = null;

        public static async Task<Location> GetCurrentLocationAsync(GeolocationAccuracy desiredAccuracy = GeolocationAccuracy.Best, int timeout = 30, int minAccuracy = 20)
        {
            if (!currentlyRequesting)
            {
                currentlyRequesting = true;
                var request = new GeolocationRequest(desiredAccuracy, TimeSpan.FromSeconds(timeout));
                gpsCancellationToken = new CancellationTokenSource();
                var location = await Geolocation.GetLocationAsync(request, gpsCancellationToken.Token);
                gpsCancellationToken = null;
                currentlyRequesting = false;
                if (location != null)
                {
                    var accuracy = location.Accuracy;
                    if (accuracy <= minAccuracy)
                    {
                        return location;
                    }
                }
            }
            return null;
        }

        public static async Task SendGeoDataToServerAsync()
        {
            //This method must be called only for devs or drivers, otherways, server will ignore the request
            Location location = await GetCurrentLocationAsync();
            var latitude = location.Latitude;
            var longitude = location.Longitude;
            _ = Models.Api.User.UpdateDriverLatitudeLongitude(Preferences.Get(Constants.PreferenceKeys.UserApiToken, ""), latitude, longitude);
        }
    }
}
