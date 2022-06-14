using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using MySqlConnector;

namespace App2
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CRanks : ContentPage
	{
        
        public CRanks ()
		{
			InitializeComponent ();
            {
                Grid grid = new Grid
                {
                    
                    VerticalOptions = LayoutOptions.FillAndExpand,
                    ColumnSpacing = 1,

                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) },
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }
                    },
                    ColumnDefinitions =
                    {
                        new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star)},
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star)},
                        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) }
                    }
                };
                grid.Margin = new Thickness(20);


                string UserID = Helper.GeneralSettings;
                string strscr = "0";
                MySqlConnection sindesi = new MySqlConnection(Properties.Resources.my_db);
                //sindesi.Open();
                int sum = 0;
                


                grid.Children.Add(new Label
                {
                    Text = "Ενότητα ",
                    TextColor = Color.Red,
                }, 0, 1, 0, 1);

                for (int i=1; i <= 10; i++){
                    grid.Children.Add(new Label
                    {
                        Text = "Ενότητα "+i,
                        TextColor = Color.Black,

                    }, 0, 1, i, i+1);
                }

                grid.Children.Add(new Label
                {
                    Text = "Πόντοι",
                    HorizontalOptions = LayoutOptions.End,
                    TextColor = Color.Red,

                }, 1, 2, 0, 1);
                //int sum = 0;

                for (int i = 1; i <= 10; i++)
                {
                    sindesi.Open();

                    MySqlCommand chk = new MySqlCommand("SELECT class FROM users WHERE id = @id ", sindesi);
                    chk.Parameters.AddWithValue("@id", UserID);
                    int chkclass = Convert.ToInt32(chk.ExecuteScalar());

                    Console.WriteLine(chkclass);
                    int j = i;

                    if (chkclass > 1)
                    {
                        j = i + (10 * (chkclass-1));
                    }
                    
                    

                    MySqlCommand getData = sindesi.CreateCommand();
                    getData.CommandText = "SELECT score FROM classic_ranks WHERE user_id = @userid AND chapter = @j";
                    getData.Parameters.AddWithValue("@userid", UserID);
                    getData.Parameters.AddWithValue("@j", j);
                    

                    MySqlDataReader rdr2 = getData.ExecuteReader();
                    while (rdr2.Read())
                    {
                        
                        int score = (int)rdr2["score"];
                        Console.WriteLine(score);
                        strscr = score.ToString();
                        sum = sum + score;
                    }
                    rdr2.Close();
                    

                    grid.Children.Add(new Label
                    {
                        Text = strscr,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.End,

                    }, 1, 2, i, i + 1);
                    strscr = "0";


                    sindesi.Close();

                }
                
                grid.Children.Add(new Label
                {
                    Text = "Συνολο",
                    HorizontalOptions = LayoutOptions.End,
                    TextColor = Color.Red,
                }, 2, 3, 0, 1);

                for (int i = 1; i <= 10; i++)
                {
                    grid.Children.Add(new Label
                    {
                        Text = "100",
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.End,

                    }, 2, 3, i, i + 1);
                }



                // Build the page.
                this.Content = grid;
            }

        }


	}
}