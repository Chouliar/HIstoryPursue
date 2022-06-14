using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Plugin.Settings;
using Plugin.Settings.Abstractions;
using Xamarin.Forms;




namespace App2
{
    public class Helper : ContentPage
    {
        private static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }

        }
        #region Setting Constants

        private const string SettingsKey = "settings_key";
        private static readonly string SettingsDefault = string.Empty;

        #endregion

        public static string GeneralSettings
        {
            get
            {
                return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(SettingsKey, value);
            }
        }

    }
}
