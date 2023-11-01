using System.Linq;

public class Villager
{
    public string Name { get; set; }
    public int Age { get; set; }
    public List<InventoryItem> Inventory { get; set; }
    public int Money { get; set; }
    public int Health { get; set; }
    public Location CurrentLocationObj { get; set; }
    
    public Villager(string name, int age, List<InventoryItem> inventory, int money, Location currentLocation, int health)
    {
        Name = name;
        Age = age;
        Inventory = inventory;
        Money = money;
        CurrentLocationObj = currentLocation; // Set the current location object
        Health = health;
    }
    public void BuyItem(Villager seller, IItem item, int price)
    {
        if (Money >= price)
        {
            Money -= price;
            seller.Money += price;

            Inventory.Add(new InventoryItem { Item = item });

            Console.WriteLine($"{Name} bought {item.Name} from {seller.Name} for {price} gold.");
        }
        else
        {
            Console.WriteLine($"{Name} doesn't have enough money to buy {item.Name} from {seller.Name}.");
        }
    }

    public void SellItem(Villager buyer, IItem item, int price)
    {
        var inventoryItem = Inventory.FirstOrDefault(ii => ii.Item == item);
        if (inventoryItem != null)
        {
            Money += price;
            buyer.Money -= price;

            Inventory.Remove(inventoryItem);

            Console.WriteLine($"{Name} sold {item.Name} to {buyer.Name} for {price} gold.");
        }
        else
        {
            Console.WriteLine($"{Name} doesn't have {item.Name} to sell to {buyer.Name}.");
        }
    }

}


public class Pillager : Villager
{
    public string AggressionLevel { get; set; }
    public bool Infamous { get; set; }
    public List<Loot> loot { get; set; }
    public bool AttackedPeasant { get; set; }
    public bool IsDefeated { get; set; }

    public Pillager(string AggressionLevel, bool Infamous, List<Loot> loot, bool AttackedPeasant, bool IsDefeated, string name, int age, List<InventoryItem> inventory, int money, Location currentLocation, int health)
        : base(name, age, inventory, money, currentLocation, health)
    {
        this.AggressionLevel = AggressionLevel;
    }

    public void AttackPeasant(Peasant peasant)
    {
        // Simulate the attack
        // ...

        AttackedPeasant = true;
    }
}


public class Paladins : Villager
{
    public string Armor { get; set; }
    public bool HasShield { get; set; }
    public Weapon EquippedWeapon { get; set; }

    public Paladins(string Armor, bool HasShield, string name, int age, List<InventoryItem> inventory, int money, Location currentLocation, int health, Weapon equippedWeapon)
        : base(name, age, inventory, money, currentLocation, health)
    {
        this.Armor = Armor;
        this.HasShield = HasShield;
        this.EquippedWeapon = equippedWeapon;
    }

    public void Attack(Pillager pillager)
    {
        int damage = CalculateAttackDamage();
        pillager.Health -= damage;

        if (pillager.Health <= 0)
        {
            Console.WriteLine($"{Name} has defeated {pillager.Name}!");
        }
        else
        {
            Console.WriteLine($"{Name} attacked {pillager.Name} for {damage} damage.");
        }
    }

    private int CalculateAttackDamage()
    {
        // Implement your damage calculation logic based on the equipped weapon, skills, etc.
        int baseDamage = EquippedWeapon != null ? EquippedWeapon.Damage : 10; // Default damage if no weapon is equipped
        int minDamage = baseDamage - 5;
        int maxDamage = baseDamage + 5;
        return RNG.GenerateRandomNumber(minDamage, maxDamage);
    }
}


public class Peasant : Villager
{
    public string Job { get; set; }
    public List<TradeGood> TradeGoods { get; set; }
    public List<Loot> loots { get; set; }

    public Peasant(string job, List<TradeGood> tradeGoods, string name, int age, List<InventoryItem> inventory, int money, Location currentLocation, int health)
        : base(name, age, inventory, money, currentLocation, health)
    {
        Job = job;
        TradeGoods = tradeGoods; // Assign the TradeGoods property
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

public class Loot
{
    public string Name { get; set; }
    public Loot(string name)
    {
        Name = name;
    }

}
public class King : Villager
{
    public string Title { get; set; }
    public string Enemy { get; set; }

    public King(string name, int age, List<InventoryItem> inventory, int money, Location currentLocation, int health, string title, string enemy)
        : base(name, age, inventory, money, currentLocation, health)
    {
        Title = title;
        Enemy = enemy;
    }
}

public class InventoryItem
{
    public IItem Item { get; set; }
}


// Define interfaces for items
public interface IItem
{
    string Name { get; }
    Price Price { get; }
}

public class Weapon : IItem
{
    public string Name { get; set; }
    public int Damage { get; set; }
    public Price Price { get; set; }

    public Weapon(string name, int damage, Price price)
    {
        Name = name;
        Damage = damage;
        Price = price;
    }
}


public class Food : IItem
{
    public string Name { get; set; }
    public int Nutrition { get; set; }
    public Price Price { get; set; }
}

public class Pony : IItem
{
    public string Name { get; set; }
    public int Age { get; set; }
    public string Color { get; set; }
    public Price Price { get; set; }
}

public class Price
{
    public int Value { get; }

    public Price(int value)
    {
        Value = value;
    }
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
    public List<Villager> Inhabitants { get; set; }

    public Location(string name, string description)
    {
        Name = name;
        Description = description;
        Inhabitants = new List<Villager>();
    }
}

