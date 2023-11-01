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
            new Peasant("Coal Miner", GetRandomTradeGoods(), "Mads", 20, 500, village, 100),
            new Peasant("Woodcutter", GetRandomTradeGoods(), "Andreas", 23, 200, village, 100),
            new Peasant("Fisher", GetRandomTradeGoods(), "Mikkel", 18, 50, fishingHut, 100)
        };

        village.Inhabitants.AddRange(peasants);

        pillagers = new List<Pillager>
        {
            new Pillager("High", true, new List<Loot>(), false, false, "Pillager Joe", 30, new List<InventoryItem>(), 100, pillagersCamp, 100),
            new Pillager("Medium", true, new List<Loot>(), false, false, "Pillager Monty", 30, new List<InventoryItem>(), 100, pillagersCamp, 100),
            new Pillager("Low", false, new List<Loot>(), false, false, "Pillager Clyde", 30, new List<InventoryItem>(), 100, pillagersCamp, 100),
        };
        pillagersCamp.Inhabitants.AddRange(pillagers);


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
            PillagersAttackPeasants();
        }
        else
        {
            PaladinsAttackPillagers();
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
            default:
                return "A peaceful day in the village.";
        }
    }

    static void PillagersAttackPeasants()
    {
        Console.WriteLine("Pillagers are attacking peasants!");

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

    static void PaladinsAttackPillagers()
    {
        Console.WriteLine("Paladins are attacking pillagers!");

        // Select a random Pillager to target from the list of Pillagers at the pillagersCamp
        List<Pillager> pillagersAtCamp = pillagersCamp.Inhabitants.OfType<Pillager>().ToList();

        if (pillagersAtCamp.Count == 0)
        {
            Console.WriteLine("There are no pillagers at the camp to attack.");
            return; // Exit the method if there are no pillagers to attack
        }

        int randomPillagerIndex = RNG.GenerateRandomNumber(0, pillagersAtCamp.Count - 1);
        Pillager targetPillager = pillagersAtCamp[randomPillagerIndex];

        // Implement logic to decrease the targeted Pillager's health
        int damage = RNG.GenerateRandomNumber(10, 30); // Random damage amount
        targetPillager.Health -= damage;

        Console.WriteLine($"{targetPillager.Name} suffered {damage} points of damage.");
        Console.WriteLine($"Pillager {targetPillager.Name}'s Health: {targetPillager.Health}");

        // Check if the pillager's health drops to or below zero
        if (targetPillager.Health <= 0)
        {
            Console.WriteLine($"{targetPillager.Name} has been defeated!");
            // Remove the defeated pillager from the pillagersCamp.Inhabitants list
            pillagersCamp.Inhabitants.Remove(targetPillager);
        }
    }


    static void ViewAllCharacters()
    {
        foreach (Paladins paladin in paladins)
        {
            Console.WriteLine("Paladin Name: " + paladin.Name);
            Console.WriteLine("Health: " + paladin.Health);
            Console.WriteLine("Inventory Weapon: " + paladin.EquippedWeapon.Name);
            Console.WriteLine("Location: " + paladin.CurrentLocationObj.Name);
            Console.WriteLine();
        }

        foreach (Peasant peasant in peasants)
        {
            Console.WriteLine("Peasant name: " + peasant.Name);
            Console.WriteLine("Peasant age: " + peasant.Age);
            Console.WriteLine("Peasant health: " + peasant.Health);
            Console.WriteLine("Gold: " + peasant.Money);
            if (peasant.Inventory != null && peasant.Inventory.Any())
            {
                Console.WriteLine("Inventory Items: " + string.Join(", ", peasant.Inventory.Select(ii => ii.Item.Name)));
            }
            else
            {
                Console.WriteLine("Peasant's Inventory is empty.");
            }
            if (peasant.TradeGoods != null)
            {
                Console.WriteLine("Peasant Trade Goods: " + string.Join(", ", peasant.TradeGoods.Select(g => g.Name)));
            }
            else
            {
                Console.WriteLine("Peasant Trade Goods are not defined.");
            }
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

    static void HandleCharacterInteractions()
    {
        Console.WriteLine("Random Character Interaction:");

        // Randomly select two distinct peasants
        Peasant peasant1 = GetRandomPeasant(peasants);
        Peasant peasant2;

        do
        {
            peasant2 = GetRandomPeasant(peasants);
        } while (peasant2 == peasant1);

        if (peasant1 != null && peasant2 != null && peasant1.Health > 0 && peasant2.Health > 0)
        {
            // Both peasants are alive and can interact.

            // Generate a random interaction option
            int randomInteraction = RNG.GenerateRandomNumber(1, 4);

            switch (randomInteraction)
            {
                case 1:
                    Console.WriteLine($"{peasant1.Name} and {peasant2.Name} traded items.");
                    TradeItems(peasant1, peasant2);
                    break;
                case 2:
                    Console.WriteLine($"{peasant1.Name} and {peasant2.Name} talked about the weather.");
                    break;
                case 3:
                    Console.WriteLine($"{peasant1.Name} and {peasant2.Name} played a game.");
                    break;
                case 4:
                    Console.WriteLine($"{peasant1.Name} and {peasant2.Name} had a meal together.");
                    break;
            }
        }
        else
        {
            Console.WriteLine($"{peasant1.Name} and {peasant2.Name} cannot interact because one or both are dead.");
        }
    }

    static void HandleRandomEvent(string message)
    {
        Console.WriteLine("Random Event: " + message);
    }

    static Peasant GetRandomPeasant(List<Peasant> peasants)
    {
        // Check if there are peasants in the list

        // Use RNG.GenerateRandomNumber to pick a random index from the list
        int randomIndex = RNG.GenerateRandomNumber(0, peasants.Count - 1);

        // Return the selected peasant
        return peasants[randomIndex];
    }

    static void TradeItems(Peasant peasant1, Peasant peasant2)
    {
        // Check if there are enough eligible peasants for trading
        if (peasant1 == null || peasant2 == null)
        {
            return;
        }

        // Check if both peasants have items to trade
        if (peasant1.Inventory.Count > 0 && peasant2.Inventory.Count > 0)
        {
            // Select random items from each peasant's inventory
            var randomItemFromPeasant1 = GetRandomItem(peasant1);
            var randomItemFromPeasant2 = GetRandomItem(peasant2);

            // Check if the selected items are different
            if (randomItemFromPeasant1 != randomItemFromPeasant2)
            {
                // Swap the items between the peasants
                peasant1.Inventory.Remove(new InventoryItem { Item = randomItemFromPeasant1 });
                peasant2.Inventory.Remove(new InventoryItem { Item = randomItemFromPeasant2 });

                peasant1.Inventory.Add(new InventoryItem { Item = randomItemFromPeasant2 });
                peasant2.Inventory.Add(new InventoryItem { Item = randomItemFromPeasant1 });

                Console.WriteLine($"{peasant1.Name} traded {randomItemFromPeasant1.Name} with {peasant2.Name} for {randomItemFromPeasant2.Name}.");
            }
            else
            {
                Console.WriteLine("Trade failed: Peasants couldn't agree on the terms.");
            }
        }
        else
        {
            Console.WriteLine("Trade failed: One or both of the peasants do not have items to trade.");
        }
    }

    static IItem GetRandomItem(Peasant peasant)
    {
        // Check if the peasant has items in their inventory
        if (peasant.Inventory.Count > 0)
        {
            // Use RNG.GenerateRandomNumber to pick a random item from the inventory
            int randomIndex = RNG.GenerateRandomNumber(0, peasant.Inventory.Count - 1);

            // Return the selected item
            return peasant.Inventory[randomIndex].Item;
        }
        return null; // Return null if no items are available for trade.
    }


    static Weapon GetRandomSwordOrSpear()
    {
        Random random = new Random();
        int randomValue = random.Next(2); // 0 or 1

        return randomValue == 0 ? new Weapon("Sword", 30, new Price(15)) : new Weapon("Spear", 25, new Price(10));
    }
}
