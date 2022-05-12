#nullable enable

namespace rabbit_house_menu.Data;

public static class AppState {
    public static List<MenuPlus> CartItems { get; private set; } = new();

    public static void AddToCart(MenuPlus menu) {
        CartItems.Add(menu);
    }

    public static void AddToCart(Menu menu, string category, Views.Restaurant restaurant) {
        CartItems.Add(
            new MenuPlus(
                menu.name,
                menu.price,
                category,
                Views.RestaurantUtil.Resolve(restaurant)
            )
        );
    }
}
