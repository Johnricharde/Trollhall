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
        Random rand = new Random();

        public string name = "Biff";
        public int id;
        public int coins = 9999;
        public int level = 1;
        public int xp = 0;
        public int health = 10;
        public int damage = 1;
        public int armorValue = 0;
        public int potions = 5;
        public int weaponValue = 1;
        public int difficultyMod = 0;

        public enum PlayerClass {Soldier, Hunter, Cleric}
        public PlayerClass currentClass = PlayerClass.Soldier;

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
        public void LevelUp()
        {
            while (CanLevelUp())
            {
                xp -= GetLevelUpValue();
                level++;
            }
            WaveOutEvent levelUp = new WaveOutEvent();
            levelUp.Init(new AudioFileReader("./audio/level-up.wav"));
            levelUp.Play();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Program.Print($"You gained a level!\nYou're level is {level}");
            Console.ResetColor();
        }
    }
}
