using Plugin.Connectivity;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using PyConsumerApp.Controls;
using PyConsumerApp.ViewModels.Forms;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace PyConsumerApp.Views.Forms
{
    /// <summary>
    /// Page to login with user name and password
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        public LoginPage()
        {
            InitializeComponent();
            LoginPageViewModel vm = new LoginPageViewModel(Navigation);
            vm.Initailize(this);
            BindingContext = vm;
        }
        public LoginPage(bool SomeError)
        {
            if(SomeError)
            {
                try
                {
                    DependencyService.Get<IToastMessage>().LongTime("Your session has expired. Please relogin");
                }
                catch { }
            }
            InitializeComponent();
            LoginPageViewModel vm = new LoginPageViewModel(Navigation);
            vm.Initailize(this);
            BindingContext = vm;
        }
    }
}