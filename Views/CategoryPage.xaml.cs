using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace rabbit_house_menu.Views;

/// <summary>
/// An empty page that can be used on its own or navigated to within a Frame.
/// </summary>
public sealed partial class CategoryPage : Page
{
    public CategoryPage()
    {
        this.InitializeComponent();
    }

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        var param = e.Parameter as CategoryPayload;

        CategoryText.Text = param.Category;
        ShowMenu(param);
    }

    public List<Data.Menu> ThisMenu { get; set; }

    private void ShowMenu(CategoryPayload payload)
    {
        ThisMenu = payload.Restaurant == Restaurant.RABBIT_HOUSE ? Data.MenusManager.RabbitHouseMenu[payload.Category] : Data.MenusManager.FleurDeLapinMenu[payload.Category];

        DataContext = this;
    }
}

public class CategoryPayload
{
    public string Category { get; set; }
    public Restaurant Restaurant { get; set; }

    public CategoryPayload(string category, Restaurant restaurant)
    {
        Category = category;
        Restaurant = restaurant;
    }
}

public enum Restaurant
{
    RABBIT_HOUSE,
    FLEUR_DE_LAPIN,
}
