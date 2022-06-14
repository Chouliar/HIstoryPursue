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



    public partial class AddStudent : ContentPage
    {
        
        public AddStudent()
        {
            InitializeComponent();
        }

        private void SaveButton_Clicked(object sender, EventArgs e)
        {
            MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
            sindesi.Open();

            if (st_name.Text == null || st_class.Text == null || st_username.Text == null || st_surname.Text == null || st_password.Text == null || st_passwordath.Text == null 
                || st_name.Text == "" || st_class.Text == "" || st_username.Text == "" || st_surname.Text == "" || st_password.Text == "" || st_passwordath.Text == "")
            {
                DisplayAlert("Προσοχή.", "Συμπληρώστε όλα τα πεδία.", "ΟΚ");
            }
            else
            {
                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM users WHERE username = @username ", sindesi);
                chk.Parameters.AddWithValue("@username", st_username.Text);

                int chkbase = Convert.ToInt32(chk.ExecuteScalar());
                int classNum = Int32.Parse(st_class.Text);
                int cmprsl1 = classNum.CompareTo(6);
                int cmprsl2 = classNum.CompareTo(1);

                //Console.WriteLine(+classNum);
                //Console.WriteLine(+cmprsl1 + " " + cmprsl2);

                if (chkbase > 0)
                {
                    DisplayAlert("Προσοχή.", "Ψευδώνυμο ήδη σε χρήση. Παρακαλώ διαλέξτε άλλο.", "OK");
                    st_username.Text = null;
                }
                else if (st_password.Text != st_passwordath.Text)
                {
                    DisplayAlert("Προσοχή.", "Οι κωδικοί δεν είναι ίδιοι.", "OK");
                    st_passwordath.Text = null;

                }
                else if ((cmprsl1 > 0) || (cmprsl2 < 0))
                {
                    DisplayAlert("Προσοχή.", "Αποδεχτός αριθμός τάξης (1-6).", "OK");
                    st_class.Text = null;
                }
                else
                {
                    MySqlCommand cmd = new MySqlCommand("insert into users(id,name,last_name,username,password,role_id,class) values('NULL','" + st_name.Text + "','" + st_surname.Text + "','" + st_username.Text + "','" + st_password.Text + "','1','" + st_class.Text + "' )", sindesi);
                    var rd = cmd.ExecuteReaderAsync();
                    sindesi.Close();
                    DisplayAlert("Πληροφορία.", "Αποθηκεύτηκε.", "ΟΚ");

                    st_name.Text = null;
                    st_surname.Text = null;
                    st_password.Text = null;
                    st_username.Text = null;
                    st_passwordath.Text = null;
                    st_class.Text = null;
                }
            }
        }
            
    }
}
