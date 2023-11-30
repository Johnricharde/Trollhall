

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
            int _weaponPrice;
            int _armorPrice;
            int _potionPrice;
            int _difficultyPrice;

            var _encounter = new Encounters();
            var _program = new Program();  

            while (true)
            {
                _weaponPrice = 100 * player.weaponValue;
                _armorPrice = 100 * (player.armorValue + 1);
                _potionPrice = 20 + 10 * player.difficultyMod;
                _difficultyPrice = 300 + 100 * player.difficultyMod;

                ShowShop();

                string _input = Console.ReadLine().ToLower();

                if (_input == "w" || _input == "weapon")
                    TryBuy("weapon", _weaponPrice, player);
                else if (_input == "a" || _input == "armor")
                    TryBuy("armor", _armorPrice, player);
                else if (_input == "p" || _input == "potion")
                    TryBuy("potion", _potionPrice, player);
                else if (_input == "d" || _input == "difficulty")
                    TryBuy("difficulty", _difficultyPrice, player);
                else if (_input == "s" || _input == "save")
                    _program.Quit();
                else if (_input == "e" || _input == "exit")
                    _encounter.RandomEncounter(); 
            }
            void ShowShop()
            {
                Console.Clear();
                Console.WriteLine("===========================");
                Console.WriteLine("       VILLAGE SHOP        ");
                Console.WriteLine("===========================");
                Console.WriteLine($"        GOLD: £{player.coins}\n");
                Console.WriteLine($" [W]eapon:            £{_weaponPrice}");
                Console.WriteLine($" [A]rmor:             £{_armorPrice}");
                Console.WriteLine($" [P]otion:            £{_potionPrice}");
                Console.WriteLine($" [D]ifficulty:        £{_difficultyPrice}");
                Console.WriteLine("===========================");
                Console.WriteLine("       PLAYER STATS        ");
                Console.WriteLine("===========================");
                Console.WriteLine($" NAME: {player._name} the {player.currentClass}\n");
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
