using Windows.ApplicationModel.Core;
using Windows.UI;
using Windows.UI.ViewManagement;

#nullable enable

namespace rabbit_house_menu;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class MainPage : Page {
    private readonly Task task = Data.MenusManager.LoadAllData();

    public MainPage() {
        InitializeComponent();

        // These below code along with methods are copied from
        // https://docs.microsoft.com/en-us/windows/apps/design/style/mica#title-bar-code-behind

        var titleBar = ApplicationView.GetForCurrentView().TitleBar;

        titleBar.ButtonBackgroundColor = Colors.Transparent;
        titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;

        // Hide default title bar.
        var coreTitleBar = CoreApplication.GetCurrentView().TitleBar;
        coreTitleBar.ExtendViewIntoTitleBar = true;
        UpdateTitleBarLayout(coreTitleBar);

        // Set XAML element as a draggable region.
        Window.Current.SetTitleBar(AppTitleBar);

        // Register a handler for when the size of the overlaid caption control changes.
        // For example, when the app moves to a screen with a different DPI.
        coreTitleBar.LayoutMetricsChanged += CoreTitleBar_LayoutMetricsChanged;

        // Register a handler for when the title bar visibility changes.
        // For example, when the title bar is invoked in full screen mode.
        coreTitleBar.IsVisibleChanged += CoreTitleBar_IsVisibleChanged;

        //Register a handler for when the window changes focus
        Window.Current.Activated += Current_Activated;
    }

    private void CoreTitleBar_LayoutMetricsChanged(CoreApplicationViewTitleBar sender, object args) {
        UpdateTitleBarLayout(sender);
    }

    private void UpdateTitleBarLayout(CoreApplicationViewTitleBar coreTitleBar) {
        // Update title bar control size as needed to account for system size changes.
        AppTitleBar.Height = coreTitleBar.Height;

        // Ensure the custom title bar does not overlap window caption controls
        var currMargin = AppTitleBar.Margin;
        AppTitleBar.Margin = new Thickness(currMargin.Left, currMargin.Top, coreTitleBar.SystemOverlayRightInset, currMargin.Bottom);
    }

    private void CoreTitleBar_IsVisibleChanged(CoreApplicationViewTitleBar sender, object args) {
        if (sender.IsVisible) {
            AppTitleBar.Visibility = Visibility.Visible;
        } else {
            AppTitleBar.Visibility = Visibility.Collapsed;
        }
    }

    // Update the TitleBar based on the inactive/active state of the app
    private void Current_Activated(object sender, Windows.UI.Core.WindowActivatedEventArgs e) {
        var defaultForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorPrimaryBrush"];
        var inactiveForegroundBrush = (SolidColorBrush)Application.Current.Resources["TextFillColorDisabledBrush"];

        if (e.WindowActivationState == Windows.UI.Core.CoreWindowActivationState.Deactivated) {
            AppTitle.Foreground = inactiveForegroundBrush;
        } else {
            AppTitle.Foreground = defaultForegroundBrush;
        }
    }

    private async void NavigationView_Loaded(object sender, RoutedEventArgs e) {
        contentFrame.Navigate(typeof(Views.WelcomePage));

        await task;

        foreach (var entry in Data.MenusManager.RabbitHouseMenu) {
            MUXC.NavigationViewItem menu = new() {
                Content = entry.Key
            };

            RabbitHouseMenu.MenuItems.Add(menu);
        }

        foreach (var entry in Data.MenusManager.FleurDeLapinMenu) {
            MUXC.NavigationViewItem menu = new() {
                Content = entry.Key
            };

            FleurDeLapinMenu.MenuItems.Add(menu);
        }
    }

    private void NavigationView_SelectionChanged(object sender, MUXC.NavigationViewSelectionChangedEventArgs args) {
        if (args.IsSettingsSelected) {
            contentFrame.Navigate(typeof(Views.SettingsPage));
            return;
        }

        var selected = args.SelectedItem as MUXC.NavigationViewItem;

        if (selected == Welcome) {
            contentFrame.Navigate(typeof(Views.WelcomePage));
        } else if (selected == RabbitHouseMenu) {
            contentFrame.Navigate(typeof(Views.MenuPage), Views.Restaurant.RABBIT_HOUSE);
        } else if (selected == FleurDeLapinMenu) {
            contentFrame.Navigate(typeof(Views.MenuPage), Views.Restaurant.FLEUR_DE_LAPIN);
        } else if (selected == ShoppingCart) {
            contentFrame.Navigate(typeof(Views.CartPage));
        } else {
            // Probably NavigationView.MenuItems

            if (selected.Content is not string cate) {
                return;
            }

            Views.Restaurant restaurant;

            if (RabbitHouseMenu.MenuItems.IndexOf(selected) != -1) {
                restaurant = Views.Restaurant.RABBIT_HOUSE;
            } else {
                restaurant = Views.Restaurant.FLEUR_DE_LAPIN;
            }

            contentFrame.Navigate(
                typeof(Views.CategoryPage),
                new Views.CategoryPayload(cate, restaurant)
            );
        }
    }
}
