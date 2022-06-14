using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySqlConnector;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace App2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Assignments : ContentPage
    {
        public int curUser; //ΙD χρήστη
        public List<int> found1 = new List<int>(); //Λιστα με τα id τμηματων
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
        public Button departments;
        public string chosen;
        public int chosenb;
        private int chkrole;

        public Assignments()
        {
            InitializeComponent(); InitializeComponent();
            string UserID = Helper.GeneralSettings;
            curUser = Int32.Parse(UserID);
            MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);

            {
                Grid grid = new Grid
                {
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    //HorizontalOptions = LayoutOptions.CenterAndExpand,

                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                    }
                };
                grid.Margin = new Thickness(20);

                sindesi.Open();

                //μαθητής ή καθηγητής
                MySqlCommand chk2 = new MySqlCommand("SELECT role_id FROM users WHERE id = @id ", sindesi);
                chk2.Parameters.AddWithValue("@id", UserID);
                chkrole = Convert.ToInt32(chk2.ExecuteScalar());
                sindesi.Close();

                //Console.WriteLine(chkrole);

                //δασκαλος
                if (chkrole == 2)
                {
                    sindesi.Open();
                    MySqlCommand getData = sindesi.CreateCommand();
                    getData.CommandText = "SELECT id FROM departments WHERE teacher = @teacher ";
                    getData.Parameters.AddWithValue("@teacher", curUser);

                    MySqlDataReader rdr2 = getData.ExecuteReader();

                    if (rdr2.HasRows)
                    {
                        while (rdr2.Read())
                        {
                            found1.Add((int)rdr2[0]);
                        }
                    }
                    found1.ForEach(Console.WriteLine); //λιστα με τα id των τμηματων που εχει δημιουργήσει ο καθηγητής
                    rdr2.Close();
                    sindesi.Close();
                }
                //μαθητης
                else if (chkrole == 1)
                {
                    //επιλογη τμηματων που αντιστοιχουν στo μαθητη
                    sindesi.Open();
                    MySqlCommand getData = sindesi.CreateCommand();
                    getData.CommandText = "SELECT dep FROM studentsdep WHERE studentid = @studentid ";
                    getData.Parameters.AddWithValue("@studentid", UserID);

                    MySqlDataReader rdr2 = getData.ExecuteReader();

                    if (rdr2.HasRows)
                    {
                        while (rdr2.Read())
                        {
                            found1.Add((int)rdr2[0]);
                        }
                    }
                    found1.ForEach(Console.WriteLine); //λιστα με τα id των τμηματων που εχει κανει join ο μαθητης
                    rdr2.Close();
                    sindesi.Close();
                }

                for (int i = 0; i < found1.Count; i++)
                {
                    sindesi.Open();

                    MySqlCommand getData = sindesi.CreateCommand();
                    getData.CommandText = "SELECT name FROM departments WHERE id = @id ";
                    getData.Parameters.AddWithValue("@id", found1[i]);
                    MySqlDataReader rdr2 = getData.ExecuteReader();
                    while (rdr2.Read())
                    {
                        //Console.WriteLine(found1[i]);
                        chosen = (string)rdr2["name"];
                        //Console.WriteLine(chosen);
                    }
                    rdr2.Close();


                    Console.WriteLine(i);
                    grid.Children.Add(departments = new Button
                    {
                        Text = chosen,
                        TextColor = Color.White,
                        BackgroundColor = Color.Black,
                        StyleId = i.ToString(),


                    }, 0, 1, i, i + 1);
                    departments.Clicked += OnMenuClicked;
                    sindesi.Close();
                }

                this.Content = grid;
            }
        }
        async void OnMenuClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;
            chosenb = Int32.Parse(button.StyleId);

            Console.WriteLine(chosenb);
            Console.WriteLine(found1[chosenb]);//id in departments

            if (chkrole == 1) //μαθητής
            {
                string action = await DisplayActionSheet("Μενού", "Άκυρο", null, "Προβολή Εργασίας", "Αποχώρηση από το Τμήμα");
                if (action == "Προβολή Εργασίας")
                {
                    sindesi.Open();
                    MySqlCommand chk = new MySqlCommand("SELECT message FROM assignments WHERE dep = @id", sindesi);
                    chk.Parameters.AddWithValue("@id", found1[chosenb]);
                    string chkmessage = Convert.ToString(chk.ExecuteScalar());
                    sindesi.Close();
                    Console.WriteLine(chkmessage);
                    await DisplayActionSheet("Εργασία", "Άκυρο", null, chkmessage);
                }
                else if (action == "Αποχώρηση από το Τμήμα")
                {
                    sindesi.Open();
                    MySqlCommand chk = new MySqlCommand("DELETE FROM studentsdep WHERE dep = @id AND studentid = @student", sindesi);
                    chk.Parameters.AddWithValue("@id", found1[chosenb]);
                    chk.Parameters.AddWithValue("@student", curUser);
                    chk.ExecuteNonQuery();
                    sindesi.Close();
                }
            }
            if (chkrole == 2) //καθηγητης
            {
                string action3 = await DisplayActionSheet("Μενού", "Άκυρο", null, "Προβολή Εργασίας", "Διαγραφή Ανακοίνωσης");
                sindesi.Open();
                MySqlCommand chk = new MySqlCommand("SELECT message FROM assignments WHERE dep = @id", sindesi);
                chk.Parameters.AddWithValue("@id", found1[chosenb]);
                string chkmessage = Convert.ToString(chk.ExecuteScalar());
                sindesi.Close();
                Console.WriteLine(chkmessage);
                if (action3 == "Προβολή Εργασίας")
                {
                    await DisplayActionSheet("Εργασία", "Άκυρο", null, chkmessage);
                }
                else if (action3 == "Διαγραφή Ανακοίνωσης")
                {
                    sindesi.Open();
                    MySqlCommand chk4 = new MySqlCommand("DELETE FROM assignments WHERE dep = @id AND message = @message", sindesi);
                    chk4.Parameters.AddWithValue("@id", found1[chosenb]);
                    chk4.Parameters.AddWithValue("@message", chkmessage);
                    chk4.ExecuteNonQuery();
                    sindesi.Close();
                }
            }
            button.IsEnabled = true;

        }
    }
}