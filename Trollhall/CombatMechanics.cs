

namespace Trollhall
{
    internal class CombatMechanics
    {
        private readonly Random     _rand      = new();
        private readonly Encounters _encounter = new();
        private readonly Program    _program   = new();
        public Tools _tools = new();

        public void Combat(bool random, string name, int power, int health)
        {
            string enemyName;
            int enemyPower;
            int enemyHealth;
            if (random)
            {
                enemyName   = _encounter.GetRandomName();
                enemyPower  = Program.currentPlayer.GetPower();
                enemyHealth = Program.currentPlayer.GetHealth();
            }
            else
            {
                enemyName   = name;
                enemyPower  = power;
                enemyHealth = health;
            }
            while (enemyHealth > 0)
            {
                ShowCombatUI();
                ChooseAction();
                PlayerLosesCombat();
            }
            PlayerWinsCombat();



            // COMBAT UI ------------------------------------------------------------------------------ COMBAT UI //
            void ShowCombatUI()
            {
                // Enemy Stats //
                Console.Clear();
                Console.WriteLine($" ENEMY: {enemyName.ToUpper()}");
                Console.WriteLine($" Power: {enemyPower} / Health: {enemyHealth}\n");

                Console.WriteLine(" =======================");
                Console.WriteLine(" | [A]ttack | [D]efend |");
                Console.WriteLine(" | [R]un    | [H]eal   |");
                Console.WriteLine(" =======================");
                Console.Write("|");
                _tools.ExperienceBar("▓", ((decimal)Program.currentPlayer.xp / (decimal)Program.currentPlayer.GetLevelUpValue()), 25);
                Console.WriteLine($"|Lvl: {Program.currentPlayer.level}\n");
                Console.WriteLine($" PLAYER:  {Program.currentPlayer._name.ToUpper()}");

                // Health //
                Console.Write(" Health:  ");
                if ((float)Program.currentPlayer.health / Program.currentPlayer.maxHealth <= 0.25)
                    Console.ForegroundColor = ConsoleColor.Red;
                else if ((float)Program.currentPlayer.health / Program.currentPlayer.maxHealth <= 0.50)
                    Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"{Program.currentPlayer.health}");
                Console.ResetColor();
                Console.WriteLine($"/{Program.currentPlayer.maxHealth}");

                // Potions //
                if (Program.currentPlayer.potions <= 0)
                    Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($" Potions: {Program.currentPlayer.potions}");
                Console.ResetColor();
            }
            // CHOOSE ACTION ---------------------------------------------------------------------- CHOOSE ACTION //
            void ChooseAction()
            {
                string input = Console.ReadLine();

                if (input.ToLower() == "a" || input.ToLower() == "attack")
                    Attack();

                else if (input.ToLower() == "d" || input.ToLower() == "defend")
                    Defend();

                else if (input.ToLower() == "r" || input.ToLower() == "run")
                    Run();

                else if (input.ToLower() == "h" || input.ToLower() == "heal")
                    Heal();
            }
            // ATTACK ------------------------------------------------------------------------------------ ATTACK //
            void Attack()
            {
                _tools.PlaySoundEffect("attack");
                int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                if (enemyDmg < 0)
                    enemyDmg = 0;
                int playerDmg = _rand.Next(0, Program.currentPlayer.weaponValue) + _rand.Next(1, 4) +
                    ((Program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 2 : 0);
                enemyHealth -= playerDmg;
                Program.currentPlayer.health -= enemyDmg;
                _tools.Print(false, $"You attack the {enemyName}, it strikes back!\nYou take {enemyDmg} damage and you deal {playerDmg} damage");
            }
            // DEFEND ------------------------------------------------------------------------------------ DEFEND //
            void Defend()
            {
                _tools.PlaySoundEffect("defend");
                int enemyDmg = (enemyPower / 4) - Program.currentPlayer.armorValue;
                if (enemyDmg < 0)
                    enemyDmg = 0;
                int playerDmg = _rand.Next(0, Program.currentPlayer.weaponValue) / 2;
                enemyHealth -= playerDmg;
                Program.currentPlayer.health -= enemyDmg;
                _tools.Print(false, $"As the {enemyName} prepares to strike, you hunker behind your shield!\nYou take {enemyDmg} damage and you deal {playerDmg} damage");
            }
            // RUN ------------------------------------------------------------------------------------------ RUN //
            void Run()
            {
                // Player fails to run away
                if (Program.currentPlayer.currentClass != Player.PlayerClass.Ranger && _rand.Next(0, 2) == 0)
                {
                    int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    Program.currentPlayer.health -= enemyDmg;
                    _tools.Print(true, $"As you attempt to flee the {enemyName}, it's strikes you from behind!\nYou lose {enemyDmg} health and are unable to escape!");
                }
                // Player runs away
                else
                {
                    var _shop = new Shop();
                    _tools.PlaySoundEffect("run-away");
                    _tools.Print(true, $"You evade the {enemyName}'s attack and manage to escape!");
                    _shop.LoadShop(Program.currentPlayer);
                }
            }
            // HEAL ---------------------------------------------------------------------------------------- HEAL //
            void Heal()
            {
                // Player has no potions left //
                if (Program.currentPlayer.potions == 0)
                {
                    int enemyDmg = enemyPower - Program.currentPlayer.armorValue;
                    if (enemyDmg < 0)
                        enemyDmg = 0;
                    _tools.Print(false, "You have no potions left!\nThe {enemyName} strikes at you! Dealing {enemyDmg} damage!");
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
                    _tools.PlaySoundEffect("drink");
                    _tools.Print(false, $"You drink a potion! You recover {potionValue} health!\nThe {enemyName} strikes! Dealing {enemyDmg} damage!");
                }
            }
            // PLAYER LOSES COMBAT ------------------------------------------------------------ PLAYER LOSES COMBAT //
            void PlayerLosesCombat()
            {
                if (Program.currentPlayer.health <= 0)
                {
                    Program.currentPlayer.playerDeath($"\nYou were slain by {enemyName}");
                }
                Console.ReadKey();
            }
            // PLAYER WINS COMBAT ------------------------------------------------------------ PLAYER WINS COMBAT //
            void PlayerWinsCombat()
            {
                int coins = Program.currentPlayer.GetCoins();
                int experience = Program.currentPlayer.GetXP();

                _tools.PlaySoundEffect("enemy-death");
                _tools.Print(false, $"You defeat the {enemyName}!\nYou loot {coins} coins!\nYou recieve {experience} experience!");
                Program.currentPlayer.coins += coins;
                Program.currentPlayer.xp += experience;
                if (Program.currentPlayer.CanLevelUp())
                {
                    Program.currentPlayer.LevelUp();
                }
                Console.ReadKey();
                _encounter.RandomEncounter();
            }
        }

    }
}
