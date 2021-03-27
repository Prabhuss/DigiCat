
using Plugin.Connectivity;
using PyConsumerApp.Models;
using PyConsumerApp.ViewModels.Transaction;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PyConsumerApp.Views.Transaction
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddressEditPage : ContentPage
    {
        public AddressEditPage(Address ThisAddress) 
        {
            
            AddressEditViewodel viewModel = new AddressEditViewodel(ThisAddress);
            BindingContext = viewModel;
            InitializeComponent();
        }
    }
}