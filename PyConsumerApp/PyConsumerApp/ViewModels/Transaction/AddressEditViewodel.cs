using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Geolocator;
using PyConsumerApp.Controls;
using PyConsumerApp.DataService;
using PyConsumerApp.Models;
using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using System.Linq;

namespace PyConsumerApp.ViewModels.Transaction
{

    [Preserve(AllMembers = true)]
    [DataContract]
    class AddressEditViewodel : BaseViewModel
    {
        public Command ChangeAddressInfo { get; set; }
        public Command UseMyLocationCommand { get; set; }
        
        public Address customerAddress;

        public ObservableCollection<string> addressTypesList;
        public ObservableCollection<string> AddressTypesList
        {
            get { return addressTypesList; }
            set
            {
                if (addressTypesList != value)
                {
                    addressTypesList = value;
                    OnPropertyChanged();
                }
            }
        }
        
        string _addressLocationMessage;
        [DataMember(Name = "AddressLocationMessage")]
        public string AddressLocationMessage
        {
            get { return _addressLocationMessage; }
            set
            {
                if (_addressLocationMessage != value)
                {
                    _addressLocationMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        string selectedAddressType;
        [DataMember(Name = "selectedAddressType")]
        public string SelectedAddressType
        {
            get { return selectedAddressType; }
            set
            {
                if (selectedAddressType != value)
                {
                    selectedAddressType = value;
                    OnPropertyChanged();
                }
            }
        }

        [DataMember(Name = "customerAddress")]
        public Address CustomerAddress
        {
            get => customerAddress;

            set
            {
                if (customerAddress == value) return;
                customerAddress = value;
                OnPropertyChanged();
                NotifyPropertyChanged();
            }
        }

        private bool _locationCheckValue = true;
        [DataMember(Name = "LocationCheckValue")]
        public bool LocationCheckValue
        {
            get => _locationCheckValue;

            set
            {
                if (_locationCheckValue == value) return;
                _locationCheckValue = value;
                NotifyPropertyChanged();
            }
        }
        public AddressEditViewodel(Address ThisAddress)
        {
            customerAddress = new Address();
            CustomerAddress = ThisAddress;
            addressTypesList = new ObservableCollection<string>();
            addressTypesList.Add("Office");
            addressTypesList.Add("Home");
            addressTypesList.Add("Other");
            selectedAddressType = "";
            InitilizePage();
            ChangeAddressInfo = new Command(this.ChangeAddress_Clicked);
        }

        private Command backButtonCommand;
        [DataMember(Name = "backButtonCommand")]
        public Command BackButtonCommand => backButtonCommand ?? (backButtonCommand = new Command(BackButtonClicked));

        private async void BackButtonClicked(object obj)
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }

        private async Task UseMyLocation_Clicked()
        {
            try
            {
                //check for cache location because it is faster
                var location = await Geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    //if no cache location is stored generate new request for location
                    var request = new GeolocationRequest(GeolocationAccuracy.Medium);
                    location = await Geolocation.GetLocationAsync(request);
                    if (location == null)
                    {
                        //if erroe in fetching the location 
                        DependencyService.Get<IToastMessage>().ShortTime("Error L01: Unable to fetch Current location");
                        return;
                    }
                }
                //assigning location to the Customer address
                CustomerAddress.Latitude = location.Latitude.ToString();
                CustomerAddress.Longitude = location.Longitude.ToString();
            }
            catch (FeatureNotSupportedException fnsEx)
            {
                DependencyService.Get<IToastMessage>().ShortTime("Error L02: Location Feature is not supported by your device.");
                return;
            }
            catch (PermissionException pEx)
            {
                DependencyService.Get<IToastMessage>().ShortTime("Error L03: Seems like your GPS is OFF.");
                return;
            }
            catch (System.Exception ex)
            {
                DependencyService.Get<IToastMessage>().ShortTime("Error L04: Unable to fetch Current Location");
                return;
            }
        }
        private async void ChangeAddress_Clicked(object obj)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                if (CustomerAddress.FirstName == null || CustomerAddress.FirstName.Replace(" ", "") == "" ||
                    CustomerAddress.Address2 == null || CustomerAddress.Address2.Replace(" ", "") == "" ||
                    CustomerAddress.TagName == null || CustomerAddress.TagName.Replace(" ", "") == "" ||
                    CustomerAddress.Address1 == null || CustomerAddress.Address1.Replace(" ", "") == "" ||
                    CustomerAddress.PostalCodeZipCode == null || CustomerAddress.PostalCodeZipCode.Replace(" ", "") == "")
                {
                    await Application.Current.MainPage.DisplayAlert("Fields Empty Error", "Please fill all the Mandatory fields", "Ok");
                    return;
                }
                try
                {
                    //reset Location
                    CustomerAddress.Latitude = null;
                    CustomerAddress.Longitude = null;
                    if (LocationCheckValue)
                    {
                        bool PermissionForLocation = await CheckLocationPermissions();
                        if (PermissionForLocation)
                           await UseMyLocation_Clicked();
                    }
                    await Task.Delay(500);
                    bool resonse = await CartDataService.Instance.SaveAddressInfo(CustomerAddress);
                    if (resonse == true)
                    {
                        DependencyService.Get<IToastMessage>().LongTime("Address changed successfully");
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        DependencyService.Get<IToastMessage>().ShortTime("Error AD01: Something went wrong while changing the address details");
                    }
                }
                catch (Exception e)
                {
                    await Application.Current.MainPage.DisplayAlert("Error AD02", e.Message, "OK");
                }
            }
            else
            {
                try
                {
                    DependencyService.Get<IToastMessage>().ShortTime("No Internet Connection");
                }
                catch { }
            }
        }

        //we will fetch actomatic location component and Location message here
        public void InitilizePage()
        {
            //Location Component
            FetchLocationFieldsFromGPS();
            SetGpsLocationMessage();
        }
        public async void FetchLocationFieldsFromGPS()
        {
            try
            {
                //fetch position
                var locator = CrossGeolocator.Current;
                var position = await locator.GetLastKnownLocationAsync();
                if (position == null)
                    position = await locator.GetPositionAsync();

                if (position != null)
                {
                    //if position fetched successfully.. Fetch address for the position
                    var address = await locator.GetAddressesForPositionAsync(position, "RJHqIE53Onrqons5CNOx~FrDr3XhjDTyEXEjng-CRoA~Aj69MhNManYUKxo6QcwZ0wmXBtyva0zwuHB04rFYAPf7qqGJ5cHb03RCDw1jIW8l");
                    if (address == null)
                        return;
                    var a = address.FirstOrDefault();
                    var myAddress = new Address();
                    myAddress.PostalCodeZipCode = a.PostalCode;
                    if (CustomerAddress.FirstName == null)
                        CustomerAddress = myAddress;
                }
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
            }
        }
        public async void SetGpsLocationMessage()
        {
            Merchantdata MerchantSettingDetails = await CategoryDataService.Instance.GetMerchantSettings("LocationMessage");
            if (MerchantSettingDetails != null)
            {
                if (MerchantSettingDetails.SettingIsActive.ToLower() == "yes")
                {
                    AddressLocationMessage = MerchantSettingDetails.SettingMessage;
                }
            }
        }

        private async Task<bool> CheckLocationPermissions()
        {
            try
            {
                PermissionStatus status = await CrossPermissions.Current.CheckPermissionStatusAsync<LocationWhenInUsePermission>();
                if (status != PermissionStatus.Granted)
                {
                    status = await CrossPermissions.Current.RequestPermissionAsync<LocationWhenInUsePermission>();
                    if (status != PermissionStatus.Granted)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return true;
                }
            }
            catch(Exception e)
            {
                //await Application.Current.MainPage.DisplayAlert("Error LP01", e.Message, "OK");
                return false;
            }
        }
    }
}
