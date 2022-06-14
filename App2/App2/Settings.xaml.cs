using System;
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
	public partial class Settings : ContentPage
	{
		public Settings ()
		{
			InitializeComponent ();
		}

        private void NewcodeButton_Clicked(object sender, EventArgs e)
        {
            MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
            sindesi.Open();
            MySqlCommand UpdateData;
            UpdateData = sindesi.CreateCommand();
            if (new_password.Text == null || new_passwordath.Text == null || new_passwordath.Text == "" || new_password.Text == "")
            {
                DisplayAlert("Προσοχή.", "Συμπληρώστε όλα τα πεδία.", "ΟΚ");
            }
            else
            {
                if (new_password.Text != new_passwordath.Text)
                {
                    DisplayAlert("Προσοχή.", "Οι κωδικοί δεν είναι ίδιοι.", "OK");
                    new_passwordath.Text = null;
                }

                else
                {
                    string current_userID = Helper.GeneralSettings;
                    int cur_UserID = Int32.Parse(current_userID);
                    Console.WriteLine(current_userID);

                    UpdateData.CommandText = "UPDATE users SET password ='" + new_password.Text + "' WHERE id = " + current_userID;
                    MySqlDataReader rdr = UpdateData.ExecuteReader();

                    while (rdr.Read())
                    {
                    }
                    rdr.Close();


                    sindesi.Close();
                    DisplayAlert("Πληροφορία.", "Ενημερώθηκε.", "ΟΚ");

                    new_username.Text = null;
                    new_usernameath.Text = null;
                    new_password.Text = null;
                    new_passwordath.Text = null;
                }
            }
        }

        private void NewAliasButton_Clicked(object sender, EventArgs e)
        {
            MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
            sindesi.Open();
            MySqlCommand UpdateData;
            UpdateData = sindesi.CreateCommand();

            MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @username ", sindesi);
            chk.Parameters.AddWithValue("@username", new_username.Text);
            int chkbase = Convert.ToInt32(chk.ExecuteScalar());

            if (new_usernameath.Text == null || new_username.Text == null || new_usernameath.Text == "" || new_username.Text == "")
            {
                DisplayAlert("Προσοχή.", "Συμπληρώστε όλα τα πεδία.", "ΟΚ");
            }
            else
            {
                if (chkbase > 0)
                {
                    DisplayAlert("Προσοχή.", "Ψευδώνυμο ήδη σε χρήση. Παρακαλώ διαλέξτε άλλο.", "OK");
                    new_username.Text = null;
                }
                else if (new_username.Text != new_usernameath.Text)
                {
                    DisplayAlert("Προσοχή.", "Τα Ψευδώνυμα δεν είναι ίδια.", "OK");
                    new_usernameath.Text = null;

                }
                else
                {
                    string current_userID = Helper.GeneralSettings;
                    int cur_UserID = Int32.Parse(current_userID);
                    Console.WriteLine(current_userID);

                    UpdateData.CommandText = "UPDATE users SET username ='" + new_username.Text + "' WHERE id = " + current_userID;
                    MySqlDataReader rdr = UpdateData.ExecuteReader();

                    while (rdr.Read())
                    {
                    }
                    rdr.Close();
                    sindesi.Close();
                    DisplayAlert("Πληροφορία.", "Ενημερώθηκε.", "ΟΚ");

                    new_username.Text = null;
                    new_usernameath.Text = null;
                    new_password.Text = null;
                    new_passwordath.Text = null;

                }

                

            }
        }

        
    }
}