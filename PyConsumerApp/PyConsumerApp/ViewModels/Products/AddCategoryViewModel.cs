using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using PyConsumerApp.Models;
using PyConsumerApp.DataService;

namespace PyConsumerApp.ViewModels.Products
{
    [Preserve(AllMembers = true)]
    [DataContract]
    class AddCategoryViewModel : BaseViewModel
    {
        public AddCategoryViewModel()
        {
            //initialize all the list
            _OptionList = new Dictionary<string, string>();
            _CategoryList = new Dictionary<string, int>();
            _SubCategory1List = new Dictionary<string, int>();
            PopulateOptionList();
            PopCatOptionsAsync = new Command(PopCatOptionsAsync_clickedAsync);
            DisplayCategoriesAsync = new Command(DisplayCategories_clicked);
            DisplaySubCategory1Async = new Command(DisplaySubCategories1_clicked);
            AddNewCategoryCommand = new Command(AddNewCategoryCommand_clicked);
            AddNewCategoryImageCommand = new Command(AddNewCategoryImageCommand_clicked);
        }
        #region PublicCommand
        public Command PopCatOptionsAsync { get; set; }
        public Command DisplayCategoriesAsync { get; set; }
        public Command DisplaySubCategory1Async { get; set; }
        public Command DisplaySubCategory2Async { get; set; }
        public Command AddNewCategoryCommand { get; set; }
        public Command AddNewCategoryImageCommand { get; set; }
        
        #endregion

        #region PublicProperties


        private Command backButtonCommand;
        public Command BackButtonCommand//{ get; set; }
        {
            get { return backButtonCommand ?? (this.backButtonCommand = new Command(this.BackButtonClicked)); }
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
        private Dictionary<string, string> _OptionList;
        public Dictionary<string, string> OptionList
        {
            get { return _OptionList; }
            set
            {
                if (_OptionList != value)
                {
                    _OptionList = value;
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

        private string _SelectedOption;
        public string SelectedOption
        {
            get { return _SelectedOption; }
            set
            {
                if (_SelectedOption != value)
                {
                    _SelectedOption = value;
                    OnPropertyChanged();
                }
            }
        }




        public bool _DisplayCategoryOption;
        public bool DisplayCategoryOption
        {
            get => _DisplayCategoryOption;

            set
            {
                if (_DisplayCategoryOption == value)
                    return;
                _DisplayCategoryOption = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }

        public bool _DisplaySubCat1Option;
        public bool DisplaySubCat1Option
        {
            get => _DisplaySubCat1Option;

            set
            {
                if (_DisplaySubCat1Option == value)
                    return;
                _DisplaySubCat1Option = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }

        public bool _DisplaySubCat2Option;
        public bool DisplaySubCat2Option
        {
            get => _DisplaySubCat2Option;

            set
            {
                if (_DisplaySubCat2Option == value)
                    return;
                _DisplaySubCat2Option = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }

        public bool _EnableCategoryTextBox;
        public bool EnableCategoryTextBox
        {
            get => _EnableCategoryTextBox;

            set
            {
                if (_EnableCategoryTextBox == value)
                    return;
                _EnableCategoryTextBox = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }

        public bool _EnableSubCat1TextBox;
        public bool EnableSubCat1TextBox
        {
            get => _EnableSubCat1TextBox;

            set
            {
                if (_EnableSubCat1TextBox == value)
                    return;
                _EnableSubCat1TextBox = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }

        public bool _EnableSubCat2TextBox;
        public bool EnableSubCat2TextBox
        {
            get => _EnableSubCat2TextBox;

            set
            {
                if (_EnableSubCat2TextBox == value)
                    return;
                _EnableSubCat2TextBox = value;
                OnPropertyChanged();
                this.NotifyPropertyChanged();
            }
        }

        #endregion
        public async void PopCatOptionsAsync_clickedAsync(object obj)
        {
            try
            {
                if (OptionList != null)
                {
                    string action = await Application.Current.MainPage.DisplayActionSheet("Select Category:", "Cancel", null, OptionList.Keys.ToArray());
                    if (!string.IsNullOrEmpty(action))
                        if (action.ToLower() != "cancel")
                        {
                            SelectedOption = action;
                            if(OptionList[action] == "cat")
                            {
                                DisplaySubCat1Option = false;
                                DisplaySubCat2Option = false;
                                EnableCategoryTextBox = true;
                                DisplayCategoryOption = true;
                            }
                            else if (OptionList[action] == "SubCat1")
                            {
                                DisplaySubCat2Option = false;
                                EnableCategoryTextBox = false;
                                DisplayCategoryOption = true;
                                DisplaySubCat1Option = true;
                                EnableSubCat1TextBox = true;
                            }
                            else if (OptionList[action] == "SubCat2")
                            {
                                EnableSubCat1TextBox = false;
                                EnableCategoryTextBox = false;
                                DisplaySubCat2Option = true;
                                DisplayCategoryOption = true;
                                DisplaySubCat1Option = true;
                                EnableSubCat2TextBox = true;
                            }
                        }
                    return;
                }
                await Application.Current.MainPage.DisplayAlert("Error", "No Category Available", "ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong while fetching the categories", "ok");
                throw ex;
            }
        }
        async void AddNewCategoryImageCommand_clicked()
        {
            await Application.Current.MainPage.DisplayAlert("Message:", "This Feature is coming soon!", "OK");
        }
        async void AddNewCategoryCommand_clicked()
        {
            try
            {
                SubCategory NewCategoryToAdd = new SubCategory();
                if (OptionList[SelectedOption] == "cat")
                {
                    if(SelectedCategory == null || SelectedCategory.Replace(" ","").Length <5)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error:", "Category Name is too short or empty!(Minimum Required Character: 6)", "OK");
                        return;
                    }
                    NewCategoryToAdd.Name = SelectedCategory;
                }
                else if (OptionList[SelectedOption] == "SubCat1")
                {
                    if (SelectedSubCategory1 == null || SelectedSubCategory1.Replace(" ", "").Length < 5)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error:", "Category Name is too short or empty!(Minimum Required Character: 6)", "OK");
                        return;
                    }
                    NewCategoryToAdd.Name = SelectedSubCategory1;
                    NewCategoryToAdd.CategoryId = CategoryList[SelectedCategory];
                }
                else if (OptionList[SelectedOption] == "SubCat2")
                {
                    if (SelectedSubCategory2 == null || SelectedSubCategory2.Replace(" ", "").Length < 5)
                    {
                        await Application.Current.MainPage.DisplayAlert("Error:", "Category Name is too short or empty!(Minimum Required Character: 6)", "OK");
                        return;
                    }
                    NewCategoryToAdd.Name = SelectedSubCategory2;
                    NewCategoryToAdd.CategoryId = SubCategory1List[SelectedSubCategory1];
                }
                var result = await AddProductDataService.Instance.UploadNewCategory(NewCategoryToAdd);
                if (result != null)
                {
                    await Application.Current.MainPage.DisplayAlert("Message:", result, "OK");
                    ResetPage();
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Message:", "Something went Wrong", "OK");
                }
            }
            catch (Exception e)
            {
                await Application.Current.MainPage.DisplayAlert("Message:", "Required Fields Are Empty", "OK");
                ResetPage();
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
        public async void DisplayCategories_clicked(object obj)
        {
            if (OptionList[SelectedOption] == "cat")
                return;
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
                            await GetSubCategory1FromSelectedCategory(CategoryList[action]);
                        }
                    return;
                }
                await Application.Current.MainPage.DisplayAlert("Error", "No Category Available", "ok");
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert("Error", "Something went wrong while fetching the categories", "ok");
                throw ex;
            }
        }

        public async void DisplaySubCategories1_clicked(object obj)
        {
            if (OptionList[SelectedOption] == "SubCat1")
                return;
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
        private void PopulateOptionList()
        {
            OptionList.Add("Category","cat");
            OptionList.Add("Sub Categoty Level 1", "SubCat1");
            OptionList.Add("Sub Categoty Level 2", "SubCat2");
        }
        public void ResetPage()
        {
            SelectedCategory = null;
            SelectedSubCategory1 = null;
            SelectedSubCategory2 = null;
        }
        private async void BackButtonClicked(object obj)
        {
            // Do something
            //await Navigation.PopAsync();
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
