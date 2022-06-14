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
	public partial class RRanks : ContentPage
	{
		public RRanks ()
		{
			InitializeComponent();
            //InitializeComponent();
            {
                Grid grid = new Grid
                {

                    VerticalOptions = LayoutOptions.FillAndExpand,
                    ColumnSpacing = 1,

                    RowDefinitions =
                    {
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
                        new RowDefinition { Height = new GridLength(1, GridUnitType.Star)},
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




                grid.Children.Add(new Label
                {
                    Text = "Τάξη",
                    TextColor = Color.Red,
                }, 0, 1, 0, 1);

                grid.Children.Add(new Label
                {
                    Text = "Τρίτη",
                    TextColor = Color.Black,
                }, 0, 1, 1, 2);

                grid.Children.Add(new Label
                {
                    Text = "Τετάρτη",
                    TextColor = Color.Black,
                }, 0, 1, 2, 3);

                grid.Children.Add(new Label
                {
                    Text = "Πέμπτη",
                    TextColor = Color.Black,
                }, 0, 1, 3, 4);
                
                grid.Children.Add(new Label
                {
                    Text = "Έκτη",
                    TextColor = Color.Black,
                }, 0, 1, 4, 5);

                grid.Children.Add(new Label
                {
                    Text = "Όλες",
                    TextColor = Color.Green,
                }, 0, 1, 5, 6);


                grid.Children.Add(new Label
                {
                    Text = "Πόντοι",
                    HorizontalOptions = LayoutOptions.End,
                    TextColor = Color.Red,

                }, 1, 2, 0, 1);

                for (int i = 1; i <= 5; i++)
                {
                    sindesi.Open();
                    MySqlCommand getData = sindesi.CreateCommand();
                    getData.CommandText = "SELECT points FROM random_ranks WHERE userID = @userid AND class = @i";
                    getData.Parameters.AddWithValue("@userid", UserID);
                    getData.Parameters.AddWithValue("@i", i+2);
                    MySqlDataReader rdr2 = getData.ExecuteReader();
                    while (rdr2.Read())
                    {
                        int points = (int)rdr2["points"];
                        Console.WriteLine(points);
                        strscr = points.ToString();
                    }

                    grid.Children.Add(new Label
                    {
                        Text = strscr,
                        TextColor = Color.Black,
                        HorizontalOptions = LayoutOptions.End,

                    }, 1, 2, i, i + 1);
                    strscr = "0";
                    rdr2.Close();
                    sindesi.Close();

                }

                grid.Children.Add(new Label
                {
                    Text = "Σύνολο",
                    HorizontalOptions = LayoutOptions.End,
                    TextColor = Color.Red,
                }, 2, 3, 0, 1);

                for (int i = 1; i <= 4; i++)
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