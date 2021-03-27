using System;
using PyConsumerApp.Models;
using PyConsumerApp.Views.Products;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Plugin.Media;
using System.Linq;
using System.IO;
using SkiaSharp;
using Rg.Plugins.Popup.Services;

namespace PyConsumerApp.ViewModels.Products
{
    [Preserve(AllMembers = true)]
    [DataContract]
    class UploadImageViewModel :BaseViewModel
    {
        public UploadImageViewModel()
        {

        }


        private Command loadImageFromCamera;
        public Command LoadImageFromCamera 
        {
            get { return loadImageFromCamera ?? (this.loadImageFromCamera = new Command(this.LoadImageFromCamera_Clicked)); }
        }

        private Command uploadPhotoFromMedia;
        public Command UploadPhotoFromMedia 
        {
            get { return uploadPhotoFromMedia ?? (this.uploadPhotoFromMedia = new Command(this.UploadPhotoFromMedia_Clicked)); }
        }

        //---------------------------------------------------------------------------------------------------------------------
        //UPLOAD PHOTO FROM Camera
        async void LoadImageFromCamera_Clicked(object ob)
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("No Camera", ":( No camera available.", "OK");
                    return;
                }
                try
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                catch { }
                var file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                {
                    RotateImage = true,
                    CompressionQuality = 40
                });
                if (file == null)
                {
                    return;
                }
                await Navigation.PushAsync(new SyncFusionImageCropper(file.GetStream()));
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Access Error", "Unable to access Camera. Please try again.", "OK");
            }
        }

        //---------------------------------------------------------------------------------------------------------------------
        //UPLOAD PHOTO FROM MEDIA

        async void UploadPhotoFromMedia_Clicked()
        {
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Upload not supported on this device", "OK");
                    return;
                }

                try
                {
                    await PopupNavigation.Instance.PopAsync();
                }
                catch { }
                var file = await CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions
                {
                    PhotoSize = Plugin.Media.Abstractions.PhotoSize.Full,
                    CompressionQuality = 40

                });

                // Convert file to Stream
                using (Stream stream = file.GetStream())
                {
                    await Navigation.PushAsync(new SyncFusionImageCropper(file.GetStream()));
                }
            }
            catch
            {
                //await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong!!", "OK");
            }
        }

    }
}
