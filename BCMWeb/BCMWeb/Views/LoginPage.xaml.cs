using BCMWeb.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BCMWeb.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel model = new LoginViewModel();

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);

            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    imgLogo.Source = ImageSource.FromFile("Images/Splash 100x100.png");
                    break;
                case Device.Android:
                    imgLogo.Source = ImageSource.FromFile("Splash_256x256.png");
                    break;
                case Device.UWP:
                    break;
            }

            BindingContext = model;
            model.ErrorCodigo = " ";
            model.ErrorLogin = " ";
            model.ErrorMessage = " ";
            model.ErrorPassw = " ";
            model.UserLogged += Model_userLogged;
            entUsuario.Text = string.Empty;
            Password.Text = string.Empty;
            entUsuario.Completed += (s, e) => Password.Focus();
            Password.Completed += (s, e) => btnLogin.Focus();
            btnClear.Clicked += (s, e) =>
            {
                SharedModel.UserToken = string.Empty;
                lblErrorCodigo.Text = string.Empty;
                lblErrorPassw.Text = string.Empty;
                lblErrorGeneral.Text = string.Empty;
                IsBusy = false;
                entUsuario.Text = string.Empty;
                Password.Text = string.Empty;
            };

            if (App.Current.Properties["user"] != null)
            {
                NavigateToEmpresa().RunSynchronously();
            }
        }

        private async Task NavigateToEmpresa()
        {
            await this.Navigation.PushAsync(new EmpresaPage());
            var Pages = Navigation.NavigationStack.ToList();
            foreach (var page in Pages)
            {
                if (page.GetType() != typeof(EmpresaPage))
                {
                    Navigation.RemovePage(page);
                }
            }
        }

        private async void Model_userLogged(object sender, EventArgs.LoginModelEventArgs e)
        {
            IDictionary<string, object> _propiedades = Application.Current.Properties;
            _propiedades["token"] = e.Token;
            _propiedades["user"] = e.Usuario;

            //model.IsBusy = false;
            await NavigateToEmpresa();
        }
    }
}