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
	public partial class Join : ContentPage
	{

        public int curUser; //ΙD χρήστη
        public List<string> found1 = new List<string>(); //Λιστα με τα id τμηματων
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
        private Button departments;
        private int chosen;
        private int chkclass;

        public Join()
        {
            InitializeComponent();
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

                //ταξη μαθητη
                MySqlCommand chk2 = new MySqlCommand("SELECT class FROM users WHERE id = @id ", sindesi);
                chk2.Parameters.AddWithValue("@id", UserID);
                chkclass = Convert.ToInt32(chk2.ExecuteScalar());
                sindesi.Close();

                //επιλογη τμηματων που αντιστοιχουν στη ταξη του μαθητη
                sindesi.Open();
                MySqlCommand getData = sindesi.CreateCommand();
                getData.CommandText = "SELECT name FROM departments WHERE class = @class ";
                getData.Parameters.AddWithValue("@class", chkclass);

                MySqlDataReader rdr2 = getData.ExecuteReader();

                if (rdr2.HasRows)
                {
                    while (rdr2.Read())
                    {
                        found1.Add(rdr2.GetString(0));
                    }
                }
                found1.ForEach(Console.WriteLine);
                rdr2.Close();
                sindesi.Close();

                for (int i = 0; i < found1.Count; i++)
                {
                    Console.WriteLine(i);
                    grid.Children.Add(departments = new Button
                    {
                        Text = found1[i],
                        TextColor = Color.White,
                        BackgroundColor = Color.Black,
                        StyleId = i.ToString(),


                    }, 0, 1, i, i + 1);
                    departments.Clicked += OnMenuClicked;
                }
                this.Content = grid;
            }
        }

        async void OnMenuClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;
            chosen = Int32.Parse(button.StyleId);

            //Console.WriteLine(chosen);
            //Console.WriteLine(found1[chosen]);

            sindesi.Open();
            MySqlCommand chk2 = new MySqlCommand("SELECT id FROM departments WHERE name = @name ", sindesi);
            chk2.Parameters.AddWithValue("@name", found1[chosen]);
            chosen = Convert.ToInt32(chk2.ExecuteScalar());
            sindesi.Close();

            //Console.WriteLine(chosen);
            

            string action = await DisplayActionSheet("Επιλογή Τμήματος", "Άκυρο", null, "ΕΓΓΡΑΦΗ");
            if (action == "ΕΓΓΡΑΦΗ")
            {
                sindesi.Open();

                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM studentsdep WHERE studentid = @studentid AND dep = @department", sindesi);
                chk.Parameters.AddWithValue("@studentid", curUser);
                chk.Parameters.AddWithValue("@department", chosen);
                int chkbase = Convert.ToInt32(chk.ExecuteScalar());
                sindesi.Close();

                if (chkbase > 0)
                {
                    await DisplayAlert("Προσοχή.", "Είσαστε ήδη μέλος του Τμήματος.", "OK");
                }
                
                else
                {
                    sindesi.Open();
                    MySqlCommand cmd = new MySqlCommand("insert into studentsdep(id,studentid,dep) values('NULL','" + curUser + "','" + chosen + "')", sindesi);
                    var rd = cmd.ExecuteReaderAsync();
                    sindesi.Close();
                    await DisplayAlert("Πληροφορία.", "Εγγραφή Ολοκληρώθηκε.", "ΟΚ");
                }
            }
            button.IsEnabled = true;
        }
    }

}