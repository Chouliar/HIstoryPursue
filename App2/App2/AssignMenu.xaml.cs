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
    public partial class AssignMenu : ContentPage
    {
        public string curUser = Helper.GeneralSettings; //ΙD χρήστη
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
        public int role;

        private string ans1String;



        public string Ans1String
        {
            get { return ans1String; }
            set
            {
                ans1String = value;
                OnPropertyChanged(nameof(Ans1String));
            }
        }

        public AssignMenu()
        {
            InitializeComponent();

            sindesi.Open();
            MySqlCommand chk2 = new MySqlCommand("SELECT role_id FROM users WHERE id = @id", sindesi);
            chk2.Parameters.AddWithValue("@id", curUser.ToString());
            MySqlDataReader rdr2 = chk2.ExecuteReader();
            while (rdr2.Read())
            {
                role = (int)rdr2["role_id"];
                if (role == 2)
                {
                    BindingContext = this;
                    Ans1String = "Δημιουργία Τμήματος";
                    newAssign.IsVisible = true;
                    newAssign.IsEnabled = true;
                }
                else if (role == 1)
                {
                    BindingContext = this;
                    Ans1String = "Εγγραφή σε Τμήμα";
                    newAssign.IsVisible = false;
                    newAssign.IsEnabled = false;
                }
            }
            rdr2.Close();
            sindesi.Close();
        }

        private void Newjoin_Clicked(object sender, EventArgs e)
        {
            if (role != 2)
            {
                Navigation.PushAsync(new Join());
            }
            else
                Navigation.PushAsync(new Create());
        }

        private void Assignments_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new Assignments());
        }

        private void NewAssign_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new NewJob());
        }
    }
}