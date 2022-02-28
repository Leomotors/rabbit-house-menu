using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using MUXC = Microsoft.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace rabbit_house_menu
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        readonly Task task = Data.MenusManager.loadData();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void NavigationView_Loaded(object sender, RoutedEventArgs e)
        {
            contentFrame.Navigate(typeof(Views.WelcomePage));

            await task;

            var categoryMenus = Menu.MenuItems;

            foreach (var entry in Data.MenusManager.menus)
            {
                var menu = new MUXC.NavigationViewItem
                {
                    Content = entry.Key
                };

                categoryMenus.Add(menu);
            }
        }

        private void NavigationView_SelectionChanged(MUXC.NavigationView sender, MUXC.NavigationViewSelectionChangedEventArgs args)
        {
            if (args.IsSettingsSelected)
            {
                contentFrame.Navigate(typeof(Views.SettingsPage));
                return;
            }

            var selected = args.SelectedItem as MUXC.NavigationViewItem;

            if (selected == Welcome)
            {
                contentFrame.Navigate(typeof(Views.WelcomePage));
            }
            else if (selected == Menu)
            {
                contentFrame.Navigate(typeof(Views.MenuPage));
            }
            else
            {
                // Probably NavigationView.MenuItems

                var cate = selected.Content as string;

                contentFrame.Navigate(typeof(Views.CategoryPage), cate);
            }
        }
    }
}
