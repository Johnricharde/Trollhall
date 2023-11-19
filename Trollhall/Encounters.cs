using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
            Program.Print("You draw your axe and charge at the troll.");
            Console.ReadKey();
            Combat(false, "Troll", 1, 5);
        }
        public static void TrollWizardEncounter()
        { 
            Console.Clear();
            Program.Print("You encounter a menacing troll shaman!");
            Console.ReadKey();
            Combat(false, "Troll Shaman", 4, 3);
        }
        public static void basicFightEncounter()
        {
            Console.Clear();
            Program.Print($"You encounter an enemy!");
            Console.ReadKey();
            Combat(true, "", 0, 0);
        }


        // ENCOUNTER TOOLS ------------------------------------------------------------------------------ ENCOUNTER TOOLS //
        public static void RandomEncounter()
        {
            switch (rand.Next(0, 2))
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
                    WaveOutEvent attack = new WaveOutEvent();
                    attack.Init(new AudioFileReader("./audio/attack.wav"));
                    attack.Play();
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    int playerDmg = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4) +
                        ((Program.currentPlayer.currentClass == Player.PlayerClass.Soldier) ? 2 : 0);
                    enemyHealth -= playerDmg;
                    Program.currentPlayer.health -= enemyDmg;
                    Program.Print($"You attack the {enemyName}, it strikes back!");
                    Program.Print($"You take {enemyDmg} damage and you deal {playerDmg} damage");
                }
                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                {
                    // DEFEND ------------------------------------------------------------------------------------ DEFEND //
                    WaveOutEvent defend = new WaveOutEvent();
                    defend.Init(new AudioFileReader("./audio/defend.wav"));
                    defend.Play();
                    Program.Print($"As the {enemyName} prepares to strike, you hunker behind your shield!");
                    int enemyDmg = (enemyPower / 4) - Program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    int playerDmg = rand.Next(0, Program.currentPlayer.weaponValue) / 2;
                    enemyHealth -= playerDmg;
                    Program.currentPlayer.health -= enemyDmg;
                    Program.Print($"You take {enemyDmg} damage and you deal {playerDmg} damage");
                }
                else if (input.ToLower() == "r" || input.ToLower() == "run")
                {
                    // RUN ------------------------------------------------------------------------------------------ RUN //
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Hunter && rand.Next(0, 2) == 0)
                    {
                        int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Program.Print($"As you attempt to flee the {enemyName}, it's strikes you from behind!");
                        Program.Print($"You lose {enemyDmg} health and are unable to escape!");
                        Console.ReadKey();
                    }
                    else
                    {
                        WaveOutEvent runAway = new WaveOutEvent();
                        runAway.Init(new AudioFileReader("./audio/run-away.wav"));
                        runAway.Play();
                        Program.Print($"You evade the {enemyName}'s attack and manage to escape!");
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
                        Program.Print("You have no potions left!");
                        Program.Print($"The {enemyName} strikes at you! Dealing {enemyDmg} damage!");
                    }
                    else
                    {
                        int potionValue = 5 +
                            ((Program.currentPlayer.currentClass == Player.PlayerClass.Cleric)? + 4 : 0);
                        int enemyDmg = enemyPower / 2 - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Program.currentPlayer.health += potionValue;
                        Program.currentPlayer.potions--;
                        WaveOutEvent drink = new WaveOutEvent();
                        drink.Init(new AudioFileReader("./audio/drink.wav"));
                        drink.Play();
                        Program.Print($"You drink a potion! You recover {potionValue} health!");
                        Program.Print($"The {enemyName} strikes! Dealing {enemyDmg} damage!");
                    }
                }
                if (Program.currentPlayer.health <= 0)
                {
                    // Death Code
                    Program.Print($"You have been slain by the {enemyName}.\nYou are dead...");
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.ReadKey();
            }
            int coins = Program.currentPlayer.GetCoins();
            int experience = Program.currentPlayer.GetXP();
            Program.Print($"You defeat the {enemyName}!\nYou loot {coins} coins!\nYou recieve {experience} experience!");
            Program.currentPlayer.coins += coins;
            Program.currentPlayer.xp += experience;
            if ( Program.currentPlayer.CanLevelUp() )
            {
                Program.currentPlayer.LevelUp();
            }
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
