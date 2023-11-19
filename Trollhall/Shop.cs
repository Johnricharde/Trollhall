using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    public class Shop
    {

        public static void LoadShop(Player player)
        {
            RunShop(player);
        }

        public static void RunShop(Player player)
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
                Console.WriteLine("           SHOP           ");
                Console.WriteLine("==========================");
                Console.WriteLine($"GOLD:             {player.coins}\n");
                Console.WriteLine($"[W]eapon:         {weaponPrice}");
                Console.WriteLine($"[A]rmor:          {armorPrice}");
                Console.WriteLine($"[P]otion:         {potionPrice}");
                Console.WriteLine($"[D]ifficulty:     {difficultyPrice}");
                Console.WriteLine("==========================");
                Console.WriteLine( "      PLAYER STATS       ");
                Console.WriteLine("==========================");
                Console.WriteLine($"PLAYER:           {player.name}\n");
                Console.WriteLine($"Health:           {player.health}");
                Console.WriteLine($"Potions:          {player.potions}\n");
                Console.WriteLine($"Weapon Mod:     + {player.weaponValue - 1}");
                Console.WriteLine($"Armor Mod:      + {player.armorValue}");
                Console.WriteLine($"Difficulty Mod: + {player.difficultyMod}");
                Console.WriteLine("==========================");
                Console.WriteLine("[E]xit shop");
                Console.WriteLine("[Q]uit game");
                // Wait for input
                string input = Console.ReadLine().ToLower();

                if (input == "w" || input == "weapon")
                    TryBuy("weapon", weaponPrice, player);
                else if (input == "a" || input == "armor")
                    TryBuy("armor", armorPrice, player);
                else if (input == "p" || input == "potion")
                    TryBuy("potion", potionPrice, player);
                else if (input == "d" || input == "difficulty")
                    TryBuy("difficulty", difficultyPrice, player);
                else if (input == "q" || input == "quit")
                    Program.Quit();
                else if (input == "e" || input == "exit")
                    break; 
            }
        }
        static void TryBuy(string item, int cost, Player player)
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
                Program.Print("You don't have enough gold!");
                Console.ReadKey();
            }
        }
    }
}
