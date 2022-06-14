using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace App2
{
    public partial class MainPage : ContentPage
    {   
        public MainPage()
        {
            InitializeComponent();
        }
   
       
        private void NewStudent_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddStudent()); 
        }

        private void NewTeacher_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddTeacher());
        }

        private void LogEntry_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new LogIn());
        }
    }

}
