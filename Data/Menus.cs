// Manual JSON Parsing, HELL

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Storage;
using Windows.Data.Json;

namespace rabbit_house_menu.Data;

public class Label
{
    public string en { get; set; }
    public string ja { get; set; }

    public Label(string en, string ja)
    {
        this.en = en;
        this.ja = ja;
    }

    public void Validate()
    {
        if (!(en.Length > 0 && ja.Length > 0))
            throw new Exception($"Invalid Name: {ja} / {en} is Empty");
    }
}

public class Price
{
    public double jpy { get; set; }
    public double usd { get; set; }

    public Price(double jpy, double usd)
    {
        this.jpy = jpy;
        this.usd = usd;
    }

    public void Validate()
    {
        if (!((jpy > 0 || jpy == -1) && (usd > 0 || usd == -1)))
        {
            throw new Exception($"Invalid Price: {jpy} / {usd}");
        }
    }
}

public class Menu
{
    public Label name { get; set; }
    public Price price { get; set; }

    public Menu(Label name, Price price)
    {
        this.name = name;
        this.price = price;
    }

    public void Validate()
    {
        name.Validate();
        price.Validate();
    }
}

public class MenuPlus : Menu
{
    public string category { get; private set; }
    public string Restaurant { get; private set; }

    public MenuPlus(Label name, Price price, string category, string Restaurant) : base(name, price)
    {
        this.category = category;
        this.Restaurant = Restaurant;
    }
}

public static class MenusManager
{
    private static bool RickRollAdded = false;

    public static Dictionary<string, List<Menu>> RabbitHouseMenu { get; private set; }
    public static Dictionary<string, List<Menu>> FleurDeLapinMenu { get; private set; }

    public static List<MenuPlus> AllMenus { get; private set; } = new();

    public static async Task LoadAllData()
    {
        var promise1 = LoadData("rabbit_house.json", "Rabbit House");
        var promise2 = LoadData("fleur_de_lapin.json", "Fleur De Lapin");

        RabbitHouseMenu = await promise1;
        FleurDeLapinMenu = await promise2;
    }

    private static async Task<Dictionary<string, List<Menu>>> LoadData(string Location, string Restaurant)
    {
        string jsonString = await FileIO.ReadTextAsync(
            await StorageFile.GetFileFromApplicationUriAsync(
                new Uri($"ms-appx:///Data/{Location}")
            )
        );

        var rootObj = JsonObject.Parse(jsonString);
        var menus = new Dictionary<string, List<Menu>>();

        foreach (var item in rootObj)
        {
            var category = item.Key;
            var cate_menus = item.Value.GetArray();

            List<Menu> menusList = new();

            foreach (var menu in cate_menus)
            {
                var parsed = parseMenu(menu);
                menusList.Add(parsed);
                AllMenus.Add(new MenuPlus(parsed.name, parsed.price, category, Restaurant));
            }

            menus[category] = menusList;
        }

        // Easter Egg, all my program should has Rick Roll, lol.
        if (!RickRollAdded)
        {
            AllMenus.Add(
              new MenuPlus(
                new Label("Rick Astley", "リック・アストリー"),
                new Price(69, 420),
                "Cursed",
                "Cursed Restaurant"
              )
            );
            RickRollAdded = true;
        }

        return menus;
    }

    private static Menu parseMenu(IJsonValue menu)
    {
        var menuObj = menu.GetObject();

        Label name = new("", "");
        Price price = new(-1, -1);

        foreach (var entry in menuObj)
        {
            if (entry.Key == "name")
                name = parseLabel(entry.Value);
            else if (entry.Key == "price")
                price = parsePrice(entry.Value);
            else
                throw new Exception($"Invalid Menu Key: {entry.Key}");
        }

        var newMenu = new Menu(name, price);
        newMenu.Validate();

        return newMenu;
    }

    private static Label parseLabel(IJsonValue label)
    {
        var labelObj = label.GetObject();

        string en = "", ja = "";

        foreach (var entry in labelObj)
        {
            if (entry.Key == "en")
                en = entry.Value.GetString();
            else if (entry.Key == "ja")
                ja = entry.Value.GetString();
            else
                throw new Exception($"Invalid Label Key: {entry.Key}");
        }

        return new Label(en, ja);
    }

    private static Price parsePrice(IJsonValue price)
    {
        var priceObj = price.GetObject();

        double jpy = 0, usd = 0;

        foreach (var entry in priceObj)
        {
            if (entry.Key == "jpy")
                jpy = entry.Value.GetNumber();
            else if (entry.Key == "usd")
                usd = entry.Value.GetNumber();
            else
                throw new Exception($"Invalid Label Key: {entry.Key}");
        }

        return new Price(jpy, usd);
    }
}
