using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    [Serializable]
    public class Player
    {
        private Random rand = new Random();

        public string name = "Bomli Bronzebottom";
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
            int staticHealAmount = 2 * difficultyMod + 5;
            int maxHealAmount = maxHealth - health;
            int healAmount = Math.Min(staticHealAmount, maxHealAmount);
            return healAmount;
        }
        public int GetHealth()
        {
            int upper = (2 * difficultyMod + 5);
            int lower = (difficultyMod + 2);
            return rand.Next(lower, upper);
        }
        public int GetPower()
        {
            int upper = (2 * difficultyMod + 2);
            int lower = (difficultyMod + 1);
            return rand.Next(lower, upper);
        }
        public int GetCoins()
        {
            int upper = (15 * difficultyMod + 50);
            int lower = (10 * difficultyMod + 10);
            return rand.Next(lower, upper);
        }

        public int GetXP()
        {
            int upper = (20 * difficultyMod + 50);
            int lower = (15 * difficultyMod + 10);
            return rand.Next(lower, upper);
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
        public void LevelUp(Program Game)
        {
            maxHealth += 5;
            while (CanLevelUp())
            {
                xp -= GetLevelUpValue();
                level++;
            }
            Program.PlaySoundEffect("level-up");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Game.Print(false, $"You gained a level!\nYou're level is now {level}.");
            Console.ResetColor();
        }

        public void playerDeath(string deathMessage, Program Game)
        {
            Program.PlaySoundEffect("player-death");
            Console.ForegroundColor = ConsoleColor.Red;
            Game.Print(true, $"{deathMessage}\nYOU ARE DEAD...", 50);
            Console.ResetColor();
            System.Environment.Exit(0);
        }
    }
}
