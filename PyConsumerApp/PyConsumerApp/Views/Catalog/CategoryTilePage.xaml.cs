using PyConsumerApp.DataService;
using PyConsumerApp.Models;
using PyConsumerApp.ViewModels.Catalog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using PyConsumerApp.DataService;
using System;
using Plugin.Permissions;
using Plugin.Connectivity;
using PyConsumerApp.Controls;
using PyConsumerApp.ViewModels.Transaction;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;
using Plugin.Geolocator;

namespace PyConsumerApp.Views.Catalog
{
    /// <summary>
    /// The Category Tile page.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryTilePage
    {
        //private CategoryPageViewModel vm;
        private SubCategoryPageViewModel vm;
        private string Latitude;
        private string Longitude;
        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryTilePage" /> class.
        /// </summary>
        public CategoryTilePage()
        {
            InitializeComponent();
            //vm = new CategoryPageViewModel();
            vm = new SubCategoryPageViewModel();
            this.BindingContext = vm;
            vm.IsLoading = true;
        }

    }
}