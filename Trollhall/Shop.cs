﻿using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    public class Shop
    {

        public static void LoadShop(Player player, Program Game)
        {
            Shop shop = new Shop();
            RunShop(player, shop, Game);
        }
        private static void RunShop(Player player, Shop shop, Program Game)
        {
            int weaponPrice;
            int armorPrice;
            int potionPrice;
            int difficultyPrice;

            while (true)
            {
                weaponPrice = 100 * player.weaponValue;
                armorPrice = 100 * (player.armorValue + 1);
                potionPrice = 20 + 10 * player.difficultyMod;
                difficultyPrice = 300 + 100 * player.difficultyMod;

                Console.Clear();
                Console.WriteLine("===========================");
                Console.WriteLine("       VILLAGE SHOP        ");
                Console.WriteLine("===========================");
                Console.WriteLine($"        GOLD: £{player.coins}\n");
                Console.WriteLine($" [W]eapon:            £{weaponPrice}");
                Console.WriteLine($" [A]rmor:             £{armorPrice}");
                Console.WriteLine($" [P]otion:            £{potionPrice}");
                Console.WriteLine($" [D]ifficulty:        £{difficultyPrice}");
                Console.WriteLine("===========================");
                Console.WriteLine("       PLAYER STATS        ");
                Console.WriteLine("===========================");
                Console.WriteLine($" NAME: {player.name} the {player.currentClass}\n");
                Console.WriteLine($" Health:           {player.health}/{player.maxHealth}");
                Console.WriteLine($" Potions:          {player.potions}\n");
                Console.WriteLine($" Weapon        Mod:  + {player.weaponValue - 1}");
                Console.WriteLine($" Armor         Mod:  + {player.armorValue}");
                Console.WriteLine($" Difficulty    Mod:  + {player.difficultyMod}");
                Console.Write("|");
                Program.ExperienceBar("▓", ((decimal)player.xp / (decimal)player.GetLevelUpValue()), 25);
                Console.WriteLine($"|Lvl: {player.level}");
                Console.WriteLine("===========================");
                Console.WriteLine(" [E]xit shop");
                Console.WriteLine(" [S]ave and quit");

                string input = Console.ReadLine().ToLower();

                if (input == "w" || input == "weapon")
                    shop.TryBuy("weapon", weaponPrice, player, Game);
                else if (input == "a" || input == "armor")
                    shop.TryBuy("armor", armorPrice, player, Game);
                else if (input == "p" || input == "potion")
                    shop.TryBuy("potion", potionPrice, player, Game);
                else if (input == "d" || input == "difficulty")
                    shop.TryBuy("difficulty", difficultyPrice, player, Game);
                else if (input == "s" || input == "save")
                    Program.Quit();
                else if (input == "e" || input == "exit")
                    Encounters.RandomEncounter(Game); 
            }
        }
        private void TryBuy(string item, int cost, Player player, Program Game)
        {
            if(player.coins >= cost)
            {
                if (item == "weapon")
                    player.weaponValue++;
                else if (item == "armor")
                    player.armorValue++;
                else if (item == "potion")
                    player.potions++;
                else if (item == "difficulty")
                    player.difficultyMod++;
                player.coins -= cost;
            }
            else
            {
                Game.Print(true, "You don't have enough gold!");
            }
        }

    }
}
