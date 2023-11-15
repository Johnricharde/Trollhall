using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    class Encounters
    {
        static Random rand = new Random();
        // Encounter Generic


        // Encounters
        public static void FirstEncounter()
        {
            Console.WriteLine("You draw your axe and charge at the troll.");
            Console.WriteLine("He roars...");
            Console.ReadKey();
            Combat(false, "Troll", 1, 5);

        }


        // Encounter Tools
        public static void Combat(bool random, string name, int power, int health)
        {
            string enemyName = "";
            int enemyPower = 0;
            int enemyHealth = 0;
            if (random)
            {

            }
            else
            {
                enemyName = name;
                enemyPower = power;
                enemyHealth = health;
            }
            while(enemyHealth > 0)
            {
                Console.Clear();
                Console.WriteLine($"ENEMY: {enemyName}");
                Console.WriteLine($"Power: {enemyPower} / Health: {enemyHealth}");
                Console.WriteLine("=======================");
                Console.WriteLine("| [A]ttack | [D]efend |");
                Console.WriteLine("| [R]un    | [H]eal   |");
                Console.WriteLine("=======================");
                Console.WriteLine($"PLAYER: {Program.currentPlayer.name}");
                Console.WriteLine($"Health: {Program.currentPlayer.health}");
                Console.WriteLine($"Potions: {Program.currentPlayer.potions}");
                string input = Console.ReadLine();
                if (input.ToLower() == "a" || input.ToLower() == "attack")
                {
                    // ATTACK
                    Console.WriteLine($"You attack the {enemyName}, it strikes back!");
                    int damage = enemyPower - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4);
                    Console.WriteLine($"You take {damage} damage and you deal {attack} damage");
                    Program.currentPlayer.health -= damage;
                    enemyHealth -= attack;
                }
                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                {
                    // DEFEND
                    Console.WriteLine($"As the {enemyName} prepares to strike, you hunker behind your shield!");
                    int damage = (enemyPower / 4) - Program.currentPlayer.armorValue;
                    if (damage < 0)
                        damage = 0;
                    int attack = rand.Next(0, Program.currentPlayer.weaponValue) / 2;
                    Console.WriteLine($"You take {damage} damage and you deal {attack} damage");
                    Program.currentPlayer.health -= damage;
                    enemyHealth -= attack;
                }
                else if (input.ToLower() == "r" || input.ToLower() == "run")
                {
                    // RUN
                    if (rand.Next(0, 2) == 0)
                    {
                        Console.WriteLine($"As you attempt to flee the {enemyName}, it's strikes you from behind!");
                        int damage = enemyPower - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine($"You lose {damage} health and are unable to escape!");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"You evade the {enemyName}'s attack and manage to escape!");
                        Console.ReadKey();
                        // Go to Store
                    }
                }
                else if (input.ToLower() == "h" || input.ToLower() == "heal")
                {
                    // HEAL
                    if (Program.currentPlayer.potions == 0)
                    {
                        Console.WriteLine("You have no potions left!");
                        int damage = enemyPower - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine($"The {enemyName} strikes at you! Dealing {damage} damage!");
                    }
                    else
                    {
                        int potionValue = 5;
                        Console.WriteLine($"You drink a potion! You recover {potionValue} health!");
                        Program.currentPlayer.health += potionValue;
                        int damage = enemyPower / 2 - Program.currentPlayer.armorValue;
                        if (damage < 0)
                            damage = 0;
                        Console.WriteLine($"The {enemyName} strikes! Dealing {damage} damage!");
                    }
                    Console.ReadKey();
                }
                Console.ReadKey();
            }
        }


    }
}
