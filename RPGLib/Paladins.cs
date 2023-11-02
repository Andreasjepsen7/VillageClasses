using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RPGLib
{
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

}
