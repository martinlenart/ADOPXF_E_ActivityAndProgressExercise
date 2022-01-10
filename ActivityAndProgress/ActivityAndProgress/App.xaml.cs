using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ActivityAndProgress
{
    public partial class App : Application
    {
        public static string Message;
        public static DateTime Time;


        DateTime starttime = DateTime.Now;
        string greeting = "Application Started";

        public App()
        {
            InitializeComponent();

            //Demonstrate State management
            //using parameter passing when creating a page
            MainPage = new Views.MainPage(greeting, starttime);
        }

        protected override void OnStart()
        {
            //Demonstrate State management
            //using Global variables
            Globals.Data.Message = greeting;
            Globals.Data.Time = starttime;

            //using static properties in Application
            Message = greeting;
            Time = starttime;

            //using Application Properties Dictionary
            Properties["Message"] = greeting;
            Properties["Time"] = starttime;
        }

        protected override void OnSleep()
        {
            //Application Properties Dictionary should be used for Disk persistance in Sleep
            Properties["Message"] = "Application in Sleep";
            Properties["Time"] = DateTime.Now;
        }

        protected override void OnResume()
        {
        }
    }
}
