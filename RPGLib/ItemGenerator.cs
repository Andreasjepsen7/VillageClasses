using RPGLib;
public class ItemGenerator
{
    public static List<InventoryItem> GenerateRandomItems()
    {
        List<InventoryItem> items = new List<InventoryItem>
    {
        new InventoryItem { Item = new Food { Name = "Bread", Nutrition = 10, Price = new Price(5) } },
        new InventoryItem { Item = new Food { Name = "Apples", Nutrition = 8, Price = new Price(4) } },
        new InventoryItem { Item = new Pony { Name = "Brown horse", Age = 5, Color = "Brown", Price = new Price(10) } },
        new InventoryItem { Item = new Pony { Name = "White horse", Age = 7, Color = "White", Price = new Price(15) } }
    };

        int randomWeaponType = RNG.GenerateRandomNumber(1, 2);
        if (randomWeaponType == 1)
        {
            items.Add(new InventoryItem { Item = new Weapon("Sword", 30, new Price(15)) });
        }
        else
        {
            items.Add(new InventoryItem { Item = new Weapon("Spear", 25, new Price(10)) });
        }

        return items;
    }
}
