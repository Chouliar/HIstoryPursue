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
    public partial class CasualG : ContentPage
    {
        public List<int> questnum = new List<int>(); //Λιστα με τις επιλεγμένες ερωτήσεις 
        public string curUser = Helper.GeneralSettings; //ΙD χρήστη
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
        public List<int> found1 = new List<int>(); //Λιστα με τα id ενοτήτων
        public int score = 0;
        private Button chapters;
        private int chosen;
        private int chkclass;

        public CasualG()
        {
            InitializeComponent();
            {
                Grid grid = new Grid
                {
                    VerticalOptions = LayoutOptions.FillAndExpand,

                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)}
                    }
                };
                grid.Margin = new Thickness(20);
                string UserID = Helper.GeneralSettings;
                MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);

                grid.Children.Add(new Label
                {
                    Text = "Διαλέξτε Ενότητα",
                    TextColor = Color.Black,
                    TextDecorations = TextDecorations.Underline,
                    HorizontalOptions = LayoutOptions.Center,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 20,
                }, 0, 1, 0, 1);

                sindesi.Open();

                int j = 0;
                MySqlCommand chk = new MySqlCommand("SELECT class FROM users WHERE id = @id ", sindesi);
                chk.Parameters.AddWithValue("@id", UserID);
                chkclass = Convert.ToInt32(chk.ExecuteScalar());
                if (chkclass == 3)
                {
                    j = 10;
                }
                else if (chkclass == 4)
                {
                    j = 9;
                }
                else if (chkclass == 5)
                {
                    j = 7;
                }
                else if (chkclass == 6)
                {
                    j = 5;
                }
                sindesi.Close();
                for (int i = 1; i <= j; i++)
                {
                    grid.Children.Add(chapters = new Button
                    {
                        Text = "Ενότητα " + i,
                        TextColor = Color.White,
                        BackgroundColor = Color.Black,
                        StyleId = i.ToString(),


                    }, 0, 1, i, i + 1);
                    chapters.Clicked += OnMenuClicked;
                }
                this.Content = grid;
            }

        }
        void OnMenuClicked(object sender, EventArgs args)
        {
            Button button = (Button)sender;
            button.IsEnabled = false;
            chosen = Int32.Parse(button.StyleId);
            Console.WriteLine(chosen);
            GameStart(chkclass,"casual", chosen);
            button.IsEnabled = true;
        }

        public void GameStart(int classN, string type, int chosenChapter)
        {
            sindesi.Open();
            chosenChapter = chosenChapter + ((classN - 1) * 10);
            Console.WriteLine("ChosenChapter: " + chosenChapter + "\n");

            MySqlCommand getData = sindesi.CreateCommand();
            getData.CommandText = "SELECT id FROM quests WHERE chapterID = @chapter ";
            getData.Parameters.AddWithValue("@chapter", chosenChapter);

            MySqlDataReader rdr2 = getData.ExecuteReader();

            if (rdr2.HasRows)
            {
                while (rdr2.Read())
                {

                    found1.Add(rdr2.GetInt32(0));
                    //Console.WriteLine(found.Count+"++++");
                    //found.ForEach(Console.WriteLine);
                }
            }
            rdr2.Close();
            sindesi.Close();

            Random num = new Random();

            while (questnum.Count < found1.Count && questnum.Count < 10)
            {
                int selnum = num.Next(found1.Min(), found1.Max() + 1);
                // Δημιουργία λιστας 10 τυχαίων διαφορετικών int
                if (!questnum.Contains(selnum))
                {
                    //Επιλεγμένες ερωτήσεις 
                    questnum.Add(selnum);
                    //Navigation.PushAsync(new GamePlay(questnum, curUser, classN, score));
                }
            }
            //questnum.ForEach(Console.WriteLine);
            Navigation.PushAsync(new GamePlay(questnum, curUser, classN, score,chosenChapter,"casual"));
        }
    }
}
