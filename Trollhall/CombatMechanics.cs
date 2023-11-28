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

        public static void Combat(Program program, bool random, string name, int power, int health)
        {
            string enemyName = "";
            int enemyPower = 0;
            int enemyHealth = 0;
            if (random)
            {
                enemyName = Encounters.GetRandomName();
                enemyPower = program.currentPlayer.GetPower();
                enemyHealth = program.currentPlayer.GetHealth();
            }
            else
            {
                enemyName = name;
                enemyPower = power;
                enemyHealth = health;
            }
            while (enemyHealth > 0)
            {
                    // COMBAT UI ------------------------------------------------------------------------------ COMBAT UI //
                Console.Clear();
                Console.WriteLine($" ENEMY: {enemyName.ToUpper()}");
                Console.WriteLine($" Power: {enemyPower} / Health: {enemyHealth}\n");
                Console.WriteLine(" =======================");
                Console.WriteLine(" | [A]ttack | [D]efend |");
                Console.WriteLine(" | [R]un    | [H]eal   |");
                Console.WriteLine(" =======================");
                Console.Write("|");
                program.ExperienceBar("▓", ((decimal)program.currentPlayer.xp / (decimal)program.currentPlayer.GetLevelUpValue()), 25);
                Console.WriteLine($"|Lvl: {program.currentPlayer.level}\n");
                Console.WriteLine($" PLAYER:  {program.currentPlayer.name.ToUpper()}");
                Console.Write(" Health:  ");
                if ((float)program.currentPlayer.health / program.currentPlayer.maxHealth <= 0.25)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if ((float)program.currentPlayer.health / program.currentPlayer.maxHealth <= 0.50)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{program.currentPlayer.health}");
                Console.ResetColor();
                Console.WriteLine($"/{program.currentPlayer.maxHealth}");
                if (program.currentPlayer.potions <= 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Potions: {program.currentPlayer.potions}");
                Console.ResetColor();
                string input = Console.ReadLine();

                    // ATTACK ------------------------------------------------------------------------------------ ATTACK //
                if (input.ToLower() == "a" || input.ToLower() == "attack")
                {
                    program.PlaySoundEffect("attack");
                    int enemyDmg = enemyPower - program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    int playerDmg = rand.Next(0, program.currentPlayer.weaponValue) + rand.Next(1, 4) +
                        ((program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 2 : 0);
                    enemyHealth -= playerDmg;
                    program.currentPlayer.health -= enemyDmg;
                    program.Print(false, $"You attack the {enemyName}, it strikes back!\nYou take {enemyDmg} damage and you deal {playerDmg} damage");
                }
                    // DEFEND ------------------------------------------------------------------------------------ DEFEND //
                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                {
                    program.PlaySoundEffect("defend");
                    int enemyDmg = (enemyPower / 4) - program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    int playerDmg = rand.Next(0, program.currentPlayer.weaponValue) / 2;
                    enemyHealth -= playerDmg;
                    program.currentPlayer.health -= enemyDmg;
                    program.Print(false, $"As the {enemyName} prepares to strike, you hunker behind your shield!\nYou take {enemyDmg} damage and you deal {playerDmg} damage");
                }
                    // RUN ------------------------------------------------------------------------------------------ RUN //
                else if (input.ToLower() == "r" || input.ToLower() == "run")
                {
                    // Player fails to run away
                    if (program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 2) == 0)
                    {
                        int enemyDmg = enemyPower - program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        program.currentPlayer.health -= enemyDmg;
                        program.Print(true, $"As you attempt to flee the {enemyName}, it's strikes you from behind!\nYou lose {enemyDmg} health and are unable to escape!");
                    }
                    // Player runs away
                    else
                    {
                        program.PlaySoundEffect("run-away");
                        program.Print(true, $"You evade the {enemyName}'s attack and manage to escape!");
                        Shop.LoadShop(program, program.currentPlayer);
                    }
                }
                    // HEAL ---------------------------------------------------------------------------------------- HEAL //
                else if (input.ToLower() == "h" || input.ToLower() == "heal")
                {
                    // Player has no potions left //
                    if (program.currentPlayer.potions == 0)
                    {
                        int enemyDmg = enemyPower - program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        program.Print(false, "You have no potions left!\nThe {enemyName} strikes at you! Dealing {enemyDmg} damage!");
                    }
                    // Player has potion //
                    else
                    {
                        int potionValue = program.currentPlayer.GetHeal();
                        potionValue += (program.currentPlayer.currentClass == Player.PlayerClass.Cleric ? potionValue : 0);
                        int enemyDmg = enemyPower / 2 - program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        program.currentPlayer.health += potionValue;
                        program.currentPlayer.potions--;
                        program.PlaySoundEffect("drink");
                        program.Print(false, $"You drink a potion! You recover {potionValue} health!\nThe {enemyName} strikes! Dealing {enemyDmg} damage!");
                    }
                }

                    // Player dies //
                if (program.currentPlayer.health <= 0)
                {
                    program.currentPlayer.playerDeath($"\nYou were slain by {enemyName}");
                }
                Console.ReadKey();
            }
            // Player wins combat //
            int coins = program.currentPlayer.GetCoins();
            int experience = program.currentPlayer.GetXP();

            program.PlaySoundEffect("enemy-death");
            program.Print(false, $"You defeat the {enemyName}!\nYou loot {coins} coins!\nYou recieve {experience} experience!");
            program.currentPlayer.coins += coins;
            program.currentPlayer.xp += experience;
            if (program.currentPlayer.CanLevelUp())
            {
                program.currentPlayer.LevelUp();
            }
            Console.ReadKey();
            Encounters.RandomEncounter(program);

        }

    }
}
