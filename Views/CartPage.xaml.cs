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
public sealed partial class CartPage : Page
{
    public CartPage()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
    }

    private void OnSizeChanged(object sender, SizeChangedEventArgs e)
    {
        var bounds = Window.Current.Bounds;
        double height = bounds.Height;

        MenuListView.Height = Math.Max(120, height - 350);
    }

    private List<Data.MenuPlus> CartItems { get { return Data.AppState.CartItems; } }

    private double TotalPriceJPY { get; set; } = 0;
    private double TotalPriceUSD { get; set; } = 0;

    protected override void OnNavigatedTo(NavigationEventArgs e)
    {
        base.OnNavigatedTo(e);

        DataContext = CartItems;

        foreach (var menu in CartItems)
        {
            TotalPriceJPY += menu.price.jpy;
            TotalPriceUSD += menu.price.usd;
        }

        TotalPrice.Text =
            $"The Total Price for {CartItems.Count} Items is " +
            $"{TotalPriceJPY}円 ({TotalPriceUSD} USD)";
    }

    private async void Checkout_Click(object sender, RoutedEventArgs e)
    {
        const string title = "Caffè Latte, Caffè Mocha, CappuChino!";

        var dialog = CartItems.Count > 0 ? new ContentDialog
        {
            Title = title,
            Content = $"ご注文は {CartItems.Count} メニューですか？\n" +
                      $"全部 {TotalPriceJPY}円 ({TotalPriceUSD} USD) です。",
            PrimaryButtonText = "Coffee",
            CloseButtonText = "Cancel",
            DefaultButton = ContentDialogButton.Primary
        } : new ContentDialog
        {
            Title = title,
            Content = "ご注文は。。。 ない！？",
            CloseButtonText = "Continue Shopping"
        };

        var choice = await dialog.ShowAsync();
        if (choice == ContentDialogResult.Primary)
        {
            Data.AppState.CartItems.Clear();
        }
    }
}
