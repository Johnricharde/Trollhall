using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    public class Encounters
    {
        static Random rand = new Random();
        // ENCOUNTER GENERIC -------------------------------------------------------------------------- ENCOUNTER GENERIC //


        // ENCOUNTERS ---------------------------------------------------------------------------------------- ENCOUNTERS //
        public static void FirstEncounter()
        {
            Console.WriteLine("You draw your axe and charge at the troll.");
            Console.ReadKey();
            Combat(false, "Troll", 1, 5);
        }
        public static void TrollWizardEncounter()
        { 
            Console.Clear();
            Console.WriteLine("You encounter a menacing troll shaman!");
            Console.ReadKey();
            Combat(false, "Troll Shaman", 4, 3);
        }
        public static void basicFightEncounter()
        {
            Console.Clear();
            Console.WriteLine($"You encounter an enemy!");
            Console.ReadKey();
            Combat(true, "", 0, 0);
        }


        // ENCOUNTER TOOLS ------------------------------------------------------------------------------ ENCOUNTER TOOLS //
        public static void RandomEncounter()
        {
            switch(rand.Next(0, 2))
            {
                case 0:
                    basicFightEncounter();
                    break;
                case 1:
                    TrollWizardEncounter();
                    break;
            }
        }
        public static void Combat(bool random, string name, int power, int health)
        {
            string enemyName = "";
            int enemyPower = 0;
            int enemyHealth = 0;
            if (random)
            {
                enemyName = GetName();
                enemyPower = Program.currentPlayer.GetPower();
                enemyHealth = Program.currentPlayer.GetHealth();
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
                Console.WriteLine($"Power: {enemyPower} / Health: {enemyHealth}\n");
                Console.WriteLine("=======================");
                Console.WriteLine("| [A]ttack | [D]efend |");
                Console.WriteLine("| [R]un    | [H]eal   |");
                Console.WriteLine("=======================\n");
                Console.WriteLine($"PLAYER:  {Program.currentPlayer.name}");
                Console.WriteLine($"Health:  {Program.currentPlayer.health}");
                Console.WriteLine($"Potions: {Program.currentPlayer.potions}\n");
                string input = Console.ReadLine();
                if (input.ToLower() == "a" || input.ToLower() == "attack")
                {
                    // ATTACK ------------------------------------------------------------------------------------ ATTACK //
                    int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    int playerDmg = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4);
                    enemyHealth -= playerDmg;
                    Program.currentPlayer.health -= enemyDmg;
                    Console.WriteLine($"You attack the {enemyName}, it strikes back!");
                    Console.WriteLine($"You take {enemyDmg} damage and you deal {playerDmg} damage");
                }
                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                {
                    // DEFEND ------------------------------------------------------------------------------------ DEFEND //
                    Console.WriteLine($"As the {enemyName} prepares to strike, you hunker behind your shield!");
                    int enemyDmg = (enemyPower / 4) - Program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    int playerDmg = rand.Next(0, Program.currentPlayer.weaponValue) / 2;
                    enemyHealth -= playerDmg;
                    Program.currentPlayer.health -= enemyDmg;
                    Console.WriteLine($"You take {enemyDmg} damage and you deal {playerDmg} damage");
                }
                else if (input.ToLower() == "r" || input.ToLower() == "run")
                {
                    // RUN ------------------------------------------------------------------------------------------ RUN //
                    if (rand.Next(0, 2) == 0)
                    {
                        int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Console.WriteLine($"As you attempt to flee the {enemyName}, it's strikes you from behind!");
                        Console.WriteLine($"You lose {enemyDmg} health and are unable to escape!");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine($"You evade the {enemyName}'s attack and manage to escape!");
                        Console.ReadKey();
                        Shop.LoadShop(Program.currentPlayer);
                    }
                }
                else if (input.ToLower() == "h" || input.ToLower() == "heal")
                {
                    // HEAL ---------------------------------------------------------------------------------------- HEAL //
                    if (Program.currentPlayer.potions == 0)
                    {
                        int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Console.WriteLine("You have no potions left!");
                        Console.WriteLine($"The {enemyName} strikes at you! Dealing {enemyDmg} damage!");
                    }
                    else
                    {
                        int potionValue = 5;
                        int enemyDmg = enemyPower / 2 - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Program.currentPlayer.health += potionValue;
                        Program.currentPlayer.potions--;
                        Console.WriteLine($"You drink a potion! You recover {potionValue} health!");
                        Console.WriteLine($"The {enemyName} strikes! Dealing {enemyDmg} damage!");
                    }
                }
                if (Program.currentPlayer.health <= 0)
                {
                    // Death Code
                    Console.WriteLine($"You have been slain by the {enemyName}.\nYou are dead...");
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.ReadKey();
            }
            int coins = Program.currentPlayer.GetCoins();
            Console.WriteLine($"You defeat the {enemyName}! You loot {coins} coins!");
            Program.currentPlayer.coins += coins;
            Console.ReadKey();
        }
        public static string GetName()
        {
            switch (rand.Next(0, 4))
            {
                case 0:
                    return "Rock Spider";
                case 1:
                    return "Firebeetle";
                case 2:
                    return "Giant Bat";
                case 3:
                    return "Troll";
            }
            return "Beast";
        }
    }
}
