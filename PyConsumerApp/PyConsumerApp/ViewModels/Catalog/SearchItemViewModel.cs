using Microsoft.AppCenter.Analytics;
using Plugin.Connectivity;
using PyConsumerApp.Controls;
using PyConsumerApp.DataService;
using PyConsumerApp.Models;
using PyConsumerApp.Views.Bookmarks;
using PyConsumerApp.Views.Detail;
using PyConsumerApp.Views.Products;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.ViewModels.Catalog
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class SearchItemViewModel : BaseViewModel
    {
        string _searchText;
        bool _isVisibleStatus;
        bool isBusy;
        private int cartItemCount;
        private Command _searchCommand;
        private Command itemSelectedCommand;
        private Command quantitySelectedCommand;
        private Command cardItemCommand;
        private Command backButtonCommand;
        private Command loadMoreItemsCommand;
        private ObservableCollection<Product> products;
        App app;
        public SearchItemViewModel()
        {
            app = App.Current as App;
            products = new ObservableCollection<Product>();
           // LoadMoreItemsCommand = new Command(async () => await LoadData(), CanExecute);
        }
        //-------------------------------------------------------------------------------------------------------------------------
        // UPDATE Button Command
        private Command redirectToEditProductPageCommand;
        public Command RedirectToEditProductPageCommand
        {
            get { return this.redirectToEditProductPageCommand ?? (this.redirectToEditProductPageCommand = new Command(this.RedirectToEditProductPage_Clicked)); }
        }

        public async void RedirectToEditProductPage_Clicked(object obj)
        {
            try
            {
                if (obj != null && obj is Product product && product != null)
                {
                    await Navigation.PushAsync(new AddProductPage((Product)obj));
                }
            }
            catch
            { }
        }

        [DataMember(Name = "products")]
        public ObservableCollection<Product> Products
        {
            get
            {
                return this.products;
            }

            set
            {
                if (this.products == value)
                {
                    return;
                }

                this.products = value;
                this.NotifyPropertyChanged();
            }
        }
        public int CartItemCount
        {
            get => cartItemCount;
            set
            {
                cartItemCount = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsLoading
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                OnPropertyChanged("IsLoading");
            }
        }
        private bool CanExecute()
        {
            if (Products.Count == 0)
            {
                return false;
            }
            else if (Products.Count > 0)
            {
                return true;
            }
            return false;
        }
        public async void UpdateCartItemCount()
        {
            try
            {
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
        public Command LoadMoreItemsCommand
         {
             get
             {
                 //return loadMoreItemsCommand ?? (loadMoreItemsCommand = new Command(async () => await LoadData(), CanExecute));
                 return loadMoreItemsCommand ?? (loadMoreItemsCommand = new Command(this.LoadData));
            }
         }
        public Command BackButtonCommand
        {
            get { return this.backButtonCommand ?? (this.backButtonCommand = new Command(this.BackButtonClicked)); }
        }

        private async void BackButtonClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        public Command CardItemCommand
        {
            get { return this.cardItemCommand ?? (this.cardItemCommand = new Command(this.CartClicked)); }
        }
        public Command ItemSelectedCommand
        {
            get { return this.itemSelectedCommand ?? (this.itemSelectedCommand = new Command(this.ItemSelected)); }
        }

        public Command SearchCommand
        {
            get
            {
                return _searchCommand ?? (_searchCommand = new Command<string>((text) =>
                {
                    this.SearchClicked(text);
                }));
            }
        }
        public Command QuantitySelectedCommand
        {
            get
            {
                return this.quantitySelectedCommand ?? (this.quantitySelectedCommand = new Command(this.QuantitySelected));
            }
        }

        private void QuantitySelected(object obj)
        {
            Console.Write(obj);
        }

        public string SearchText
        {
            get{ return _searchText;}
            set{if (value != null){ _searchText = value;this.NotifyPropertyChanged();}}
        }

        public bool IsVisibleStatus
        {
            get { return _isVisibleStatus; }
            set { _isVisibleStatus = value;this.NotifyPropertyChanged(); }
        }

        private async void CartClicked(object obj)
        {
            try
            {
                IsBusy = true;
                if (CartItemCount > 0)
                    await Application.Current.MainPage.Navigation.PushAsync(new CartPage());
            }
            catch (Exception e)
            {

            }
            finally
            {
                IsBusy = false;
            }
            

        }
        
        private async void SearchClicked(string str)
        {
            IsBusy = true;
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    Analytics.TrackEvent("Search_Clicked", new Dictionary<string, string> {
                            { "MerchantBranchId", app.Merchantid},
                            { "UserPhoneNumber", app.UserPhoneNumber},
                            { "SearchItem", SearchText.Trim()},
                            });
                    if (Products.Count > 0)
                    {
                        Products.Clear();
                    }
                    int pageno = 1;
                    ObservableCollection<Product> productlist = await CategoryDataService.Instance.SearchItems(pageno, SearchText.Trim());
                    if (productlist.Count != 0)
                    {
                        foreach (var item in productlist)
                        {
                            if (products != null)
                            {
                                var isProductAlreadyAdded = products.Any(s => s.CitrineProdId == item.CitrineProdId);
                                if (!isProductAlreadyAdded)
                                {
                                    Products.Add(item);
                                }
                            }
                            else
                            {
                                Products.Add(item);
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    try
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Something went wrong. Please try again.");
                    }
                    catch { }
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    IsBusy = false;
                }
            }
            else
            {
                try
                {
                    DependencyService.Get<IToastMessage>().LongTime("No Internet Connection");
                }
                catch { }
                IsBusy = false;
            }

        }
        /* 
         * Load data on scrolling 
         */
        private async void LoadData()
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    IsLoading = true;
                    int pageno = 0;
                    if (products.Count > 0)
                    {
                        pageno = (products.Count / 10) + 1;
                    }
                    else
                    {
                        pageno = 1;
                    }
                    ObservableCollection<Product> productlist = await CategoryDataService.Instance.SearchItems(pageno, SearchText.Trim());
                    if (productlist.Count != 0)
                    {
                        foreach (var item in productlist)
                        {
                            if (products != null)
                            {
                                var isProductAlreadyAdded = products.Any(s => s.CitrineProdId == item.CitrineProdId);
                                if (!isProductAlreadyAdded)
                                {
                                    Products.Add(item);
                                }
                            }
                            else
                            {
                                Products.Add(item);
                            }

                        }
                    }
                }
                catch (Exception e)
                {

                }
                finally
                {
                    IsLoading = false;
                }
            }
            else
            {
                try
                {
                    DependencyService.Get<IToastMessage>().LongTime("No Internet Connection");
                }
                catch { }
                IsLoading = false;
            }
        }

        private async void ItemSelected(object obj)
        {
            Console.WriteLine(obj);
            IsBusy = true;
            Log.Debug("[ItemSelected]", "Selected Item" + obj);
            await Navigation.PushAsync(new DetailPage((Product)obj));
            await Task.Delay(100);
            IsBusy = false;

        }

    }
}
