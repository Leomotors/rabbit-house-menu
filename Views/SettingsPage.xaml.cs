using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Reflection;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Storage;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace rabbit_house_menu.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            this.InitializeComponent();

            // Copied from microsoft/Xaml-Controls-Gallery
            var version = Windows.ApplicationModel.Package.Current.Id.Version;
            AppVersion.Text = $"Version: {version.Major}.{version.Minor} Build {version.Build}.{version.Revision}";
        }

        private async void ViewLicense_Clicked(object sender, RoutedEventArgs e)
        {
            string License = await FileIO.ReadTextAsync(
                await StorageFile.GetFileFromApplicationUriAsync(
                    new Uri("ms-appx:///LICENSE")
                )
            );

            const string tmpToken = "__@$";

            var dialog = new ContentDialog
            {
                Title = "LICENSE",
                // Remove "\n" but not "\n\n"
                Content = License.Replace("\n\n", tmpToken).Replace("\n", " ").Replace(tmpToken, "\n\n"),
                CloseButtonText = "Close"
            };

            await dialog.ShowAsync();
        }
    }
}
