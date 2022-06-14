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
    public partial class AddTeacher : ContentPage
    {
        public AddTeacher()
        {
            InitializeComponent();
        }


        private void SaveButton_Clicked(object sender, EventArgs e)
        {

            MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
            sindesi.Open();

            if (t_name.Text == null || t_surname.Text == null || t_username.Text == null || t_password.Text == null || t_passwordath.Text == null 
                || t_name.Text == "" || t_surname.Text == "" || t_username.Text =="" || t_password.Text == "" || t_passwordath.Text == "")
            {
                DisplayAlert("Προσοχή.", "Συμπληρώστε όλα τα πεδία.", "ΟΚ");
            }
            else
            {
                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @username ", sindesi);
                chk.Parameters.AddWithValue("@username", t_username.Text);

                int chkbase = Convert.ToInt32(chk.ExecuteScalar());

                if (chkbase > 0)
                {
                    DisplayAlert("Προσοχή.", "Ψευδώνυμο ήδη σε χρήση. Παρακαλώ διαλέξτε άλλο.", "OK");
                    t_username.Text = null;
                }
                else if (t_password.Text != t_passwordath.Text)
                {
                    DisplayAlert("Προσοχή.", "Οι κωδικοί δεν είναι ίδιοι.", "OK");
                    t_passwordath.Text = null;

                }

                else
                {
                    MySqlCommand cmd = new MySqlCommand("insert into users(id,name,last_name,username,password,role_id,class) values('NULL','" + t_name.Text + "','" + t_surname.Text + "','" + t_username.Text + "','" + t_password.Text + "','2','0')", sindesi);
                    var rd = cmd.ExecuteReaderAsync();
                    sindesi.Close();
                    DisplayAlert("Πληροφορία.", "Αποθηκεύτηκε.", "ΟΚ");

                    t_name.Text = null;
                    t_surname.Text = null;
                    t_password.Text = null;
                    t_username.Text = null;
                    t_passwordath.Text = null;

                }
            }
        }
    }   
}