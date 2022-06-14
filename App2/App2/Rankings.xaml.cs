using System;
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
	public partial class Rankings : ContentPage
	{
		public Rankings ()
		{
			InitializeComponent ();
        }

        private void ClassicRanks_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new CRanks());
        }

        private void RandomRanks_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RRanks());
        }

    }
}