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
	public partial class NewJob : ContentPage
	{

        public string curUser = Helper.GeneralSettings; //ΙD χρήστη
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
       
        public string assignID;

        public NewJob ()
		{
			InitializeComponent ();
		}


        private void NewTaskButton_Clicked(object sender, EventArgs e)
        {
            MySqlCommand UpdateData;
            UpdateData = sindesi.CreateCommand();

            sindesi.Open();
           

            if (new_task.Text == null || dep_name.Text == null || new_task.Text == "" || dep_name.Text == "")
            {
                DisplayAlert("Προσοχή.", "Συμπληρώστε όλα τα πεδία.", "ΟΚ");
            }
            else
            {
                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM assignments WHERE message = @message AND dep = @department", sindesi);
                chk.Parameters.AddWithValue("@message", new_task.Text);
                chk.Parameters.AddWithValue("@department", dep_name.Text);

                int chkbase = Convert.ToInt32(chk.ExecuteScalar());
                
                if (chkbase > 0)
                {
                    DisplayAlert("Προσοχή.", "Υπάρχει ήδη αυτό το μήνυμα για το Τμήμα", "OK");
                    new_task.Text = null;
                }
                sindesi.Close();
                sindesi.Open();

                MySqlCommand chk2 = new MySqlCommand("SELECT COUNT(*) FROM departments WHERE name = @name", sindesi);
                chk2.Parameters.AddWithValue("@name", dep_name.Text);
                int chkbase2 = Convert.ToInt32(chk2.ExecuteScalar());
                sindesi.Close();
                if (chkbase2 < 1)
                {
                    DisplayAlert("Προσοχή.", "Δεν υπάρχει Τμήμα με αυτό το όνομα", "OK");
                    dep_name.Text = null;
                }
                else
                {
                    sindesi.Open();
                    MySqlCommand getData = sindesi.CreateCommand();
                    getData.CommandText = "SELECT id FROM departments WHERE name = @depname AND teacher = @id";
                    getData.Parameters.AddWithValue("@depname", dep_name.Text);
                    getData.Parameters.AddWithValue("@id", curUser);
                    MySqlDataReader rdr2 = getData.ExecuteReader();
                    while (rdr2.Read())
                    {
                        int temp = (int)rdr2["id"];
                        assignID = temp.ToString();
                    }
                    sindesi.Close();

                    sindesi.Open();

                    MySqlCommand chk3 = new MySqlCommand("SELECT COUNT(*) FROM assignments WHERE dep = @depa", sindesi);
                    chk3.Parameters.AddWithValue("@depa", assignID);
                    int chkbase3 = Convert.ToInt32(chk3.ExecuteScalar());
                    sindesi.Close();
                    

                    if (chkbase3 >= 1)
                    {
                        //gia monadiki anakoinosi ana tmima vgale apo ta sxolia
                        sindesi.Open();

                        UpdateData.CommandText = "UPDATE assignments SET message ='" + new_task.Text + "' WHERE  dep = " + assignID;
                        MySqlDataReader rdr = UpdateData.ExecuteReader();

                        while (rdr.Read())
                        {
                        }
                        rdr.Close();
                        sindesi.Close();
                        DisplayAlert("Πληροφορία.", "Ενημερώθηκε.", "ΟΚ");
                        new_task.Text = null;
                        dep_name.Text = null;
                    }
                    else
                    {
                        sindesi.Open();
                        MySqlCommand cmd = new MySqlCommand("insert into assignments(id,message,dep) values('NULL','" + new_task.Text + "','" + assignID + "')", sindesi);
                        var rd = cmd.ExecuteReaderAsync();
                        sindesi.Close();
                        DisplayAlert("Πληροφορία.", "Αποθηκεύτηκε.", "ΟΚ");

                        new_task.Text = null;
                        dep_name.Text = null;
                    }

                }
            }
        }
    }
}