using PyConsumerApp.ViewModels.Products;
using SkiaSharp;
using Syncfusion.SfImageEditor.XForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PyConsumerApp.Views.Products
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SyncFusionImageCropper : ContentPage
    {

        public SyncFusionImageCropper()
        {
            InitializeComponent();
        }

        public SyncFusionImageCropper(Stream Imagestream = null)
        {
            InitializeComponent();
            ImageCropper.SetToolbarItemVisibility("Back, Text, Add, TextColor, FontFamily, Arial, Noteworthy, Marker Felt, Bradley Hand, SignPainter, Opacity, Path, StrokeThickness, Colors, Opacity, Shape, Rectangle, StrokeThickness, Circle, Arrow, Undo, Redo, Effects, Hue, Saturation, Brightness, Contrast, Blur and Sharpen", false);
            ImageCropper.ToggleCropping(new Rectangle(20, 20, 20, 20));
            if (Imagestream == null)
            {
                ImageCropper.Source = ImageSource.FromUri(new Uri("http://photos2.insidercdn.com/iphone4scamera-111004-full.JPG"));
            }
            else
            {
                ImageCropper.Source = ImageSource.FromStream(() =>
                {
                    return Imagestream;
                });
            }
        }

        [Obsolete]
        private async void ImageCropper_ImageSaving(object sender, ImageSavingEventArgs args)
        {
            var x = args.Stream;
            App.CroppedPhotoStream = x;
            args.Cancel = true;

            var bitmap = BitmapExtensions.LoadBitmapStream(x);
            var ImgHeight = bitmap.Height;
            var ImgWidth = bitmap.Width;
            var requiredHeight = 600;
            var reqiredWidth = ImgWidth;
            if(ImgHeight > requiredHeight)
            {
                var resizePercentage = (ImgHeight - requiredHeight) * 100 / ImgHeight;
                reqiredWidth = ImgWidth - (resizePercentage * ImgWidth / 100);
            }

            var dstInfo = new SKImageInfo(reqiredWidth, requiredHeight);
            var uploadbitMap = bitmap.Resize(dstInfo, SKBitmapResizeMethod.Box);
            App.CroppedPhotoBitMap = uploadbitMap;
            await Navigation.PopAsync(true);
            return;
        }
    }
}