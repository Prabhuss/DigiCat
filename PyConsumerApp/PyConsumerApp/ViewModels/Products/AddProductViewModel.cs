using System;
using System.Collections.Generic;
using PyConsumerApp.DataService;
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
using Rg.Plugins.Popup.Extensions;
using PyConsumerApp.Controls;
using Rg.Plugins.Popup.Services;
using PyConsumerApp.Views.Catalog;
using System.Net;

namespace PyConsumerApp.ViewModels.Products
{
    [Preserve(AllMembers = true)]
    [DataContract]
    public class AddProductViewModel : BaseViewModel
    {
        public AddProductViewModel()
        {
            PageTitle = "Add Product";
            DisplayBackButton = false;
            this.LoadImageFromCamera = new Command(OnAddImageButton_Clicked);
            //initialize all the list
            _CategoryList = new Dictionary<string, int>();
            _SubCategory1List = new Dictionary<string, int>();
            _SubCategory2List = new Dictionary<string, int>();
            _ThisNewProduct = new Product();
            //ThisNewProduct.AvailabilityStatusBool = true;
            //ThisNewProduct.VisibilityStatusBool = true;
            DisplayCategoriesAsync = new Command(DisplayCategories_clicked);
            DisplaySubCategory1Async = new Command(DisplaySubCategories1_clicked);
            DisplaySubCategory2Async = new Command(DisplaySubCategories2_clicked);
            MenuButtonCommand = new Command(MenuButtonCommand_clicked);
            ProductCommand = new Command(AddNewProduct_clicked);
            ResetFormCommand = new Command(ResetFormForNewProduct_Clicked);
        }

        public AddProductViewModel(Product fetchedProduct)
        {
            PageTitle = "Edit Product";
            DisplayBackButton = true;
            this.LoadImageFromCamera = new Command(OnAddImageButton_Clicked);
            //initialize all the list
            _CategoryList = new Dictionary<string, int>();
            _SubCategory1List = new Dictionary<string, int>();
            _SubCategory2List = new Dictionary<string, int>();
            _ThisNewProduct = new Product();
            OriginalProductDetails = fetchedProduct;
            ThisNewProduct = new Product(fetchedProduct);
            ProductImageBeforeEdit = UrlToSkbitmap(ThisNewProduct.PreviewImage);
            App.CroppedPhotoBitMap = ProductImageBeforeEdit;
            if(App.CroppedPhotoBitMap !=null)
                ImageExist = true;
            SetupFormForFetchedProduct(fetchedProduct);
             DisplayCategoriesAsync = new Command(DisplayCategories_clicked);
            DisplaySubCategory1Async = new Command(DisplaySubCategories1_clicked);
            DisplaySubCategory2Async = new Command(DisplaySubCategories2_clicked);
            MenuButtonCommand = new Command(MenuButtonCommand_clicked);
            ResetFormCommand = new Command(ResetFormForUpdateProduct_Clicked);
            ProductCommand = new Command(UpdateProduct_clicked);
        }
        public Product _ThisNewProduct;

        public bool _ImageExist;
        public bool _IsSubCat1Available;
        public bool _IsSubCat2Available;
        private SKBitmap ProductImageBeforeEdit;
        public Command DisplayCategoriesAsync { get; set; }
        public Command DisplaySubCategory1Async { get; set; }
        public Command DisplaySubCategory2Async { get; set; }
        public Command ProductCommand { get; set; }
        public Command MenuButtonCommand { get; set; }
        public Command ResetFormCommand { get; set; }


        private Command backButtonCommand;
        public Command BackButtonCommand//{ get; set; }
        {
            get { return backButtonCommand ?? (this.backButtonCommand = new Command(this.BackButtonClicked)); }
        }

        public bool _DisplayBackButton;
        public bool DisplayBackButton
        {
            get => _DisplayBackButton;

            set
            {
                if (_DisplayBackButton == value)
                    return;
                _DisplayBackButton = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }
        public bool IsSubCat1Available
        {
            get => _IsSubCat1Available;

            set
            {
                if (_IsSubCat1Available == value)
                    return;
                _IsSubCat1Available = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }
        public bool IsSubCat2Available
        {
            get => _IsSubCat2Available;

            set
            {
                if (_IsSubCat2Available == value)
                    return;
                _IsSubCat2Available = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }
        public Product OriginalProductDetails;
        public Product ThisNewProduct
        {
            get => _ThisNewProduct;

            set
            {
                if (_ThisNewProduct == value)
                    return;
                _ThisNewProduct = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }
        public bool ImageExist
        {
            get => _ImageExist;

            set
            {
                if (_ImageExist == value)
                    return;
                _ImageExist = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }
        public bool _EnableId = true;
        public bool EnableId
        {
            get => _EnableId;

            set
            {
                if (_EnableId == value)
                    return;
                _EnableId = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }
        
        public bool _EnableCat = true;
        public bool EnableCat
        {
            get => _EnableCat;

            set
            {
                if (_EnableCat == value)
                    return;
                _EnableCat = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }
        public bool _EnableName = true;
        public bool EnableName
        {
            get => _EnableName;

            set
            {
                if (_EnableName == value)
                    return;
                _EnableName = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }
        
        public string _PageTitle;
        public string PageTitle
        {
            get { return _PageTitle; }
            set
            {
                if (_PageTitle != value)
                {
                    _PageTitle = value;
                    OnPropertyChanged();
                }
            }
        }
        public string _SelectedCategory;
        public string SelectedCategory
        {
            get { return _SelectedCategory; }
            set
            {
                if (_SelectedCategory != value)
                {
                    _SelectedCategory = value;
                    OnPropertyChanged();
                }
            }
        }
        public string _SelectedSubCategory1;
        public string SelectedSubCategory1
        {
            get { return _SelectedSubCategory1; }
            set
            {
                if (_SelectedSubCategory1 != value)
                {
                    _SelectedSubCategory1 = value;
                    OnPropertyChanged();
                }
            }
        }
        public string _SelectedSubCategory2;
        public string SelectedSubCategory2
        {
            get { return _SelectedSubCategory2; }
            set
            {
                if (_SelectedSubCategory2 != value)
                {
                    _SelectedSubCategory2 = value;
                    OnPropertyChanged();
                }
            }
        }
        public Dictionary<string, int> _CategoryList;
        public Dictionary<string, int> CategoryList
        {
            get { return _CategoryList; }
            set
            {
                if (_CategoryList != value)
                {
                    _CategoryList = value;
                    OnPropertyChanged();
                }
            }
        }
        public Dictionary<string, int> _SubCategory1List;
        public Dictionary<string, int> SubCategory1List
        {
            get { return _SubCategory1List; }
            set
            {
                if (_SubCategory1List != value)
                {
                    _SubCategory1List = value;
                    OnPropertyChanged();
                }
            }
        }

        public Dictionary<string, int> _SubCategory2List;
        public Dictionary<string, int> SubCategory2List
        {
            get { return _SubCategory2List; }
            set
            {
                if (_SubCategory2List != value)
                {
                    _SubCategory2List = value;
                    OnPropertyChanged();
                }
            }
        }
        public async void OnAddImageButton_Clicked(object obj)
        {
            //await loadImageFromCamera();
            //await UploadPhotoFromMedia();

            await Navigation.PushPopupAsync(new UploadImagePopup(),true);
        }

        public bool isImageDisplayRequired = false;

        public Command LoadImageFromCamera { get; set; }
         
        public SKBitmap UrlToSkbitmap(string url)
        {
            try
            {
                byte[] stream = null;
                using (var webClient = new WebClient())
                {
                    stream = webClient.DownloadData(url);

                    // decode the bitmap stream
                    return SKBitmap.Decode(stream);
                }
            }
            catch (Exception e)
            {
                var ex = e.Message;
                return null;
            }
        }
        async public Task<bool> PopulateCategoryListData()
        {
            try
            {
                var hasItem = false;
                ObservableCollection<SubCategory> categoriesLsit = await CategoryDataService.Instance.FetchCatagoryListForAddProductPage();
                if (categoriesLsit != null)
                {
                    foreach (var category in categoriesLsit)
                    {
                        try
                        {
                            CategoryList.Add(category.Name, category.SubCategoryId);
                            hasItem = true;
                        }
                        catch(Exception e) { }
                    }
                }
                return hasItem;
            }
            catch (Exception e)
            {
                string a = e.Message;
                return false;
            }
        }

        public async Task<bool> GetSubCategory1FromSelectedCategory(double SubCatID)
        {
            try
            {
                var items = await CategoryDataService.Instance.GetSubCategoriesForAddProductForm(SubCatID);
                if (items != null)
                {
                    bool hasItem = false;
                    foreach (var category in items)
                        try
                        {
                            SubCategory1List.Add(category.Name, category.SubCategoryId);
                            hasItem = true;
                        }
                        catch { }
                    return hasItem;
                }
                return false;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return false;
            }
        }
        public async Task<bool> GetSubCategory2FromSelectedCategory(double SubCatID)
        {
            try
            {
                var hasItem = false;
                var items = await CategoryDataService.Instance.GetSubCategoriesForAddProductForm(SubCatID);
                if (items != null)
                {
                    foreach (var category in items)
                        try
                        {
                            SubCategory2List.Add(category.Name, category.SubCategoryId);
                            hasItem = true;
                        }
                        catch { }
                }
                return hasItem;
            }
            catch (Exception e)
            {
                var msg = e.Message;
                return false;
            }
        }

        public async void UpdateProduct_clicked(object obj)
        {
            try
            {
                /*Image
                if (App.CroppedPhotoBitMap == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Please select image for the product", "ok");
                    return;
                }*/
                await Task.Delay(500);
                //entries--//entries--
                if (ThisNewProduct.productName == null || ThisNewProduct.productName.Replace(" ", "") == "")
                {
                    DependencyService.Get<IToastMessage>().LongTime("Please fill all the Mandatory fields!");
                }
                else if (ThisNewProduct.SellingPrice <= 0 || ThisNewProduct.SellingPrice == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Selling Price can not be '0' or less than '0'", "ok");
                }
                else if (ThisNewProduct.DiscountPrice < 0 || ThisNewProduct.DiscountPrice == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "'Selling Price' can not be more than 'MRP'", "ok");
                }
                else if(ThisNewProduct.GST >100 || ThisNewProduct.CESS >100)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "'GST' and 'CESS' can not be more than 100%", "ok");
                }
                else
                {
                    IsBusy = true;
                    if (ThisNewProduct.AvailabilityStatusBool)
                        ThisNewProduct.Availability_Status = "Yes";
                    else
                        ThisNewProduct.Availability_Status = "No";

                    if (ThisNewProduct.VisibilityStatusBool)
                        ThisNewProduct.Visibility_Status = "Yes";
                    else
                        ThisNewProduct.Visibility_Status = "No";
                    if (App.CroppedPhotoBitMap != ProductImageBeforeEdit && App.CroppedPhotoBitMap != null)
                    {
                        SaSTokenModel TokenDetails = await AddProductDataService.Instance.GetSasTokenInfoForProductImage();
                        var pictureStream = ViewBitmapAsStream(App.CroppedPhotoBitMap);
                        var ImageUrl = await AddProductDataService.Instance.UploadProductImage(pictureStream, TokenDetails.blobURL, TokenDetails.SasToken);
                        if(ImageUrl != null)
                        {
                            ThisNewProduct.productPicUrl = ImageUrl;
                            ThisNewProduct.ImageLinkFlag = "F";
                        }
                    }
                    var result = await AddProductDataService.Instance.UpdateProductDetailsToDataBase(ThisNewProduct);
                    if (result != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", result, "ok");
                        OriginalProductDetails.CopyProductFrom(ThisNewProduct);
                        App.CroppedPhotoBitMap = null;
                        await Application.Current.MainPage.Navigation.PopAsync();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Error in Updating Product Details!", "ok");
                    }
                    IsBusy = false;
                }
            }
            catch(Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Message", "Some error!!", "ok");
            }
        }

        public async void AddNewProduct_clicked(object obj)
        {
            try
            {
                await Task.Delay(500);
                //Form Validation--------------------------------------------------------------------------------------------
                //Category--
                if (SelectedSubCategory2 != null)
                {
                    foreach(var listitem in SubCategory2List)
                    {
                        if(listitem.Key == SelectedSubCategory2)
                        {
                            ThisNewProduct.Category = SelectedSubCategory2;
                            foreach (var Parentlistitem in SubCategory1List)
                            {
                                if (Parentlistitem.Key == SelectedSubCategory1)
                                    ThisNewProduct.SubCategory = Parentlistitem.Value.ToString();
                            }
                        }
                    }
                }
                else if (SelectedSubCategory1 != null)
                {
                    if (IsSubCat2Available)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Please select Sub-Category 2", "ok");
                        return;
                    }
                    foreach (var listitem in SubCategory1List)
                    {
                        if (listitem.Key == SelectedSubCategory1)
                        {
                            ThisNewProduct.Category = SelectedSubCategory1;
                            foreach (var Parentlistitem in CategoryList)
                            {
                                if (Parentlistitem.Key == SelectedCategory)
                                    ThisNewProduct.SubCategory = Parentlistitem.Value.ToString();
                            }
                        }
                    }
                }
                else if (SelectedCategory != null)
                {
                    if(IsSubCat1Available)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error", "Please select Sub-Category 1", "ok");
                        return;
                    }
                    foreach (var listitem in CategoryList)
                    {
                        if (listitem.Key == SelectedCategory)
                        {
                            ThisNewProduct.Category = SelectedCategory;
                            ThisNewProduct.SubCategory = listitem.Value.ToString();
                        }
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Please select category", "ok");
                    return;
                }
                //Image
                if(App.CroppedPhotoBitMap == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Please select image for the product", "ok");
                    return;
                }
                //entries--
                if (ThisNewProduct.productName == null || ThisNewProduct.productName.Replace(" ", "") == "")
                {
                    DependencyService.Get<IToastMessage>().LongTime("Please fill all the Mandatory fields!");
                }
                else if (ThisNewProduct.SellingPrice <= 0 || ThisNewProduct.SellingPrice == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "Selling Price can not be '0' or less than '0'", "ok");
                }
                else if (ThisNewProduct.DiscountPrice < 0 || ThisNewProduct.DiscountPrice == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "'Selling Price' can not be more than 'MRP'", "ok");
                }
                else if(ThisNewProduct.GST >100 || ThisNewProduct.CESS >100)
                {
                    await Application.Current.MainPage.DisplayAlert("Message", "'GST' and 'CESS' can not be more than 100%", "ok");
                }
                else
                {
                    IsBusy = true;
                    if (ThisNewProduct.AvailabilityStatusBool)
                        ThisNewProduct.Availability_Status = "Yes";
                    else
                        ThisNewProduct.Availability_Status = "No";

                    if (ThisNewProduct.VisibilityStatusBool)
                        ThisNewProduct.Visibility_Status = "Yes";
                    else
                        ThisNewProduct.Visibility_Status = "No";
                    SaSTokenModel TokenDetails = await AddProductDataService.Instance.GetSasTokenInfoForProductImage();
                    var pictureStream = ViewBitmapAsStream(App.CroppedPhotoBitMap);
                    var ImageUrl = await AddProductDataService.Instance.UploadProductImage(pictureStream, TokenDetails.blobURL, TokenDetails.SasToken);
                    ThisNewProduct.productPicUrl = ImageUrl;
                    var result = await AddProductDataService.Instance.AddProductToDataBase(ThisNewProduct);
                    if (result != null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", result, "ok");
                        ResetForm();
                    }
                    else
                    {
                        await Application.Current.MainPage.DisplayAlert("Message", "Error in Adding Product", "ok");
                    }
                    IsBusy = false;
                }

            }
            catch(Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Message", "Something Went wrong!!", "ok");
                var x = e.Message;
                IsBusy = false;
            }

        }
        public async void DisplayCategories_clicked(object obj)
        {
            try
            {
                await PopulateCategoryListData();
                if (CategoryList != null)
                {
                    string action = await Application.Current.MainPage.DisplayActionSheet("Select Category:", "Cancel", null, CategoryList.Keys.ToArray());
                    if (!string.IsNullOrEmpty(action))
                        if (action.ToLower() != "cancel")
                        {
                            SelectedCategory = action;
                            SubCategory1List.Clear();
                            SelectedSubCategory1 = null;
                            SelectedSubCategory2 = null;
                            IsSubCat1Available = await GetSubCategory1FromSelectedCategory(CategoryList[action]);
                        }
                    return;
                }
                await Application.Current.MainPage.DisplayAlert("Error", "No Category Available", "ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong while fetching the categories", "ok");
                return;
            }
        }

        public async void DisplaySubCategories1_clicked(object obj)
        {
            try
            {
                if (SubCategory1List != null)
                {
                    string action = await Application.Current.MainPage.DisplayActionSheet("Select Sub-Category1:", "Cancel", null, SubCategory1List.Keys.ToArray());
                    if (!string.IsNullOrEmpty(action))
                    {
                        if (action.ToLower() != "cancel")
                        {
                            SelectedSubCategory1 = action;
                            SubCategory2List.Clear();
                            SelectedSubCategory2 = null;
                            IsSubCat2Available = await GetSubCategory2FromSelectedCategory(SubCategory1List[action]);
                        }
                    }
                    return;
                }
                await Application.Current.MainPage.DisplayAlert("Error", "No Category Available", "ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong while fetching the categories", "ok");
            }
        }

        public async void DisplaySubCategories2_clicked(object obj)
        {
            try
            {
                if (SubCategory2List != null)
                {
                    string action = await Application.Current.MainPage.DisplayActionSheet("Select Sub-Category2:", "Cancel", null, SubCategory2List.Keys.ToArray());
                    if (!string.IsNullOrEmpty(action))
                    {
                        if (action.ToLower() != "cancel")
                        {
                            SelectedSubCategory2 = action;
                            //SubCategory1List.Clear();
                            //GetSubCategoryFromSelectedCategory(CategoryList[action]);
                        }
                    }
                    return;
                }
                await Application.Current.MainPage.DisplayAlert("Error", "No Category Available", "ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Somthing went wrong while fetching the categories", "ok");
                
            }
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

        public async void MenuButtonCommand_clicked()
        {
            await Navigation.PushPopupAsync(new MenuPopup(this),true);
        }

        public void SetupFormForFetchedProduct(Product p)
        {
            //EnableId = false;
            //EnableName = false;
            EnableCat = false;
            if (p.Availability_Status.ToString().ToLower() == "yes")
            {
                ThisNewProduct.AvailabilityStatusBool = true;
            }
            if (p.Visibility_Status.ToString().ToLower() == "yes")
            {
                ThisNewProduct.VisibilityStatusBool = true;
            }

        }

        public void ResetForm()
        {
            ThisNewProduct = new Product();
            IsSubCat1Available = false;
            IsSubCat2Available = false;
            SelectedCategory = null;
            SelectedSubCategory1 = null;
            SelectedSubCategory2 = null;
            App.CroppedPhotoBitMap = null;
            ImageExist = false;
        }


        private async void BackButtonClicked(object obj)
        {
            // Do something
            //await Navigation.PopAsync();
            ResetForm();
            await Application.Current.MainPage.Navigation.PopAsync();
        }


        //------------------------------------------------------------------------------------------------------
        //Menu POPUP Commands

        private Command _OpenAddCategoryPopupCommand;
        private Command _OpenSearchPageCommand;
        public Command OpenAddCategoryPopupCommand
        {
            get { return _OpenAddCategoryPopupCommand ?? (this._OpenAddCategoryPopupCommand = new Command(this.OpenAddCategoryPopupCommand_Clicked)); }
        }
        public Command OpenSearchPageCommand
        {
            get { return _OpenSearchPageCommand ?? (this._OpenSearchPageCommand = new Command(this.OpenSearchPageCommand_Clicked)); }
        }

        public async void OpenAddCategoryPopupCommand_Clicked(object obj)
        {
            await PopupNavigation.Instance.PopAsync();
            await Application.Current.MainPage.Navigation.PushPopupAsync(new AddCategoryPage(), true);
        }
        public async void OpenSearchPageCommand_Clicked(object obj)
        {
            await PopupNavigation.Instance.PopAsync();
            await Application.Current.MainPage.Navigation.PushAsync(new SearchItem(), true);
        }
        public async void ResetFormForNewProduct_Clicked(object obj)
        {
            await PopupNavigation.Instance.PopAsync();
            ResetForm();
        }
        public async void ResetFormForUpdateProduct_Clicked(object obj)
        {
            await PopupNavigation.Instance.PopAsync();
            ThisNewProduct = new Product(OriginalProductDetails);
        }
    }
}
