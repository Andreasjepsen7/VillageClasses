
using RPGLib;

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

            ConsoleKeyInfo keyInfo = Console.ReadKey();

            switch (keyInfo.Key)
            {
                case ConsoleKey.D1:
                    Console.Clear();
                    SimulateDay();
                    break;
                case ConsoleKey.D2:
                    Console.WriteLine("\nGoodbye!");
                    return;
                case ConsoleKey.D3:
                    Console.Clear();
                    ViewAllCharacters();
                    break;
                default:
                    Console.WriteLine("\nInvalid choice. Please try again.");
                    break;
            }
        }
    }


    static void SimulateDay()
    {
        // Simulate a day in your RPG world
        int eventCount = RNG.GenerateRandomNumber(1, 3);

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
        string[] eventMessages = {
        "A mysterious traveler arrives in the village.",
        "A festival is held in the town square.",
        "A monster is spotted in the nearby forest."
    };

        if (eventType >= 1 && eventType <= eventMessages.Length)
        {
            return eventMessages[eventType - 1];
        }

        return "A peaceful day in the village.";
    }


    static void PillagersAttackPeasants()
    {
        Console.WriteLine("Pillagers are attacking peasants!");

        List<Peasant> peasantsInVillage = village.Inhabitants.OfType<Peasant>().ToList();

        if (peasantsInVillage.Count == 0)
        {
            Console.WriteLine("There are no peasants in the village to attack.");
            return;
        }

        int randomPeasantIndex = RNG.GenerateRandomNumber(0, peasantsInVillage.Count - 1);
        Peasant targetPeasant = peasantsInVillage[randomPeasantIndex];

        int damage = RNG.GenerateRandomNumber(10, 30); // Random damage amount

        // Apply damage to the targeted Peasant
        targetPeasant.Health -= damage;

        Console.WriteLine($"{targetPeasant.Name} suffered {damage} points of damage.");
        Console.WriteLine($"Peasant {targetPeasant.Name}'s Health: {targetPeasant.Health}");

        if (targetPeasant.Health <= 0)
        {
            Console.WriteLine($"{targetPeasant.Name} has been defeated!");
            RemoveDefeatedPeasant(targetPeasant);
        }
    }

    static void RemoveDefeatedPeasant(Peasant peasant)
    {
        village.Inhabitants.Remove(peasant);
    }


    static void PaladinsAttackPillagers()
    {
        Console.WriteLine("Paladins are attacking pillagers!");

        List<Pillager> pillagersAtCamp = pillagersCamp.Inhabitants.OfType<Pillager>().ToList();

        if (pillagersAtCamp.Count == 0)
        {
            Console.WriteLine("There are no pillagers at the camp to attack.");
            return;
        }

        int randomPillagerIndex = RNG.GenerateRandomNumber(0, pillagersAtCamp.Count - 1);
        Pillager targetPillager = pillagersAtCamp[randomPillagerIndex];

        int damage = RNG.GenerateRandomNumber(10, 30); // Random damage amount

        // Apply damage to the targeted Pillager
        targetPillager.Health -= damage;

        Console.WriteLine($"{targetPillager.Name} suffered {damage} points of damage.");
        Console.WriteLine($"Pillager {targetPillager.Name}'s Health: {targetPillager.Health}");

        if (targetPillager.Health <= 0)
        {
            Console.WriteLine($"{targetPillager.Name} has been defeated!");
            RemoveDefeatedPillager(targetPillager);
        }
    }

    static void RemoveDefeatedPillager(Pillager pillager)
    {
        pillagersCamp.Inhabitants.Remove(pillager);
    }



    static void ViewAllCharacters()
    {
        DisplayPaladinsInformation();
        DisplayPeasantsInformation();
        DisplayPillagersInformation();
    }

    static void DisplayPaladinsInformation()
    {
        Console.WriteLine("Paladins:");
        foreach (Paladins paladin in paladins)
        {
            Console.WriteLine($"Name: {paladin.Name}");
            Console.WriteLine($"Health: {paladin.Health}");
            Console.WriteLine($"Inventory Weapon: {paladin.EquippedWeapon.Name}");
            Console.WriteLine($"Location: {paladin.CurrentLocationObj.Name}");
            Console.WriteLine();
        }
    }

    static void DisplayPeasantsInformation()
    {
        Console.WriteLine("Peasants:");
        foreach (Peasant peasant in peasants)
        {
            Console.WriteLine($"Name: {peasant.Name}");
            Console.WriteLine($"Age: {peasant.Age}");
            Console.WriteLine($"Health: {peasant.Health}");
            Console.WriteLine($"Gold: {peasant.Money}");
            Console.WriteLine($"Inventory Items: {(peasant.Inventory.Any() ? string.Join(", ", peasant.Inventory.Select(ii => ii.Item.Name)) : "Peasant's Inventory is empty.")}");
            Console.WriteLine($"Trade Goods: {(peasant.TradeGoods != null ? string.Join(", ", peasant.TradeGoods.Select(g => g.Name)) : "Peasant Trade Goods are not defined.")}");
            Console.WriteLine();
        }
    }

    static void DisplayPillagersInformation()
    {
        Console.WriteLine("Pillagers:");
        foreach (Pillager pillager in pillagers)
        {
            Console.WriteLine($"Name: {pillager.Name}");
            Console.WriteLine($"Age: {pillager.Age}");
            Console.WriteLine($"Health: {pillager.Health}");
            Console.WriteLine($"Aggression: {pillager.AggressionLevel}");
            Console.WriteLine();
        }
    }


    static void CheckAndHandleDeceasedVillagers(Location location)
    {
        List<Peasant> deceasedPeasants = location.Inhabitants.OfType<Peasant>()
                                                          .Where(peasant => peasant.Health <= 0)
                                                          .ToList();

        foreach (Peasant deceasedPeasant in deceasedPeasants)
        {
            location.Inhabitants.Remove(deceasedPeasant);
        }
    }


    static void HandleCharacterInteractions()
    {
        Console.WriteLine("Random Character Interaction:");

        Peasant peasant1 = GetRandomLivingPeasant(peasants);
        Peasant peasant2 = GetRandomLivingPeasant(peasants);

        if (peasant1 != null && peasant2 != null)
        {
            // Generate a random interaction option
            int randomInteraction = RNG.GenerateRandomNumber(1, 4);

            Console.WriteLine($"{peasant1.Name} and {peasant2.Name}:");

            switch (randomInteraction)
            {
                case 1:
                    Console.WriteLine("traded items.");
                    TradeItems(peasant1, peasant2);
                    break;
                case 2:
                    Console.WriteLine("talked about the weather.");
                    break;
                case 3:
                    Console.WriteLine("played a game.");
                    break;
                case 4:
                    Console.WriteLine("had a meal together.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Two living peasants are needed for interaction.");
        }
    }

    static Peasant GetRandomLivingPeasant(List<Peasant> peasantList)
    {
        List<Peasant> livingPeasants = peasantList.Where(peasant => peasant.Health > 0).ToList();

        if (livingPeasants.Count > 0)
        {
            int randomIndex = RNG.GenerateRandomNumber(0, livingPeasants.Count - 1);
            return livingPeasants[randomIndex];
        }

        return null;
    }


    static void HandleRandomEvent(string message)
    {
        Console.WriteLine("Random Event: " + message);
    }

    static void TradeItems(Peasant peasant1, Peasant peasant2)
    {
        // Check if both peasants are eligible for trading and have items to trade
        if (peasant1 == null || peasant2 == null || peasant1.Inventory.Count == 0 || peasant2.Inventory.Count == 0)
        {
            Console.WriteLine("Trade failed: One or both of the peasants do not have items to trade.");
            return;
        }

        // Select random items from each peasant's inventory
        var randomItemFromPeasant1 = GetRandomItem(peasant1);
        var randomItemFromPeasant2 = GetRandomItem(peasant2);

        // Check if the selected items are different
        if (randomItemFromPeasant1 == randomItemFromPeasant2)
        {
            Console.WriteLine("Trade failed: Peasants couldn't agree on the terms.");
            return;
        }

        // Swap the items between the peasants
        peasant1.Inventory.Remove(new InventoryItem { Item = randomItemFromPeasant1 });
        peasant2.Inventory.Remove(new InventoryItem { Item = randomItemFromPeasant2 });

        peasant1.Inventory.Add(new InventoryItem { Item = randomItemFromPeasant2 });
        peasant2.Inventory.Add(new InventoryItem { Item = randomItemFromPeasant1 });

        Console.WriteLine($"{peasant1.Name} traded {randomItemFromPeasant1.Name} with {peasant2.Name} for {randomItemFromPeasant2.Name}.");
    }

    static IItem GetRandomItem(Peasant peasant)
    {
        if (peasant.Inventory.Count == 0)
        {
            return null; // Return null if no items are available for trade.
        }

        int randomIndex = RNG.GenerateRandomNumber(0, peasant.Inventory.Count - 1);
        return peasant.Inventory[randomIndex].Item;
    }


    static Weapon GetRandomSwordOrSpear()
    {
        int randomValue = RNG.GenerateRandomNumber(0, 1);

        return randomValue == 0 ? new Weapon("Sword", 30, new Price(15)) : new Weapon("Spear", 25, new Price(10));
    }
}
