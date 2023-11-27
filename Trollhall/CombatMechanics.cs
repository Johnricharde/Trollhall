using NAudio.Codecs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    internal class CombatMechanics
    {
        private static Random rand = new Random();
        public static void Combat(bool random, string name, int power, int health)
        {
            string enemyName = "";
            int enemyPower = 0;
            int enemyHealth = 0;
            if (random)
            {
                enemyName = Encounters.GetName();
                enemyPower = Program.currentPlayer.GetPower();
                enemyHealth = Program.currentPlayer.GetHealth();
            }
            else
            {
                enemyName = name;
                enemyPower = power;
                enemyHealth = health;
            }
            while (enemyHealth > 0)
            {
                // Combat UI //
                Console.Clear();
                Console.WriteLine($" ENEMY: {enemyName.ToUpper()}");
                Console.WriteLine($" Power: {enemyPower} / Health: {enemyHealth}\n");
                Console.WriteLine(" =======================");
                Console.WriteLine(" | [A]ttack | [D]efend |");
                Console.WriteLine(" | [R]un    | [H]eal   |");
                Console.WriteLine(" =======================");
                Console.Write("|");
                Program.ExperienceBar("▓", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 25);
                Console.WriteLine($"|Lvl: {Program.currentPlayer.level}\n");
                Console.WriteLine($" PLAYER:  {Program.currentPlayer.name.ToUpper()}");
                Console.Write(" Health:  ");
                if ((float)Program.currentPlayer.health / Program.currentPlayer.maxHealth <= 0.25)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if ((float)Program.currentPlayer.health / Program.currentPlayer.maxHealth <= 0.50)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{Program.currentPlayer.health}");
                Console.ResetColor();
                Console.WriteLine($"/{Program.currentPlayer.maxHealth}");
                if (Program.currentPlayer.potions <= 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Potions: {Program.currentPlayer.potions}");
                Console.ResetColor();
                string input = Console.ReadLine();

                    // ATTACK ------------------------------------------------------------------------------------ ATTACK //
                if (input.ToLower() == "a" || input.ToLower() == "attack")
                {
                    Program.PlaySoundEffect("attack");
                    int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    int playerDmg = rand.Next(0, Program.currentPlayer.weaponValue) + rand.Next(1, 4) +
                        ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 2 : 0);
                    enemyHealth -= playerDmg;
                    Program.currentPlayer.health -= enemyDmg;
                    Program.Print(false, $"You attack the {enemyName}, it strikes back!\nYou take {enemyDmg} damage and you deal {playerDmg} damage");
                }
                    // DEFEND ------------------------------------------------------------------------------------ DEFEND //
                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                {
                    Program.PlaySoundEffect("defend");
                    int enemyDmg = (enemyPower / 4) - Program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    int playerDmg = rand.Next(0, Program.currentPlayer.weaponValue) / 2;
                    enemyHealth -= playerDmg;
                    Program.currentPlayer.health -= enemyDmg;
                    Program.Print(false, $"As the {enemyName} prepares to strike, you hunker behind your shield!\nYou take {enemyDmg} damage and you deal {playerDmg} damage");
                }
                    // RUN ------------------------------------------------------------------------------------------ RUN //
                else if (input.ToLower() == "r" || input.ToLower() == "run")
                {
                    // Player fails to run away
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 2) == 0)
                    {
                        int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Program.currentPlayer.health -= enemyDmg;
                        Program.Print(true, $"As you attempt to flee the {enemyName}, it's strikes you from behind!\nYou lose {enemyDmg} health and are unable to escape!");
                    }
                    // Player runs away
                    else
                    {
                        Program.PlaySoundEffect("run-away");
                        Program.Print(true, $"You evade the {enemyName}'s attack and manage to escape!");
                        Shop.LoadShop(Program.currentPlayer);
                    }
                }
                    // HEAL ---------------------------------------------------------------------------------------- HEAL //
                else if (input.ToLower() == "h" || input.ToLower() == "heal")
                {
                    // Player has no potions left //
                    if (Program.currentPlayer.potions == 0)
                    {
                        int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Program.Print(false, "You have no potions left!\nThe {enemyName} strikes at you! Dealing {enemyDmg} damage!");
                    }
                    // Player has potion //
                    else
                    {
                        int potionValue = Program.currentPlayer.GetHeal();
                        potionValue += (Program.currentPlayer.currentClass == Player.PlayerClass.Cleric ? potionValue : 0);
                        int enemyDmg = enemyPower / 2 - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Program.currentPlayer.health += potionValue;
                        Program.currentPlayer.potions--;
                        Program.PlaySoundEffect("drink");
                        Program.Print(false, $"You drink a potion! You recover {potionValue} health!\nThe {enemyName} strikes! Dealing {enemyDmg} damage!");
                    }
                }

                    // Player dies //
                if (Program.currentPlayer.health <= 0)
                {
                    Program.currentPlayer.playerDeath($"\nYou were slain by {enemyName}");
                }
                Console.ReadKey();
            }
            // Player wins combat //
            int coins = Program.currentPlayer.GetCoins();
            int experience = Program.currentPlayer.GetXP();

            Program.PlaySoundEffect("enemy-death");
            Program.Print(false, $"You defeat the {enemyName}!\nYou loot {coins} coins!\nYou recieve {experience} experience!");
            Program.currentPlayer.coins += coins;
            Program.currentPlayer.xp += experience;
            if (Program.currentPlayer.CanLevelUp())
            {
                Program.currentPlayer.LevelUp();
            }
            Console.ReadKey();
            Encounters.RandomEncounter();

        }

    }
}
