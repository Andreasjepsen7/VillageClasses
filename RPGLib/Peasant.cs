using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGLib
{
    public class Peasant : Villager
    {
        public string Job { get; set; }
        public List<TradeGood> TradeGoods { get; set; }

        public Peasant(string job, List<TradeGood> tradeGoods, string name, int age, int money, Location currentLocation, int health)
           : base(name, age, new List<InventoryItem>(), money, currentLocation, health)
        {
            Job = job;
            TradeGoods = tradeGoods; // Assign the TradeGoods property
            Inventory = ItemGenerator.GenerateRandomItems(); // Generate random items for the peasant
        }


    }

}
