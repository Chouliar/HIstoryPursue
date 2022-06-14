using System;
using System.Data;
using MySql.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Buffers;
using MySqlConnector;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        public Menu()
        {
            InitializeComponent();
            UserIn.Text = Helper.GeneralSettings;

            Console.WriteLine(UserIn.Text);
            MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
            sindesi.Open();

            MySqlCommand getData = sindesi.CreateCommand();
            getData.CommandText = "SELECT id FROM users WHERE username=@username";
            getData.Parameters.AddWithValue("@username", UserIn.Text);
            MySqlDataReader rdr2 = getData.ExecuteReader();
            while (rdr2.Read())
            {
                int uid = (int)rdr2["id"];
                Console.WriteLine(uid);
                string struid = uid.ToString();
                Helper.GeneralSettings = struid;
            }

            rdr2.Close();
            sindesi.Close();
            UserIn.Text = Helper.GeneralSettings;

        }

        async void NewGame_Clicked(object sender, EventArgs e)
        {
            string action = await DisplayActionSheet("Νέο Παιχνίδι", "Άκυρο", null, "ΚΑΝΟΝΙΚΟ", "ΤΥΧΑΙΟ");
            if (action == "ΚΑΝΟΝΙΚΟ")
            {
                await Navigation.PushAsync(new CasualG());
            }
            else if (action == "ΤΥΧΑΙΟ")
            {
                await Navigation.PushAsync(new RandomG());
            }
            Console.WriteLine("Action: " + action);
        }

        private void Settings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Settings());
        }

        private void Rankings_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Rankings());
        }

        private void Homework_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AssignMenu());
        }
    }
}