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
	public class GameStart : ContentPage
    { 
        //public new event PropertyChangedEventHandler PropertyChanged;

        public List<int> questnum = new List<int>(); //Λιστα με τις επιλεγμένες ερωτήσεις 
        public string curUser = Helper.GeneralSettings; //ΙD χρήστη
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
        public List<int> found = new List<int>(); //Λιστα με τα id ενοτήτων



        public GameStart(int classN, string type, int chosenChapter)
        {

            found.Clear();
            questnum.Clear();


            if (type == "random")
            {
                sindesi.Open();

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
                //questnum.ForEach(Console.WriteLine);

                //InitializeComponent();
                //Navigation.PushAsync(new GamePlay(questnum,curUser,classN));
            }
            else if (type == "casual")
            {
                sindesi.Open();
                chosenChapter = chosenChapter + ((classN - 1) * 10);
                //Console.WriteLine("ChosenChapter: " + chosenChapter + "\n");

                MySqlCommand getData = sindesi.CreateCommand();
                getData.CommandText = "SELECT id FROM quests WHERE chapterID = @chapter ";
                getData.Parameters.AddWithValue("@chapter", chosenChapter);



                MySqlDataReader rdr2 = getData.ExecuteReader();

                if (rdr2.HasRows)
                {
                    while (rdr2.Read())
                    {

                        found.Add(rdr2.GetInt32(0));
                        //Console.WriteLine(found.Count+"++++");
                        //found.ForEach(Console.WriteLine);
                    }
                }
                rdr2.Close();


                Random num = new Random();

                while (questnum.Count < found.Count && questnum.Count < 10)
                {
                    int selnum = num.Next(found.Min(), found.Max() + 1);
                    // Δημιουργία λιστας 10 τυχαίων διαφορετικών int
                    if (!questnum.Contains(selnum))
                    {
                        //Επιλεγμένες ερωτήσεις 
                        questnum.Add(selnum);
                    }
                }
                //questnum.ForEach(Console.WriteLine);
                //Navigation.PushAsync(new GamePlay(questnum,curUser,classN));
            }
        }
    }
}



