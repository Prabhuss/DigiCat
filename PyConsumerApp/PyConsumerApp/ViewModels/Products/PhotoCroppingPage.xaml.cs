using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;


using SkiaSharp;
using SkiaSharp.Views.Forms;
using System.IO;


namespace PyConsumerApp.ViewModels.Products
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoCroppingPage : ContentPage
    {
        PhotoCropperCanvasView photoCropper;
        SKBitmap croppedBitmap;
        SKCanvasView canvasView;


        public PhotoCroppingPage(Stream stream = null)
        {
            InitializeComponent();
            SKBitmap bitmap = null;
            if (stream == null)
            {
                bitmap = BitmapExtensions.LoadImageFromInternet(
                    "http://photos2.insidercdn.com/iphone4scamera-111004-full.JPG");
            }
            else
            {
                bitmap = BitmapExtensions.LoadBitmapStream(stream);
            }

            photoCropper = new PhotoCropperCanvasView(bitmap);
            canvasViewHost.Children.Add(photoCropper);
        }

        async void OnDoneButtonClicked(object sender, EventArgs args)
        {
            //await DisplayAlert("Cropped the image, about to go back to previous screen", "", "OK");
            croppedBitmap = photoCropper.CroppedBitmap;
            App.CroppedPhotoBitMap = croppedBitmap;
            await Navigation.PopAsync(true);
        }

        void onRotateRightClicked(object send, EventArgs args)
        {

        }

        void onRotateLeftClicked(object send, EventArgs args)
        {

        }
        SKSurface skSurface;
        SKImageInfo skImageInfo;
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = skImageInfo = args.Info;
            SKSurface surface = skSurface = args.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();
            canvas.DrawBitmap(croppedBitmap, info.Rect, BitmapStretch.Uniform);
        }
    }
}