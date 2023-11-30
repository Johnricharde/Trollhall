

namespace Trollhall
{
    [Serializable]
    public class Player
    {
        private Random _rand = new Random();
        private Program _program = new Program();

        public string _name = "Bomli Bronzebottom";
        public int id;
        public int coins = 9999;
        public int level = 1;
        public int xp = 0;
        public int health = 10;
        public int maxHealth = 10;
        public int damage = 1;
        public int armorValue = 0;
        public int potions = 5;
        public int weaponValue = 1;
        public int difficultyMod = 0;

        public enum PlayerClass {Warrior, Ranger, Cleric}
        public PlayerClass currentClass = PlayerClass.Warrior;

        public int GetHeal()
        {
            int _staticHealAmount = 2 * difficultyMod + 5;
            int _maxHealAmount = maxHealth - health;
            int _healAmount = Math.Min(_staticHealAmount, _maxHealAmount);
            return _healAmount;
        }
        public int GetHealth()
        {
            int _upper = (2 * difficultyMod + 5);
            int _lower = (difficultyMod + 2);
            return _rand.Next(_lower, _upper);
        }
        public int GetPower()
        {
            int _upper = (2 * difficultyMod + 2);
            int _lower = (difficultyMod + 1);
            return _rand.Next(_lower, _upper);
        }
        public int GetCoins()
        {
            int _upper = (15 * difficultyMod + 50);
            int _lower = (10 * difficultyMod + 10);
            return _rand.Next(_lower, _upper);
        }

        public int GetXP()
        {
            int _upper = (20 * difficultyMod + 50);
            int _lower = (15 * difficultyMod + 10);
            return _rand.Next(_lower, _upper);
        }
        public int GetLevelUpValue()
        {
            return 100 * level + 400;
        }
        public bool CanLevelUp()
        {
            if (xp >= GetLevelUpValue())
                return true;
            else
                return false;
        }
        public void LevelUp()
        {
            maxHealth += 5;
            while (CanLevelUp())
            {
                xp -= GetLevelUpValue();
                level++;
            }
            _program.PlaySoundEffect("level-up");
            Console.ForegroundColor = ConsoleColor.Yellow;
            _program.Print(false, $"You gained a level!\nYou're level is now {level}.");
            Console.ResetColor();
        }

        public void playerDeath(string deathMessage)
        {
            _program.PlaySoundEffect("player-death");
            Console.ForegroundColor = ConsoleColor.Red;
            _program.Print(true, $"{deathMessage}\nYOU ARE DEAD...", 50);
            Console.ResetColor();
            System.Environment.Exit(0);
        }
    }
}
