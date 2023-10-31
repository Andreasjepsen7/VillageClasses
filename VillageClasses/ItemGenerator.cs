public class ItemGenerator
{
    public static List<InventoryItem> GenerateRandomItems()
    {
        List<InventoryItem> items = new List<InventoryItem>();

        // Add random items to the list
        items.Add(new InventoryItem { Item = new Food { Name = "Bread", Nutrition = 10, Price = new Price(5) } });
        items.Add(new InventoryItem { Item = new Food { Name = "Apples", Nutrition = 8, Price = new Price(4) } });

        // Add more items or customize the items as needed

        return items;
    }
}
