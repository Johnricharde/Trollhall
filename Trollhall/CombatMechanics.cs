﻿using NAudio.Codecs;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    internal class CombatMechanics
    {
        static Random rand = new Random();
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
                        ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 2 : 0);
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
                    // Player fails to run away
                    if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && rand.Next(0, 2) == 0)
                    {
                        int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Program.currentPlayer.health -= enemyDmg;
                        Program.Print($"As you attempt to flee the {enemyName}, it's strikes you from behind!");
                        Program.Print($"You lose {enemyDmg} health and are unable to escape!");
                        Console.ReadKey();
                    }
                    else
                    // Player runs away
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
                    // Player has no potions left
                    if (Program.currentPlayer.potions == 0)
                    {
                        int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                        if (enemyDmg < 0)
                            enemyDmg = 0;
                        Program.Print("You have no potions left!");
                        Program.Print($"The {enemyName} strikes at you! Dealing {enemyDmg} damage!");
                    }
                    else
                    // Player has potion
                    {
                        int potionValue = 5 +
                            ((Program.currentPlayer.currentClass == Player.PlayerClass.Cleric) ? +4 : 0);
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
                    // Player dies
                    WaveOutEvent playerDeath = new WaveOutEvent();
                    playerDeath.Init(new AudioFileReader("./audio/player-death.wav"));
                    playerDeath.Play();
                    Program.Print($"You have been slain by the {enemyName}.\nYou are dead...");
                    Console.ReadKey();
                    System.Environment.Exit(0);
                }
                Console.ReadKey();
            }
            // Player wins combat
            int coins = Program.currentPlayer.GetCoins();
            int experience = Program.currentPlayer.GetXP();

            WaveOutEvent enemyDeath = new WaveOutEvent();
            enemyDeath.Init(new AudioFileReader("./audio/enemy-death.wav"));
            enemyDeath.Play();

            Program.Print($"You defeat the {enemyName}!\nYou loot {coins} coins!\nYou recieve {experience} experience!");
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