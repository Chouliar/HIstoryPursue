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
	public partial class LogIn : ContentPage
	{
		public LogIn ()
		{
			InitializeComponent ();
		}
        private void Login_Clicked(object sender, EventArgs e)
        {
            MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
            sindesi.Open();

            MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @username ", sindesi);
            chk.Parameters.AddWithValue("@username", l_username.Text);

            int chkbase = Convert.ToInt32(chk.ExecuteScalar());

            if (chkbase == 0)
            {
                DisplayAlert("Προσοχή.", "Δε βρέθηκε χρήστης.", "ΟΚ");
                l_username.Text = null;
                l_password.Text = null;
            }


            MySqlCommand selectData;
            selectData = sindesi.CreateCommand();

            selectData.CommandText = "SELECT password FROM users WHERE username=@username";
            selectData.Parameters.AddWithValue("@username", l_username.Text);
            MySqlDataReader rdr = selectData.ExecuteReader();

            while (rdr.Read())
            {

                string pass = (string)rdr["password"];
                Console.WriteLine(pass);
                if (!(pass == l_password.Text))
                {
                    DisplayAlert("Προσοχή.", "Λάθος κωδικός.", "ΟΚ");
                    l_password.Text = null;
                }
                else
                {
                    Helper.GeneralSettings = l_username.Text;
                    Navigation.PushAsync(new Menu());
                }

            }
          
            rdr.Close();
            sindesi.Close();

        }
    }
}