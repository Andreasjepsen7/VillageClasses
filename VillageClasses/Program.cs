public class Villager
{
    public string Name { get; set; }
    public int Age { get; set; }
    public List<InventoryItem> Inventory { get; set; }
    public int Money { get; set; }
    public string CurrentLocation { get; set; }
    public int Health { get; set; }

    public Villager(string name, int age, List<InventoryItem> inventory, int money, string currentLocation, int health)
    {
        Name = name;
        Age = age;
        Inventory = inventory;
        Money = money;
        CurrentLocation = currentLocation;
        Health = health;
    }
}

public class Pillager : Villager
{
    public string AggressionLevel { get; set; }
    public bool Infamous { get; set; }
    public List<string> CombatSkills { get; set; }
    public List<string> Loot { get; set; }

    public Pillager(string name, int age, List<InventoryItem> inventory, int money, string currentLocation, int health)
        : base(name, age, inventory, money, currentLocation, health)
    {
    }
}


public class Paladins : Villager
{
    public string Armor { get; set; }
    public bool HasShield { get; set; }

    public Paladins(string name, int age, List<InventoryItem> inventory, int money, string currentLocation, int health)
        : base(name, age, inventory, money, currentLocation, health)
    {
    }
}

public class Peasant : Villager
{
    public string Job { get; set; }
    public List<TradeGood> TradeGoods { get; set; }

    public Peasant(string name, int age, List<InventoryItem> inventory, int money, string currentLocation, int health)
        : base(name, age, inventory, money, currentLocation, health)
    {
    }
}

public class TradeGood
{
    public string Name { get; set; }

    public TradeGood(string name)
    {
        Name = name;
    }
}

public class King : Villager
{
    public string Title { get; set; }
    public string Enemy { get; set; }

    public King(string name, int age, List<InventoryItem> inventory, int money, string currentLocation, int health, string title, string enemy)
        : base(name, age, inventory, money, currentLocation, health)
    {
        Title = title;
        Enemy = enemy;
    }
}

public class InventoryItem
{
    public Weapon Weapon { get; set; }
    public Food Food { get; set; }
    public Pony Pony { get; set; }
}

public class Price
{
    public int Value { get; }

    public Price(int value)
    {
        Value = value;
    }
}

public class Weapon
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public string Type { get; set; }
    public Price price { get; set; }
}

public class Food
{
    public string Name { get; set; }
    public int Nutrition { get; set; }
    public Price price { get; set; }
}

public class Pony
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Color { get; set; }

    public Price price { get; set; }
}
public class RNG
{
    private static Random random = new Random();

    //int min = 1; // Minimum value for the random number
    //int max = 100; // Maximum value for the random number

    //int randomNumber = GenerateRandomNumber(min, max);

    public static int GenerateRandomNumber(int min, int max)
    {
        return random.Next(min, max + 1);
    }
}

public class Location
{
    public string Name { get; set; }
    public string Description { get; set; }
}
