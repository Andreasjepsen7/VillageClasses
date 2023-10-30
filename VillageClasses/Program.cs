public class Villager
{
    public string Name { get; set; }
    public int Age { get; set; }
    public List<Inventory> Inventory { get; set; }
    public int Money { get; set; }
    public string CurrentLocation { get; set; }
    public int Health { get; set; }

    public Villager(string name, int age, List<Inventory> inventory, int money, string currentLocation, int health)
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

    public Pillager(string name, int age, List<Inventory> inventory, int money, string currentLocation, int health)
        : base(name, age, inventory, money, currentLocation, health)
    {
        // Initialize Pillager-specific attributes here
    }
}


public class Paladins : Villager
{
    public string Armor { get; set; }
    public bool HasShield { get; set; }

    public Paladins(string name, int age, List<Inventory> inventory, int money, string currentLocation, int health)
        : base(name, age, inventory, money, currentLocation, health)
    {
        // Initialize Paladins-specific attributes here
    }
}

public class Peasant : Villager
{
    public string Job { get; set; }
    public List<TradeGood> TradeGoods { get; set; }

    public Peasant(string name, int age, List<Inventory> inventory, int money, string currentLocation, int health)
        : base(name, age, inventory, money, currentLocation, health)
    {
        // Initialize Peasant-specific attributes here
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

    public King(string name, int age, List<Inventory> inventory, int money, string currentLocation, int health, string title, string enemy)
        : base(name, age, inventory, money, currentLocation, health)
    {
        Title = title;
        Enemy = enemy;
    }
}

public class Inventory
{
    public string Weapon { get; set; }
    public string Food { get; set; }
    public string Pony { get; set; }
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
