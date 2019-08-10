using BCMWeb.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BCMWeb
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            Application.Current.Properties["user"] = null;
            Application.Current.Properties["token"] = null;

            NavigationPage.SetHasNavigationBar(this, false);
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
