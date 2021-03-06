﻿using Microsoft.AppCenter.Analytics;
using Plugin.Connectivity;
using PyConsumerApp.Controls;
using PyConsumerApp.DataService;
using PyConsumerApp.Models;
using PyConsumerApp.Views.Bookmarks;
using PyConsumerApp.Views.Detail;
using PyConsumerApp.Views.Products;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.ViewModels.Detail
{
    [Preserve(AllMembers = true)]
    public class DetailPageViewModel : BaseViewModel
    {
        private double productRating;
        //private ObservableCollection<Category> categories;
        private bool isFavourite;
        private bool isReviewVisible;
        private int? cartItemCount;
        //private readonly ICatalogDataService catalogDataService;
        //private readonly ICartDataService cartDataService;
        //private readonly IWishlistDataService wishlistDataService;
        /// <summary>
        /// /////////////////
        /// </summary>
        private readonly Product selectedProduct;
        public Product productDetail;
        public Product ProductDetail
        {
            get => productDetail;

            set
            { 
                productDetail = value;
                OnPropertyChanged();
                NotifyPropertyChanged();
            }
        }

       public DetailPageViewModel()
        {

        }
        public DetailPageViewModel(Product selectedProduct)
        {
           
            ProductDetail = selectedProduct;
            CardItemCommand = new Command(CartClicked);

            //this.selectedProduct = selectedProduct;
            //FetchProduct(selectedProduct.id.ToString());
            Debug.WriteLine(@"ProductDetail" + ProductDetail);
            /* Device.BeginInvokeOnMainThread(() =>
             {
                 FetchProduct(selectedProduct.Id.ToString());
             });*/

        }

        private async void CartClicked(object obj)
        {
            if (CartItemCount > 0)
                await Application.Current.MainPage.Navigation.PushAsync(new CartPage());
            
        }
        public Product GetProductDetail(Product product)
        {
            var selectedPoductDetail = new Product();
            selectedPoductDetail = product;

            /* if (selectedPoductDetail.Reviews == null || selectedPoductDetail.Reviews.Count == 0)
                 IsReviewVisible = true;
             else
                 foreach (var review in selectedPoductDetail.Reviews)
                     productRating += review.Rating;

             if (productRating > 0)
                 selectedPoductDetail.OverallRating = productRating / selectedPoductDetail.Reviews.Count;
             */

            return selectedPoductDetail;
        }
        public bool IsFavourite
        {
            get => isFavourite;
            set
            {
                isFavourite = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsReviewVisible
        {
            get
            {
                if (productDetail != null && productDetail.Reviews != null)
                    if (productDetail.Reviews.Count == 0)
                        isReviewVisible = true;

                return isReviewVisible;
            }
            set
            {
                isReviewVisible = value;
                NotifyPropertyChanged();
            }
        }
        public int? CartItemCount
        {
            get => cartItemCount;
            set
            {
                cartItemCount = value;
                NotifyPropertyChanged();
            }
        }
        public Command AddFavouriteCommand { get; set; }

        public Command NotificationCommand { get; set; }

        public Command AddToCartCommand { get; set; }

        public Command LoadMoreCommand { get; set; }

        public Command ShareCommand { get; set; }

        public Command VariantCommand { get; set; }

        public Command CardItemCommand { get; set; }

        private Command backButtonCommand;
        public Command BackButtonCommand//{ get; set; }
        {
            get { return backButtonCommand ?? (this.backButtonCommand = new Command(this.BackButtonClicked)); }
        }

        public Command imageTappedCommand;
        public Command ImageTappedCommand
        {
            get { return imageTappedCommand ?? (this.imageTappedCommand = new Command(this.imageTappedCommand_Clicked)); }
        }
        public async void UpdateCartItemCount()
        {
            try
            {
               // if (App.CurrentUserId > 0)
                {
                    
                    var cartItems = await CartDataService.Instance.GetCartItemAsync();
                    if (cartItems != null) CartItemCount = cartItems.Count;
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
        private async void BackButtonClicked(object obj)
        {
            // Do something
            //await Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private Command redirectToEditProductPageCommand;
        public Command RedirectToEditProductPageCommand
        {
            get { return this.redirectToEditProductPageCommand ?? (this.redirectToEditProductPageCommand = new Command(this.RedirectToEditProductPage_Clicked)); }
        }

        public async void RedirectToEditProductPage_Clicked(object obj)
        {
            IsBusy = true;
            try
            {
                await Navigation.PushAsync(new AddProductPage(ProductDetail));
                IsBusy = false;
            }
            catch(Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong while opening the editor", "OK");
                IsBusy = false;
            }
        }


        public async void imageTappedCommand_Clicked(object obj)
        {
            try
            {
                var imageUrl = new Uri(obj as string);

                await Application.Current.MainPage.Navigation.PushAsync(new DisplayImagePage(imageUrl));
            }
            catch { }
        }
    }
}

