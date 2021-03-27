using PyConsumerApp.ViewModels.Products;
using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PyConsumerApp.Views.Products
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    [DataContract]
    public partial class MenuPopup : PopupPage
    {
        public MenuPopup(AddProductViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}