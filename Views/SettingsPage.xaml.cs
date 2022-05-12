using Windows.Storage;

#nullable enable

namespace rabbit_house_menu.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class SettingsPage : Page {
    public SettingsPage() {
        InitializeComponent();

        // Copied from microsoft/Xaml-Controls-Gallery
        var version = Windows.ApplicationModel.Package.Current.Id.Version;
        AppVersion.Text = $"Version: {version.Major}.{version.Minor} Build {version.Build}.{version.Revision}";
    }

    private async void ViewLicense_Clicked(object sender, RoutedEventArgs e) {
        var License = await FileIO.ReadTextAsync(
            await StorageFile.GetFileFromApplicationUriAsync(
                new Uri("ms-appx:///LICENSE")
            )
        );

        const string tmpToken = "__@$";

        var dialog = new ContentDialog {
            Title = "LICENSE",
            // Remove "\n" but not "\n\n"
            Content = License.Replace("\n\n", tmpToken).Replace("\n", " ").Replace(tmpToken, "\n\n"),
            CloseButtonText = "Close"
        };

        await dialog.ShowAsync();
    }
}
