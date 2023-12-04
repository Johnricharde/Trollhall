

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
            var _program   = new Program();
            var _tools     = new Tools();

            while (true)
            {
                ShowShop();

                ChooseItem();
            }



            // SHOW SHOP UI ------------------------------------------------------------------------ SHOW SHOP UI //
            void ShowShop()
            {
                _weaponPrice = 100 * player.weaponValue;
                _armorPrice = 100 * (player.armorValue + 1);
                _potionPrice = 20 + 10 * player.difficultyMod;
                _difficultyPrice = 300 + 100 * player.difficultyMod;

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
                _tools.ExperienceBar("▓", ((decimal)player.xp / (decimal)player.GetLevelUpValue()), 25);
                Console.WriteLine($"|Lvl: {player.level}");
                Console.WriteLine("===========================");
                Console.WriteLine(" [E]xit shop");
                Console.WriteLine(" [S]ave and quit");
            }
            // CHOOSE ITEM -------------------------------------------------------------------------- CHOOSE ITEM //
            void ChooseItem()
            {
                string _input = Console.ReadLine().ToLower();

                if (_input == "w" || _input == "weapon")
                    TryToBuy("weapon", _weaponPrice, player);
                else if (_input == "a" || _input == "armor")
                    TryToBuy("armor", _armorPrice, player);
                else if (_input == "p" || _input == "potion")
                    TryToBuy("potion", _potionPrice, player);
                else if (_input == "d" || _input == "difficulty")
                    TryToBuy("difficulty", _difficultyPrice, player);
                else if (_input == "s" || _input == "save")
                    _program.Quit();
                else if (_input == "e" || _input == "exit")
                    _encounter.RandomEncounter();
            }
            // TRY TO BUY ---------------------------------------------------------------------------- TRY TO BUY //
            void TryToBuy(string item, int cost, Player player)
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
                    _tools.Print(true, "You don't have enough gold!");
                }
            }
        }

    }
}
