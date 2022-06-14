using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using MySqlConnector;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Create : ContentPage
	{

        public string curUser = Helper.GeneralSettings; //ΙD χρήστη
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);


        public Create ()
		{
			InitializeComponent ();
		}

        private void NewdepButton_Clicked(object sender, EventArgs e)
        {
            sindesi.Open();

            if (forclass.Text == null || new_dep.Text == null || forclass.Text == "" || new_dep.Text == "")
            {
                DisplayAlert("Προσοχή.", "Συμπληρώστε όλα τα πεδία.", "ΟΚ");
            }
            else
            {
                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM departments WHERE name = @name ", sindesi);
                chk.Parameters.AddWithValue("@name", new_dep.Text);

                int chkbase = Convert.ToInt32(chk.ExecuteScalar());
                int classNum = Int32.Parse(forclass.Text);
                int cmprsl1 = classNum.CompareTo(6);
                int cmprsl2 = classNum.CompareTo(1);

                if (chkbase > 0)
                {
                    DisplayAlert("Προσοχή.", "Το όνομα χρησιμοποιείται. Παρακαλώ διαλέξτε άλλο.", "OK");
                    new_dep.Text = null;
                }
                else if ((cmprsl1 > 0) || (cmprsl2 < 0))
                {
                    DisplayAlert("Προσοχή.", "Αποδεχτός αριθμός τάξης (1-6).", "OK");
                    forclass.Text = null;
                }
                else
                {
                    MySqlCommand cmd = new MySqlCommand("insert into departments(id,name,teacher,class) values('NULL','" + new_dep.Text + "','" + curUser + "','" + forclass.Text + "' )", sindesi);
                    var rd = cmd.ExecuteReaderAsync();
                    sindesi.Close();
                    DisplayAlert("Πληροφορία.", "Αποθηκεύτηκε.", "ΟΚ");

                    forclass.Text = null;
                    new_dep.Text = null;

                }

                sindesi.Close();

            }
        }
    }
}