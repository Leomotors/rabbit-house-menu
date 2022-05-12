#nullable enable

namespace rabbit_house_menu.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MenuPage : Page {
    public MenuPage() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);

        var res = e.Parameter as Restaurant?;

        Title.Text = $"{RestaurantUtil.Resolve(res)}'s Menu";
    }
}
