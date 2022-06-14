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
	public partial class RandomG : ContentPage
	{
        public List<int> questnum = new List<int>(); //Λιστα με τις επιλεγμένες ερωτήσεις 
        public string curUser = Helper.GeneralSettings; //ΙD χρήστη
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
        public List<int> found1 = new List<int>(); //Λιστα με τα id ενοτήτων
        public int score = 0;

        public RandomG ()
		{
			InitializeComponent ();
        }

        private void Third_Clicked(object sender, EventArgs e)
        {
            string gtype = "random";
            GameStart(3,gtype,0);
        }

        private void Fourth_Clicked(object sender, EventArgs e)
        {
            string gtype = "random";
            GameStart(4, gtype, 0);
        }

        private void Fifth_Clicked(object sender, EventArgs e)
        {
            string gtype = "random";
            GameStart(5, gtype, 0);
        }

        private void Sixth_Clicked(object sender, EventArgs e)
        {
            string gtype = "random";
            GameStart(6, gtype, 0);
        }

        private void All_Clicked(object sender, EventArgs e)
        {
            string gtype = "random";
            GameStart(7, gtype, 0);
        }


        public void GameStart(int classN, string type, int chosenChapter)
        {
            sindesi.Open();

            if (classN != 7)
            {
                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM quests WHERE class = @class ", sindesi);
                chk.Parameters.AddWithValue("@class", classN);

                int chkbase = Convert.ToInt32(chk.ExecuteScalar());
                Console.WriteLine(chkbase);

                Random num = new Random();

                while (questnum.Count < 10)
                {
                    int selnum = num.Next(1, chkbase + 1);
                    // Δημιουργία λιστας 10 τυχαίων διαφορετικών int
                    if (!questnum.Contains(selnum))
                    {
                        //Επιλεγμένες ερωτήσεις 
                        questnum.Add(selnum);
                    }
                }
            }
            else
            {
                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM quests", sindesi);
                int chkbase = Convert.ToInt32(chk.ExecuteScalar());
                Console.WriteLine(chkbase);

                Random num = new Random();

                while (questnum.Count < 10)
                {
                    int selnum = num.Next(1, chkbase + 1);
                    // Δημιουργία λιστας 10 τυχαίων διαφορετικών int
                    if (!questnum.Contains(selnum))
                    {
                        //Επιλεγμένες ερωτήσεις 
                        questnum.Add(selnum);
                    }
                }
            }
            sindesi.Close();
            //questnum.ForEach(Console.WriteLine);
            Navigation.PushAsync(new GamePlay(questnum, curUser, classN, score,0,"random"));
        }
    }    
}