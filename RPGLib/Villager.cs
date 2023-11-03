using System.ComponentModel;

namespace RPGLib
{
    public class Villager
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public List<InventoryItem> Inventory { get; set; }
        public int Money { get; set; }
        public int Health { get; set; }
        public Location CurrentLocationObj { get; set; }

        public override string ToString()
        {
            return Name;
        }

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
    }
}