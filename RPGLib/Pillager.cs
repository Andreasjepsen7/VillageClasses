using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGLib
{
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
}
