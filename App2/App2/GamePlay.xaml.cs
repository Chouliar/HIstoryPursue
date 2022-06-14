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
    public partial class GamePlay : ContentPage
    {
        public List<int> questnum = new List<int>(); //Λιστα με τις επιλεγμένες ερωτήσεις 
        public string curUser = Helper.GeneralSettings; //ΙD χρήστη
        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
        public int classSel;
        public int sol = 0;
        public int scoreC;
        public string gtype;
        public int cchapter;

        private string questString;
        private string ans1String;

        public string QuestString
        {
            get { return questString; }
            set
            {
                questString = value;
                OnPropertyChanged(nameof(QuestString));
            }
        }

        public string Ans1String
        {
            get { return ans1String; }
            set
            {
                ans1String = value;
                OnPropertyChanged(nameof(Ans1String));
            }
        }

        public GamePlay(List<int> order, string userID, int pclass, int score, int chapter, string type)
        {
            
            InitializeComponent();
            System.Threading.Thread.Sleep(2000);
            classSel = pclass;
            scoreC = score;
            questnum = order;
            gtype = type;
            cchapter = chapter;

            sindesi.Open();
            Console.Write(questnum[0]);
            MySqlCommand chk2 = new MySqlCommand("SELECT question, q1, q2, q3, q4, correct FROM quests WHERE id = @id", sindesi);
            chk2.Parameters.AddWithValue("@id", questnum[0]);
            MySqlDataReader rdr2 = chk2.ExecuteReader();
            while (rdr2.Read())
            {
                BindingContext = this;
                QuestString = (string)rdr2["question"];//Ερωτηση
                BindingContext = this;
                Ans1String = (string)rdr2["q1"];
                ID.Text = userID;
                Score.Text = scoreC.ToString();
                //question.Text = (string)rdr2["question"];
                //b1.Text = (string)rdr2["q1"];
                b2.Text = (string)rdr2["q2"];
                b3.Text = (string)rdr2["q3"];
                b4.Text = (string)rdr2["q4"];
                sol = (Int32)rdr2["correct"];
            }
            rdr2.Close();

            
            questnum.Remove(questnum[0]);
           
            questnum.ForEach(Console.WriteLine);
            Console.WriteLine(sol);

            sindesi.Close();
        }

        async private void B1_Clicked(object sender, EventArgs e)
        {
            b1.BackgroundColor = Color.Black;
            b2.BackgroundColor = Color.Black;
            b3.BackgroundColor = Color.Black;
            b4.BackgroundColor = Color.Black;
            if (sol == 1)
            {
                scoreC += 10;
                Score.Text = scoreC.ToString();
                b1.BackgroundColor = Color.Green;
               
            }
            else
            {
                Score.Text = scoreC.ToString();
                string cor = "b" + sol;
                if (cor == "b2")
                {
                    b2.BackgroundColor = Color.Green;
                }
                else if (cor == "b3")
                {
                    b3.BackgroundColor = Color.Green;
                }
                else if (cor == "b4")
                {
                    b4.BackgroundColor = Color.Green;
                }
                b1.BackgroundColor = Color.Red;
                
                
            }
            if (questnum.Count == 0)
            {
                await Navigation.PushAsync(new GameOver(scoreC,curUser,gtype,cchapter,classSel));
                Navigation.RemovePage(this);
            }
            else
            {
                await Navigation.PushAsync(new Page2(questnum, curUser, classSel, scoreC, cchapter, gtype));
                Navigation.RemovePage(this);
                //questnum.Remove(questnum[0]);
                //Navigation.InsertPageBefore(new Page2(questnum, curUser, classSel, scoreC), this); // 'this' refers to current page which is Page2 
                //await Navigation.PopAsync().ConfigureAwait(false);
            }
        }


        async private void B2_Clicked(object sender, EventArgs e)
        {
            questnum.ForEach(Console.WriteLine);

            b1.BackgroundColor = Color.Black;
            b2.BackgroundColor = Color.Black;
            b3.BackgroundColor = Color.Black;
            b4.BackgroundColor = Color.Black;
            if (sol == 2)
            {
                scoreC += 10;
                Score.Text = scoreC.ToString();
                b2.BackgroundColor = Color.Green;
                

            }
            else
            {
                Score.Text = scoreC.ToString();
                string cor = "b" + sol;
                if (cor == "b1")
                {
                    b1.BackgroundColor = Color.Green;
                }
                else if (cor == "b3")
                {
                    b3.BackgroundColor = Color.Green;
                }
                else if (cor == "b4")
                {
                    b4.BackgroundColor = Color.Green;
                }
                b2.BackgroundColor = Color.Red;
                

            }
            if (questnum.Count == 0)
            {
                await Navigation.PushAsync(new GameOver(scoreC, curUser, gtype, cchapter,classSel));
                Navigation.RemovePage(this);
            }
            else
            {
                await Navigation.PushAsync(new Page2(questnum, curUser, classSel, scoreC, cchapter, gtype));
                Navigation.RemovePage(this);
            }
        }

        async private void B3_Clicked(object sender, EventArgs e)
        {
            questnum.ForEach(Console.WriteLine);

            b1.BackgroundColor = Color.Black;
            b2.BackgroundColor = Color.Black;
            b3.BackgroundColor = Color.Black;
            b4.BackgroundColor = Color.Black;
            if (sol == 3)
            {
                scoreC += 10;
                Score.Text = scoreC.ToString();
                b3.BackgroundColor = Color.Green;
                
            }
            else
            {
                Score.Text = scoreC.ToString();
                string cor = "b" + sol;
                if (cor == "b1")
                {
                    b1.BackgroundColor = Color.Green;
                }
                else if (cor == "b4")
                {
                    b4.BackgroundColor = Color.Green;
                }
                else if (cor == "b2")
                {
                    b2.BackgroundColor = Color.Green;
                }
                b3.BackgroundColor = Color.Red;
            }
            if (questnum.Count == 0)
            {
                await Navigation.PushAsync(new GameOver(scoreC, curUser, gtype, cchapter, classSel));
                Navigation.RemovePage(this);
            }
            else
            {
                await Navigation.PushAsync(new Page2(questnum, curUser, classSel, scoreC, cchapter, gtype));
                Navigation.RemovePage(this);
            }
        }

    

        async private void B4_Clicked(object sender, EventArgs e)
        {
            questnum.ForEach(Console.WriteLine);

            b1.BackgroundColor = Color.Black;
            b2.BackgroundColor = Color.Black;
            b3.BackgroundColor = Color.Black;
            b4.BackgroundColor = Color.Black;
            if (sol == 4)
            {
                scoreC += 10;
                Score.Text = scoreC.ToString();
                b4.BackgroundColor = Color.Green;
                
                
            }
            else
            {
                Score.Text = scoreC.ToString();
                string cor = "b" + sol;
                if (cor == "b1")
                {
                    b1.BackgroundColor = Color.Green;
                }
                else if (cor == "b3")
                {
                    b3.BackgroundColor = Color.Green;
                }
                else if (cor == "b2")
                {
                    b2.BackgroundColor = Color.Green;
                }
                b4.BackgroundColor = Color.Red;
                
                
            }
           
            if (questnum.Count == 0)
            {
                await Navigation.PushAsync(new GameOver(scoreC, curUser, gtype, cchapter, classSel));
                Navigation.RemovePage(this);
            }
            else
            {

                await Navigation.PushAsync(new Page2(questnum, curUser, classSel, scoreC, cchapter, gtype));
                Navigation.RemovePage(this);
            }
        }
    }
}