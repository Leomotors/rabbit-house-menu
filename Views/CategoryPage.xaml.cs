#nullable enable

namespace rabbit_house_menu.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CategoryPage : Page {
    public CategoryPage() {
        InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e) {
        base.OnNavigatedTo(e);

        var param = e.Parameter as CategoryPayload;

        CategoryText.Text = Category = param.Category;
        Restaurant = param.Restaurant;

        Title.Text = $"{RestaurantUtil.Resolve(param.Restaurant)}'s Menu";

        ShowMenu(param);
    }

    public List<Data.Menu> ThisMenu { get; set; }

    private string Category { get; set; }
    private Restaurant Restaurant { get; set; }

    private void ShowMenu(CategoryPayload payload) {
        ThisMenu = payload.Restaurant == Restaurant.RABBIT_HOUSE ? Data.MenusManager.RabbitHouseMenu[payload.Category] : Data.MenusManager.FleurDeLapinMenu[payload.Category];

        DataContext = this;
    }

    private async void MenuListView_ItemClick(object sender, ItemClickEventArgs e) {
        var index = MenuListView.Items.IndexOf(e.ClickedItem);

        var selected = ThisMenu[index];

        var itemName = $"{selected.name.ja} ({selected.name.en})";

        var dialog = selected.price.jpy > 0 ? new ContentDialog {
            Title = "Buy Some Coffee or Green Tea",
            Content = $"{itemName} is {selected.price.jpy}円 ({selected.price.usd} USD)",
            PrimaryButtonText = "Add to Cart",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary
        } : new ContentDialog {
            Title = "Coffee, Green Tea",
            Content = $"{itemName} price is Unknown 「不明」",
            CloseButtonText = "Close"
        };

        var option = await dialog.ShowAsync();
        if (option == ContentDialogResult.Primary) {
            Data.AppState.AddToCart(selected, Category, Restaurant);
        }
    }
}

public class CategoryPayload {
    public string Category { get; set; }
    public Restaurant Restaurant { get; set; }

    public CategoryPayload(string category, Restaurant restaurant) {
        Category = category;
        Restaurant = restaurant;
    }
}

public enum Restaurant {
    RABBIT_HOUSE,
    FLEUR_DE_LAPIN,
}

public static class RestaurantUtil {
    public static string Resolve(Restaurant? restaurant) {
        return restaurant == Restaurant.RABBIT_HOUSE ? "Rabbit House" : "Fleur De Lapin";
    }
}
