using System;
using System.Collections.Generic;
using System.Linq;

public class Program
{
    public delegate void RandomEventDelegate(string message);
    static event RandomEventDelegate RandomEvent;
    static List<Paladins> paladins;
    static List<Peasant> peasants;

    static List<Pillager> pillagers;
    static List<TradeGood> availableTradeGoods;

    static Location castle;
    static Location village;
    static Location fishingHut;
    static Location pillagersCamp;

    static void Main(string[] args)
    {
        InitializeGame();
        RunGameLoop();
    }

    static void InitializeGame()
    {
        // Initialize locations
        castle = new Location("Castle", "A grand castle where the Paladins reside");
        village = new Location("Village", "A peaceful village of the Peasants");
        fishingHut = new Location("Fishing Hut", "A fishing hut where plenty of fish are caught every day");
        pillagersCamp = new Location("Pillager's Camp", "A small camp where the terrifying pillagers live");

        availableTradeGoods = new List<TradeGood>
        {
            new TradeGood("Coal"),
            new TradeGood("Wood"),
            new TradeGood("Fish"),
        };

        // Initialize characters and lists
        var loot = new List<Loot> { new Loot("Coal"), new Loot("Wood"), new Loot("Fish") };

        paladins = new List<Paladins>
        {
            new Paladins("Rusty armor", false, "Paladin Søren", 50, new List<InventoryItem>(), 1000, castle, 100, GetRandomSwordOrSpear()),
            new Paladins("Goldsen armor", true, "Paladin Thomas", 505, new List<InventoryItem>(), 1000, castle, 100, GetRandomSwordOrSpear()),
            new Paladins("Steel armor", true, "Paladin Kaj", 40, new List<InventoryItem>(), 1000, castle, 100, GetRandomSwordOrSpear())
        };

        peasants = new List<Peasant>
        {
            new Peasant("Coal Miner", GetRandomTradeGoods(), "Mads", 20, new List<InventoryItem>(), 500, village, 100),
            new Peasant("Woodcutter", GetRandomTradeGoods(), "Andreas", 23, new List<InventoryItem>(), 200, village, 100),
            new Peasant("Fisher", GetRandomTradeGoods(), "Mikkel", 18, new List<InventoryItem>(), 50, fishingHut, 100)
        };

        village.Inhabitants.AddRange(peasants);

        pillagers = new List<Pillager>
        {
            new Pillager("High", true, new List<Loot>(), false, false, "Pillager Joe", 30, new List<InventoryItem>(), 0, pillagersCamp, 100),
            new Pillager("Medium", true, new List<Loot>(), false, false, "Pillager Monty", 30, new List<InventoryItem>(), 0, pillagersCamp, 100),
            new Pillager("Low", false, new List<Loot>(), false, false, "Pillager Clyde", 30, new List<InventoryItem>(), 0, pillagersCamp, 100),
        };



        RandomEvent += HandleRandomEvent;

        Console.WriteLine("Welcome to the RPG game!");

        // Assign random trade goods to peasants
        GetRandomTradeGoods();
    }

    static List<TradeGood> GetRandomTradeGoods()
    {
        List<TradeGood> randomGoods = new List<TradeGood>();
        Random random = new Random();

        // Determine how many random trade goods to assign to the peasant
        int count = random.Next(1, availableTradeGoods.Count + 1);

        // Shuffle the available trade goods
        List<TradeGood> shuffledTradeGoods = availableTradeGoods.OrderBy(item => random.Next()).ToList();

        // Take the first 'count' trade goods from the shuffled list
        randomGoods.AddRange(shuffledTradeGoods.Take(count));

        return randomGoods;
    }


    static void RunGameLoop()
    {
        while (true)
        {
            Console.WriteLine("\nOptions:");
            Console.WriteLine("1. Simulate a day");
            Console.WriteLine("2. Exit");
            Console.WriteLine("3. View all characters");

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
                case "3":
                    Console.Clear();
                    ViewAllCharacters();
                    break;
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
            PillagersCampAttack();
        }

        CheckAndHandleDeceasedVillagers(village);

        HandleCharacterInteractions(paladins[0], peasants[0]);
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
            default:
                return "A peaceful day in the village.";
        }
    }

    static void PillagersCampAttack()
    {
        Console.WriteLine("Pillagers are attacking a peasant!");

        // Select a random Peasant to target from the list of Peasants in the village
        List<Peasant> peasantsInVillage = village.Inhabitants.OfType<Peasant>().ToList();

        if (peasantsInVillage.Count == 0)
        {
            Console.WriteLine("There are no peasants in the village to attack.");
            return; // Exit the method if there are no peasants to attack
        }

        int randomPeasantIndex = RNG.GenerateRandomNumber(0, peasantsInVillage.Count - 1);
        Peasant targetPeasant = peasantsInVillage[randomPeasantIndex];

        // Implement logic to decrease the targeted Peasant's health
        int damage = RNG.GenerateRandomNumber(10, 30); // Random damage amount
        targetPeasant.Health -= damage;

        Console.WriteLine($"{targetPeasant.Name} suffered {damage} points of damage.");
        Console.WriteLine($"Peasant {targetPeasant.Name}'s Health: {targetPeasant.Health}");

        // Check if the peasant's health drops to or below zero
        if (targetPeasant.Health <= 0)
        {
            Console.WriteLine($"{targetPeasant.Name} has been defeated!");
            // Remove the defeated peasant from the village.Inhabitants list
            village.Inhabitants.Remove(targetPeasant);
        }
    }



    static void ViewAllCharacters()
    {
        foreach (Paladins paladin in paladins)
        {
            if (paladin.CurrentLocationObj != null && paladin.CurrentLocationObj.Name != null)
            {
                Console.WriteLine("Paladin Name: " + paladin.Name);
                Console.WriteLine("Armor Type: " + paladin.Armor);
                Console.WriteLine("Health: " + paladin.Health);
                Console.WriteLine("Inventory Weapon: " + paladin.EquippedWeapon.Name);
                Console.WriteLine("Gold: " + paladin.Money);
                Console.WriteLine("Location: " + paladin.CurrentLocationObj.Name);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Paladin Location is not defined.");
            }
        }

        foreach (Peasant peasant in peasants)
        {
            Console.WriteLine("Peasant name: " + peasant.Name);
            Console.WriteLine("Peasant age: " + peasant.Age);
            Console.WriteLine("Peasant health: " + peasant.Health);
            if (peasant.TradeGoods != null)
            {
                Console.WriteLine("Peasant Trade Goods: " + string.Join(", ", peasant.TradeGoods.Select(g => g.Name)));
            }
            else
            {
                Console.WriteLine("Peasant Trade Goods are not defined.");
            }
            Console.WriteLine("Gold: " + peasant.Money);
            Console.WriteLine();
        }
        foreach (Pillager pillager in pillagers)
        {
            Console.WriteLine("Pillager name: " + pillager.Name);
            Console.WriteLine("Pillager age: " + pillager.Age);
            Console.WriteLine("Pillager health: " + pillager.Health);
            Console.WriteLine("Pillager aggression: " + pillager.AggressionLevel);
            Console.WriteLine();
        }
    }

    static void CheckAndHandleDeceasedVillagers(Location location)
    {
        // Create a copy of the list of inhabitants to avoid modifying it directly in the loop
        List<Peasant> deceasedPeasants = new List<Peasant>();

        foreach (Peasant peasant in location.Inhabitants.ToList())
        {
            if (peasant != null && peasant.Health <= 0)
            {
                deceasedPeasants.Add(peasant);
            }
        }

        // Remove deceased peasants from the location's inhabitants
        foreach (Peasant deceasedPeasant in deceasedPeasants)
        {
            location.Inhabitants.Remove(deceasedPeasant);
        }
    }

    static void HandleCharacterInteractions(Paladins paladin, Peasant peasant)
    {
        Console.WriteLine("Random Character Interaction:");

        if (peasant != null && peasant.Health > 0)
        {
            // The peasant is alive and can interact.

            // Generate a random interaction option
            int randomInteraction = RNG.GenerateRandomNumber(1, 4);

            switch (randomInteraction)
            {
                case 1:
                    if (peasant.TradeGoods != null)
                    {
                        paladin.BuyItem(peasant, new Food { Name = "Bread", Nutrition = 10, Price = new Price(5) }, 10);
                        Console.WriteLine($"{peasant.Name} bought food from {peasant.Name}.");
                    }
                    break;
                case 2:
                    // Peasant sells food to a Paladin
                    if (paladin != null && peasant != null && peasant.TradeGoods != null)
                    {
                        peasant.SellItem(paladin, new Food { Name = "Apples", Nutrition = 8, Price = new Price(4) }, 5);
                        Console.WriteLine($"{peasant.Name} sold food to {paladin.Name}.");
                    }
                    break;
                case 3:
                    // Villagers trade food items
                    TradeItemsBetweenPeasants(new List<Peasant> { peasant });
                    Console.WriteLine($"Peasants traded food items.");
                    break;
                case 4:
                    // Villagers trade non-food items
                    TradeItemsBetweenPeasants(new List<Peasant> { peasant });
                    Console.WriteLine("Peasants traded non-food items.");
                    break;
            }
        }
        else
        {
            Console.WriteLine($"{peasant.Name} is dead and cannot trade or interact.");
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
    static Weapon GetRandomSwordOrSpear()
    {
        Random random = new Random();
        int randomValue = random.Next(2); // 0 or 1

        return randomValue == 0 ? new Weapon("Sword", 30, new Price(15)) : new Weapon("Spear", 25, new Price(10));
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
