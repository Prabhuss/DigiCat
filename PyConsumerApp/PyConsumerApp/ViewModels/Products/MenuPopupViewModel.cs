using PyConsumerApp.Views.Products;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace PyConsumerApp.ViewModels.Products
{
    [Preserve(AllMembers = true)]
    [DataContract]
    class MenuPopupViewModel : BaseViewModel
    {
        private Command _OpenAddCategoryPopupCommand;
        public Command OpenAddCategoryPopupCommand
        {
            get { return _OpenAddCategoryPopupCommand ?? (this._OpenAddCategoryPopupCommand = new Command(this.OpenAddCategoryPopupCommand_Clicked)); }
        }

        public async void OpenAddCategoryPopupCommand_Clicked(object obj)
        {
            await PopupNavigation.Instance.PopAsync();
            await Application.Current.MainPage.Navigation.PushPopupAsync(new AddCategoryPage(), true);
        }
    }
}
