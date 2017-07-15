using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UWPStyleIssue.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace UWPStyleIssue
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            this.FixUWPStyling();

            MainPage = new UWPStyleIssue.MainPage();
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
