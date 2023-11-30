

namespace Trollhall
{
    public class Shop
    {
        public void LoadShop(Player player)
        {
            var _shop = new Shop();
            _shop.RunShop(player);
        }
        private void RunShop(Player player)
        {
            int weaponPrice;
            int armorPrice;
            int potionPrice;
            int difficultyPrice;

            var _encounter = new Encounters();
            var _program = new Program();  

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
                Console.WriteLine($" [W]eapon:             {weaponPrice}");
                Console.WriteLine($" [A]rmor:              {armorPrice}");
                Console.WriteLine($" [P]otion:             {potionPrice}");
                Console.WriteLine($" [D]ifficulty:         {difficultyPrice}");
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
                _program.ExperienceBar("▓", ((decimal)player.xp / (decimal)player.GetLevelUpValue()), 25);
                Console.WriteLine($"|Lvl: {player.level}");
                Console.WriteLine("===========================");
                Console.WriteLine(" [E]xit shop");
                Console.WriteLine(" [S]ave and quit");

                string input = Console.ReadLine().ToLower();

                if (input == "w" || input == "weapon")
                    TryBuy("weapon", weaponPrice, player);
                else if (input == "a" || input == "armor")
                    TryBuy("armor", armorPrice, player);
                else if (input == "p" || input == "potion")
                    TryBuy("potion", potionPrice, player);
                else if (input == "d" || input == "difficulty")
                    TryBuy("difficulty", difficultyPrice, player);
                else if (input == "s" || input == "save")
                    _program.Quit();
                else if (input == "e" || input == "exit")
                    _encounter.RandomEncounter(); 
            }
            void TryBuy(string item, int cost, Player player)
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
                    _program.Print(true, "You don't have enough gold!");
                }
            }
        }

    }
}
