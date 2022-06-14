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
	public partial class GameOver : ContentPage
	{
        public string GameType;
        public int Chapter;
        public int EndScore,Nclass;
        public string user;

        public MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);

        public GameOver (int score, string UserID, string type , int chapter,int classN)
		{
            Chapter = chapter;
            GameType = type;
            EndScore = score;
            user = UserID;
            Nclass = classN;

			InitializeComponent ();
            Score.Text = score.ToString();
		}

        async private void Menu_Clicked(object sender, EventArgs e)
        {
            sindesi.Open();
            if(GameType == "casual")
            {
                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM classic_ranks WHERE user_id = @id AND chapter= @chapter", sindesi);
                chk.Parameters.AddWithValue("@id", user);
                chk.Parameters.AddWithValue("@chapter", Chapter);
                int chkbase = Convert.ToInt32(chk.ExecuteScalar());
                sindesi.Close();
                if (chkbase > 0)
                {
                    sindesi.Open();
                    MySqlCommand chk2 = new MySqlCommand("SELECT score FROM classic_ranks WHERE user_ID = @id AND chapter= @chapter", sindesi);
                    chk2.Parameters.AddWithValue("@id", user);
                    chk2.Parameters.AddWithValue("@chapter", Chapter);
                    int chkbase2 = Convert.ToInt32(chk.ExecuteScalar());
                    sindesi.Close();
                    if (chkbase2 < EndScore)
                    {
                        sindesi.Open();
                        MySqlCommand UpdateData2;
                        UpdateData2 = sindesi.CreateCommand();
                        MySqlCommand rdr4 = new MySqlCommand("UPDATE classic_ranks SET score=@score WHERE user_ID = @id AND chapter= @chapter", sindesi);
                        rdr4.Parameters.AddWithValue("@score", EndScore);
                        rdr4.Parameters.AddWithValue("@id", user);
                        rdr4.Parameters.AddWithValue("@chapter", Chapter);
                        var rd = rdr4.ExecuteReaderAsync();
                        sindesi.Close();
                    }
                }
                else
                {
                    sindesi.Open();
                    MySqlCommand cmd = new MySqlCommand("insert into classic_ranks(id,user_id,chapter,score) values('NULL','" + user + "','" + Chapter + "','" + EndScore + "' )", sindesi);
                    var rd = cmd.ExecuteReaderAsync();
                    sindesi.Close();
                }
            }
            else if (GameType == "random")
            {
                MySqlCommand chk = new MySqlCommand("SELECT COUNT(*) FROM random_ranks WHERE userID = @id AND class= @class", sindesi);
                chk.Parameters.AddWithValue("@id", user);
                chk.Parameters.AddWithValue("@class", Nclass);
                int chkbase = Convert.ToInt32(chk.ExecuteScalar());
                sindesi.Close();
                if (chkbase > 0)
                {
                    sindesi.Open();
                    MySqlCommand chk2 = new MySqlCommand("SELECT points FROM random_ranks WHERE userID = @id AND class= @class", sindesi);
                    chk2.Parameters.AddWithValue("@id", user);
                    chk2.Parameters.AddWithValue("@class", Nclass);
                    int chkbase2 = Convert.ToInt32(chk.ExecuteScalar());
                    sindesi.Close();
                    if (chkbase2 < EndScore)
                    {

                        sindesi.Open();
                        MySqlCommand UpdateData;
                        UpdateData = sindesi.CreateCommand();
                        UpdateData.CommandText = "UPDATE random_Ranks SET points ='" + EndScore + "' WHERE userID = " + user;
                        MySqlDataReader rdr = UpdateData.ExecuteReader();

                        while (rdr.Read())
                        {
                        }
                        rdr.Close();
                        sindesi.Close();
                    }
                }
                else
                {
                    sindesi.Open();
                    MySqlCommand cmd = new MySqlCommand("insert into random_ranks(class,points,userID) values('"+ Nclass + "','" + EndScore + "','" + user + "' )", sindesi);
                    var rd = cmd.ExecuteReaderAsync();
                    sindesi.Close();
                }
            }
            sindesi.Close();
            Navigation.PushAsync(new Menu());
        }
    }
}