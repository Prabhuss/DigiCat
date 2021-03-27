using Microsoft.AppCenter.Analytics;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Permissions;
using PyConsumerApp.Controls;
using PyConsumerApp.DataService;
using PyConsumerApp.Models;
using PyConsumerApp.Views.Transaction;
using Syncfusion.XForms.Buttons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Essentials;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using Plugin.Geolocator;

namespace PyConsumerApp.ViewModels.Transaction
{
    /// <summary>
    /// ViewModel for Checkout page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [DataContract]
    public class CheckoutPageViewModel : BaseViewModel, IPaymentResult
    {

        // { "tokenData" , "oGeDb2sML1YU8EcNOKsLHurG/H86MFoAws/cb7+1FUY="},

        #region Constructor
        public CheckoutPageViewModel(ProfileDataService userDataService, CartDataService cartDataService,
            CatalogDataService catalogDataService)
        {
            ButtonActive = true;
            this.userDataService = userDataService;
            this.cartDataService = cartDataService;
            this.catalogDataService = catalogDataService;
            var app = App.Current as App;
            DeliveryAddress = new ObservableCollection<Address>();
            PaymentModes = new ObservableCollection<Payment>();
            MinimumOrderAmount = (double)app.MinimumOrderAmount;
            Device.BeginInvokeOnMainThread(() =>
            {
                FetchPaymentOptions();
                FetchCartList();
                GetPaymentAndPriceMessage();
                GetPaymentCredentials();
                CheckLocationPermissions();
            });
            EditCommand = new Command(EditClicked);
            AddAddressCommand = new Command(AddAddressClicked);
            PlaceOrderCommand = new Command(PlaceOrderClicked);
            PaymentOptionCommand = new Command(PaymentOptionClicked);
            ApplyCouponCommand = new Command(ApplyCouponClicked);
            AddressChanged = new Command(addressChangedClicked);
            DeliveryOptionCommand = new Command(GetSelectedDeliveryAddressId);
            StateChangedCommand = new Command(GetSelectedPaymentOption);
            //Task.Run(() => CheckLocationPermissions());
        }

        #endregion

        #region Fields

        private bool isChecked;//radiobutton
        private string paymentOption = "NONE";
        private string stage = "TEST";//PROD
        private string AppId = "3259e36b680c168d9db036fa9523";
        private string secret = "97d1e574d1c7ba2021a9991bc598978d15c7ef9d";
        private bool isOnlinePaymentEnable = false;
        private string data = "";
        private string testPaymentApi = "";
        private string prodPaymentApi = "";
        public Address selectedDeliveryAddress;
        public Color SelectedAddressColor;
        private ObservableCollection<Address> deliveryAddress;
        public string currentLocationLatitude;
        public string currentLocationLongitude;

        private ObservableCollection<UserCart> orderedItems = new ObservableCollection<UserCart>();
        private ObservableCollection<Payment> paymentModes;
        private ObservableCollection<UserCart> cartDetails;
        public Dictionary<string, int> AddressTypeAndIdPair;

        private double? totalPrice;

        private double? discountPrice;

        private double? discountPercent;

        private double minimumOrderAmount;

        private string _paymentMessage;
        private string _priceMessage;
        private double? percent;
        private readonly ProfileDataService userDataService;
        private readonly CartDataService cartDataService;
        private readonly CatalogDataService catalogDataService;
        private readonly MyOrdersDataService myOrdersDataService;

        private Command backButtonCommand;

        #endregion

        #region Public properties

        public Command DeliveryOptionCommand { get; set; }
        public int DeliveryAddressId { get; set; }
        public string selectedAddressType { get; set; }
        public int selectedAddressId { get; set; }
        [DataMember(Name = "deliveryAddress")]
        public ObservableCollection<Address> DeliveryAddress
        {
            get => deliveryAddress;

            set
            {
                if (deliveryAddress == value) return;

                deliveryAddress = value;
                NotifyPropertyChanged();
            }
        }
        private bool buttonActive;
        public bool ButtonActive
        {
            get => buttonActive;

            set
            {
                if (buttonActive == value) return;

                buttonActive = value;
                NotifyPropertyChanged();
            }
        }
        private bool showAddressLabel;
        public bool ShowAddressLabel
        {
            get => showAddressLabel;

            set
            {
                if (showAddressLabel == value) return;

                showAddressLabel = value;
                NotifyPropertyChanged();
            }
        }

        
        //radio button
        public bool IsChecked
        {
            get => isChecked; 
            set
            {
                isChecked = value;
                NotifyPropertyChanged("IsChecked");
            }
        }

        public string Stage
        {
            get => stage;
            set
            {
                stage = value;
                NotifyPropertyChanged();
            }
        }
        
        public string PriceMessage
        {
            get => _priceMessage;
            set
            {
                _priceMessage = value;
                NotifyPropertyChanged();
            }
        }
        public string PaymentMessage
        {
            get => _paymentMessage;
            set
            {
                _paymentMessage = value;
                NotifyPropertyChanged();
            }
        }
        public bool IsOnlinePaymentEnable
        {
            get => isOnlinePaymentEnable;
            set
            {
                isOnlinePaymentEnable = value;
                NotifyPropertyChanged();
            }
        }
        public ObservableCollection<Payment> PaymentModes
        {
            get => paymentModes;

            set
            {
                if (paymentModes == value) return;

                paymentModes = value;
                NotifyPropertyChanged();
            }
        }

        public string PaymentOption
        {
            get => paymentOption;
            set
            {
                paymentOption = value;
                NotifyPropertyChanged();
            }
        }

        public string PaymentAppId
        {
            get => AppId;
            set
            {
                AppId = value;
                NotifyPropertyChanged();
            }
        }
        public string PaymentSecretKey
        {
            get => secret;
            set
            {
                secret = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserCart> OrderedItems
        {
            get => orderedItems;

            set
            {
                if (orderedItems == value) return;

                orderedItems = value;
                NotifyPropertyChanged();
            }
        }

        public ObservableCollection<UserCart> CartDetails
        {
            get => cartDetails;

            set
            {
                if (cartDetails == value) return;

                cartDetails = value;
                NotifyPropertyChanged();
            }
        }

        public double? TotalPrice
        {
            get => totalPrice;

            set
            {
                if (totalPrice == value) return;

                totalPrice = value;
                NotifyPropertyChanged();
            }
        }

        public double? DiscountPrice
        {
            get => discountPrice;

            set
            {
                if (discountPrice == value) return;

                discountPrice = value;
                NotifyPropertyChanged();
            }
        }

        public double? DiscountPercent
        {
            get => discountPercent;

            set
            {
                if (discountPercent == value) return;

                discountPercent = value;
                NotifyPropertyChanged();
            }
        }

        public double MinimumOrderAmount
        {
            get => minimumOrderAmount;

            set
            {
                if (minimumOrderAmount == value) return;

                minimumOrderAmount = value;
                NotifyPropertyChanged();
            }
        }
        [DataMember(Name = "selectedAddressType")]
        public string SelectedAddressType
        {
            get => selectedAddressType;

            set
            {
                if (selectedAddressType == value) return;

                selectedAddressType = value;
                NotifyPropertyChanged();
            }
        }
        [DataMember(Name = "currentLocationLongitude")]
        public string CurrentLocationLongitude
        {
            get => currentLocationLongitude;

            set
            {
                if (currentLocationLongitude == value) return;

                currentLocationLongitude = value;
                NotifyPropertyChanged();
            }
        }
        [DataMember(Name = "selectedDeliveryAddress")]
        public Address SelectedDeliveryAddress
        {
            get => selectedDeliveryAddress;

            set
            {
                if (selectedDeliveryAddress == value) return;

                selectedDeliveryAddress = value;
                NotifyPropertyChanged();
            }
        }
        
        [DataMember(Name = "currentLocationLatitude")]
        public string CurrentLocationLatitude
        {
            get => currentLocationLatitude;

            set
            {
                if (currentLocationLatitude == value) return;

                currentLocationLatitude = value;
                NotifyPropertyChanged();
            }
        }
        [DataMember(Name = "selectedAddressId")]
        public int SelectedAddressId
        {
            get => selectedAddressId;

            set
            {
                if (selectedAddressId == value) return;

                selectedAddressId = value;
                NotifyPropertyChanged();
            }
        }

        #endregion

        #region Command

        /// <summary>
        /// Gets or sets the command that will be executed when the Edit button is clicked.
        /// </summary>
        public Command EditCommand { get; set; }
        public Command AddressChanged { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Add new address button is clicked.
        /// </summary>
        public Command AddAddressCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the Edit button is clicked.
        /// </summary>
        public Command PlaceOrderCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the payment option button is clicked.
        /// </summary>
        public Command PaymentOptionCommand { get; set; }

        /// <summary>
        /// Gets or sets the command that will be executed when the apply coupon button is clicked.
        /// </summary>
        public Command ApplyCouponCommand { get; set; }

        /// <summary>
        /// Gets or sets the command is executed when the back button is clicked.
        /// </summary>
        /// 
        public Command StateChangedCommand { get; set; }

        [DataMember(Name = "backButtonCommand")]
        public Command BackButtonCommand => backButtonCommand ?? (backButtonCommand = new Command(BackButtonClicked));




        #endregion

        #region Methods


        public async void GetPaymentAndPriceMessage()
        {
            Merchantdata MerchantSettingDetails = await CategoryDataService.Instance.GetMerchantSettings("PaymentMessage");
            if (MerchantSettingDetails != null)
            {
                if (MerchantSettingDetails.SettingIsActive.ToLower() == "yes")
                {
                    PaymentMessage = MerchantSettingDetails.SettingMessage;
                }
            }
            MerchantSettingDetails = await CategoryDataService.Instance.GetMerchantSettings("Amount");
            if (MerchantSettingDetails != null)
            {
                if (MerchantSettingDetails.SettingIsActive.ToLower() == "yes")
                {
                    PriceMessage = MerchantSettingDetails.SettingMessage;
                }
            }
        }
        public async void FetchAddresses()
        {
            SelectedAddressType = null;
            DeliveryAddress.Clear();
            try
            {
                //if (App.CurrentUserId > 0)
                {
                    var addresses = await userDataService.GetAddresses();
                    if (addresses != null && addresses.Count > 0)
                        foreach (var address in addresses)
                        {
                            //AddressTypeAndIdPair.Add(address.TagName, address.Id);
                            DeliveryAddress.Add(address);
                        }
                }
                if(DeliveryAddress.Count <1)
                {
                    ShowAddressLabel = true;
                }
                else
                {
                    ShowAddressLabel = false;
                }
            }
            catch (Exception ex)
            {
                //await Application.Current.MainPage.DisplayAlert("Error2", ex.Message, "OK");
            }
        }

        /// <summary>
        /// This method is used to get the payment options and user card details
        /// </summary>
        private async void FetchPaymentOptions()
        {
            try
            {
                var paymentOptions = await catalogDataService.GetPaymentOptionsAsync();
                if (paymentOptions != null)
                    foreach (var paymentOption in paymentOptions)
                        PaymentModes.Add(new Payment
                        { PaymentMode = paymentOption.PaymentMode, CardTypeIcon = paymentOption.CardTypeIcon });

            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error3", ex.Message, "OK");
            }
        }

        /// <summary>
        /// This method is used to get the cart products from database
        /// </summary>
        private async void FetchCartList()
        {
            try
            {
                var products = await cartDataService.GetCartItemAsync();
                if (products != null && products.Count > 0)
                {
                    CartDetails = new ObservableCollection<UserCart>(products);
                    UpdatePrice();
                }
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error4", ex.Message, "OK");
            }
        }
        private void UpdatePrice()
        {
            ResetPriceValue();
            if (CartDetails != null && CartDetails.Count > 0)
            {
                foreach (var cartDetail in CartDetails)
                {
                    if (cartDetail.TotalQuantity == 0) cartDetail.TotalQuantity = 1;

                    this.TotalPrice += cartDetail.Product.ActualPrice * cartDetail.TotalQuantity;
                    this.DiscountPrice += cartDetail.Product.DiscountPrice * cartDetail.TotalQuantity;
                    percent += cartDetail.Product.DiscountPercent;
                }
                DiscountPercent = percent > 0 ? percent / CartDetails.Count : 0;
            }
        }


        private void ResetPriceValue()
        {
            TotalPrice = 0;
            DiscountPercent = 0;
            DiscountPrice = 0;
            percent = 0;
        }
        
        private async void EditClicked(object obj)
        {
            await Task.Delay(100);
            if (obj != null && obj is Address product && product != null)
            {
                await Navigation.PushAsync(new AddressEditPage(product));
            }
            await Task.Delay(10);
        }


        private async void AddAddressClicked(object obj)
        {
            await Task.Delay(100);
            if (CrossConnectivity.Current.IsConnected)
            {
                await Navigation.PushAsync(new AddressEditPage(new Address()));
                await Task.Delay(10);
            }
            else
            {
                try
                {
                    DependencyService.Get<IToastMessage>().ShortTime("Please connect to the Internet to add new Address.");
                }
                catch { }
            }
        }


        private async void addressChangedClicked(object obj)
        {
            await Application.Current.MainPage.DisplayAlert("Order Address", "Delivery Address Selected", "Ok");
            Address SelectedAddressOption = obj as Address;
            SelectedAddressId = SelectedAddressOption.Id;
        }

        public void onComplete(string result)
        {
            Debug.WriteLine($"SDK Result: {result}");
            var json = JsonConvert.DeserializeObject(result);
            Dictionary<string,string> obj = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            if (obj["txStatus"]=="SUCCESS")
            {
                orderComplete(obj["orderId"], obj["paymentMode"]);
            }

        }

        private async void PlaceOrderClicked(object obj)
        {

            IsBusy = true;
            if (!ButtonActive)
                return;
            ButtonActive = false;
            if (CrossConnectivity.Current.IsConnected)
            {
                await Task.Delay(1000);
                try
                {
                    if (SelectedAddressType == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("No Address Selected", "Please select address from the list or add new address", "Ok");
                        ButtonActive = true;
                        IsBusy = false;
                        return;
                    }
                    if (DiscountPrice < MinimumOrderAmount)
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", PriceMessage, "Ok");
                        ButtonActive = true;
                        IsBusy = false;
                        return;
                    }
                    if (PaymentOption.ToUpper() == "ONLINE PAYMENT")
                    {
                        var app = App.Current as App;
                        string invoiceId = "Order_" + app.Merchantid + "_" + String.Format("{0:d9}", (DateTime.Now.Ticks / 10) % 1000000000);
                        Dictionary<string, string> header = new Dictionary<string, string>();
                        header.Add("x-client-id", PaymentAppId);
                        header.Add("x-client-secret", PaymentSecretKey);

                        Dictionary<string, string> formParams = new Dictionary<string, string>();
                        formParams.Add("orderId", invoiceId);
                        formParams.Add("orderAmount", this.DiscountPrice.ToString());
                        formParams.Add("orderCurrency", "INR");
                        formParams.Add("orderNote", "Test payment");
                        formParams.Add("customerName", "Customer Name");
                        formParams.Add("customerPhone", app.UserPhoneNumber);
                        formParams.Add("customerEmail", "test@cashfree.com");
                        formParams.Add("returnUrl", "http://example.com");
                        formParams.Add("notifyUrl", "http://example.com");
                        //string signature = await MyOrdersDataService.Instance.generateCFtoken(formParams, header);
                        string signature = await MyOrdersDataService.Instance.generateCFtoken(formParams, header, Stage);
                        formParams.Add("tokenData", signature);
                        await Application.Current.MainPage.Navigation.PushAsync(new CFPaymentScreen(Stage, PaymentAppId, formParams, this));
                    }
                    else if (PaymentOption.ToUpper() == "PAY ON DELIVERY")
                    {
                        orderComplete("NULL", "COD");
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Oops!!!", "Select Your Payment Method !!!", "OK");
                        IsBusy = false;
                        ButtonActive = true;
                    }

                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
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
                buttonActive = true;
            }

        }


        private async void BackButtonClicked(object obj)
        {
             await Application.Current.MainPage.Navigation.PopAsync();
        }
       

        private async void orderComplete(string orderId, string paymentMode)
        {
            try
            {
                /**
                 * orderId meant PaymentOrderid, if payment is COD it will be null
                 */

                var app = App.Current as App;
                ObservableCollection<InvoiceItem> invoiceItems = new ObservableCollection<InvoiceItem>();
                CreateOrder createOrder = new CreateOrder();
                OrderDetails orderDetails = new OrderDetails();
                Invoice invoice = new Invoice();
                InvoiceItem invoiceItem = new InvoiceItem();
                createOrder.access_key = app.SecurityAccessKey;
                createOrder.phone_number = app.UserPhoneNumber;
                createOrder.merchant_id = int.Parse(app.Merchantid);

                invoice.DiscountAmount = 0;
                invoice.TaxAmount = 1.55;
                invoice.TotalInvoiceAmount = this.TotalPrice;
                invoice.CouponCode = null;
                invoice.PayableAmount = this.DiscountPrice;
                invoice.InvoiceType = "GetPYApp";
                invoice.OrderStatus = "New";
                invoice.PaymentMode = paymentMode;
                invoice.PaymentOrderId = orderId;

                invoice.DeliverAddressId = SelectedAddressId;
                orderDetails.Invoice = invoice;
                //Location's Distance check
                foreach (var GivenAddress in DeliveryAddress)
                {
                    if(GivenAddress.Id == SelectedAddressId)
                    {
                        //Call GetLocation function
                        var ContinueOrder = await GetGpsMessage(GivenAddress.Latitude, GivenAddress.Longitude);
                        if (!ContinueOrder)
                        {
                            ButtonActive = true;
                            IsBusy = false;
                            await Task.Delay(100);
                            return;
                        }
                    }
                }

                // Dictionary<string, object> InvoiceItem = new Dictionary<string, object>();
                if (CartDetails != null && CartDetails.Count > 0)
                    foreach (var item in CartDetails)
                    {
                        invoiceItems.Add(new InvoiceItem()
                        {
                            quantity = item.TotalQuantity,
                            ProductName = item.Product.productName,
                            ProductId = item.Product.CitrineProdId,
                            UnitPrice = item.Product.mrp,
                            Discount = item.Product.discount,
                            UnitPriceAfterDiscount = item.Product.SellingPrice,
                            TotalPrice = item.Product.SellingPrice * item.TotalQuantity,
                            ProductImage = item.Product.PreviewImage,
                            Category = item.Product.Category
                        });
                    }
                orderDetails.InvoiceItem = invoiceItems.ToArray();
                createOrder.order_details = orderDetails;
                string json = JsonConvert.SerializeObject(createOrder, Formatting.Indented);
                var isOrdered = await this.cartDataService.SaveOrdereInDb(json);
                if (isOrdered != null)
                { 
                    if (isOrdered.Status.ToUpper() == "SUCCESS")
                    {
                        await cartDataService.RemoveCartItemsAsync();
                    }
                    await Task.Delay(100);
                    Application.Current.MainPage = new NavigationPage(new PaymentSuccessPage(isOrdered));
                    await Task.Delay(100);
                    /*Keeping track of New Order 
                     */
                    Analytics.TrackEvent("New Order clicked", new Dictionary<string, string> {
                            { "UserPhoneNumber", app.UserPhoneNumber },
                            { "MerchantId", app.Merchantid },
                            { "InvoiceType ", "GetPY"},
                            { "PaymentMode", paymentMode},
                            { "TotalAmount", this.TotalPrice.ToString()}
                            });

                    IsBusy = false;
                    ButtonActive = true;
                }
                else
                {
                    DependencyService.Get<IToastMessage>().LongTime("Oops.. Somthing went wrong. Please check your connection and try again.");
                    ButtonActive = true;
                    IsBusy = false; 
                    await Task.Delay(100);
                }

            }
            catch (Exception ex)
            {
                try
                {
                    await Application.Current.MainPage.DisplayAlert("Server Error!", "Server time out. Please check your order on \"My Orders\" page for confirmation.", "Ok");
                }
                catch
                {
                    ButtonActive = true;
                    IsBusy = false;
                    await Task.Delay(100);
                }
                Console.WriteLine("Error1" + ex.Message);
                ButtonActive = true;
                IsBusy = false;
                await Task.Delay(100);
            }
        }


        private void PaymentOptionClicked(object obj)
        {
           /* if (obj is RowDefinition rowDefinition && rowDefinition.Height.Value == 0)
                rowDefinition.Height = GridLength.Auto;
            */
        }

        private void GetSelectedPaymentOption(object obj)
        {
            if (obj != null)
            {
                if ((obj as SfRadioButton).IsChecked == true)
                {
                    PaymentOption = (obj as SfRadioButton).Text;
                }
                //App.Current.MainPage.DisplayAlert("Message", "You have selected", (obj as SfRadioButton).Text);
                //await  App.Current.MainPage.DisplayAlert("Message", "You have selected", PaymentOption);
            }

        }
        private void GetSelectedDeliveryAddressId(object obj)
        {
            if (obj != null)
            {
                if ((obj as SfRadioButton).IsChecked == true)
                {
                    SelectedAddressType = (obj as SfRadioButton).Text;
                    foreach(var addrs in DeliveryAddress)
                    {
                        if(SelectedAddressType == addrs.TagName)
                        {
                            SelectedAddressId = addrs.Id;
                        }
                    }

                }
                //App.Current.MainPage.DisplayAlert("Message", "You have selected", (obj as SfRadioButton).Text);
                //await  App.Current.MainPage.DisplayAlert("Message", "You have selected", PaymentOption);
            }

        }
        public async void GetPaymentCredentials()
        {
            try
            {
                Dictionary<string, string> paymentDetails = await cartDataService.GetPaymentCredentials();
                if (paymentDetails != null)
                {
                   
                    if (paymentDetails["OnlinePaymentFlag"].ToLower() == "yes")
                    {
                        PaymentAppId = paymentDetails["apiId"];
                        PaymentSecretKey = paymentDetails["SecretKey"];
                        IsOnlinePaymentEnable = true;
                        Stage = paymentDetails["PaymentMode"].ToUpper();
                    }
                    else
                    {
                        IsOnlinePaymentEnable = false;
                        PaymentModes.Remove(PaymentModes.Where(i => i.PaymentMode.ToUpper() == "ONLINE PAYMENT").Single());
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error while fetching payment credentials" + e.Message);
            }
        }

        private void ApplyCouponClicked(object obj)
        {
            // Do something
        }

        private async Task<bool> GetGpsMessage(string Lati, string Longi)
        {
            try
            {
                DistanceData DistanceInfo = await CategoryDataService.Instance.CalculateDistance(Lati, Longi);
                if(DistanceInfo.Deliverable.ToLower() == "yes")
                {
                    if(DistanceInfo.Active.ToLower() == "yes")
                    {
                        var answer = await Application.Current.MainPage.DisplayAlert("Alert", DistanceInfo.Message, "OK", "CANCEL");
                        return answer;
                    }
                    return true;
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Alert", DistanceInfo.Message, "OK");
                    return false;
                }

            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Location Access Error:", "Unable to access your location. If your location is out side our Delivery Zone. Your order will be cancelled", "OK");
                string a = e.Message;
                return true;
            }
        }

        private async Task CheckLocationPermissions()
        {
            try
            {
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationWhenInUsePermission>();
                if (status != PermissionStatus.Granted)
                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
                    //if (status != PermissionStatus.Granted)
                if (Device.RuntimePlatform == Device.Android)
                    DependencyService.Get<ILocSettings>().OpenSettings();
            }
            catch (Exception e)
            {
                var x = e.Message;
                //await Application.Current.MainPage.DisplayAlert("Error LP01", e.Message, "OK");
                try
                {
                    DependencyService.Get<IToastMessage>().ShortTime("Error L01: Unable to fetch location");
                }
                catch (Exception ex) { var xx = ex.Message; }
                return ;
            }
        }

        #endregion
    }
}
