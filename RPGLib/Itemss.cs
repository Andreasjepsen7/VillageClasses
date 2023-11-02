using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGLib
{
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

}
