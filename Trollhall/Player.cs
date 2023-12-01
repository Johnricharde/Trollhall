

namespace Trollhall
{
    [Serializable]
    public class Player
    {
        private readonly Random  _rand    = new();
        private readonly Program _program = new();

        // PLAYER STATS ------------------------------------------------------------------------ PLAYER STATS //
        public string _name = "Bomli Bronzebottom";
        public int id;

        public int xp    = 0;
        public int level = 1;

        public int damage    = 1;
        public int health    = 10;
        public int maxHealth = 10;

        public int armorValue  = 0;
        public int weaponValue = 1;

        public int coins   = 0;
        public int potions = 5;

        public int difficultyMod = 0;

        // PLAYER CLASSES -------------------------------------------------------------------- PLAYER CLASSES //
        public enum PlayerClass {Warrior, Ranger, Cleric}
        public PlayerClass currentClass = PlayerClass.Warrior;

        // GET VALUES ---------------------------------------------------------------------------- GET VALUES //
        // Gets the amount of healing that player receives //
        public int GetHeal()
        {
            int _staticHealAmount = 2 * difficultyMod + 5;
            int _maxHealAmount = maxHealth - health;
            int _healAmount = Math.Min(_staticHealAmount, _maxHealAmount);
            return _healAmount;
        }
        // Gets the amount of coins that player receives //
        public int GetCoins()
        {
            int _upper = (15 * difficultyMod + 50);
            int _lower = (10 * difficultyMod + 10);
            return _rand.Next(_lower, _upper);
        }
        // Gets the health of an enemy //
        public int GetHealth()
        {
            int _upper = (2 * difficultyMod + 5);
            int _lower = (difficultyMod + 2);
            return _rand.Next(_lower, _upper);
        }
        // Gets the power of an enemy or attack towards player //
        public int GetPower()
        {
            int _upper = (2 * difficultyMod + 2);
            int _lower = (difficultyMod + 1);
            return _rand.Next(_lower, _upper);
        }

        // EXPERIENCE AND LEVEL -------------------------------------------------------- EXPERIENCE AND LEVEL //
        // Gets the amount of experience that the players receives //
        public int GetXP()
        {
            int _upper = (20 * difficultyMod + 50);
            int _lower = (15 * difficultyMod + 10);
            return _rand.Next(_lower, _upper);
        }
        // Gets the total amount of experience required to level up //
        public int GetLevelUpValue()
        {
            return 100 * level + 400;
        }
        // Checks if the player has enough experience to level up //
        public bool CanLevelUp()
        {
            if (xp >= GetLevelUpValue())
                return true;
            else
                return false;
        }
        // Increases the players level by one //
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

        // PLAYER DEATH ------------------------------------------------------------------------ PLAYER DEATH //
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
