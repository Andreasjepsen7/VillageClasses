using System;
using System.Collections.Generic;

public class Program
{
    static Paladins paladin;
    static Peasant peasant;
    static Location village;

    public delegate void RandomEventDelegate(string message);
    static event RandomEventDelegate RandomEvent;

    static void Main(string[] args)
    {
        InitializeGame();
        RunGameLoop();
    }

    static void InitializeGame()
    {
        // Initialize your game world, characters, and locations
        Location castle = new Location("Castle", "A grand castle where the Paladins reside");
        village = new Location("Village", "A peaceful village of the Peasants");
        Location pillagersCamp = new Location("Pillager's Camp", "A small camp where the terrifying pillagers live");

        paladin = new Paladins("Paladin Kaj", 40, new List<InventoryItem>(), 1000, castle, 100)
        {
            Armor = "Shiny Armor",
            HasShield = true
        };

        List<Paladins> paladins = new List<Paladins>
        {
            new Paladins("Paladin Søren", 50, new List<InventoryItem>(), 1000, castle, 100),
            new Paladins("Paladin Thomas", 505, new List<InventoryItem>(), 1000, castle, 100)
        };

        peasant = new Peasant("Mikkel", 35, ItemGenerator.GenerateRandomItems(), 500, village, 80)
        {
            Job = "Farmer",
            TradeGoods = new List<TradeGood> { new TradeGood("Crops"), new TradeGood("Livestock") }
        };

        List<Peasant> peasants = new List<Peasant>
{
    new Peasant("Mads", 20, new List<InventoryItem>(), 500, village, 100),
    new Peasant("Andreas", 7, new List<InventoryItem>(), 200, village, 100),
};


        Pillager pillager = new Pillager("Pillager 1", 45, new List<InventoryItem>(), 100, pillagersCamp, 100)
        {
            AggressionLevel = "High",
            Infamous = true,
            CombatSkills = new List<string> { "Swordsmanship", "Archery" },
            Loot = new List<string> { "Gold", "Gems" }
        };

        List<Pillager> pillagers = new List<Pillager>
    {
        new Pillager("Pillager 2", 30, new List<InventoryItem>(), 50, pillagersCamp, 80),
        new Pillager("Pillager 3", 28, new List<InventoryItem>(), 45, pillagersCamp, 75),
        // Add more Pillagers as needed
    };

        castle.Inhabitants.Add(paladin);
        village.Inhabitants.Add(peasant);
        village.Inhabitants.AddRange(peasants);
        pillagersCamp.Inhabitants.Add(pillager);
        pillagersCamp.Inhabitants.AddRange(pillagers);

        RandomEvent += HandleRandomEvent;

        Console.WriteLine("Welcome to the RPG game!");
    }

    static void RunGameLoop()
    {
        while (true)
        {
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Simulate a day");
            Console.WriteLine("2. Exit");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Clear();
                    SimulateDay();
                    break;
                case "2":
                    Console.WriteLine("Goodbye!");
                    return;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        }
    }

    static void SimulateDay()
    {
        // Simulate a day in your RPG world
        int eventCount = RNG.GenerateRandomNumber(1, 1);

        for (int i = 0; i < eventCount; i++)
        {
            int eventType = RNG.GenerateRandomNumber(1, 3);
            string message = GenerateRandomEvent(eventType);
            RandomEvent?.Invoke(message);
        }

        bool pillagersAttack = RNG.GenerateRandomNumber(1, 2) == 1; // 50% chance of attack
        if (pillagersAttack)
        {
            // Implement the attack logic
            pillagersCampAttack();
        }

        CheckAndHandleDeceasedVillagers(village);

        HandleCharacterInteractions();
    }

    static string GenerateRandomEvent(int eventType)
    {
        switch (eventType)
        {
            case 1:
                return "A mysterious traveler arrives in the village.";
            case 2:
                return "A festival is held in the town square.";
            case 3:
                return "A monster is spotted in the nearby forest.";
            // Add more event types as needed
            default:
                return "A peaceful day in the village.";
        }
    }

    static void pillagersCampAttack()
    {
        Console.WriteLine("Pillagers are attacking a peasant!");

        // Select a random Peasant to target from the list of Peasants in the village
        List<Peasant> peasants = village.Inhabitants.OfType<Peasant>().ToList();
        if (peasants.Count > 0)
        {
            int randomPeasantIndex = RNG.GenerateRandomNumber(0, peasants.Count - 1);
            Peasant targetPeasant = peasants[randomPeasantIndex];

            // Implement logic to decrease the targeted Peasant's health or resources
            int damage = RNG.GenerateRandomNumber(10, 30); // Random damage amount
            targetPeasant.Health -= damage;

            Console.WriteLine($"{targetPeasant.Name} suffered {damage} points of damage.");
            Console.WriteLine($"Peasant {targetPeasant.Name}'s Health: {targetPeasant.Health}");
        }
        else
        {
            Console.WriteLine("There are no peasants in the village to attack.");
        }
    }

    static void CheckAndHandleDeceasedVillagers(Location location)
    {
        foreach (Peasant peasants in location.Inhabitants.ToList())
        {
            if (peasants.Health <= 0)
            {
                Console.WriteLine($"{peasants.Name} has died.");
                location.Inhabitants.Remove(peasants);
            }
        }
    }

    static void HandleCharacterInteractions()
    {
        Console.WriteLine("Random Character Interaction:");

        // Generate a random interaction option
        int randomInteraction = RNG.GenerateRandomNumber(1, 3);

        switch (randomInteraction)
        {
            case 1:
                // Paladin buys food from a Peasant
                paladin.BuyItem(peasant, new Food { Name = "Bread", Nutrition = 10, Price = new Price(5) }, 10);
                Console.WriteLine("Paladin bought food from a Peasant.");
                break;
            case 2:
                // Peasant sells food to a Paladin
                peasant.SellItem(paladin, new Food { Name = "Apples", Nutrition = 8, Price = new Price(4) }, 5);
                Console.WriteLine("Peasant sold food to a Paladin.");
                break;
            case 3:
                // Villagers trade food items
                TradeItemsBetweenPeasants(new List<Peasant> { peasant });
                Console.WriteLine("Peasants traded food items.");
                break;
            case 4:
                // Villagers trade non-food items
                TradeItemsBetweenPeasants(new List<Peasant> { peasant });
                Console.WriteLine("Peasants traded non-food items.");
                break;
        }
    }

    static void TradeItemsBetweenPeasants(List<Peasant> peasants)
    {
        Peasant peasant1 = GetRandomPeasant(peasants);
        Peasant peasant2 = GetRandomPeasant(peasants);

        if (peasant1 != null && peasant2 != null)
        {
            // Select random food items to trade
            IItem item1 = GetRandomFoodItem(peasant1);
            IItem item2 = GetRandomFoodItem(peasant2);

            if (item1 != null && item2 != null)
            {
                Console.WriteLine($"{peasant1.Name} offers {item1.Name} to {peasant2.Name} in exchange for {item2.Name}.");

                if (item1 is Food && item2 is Food)
                {
                    // If both items are food, exchange them
                    peasant1.Inventory.Remove(new InventoryItem { Item = item1 });
                    peasant1.Inventory.Add(new InventoryItem { Item = item2 });

                    peasant2.Inventory.Remove(new InventoryItem { Item = item2 });
                    peasant2.Inventory.Add(new InventoryItem { Item = item1 });

                    Console.WriteLine("Trade successful: Both items were food.");
                }
                else
                {
                    Console.WriteLine("Trade failed: Peasants couldn't agree on the terms.");
                }
            }
            else
            {
                Console.WriteLine("One of the peasants does not have food items to trade.");
            }
        }
        else
        {
            Console.WriteLine("No eligible peasants found for trading.");
        }
    }

    static IItem GetRandomFoodItem(Peasant peasant)
    {
        // Check if the peasant has food items in their inventory
        var foodItems = peasant.Inventory
            .Where(item => item.Item is Food)
            .Select(item => item.Item)
            .ToList();

        // Check if there are food items in the inventory
        if (foodItems.Count == 0)
        {
            Console.WriteLine($"{peasant.Name} does not have any food items to trade.");
            return null;
        }

        // Use RNG.GenerateRandomNumber to pick a random food item from the list
        int randomIndex = RNG.GenerateRandomNumber(0, foodItems.Count - 1);

        // Return the selected food item
        return foodItems[randomIndex];
    }




    static Peasant GetRandomPeasant(List<Peasant> peasants)
    {
        // Check if there are peasants in the list
        if (peasants.Count == 0)
        {
            Console.WriteLine("No eligible peasants found for trading.");
            return null;
        }

        // Use RNG.GenerateRandomNumber to pick a random index from the list
        int randomIndex = RNG.GenerateRandomNumber(0, peasants.Count - 1);

        // Return the selected peasant
        return peasants[randomIndex];
    }


    static void HandleRandomEvent(string message)
    {
        Console.WriteLine("Random Event: " + message);
    }
}
