using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using PyConsumerApp.Controls;
using PyConsumerApp.Droid;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(LocationSetting_Droid))]
namespace PyConsumerApp.Droid
{
    public class LocationSetting_Droid : ILocSettings
    {
        Int64
            interval = 1000 * 60 * 1,
            fastestInterval = 1000 * 50;
        private Activity p0;
        [Obsolete]
        public async void OpenSettings()
        {
            LocationManager LM = (LocationManager)Android.App.Application.Context.GetSystemService(Context.LocationService);
            if (LM.IsProviderEnabled(LocationManager.GpsProvider) == false)
            {
                Context ctx = Android.App.Application.Context;
                p0 = Platform.CurrentActivity;
                //-----------------------------------------------------------------------------------------------------------------

                try
                {
                    GoogleApiClient
                        googleApiClient = new GoogleApiClient.Builder(ctx)
                            .AddApi(LocationServices.API)
                            .Build();

                    googleApiClient.Connect();

                    LocationRequest
                        locationRequest = LocationRequest.Create()
                            .SetPriority(LocationRequest.PriorityBalancedPowerAccuracy)
                            .SetInterval(interval)
                            .SetFastestInterval(fastestInterval);

                    LocationSettingsRequest.Builder
                        locationSettingsRequestBuilder = new LocationSettingsRequest.Builder()
                            .AddLocationRequest(locationRequest);

                    locationSettingsRequestBuilder.SetAlwaysShow(false);

                    LocationSettingsResult locationSettingsResult = await LocationServices.SettingsApi.CheckLocationSettingsAsync( googleApiClient, locationSettingsRequestBuilder.Build());

                    if (locationSettingsResult.Status.StatusCode == LocationSettingsStatusCodes.ResolutionRequired)
                    {
                        locationSettingsResult.Status.StartResolutionForResult( p0, 0);
                    }
                }
                catch (Exception exception)
                {
                    // Log exception
                }

                //-----------------------------------------------------------------------------------------------------------------
                
                //ctx.StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings).SetFlags(ActivityFlags.NewTask));
                //Application.Context.StartActivity(new Android.Content.Intent(Android.Provider.Settings.ActionLocat‌​ionSourceSettings));
            }
            else
            {
                //this is handled in the PCL
            }
        }


    }
}