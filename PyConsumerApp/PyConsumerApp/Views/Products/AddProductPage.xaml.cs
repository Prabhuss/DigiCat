using PyConsumerApp.Models;
using PyConsumerApp.ViewModels.Products;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PyConsumerApp.Views.Products
{
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddProductPage : ContentPage
    {
        private AddProductViewModel vm;

        public SKBitmap IsImageAlreadyLoaded;
        public AddProductPage()
        {
            InitializeComponent();
            vm = new AddProductViewModel();
            BindingContext = vm;
        }
        public AddProductPage(Product thisProduct)
        {
            InitializeComponent();
            vm = new AddProductViewModel(thisProduct);
            BindingContext = vm;
        }

        protected override void OnAppearing()
        {
            if (App.CroppedPhotoBitMap != null )
            {
                if (IsImageAlreadyLoaded != App.CroppedPhotoBitMap)
                    addImageSourceFromSKBitmap();
            }
        }

        SKCanvasView canvasView;
        private void addCanvasView()
        {
            canvasView = new SKCanvasView();
            canvasView.PaintSurface += OnCanvasViewPaintSurface;
            imageGrid.Children.Add(canvasView, 0, 1);
        }
        private void addImageSourceFromSKBitmap()
        {
            try
            {
                IsImageAlreadyLoaded = App.CroppedPhotoBitMap;
                if (App.CroppedPhotoBitMap != null)
                {
                    vm.ImageExist = true;
                }
                else
                {
                    vm.ImageExist = false;
                    return;
                }
                MyProductImage.Source = ImageSource.FromStream(() =>
                {
                    var stream = ViewBitmapAsStream(App.CroppedPhotoBitMap);
                    return stream;
                });
            }
            catch
            {
                return;
            }
        }
        public void addImageSourceFromStream()
        {
            if (App.CroppedPhotoStream != null)
            {
                vm.ImageExist = true;
            }
            else
                return;
            MyProductImage.Source = ImageSource.FromStream(() =>
            {
                var stream = App.CroppedPhotoStream;
                return stream;
            });
        }
        public Stream ViewBitmapAsStream(SKBitmap ImageBitmap)
        {
            // get the bitmap we want to convert to a stream
            SKBitmap bitmap = ImageBitmap;

            // create an image COPY
            SKImage image = SKImage.FromBitmap(bitmap);
            // OR
            // create an image WRAPPER
            //SKImage image = SKImage.FromPixels(bitmap.PeekPixels());

            // encode the image (defaults to PNG)
            SKData encoded = image.Encode();

            // get a stream over the encoded data
            Stream ImageStream = encoded.AsStream();
            return ImageStream;
        }
        void OnCanvasViewPaintSurface(object sender, SKPaintSurfaceEventArgs args)
        {
            SKImageInfo info = args.Info;
            SKSurface surface = args.Surface;
            SKCanvas canvas = surface.Canvas;
            canvas.Clear();
            canvas.DrawBitmap(App.CroppedPhotoBitMap, 0, 0);
        }


        protected override bool OnBackButtonPressed()
        {
            App.CroppedPhotoBitMap = null;
            base.OnBackButtonPressed();
            return false;
        }
    }
}