using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    [Serializable]
    public class Player
    {
        Random rand = new Random();

        public string name;
        public int id;
        public int coins = 9999;
        public int health = 10;
        public int damage = 1;
        public int armorValue = 0;
        public int potions = 5;
        public int weaponValue = 1;
        public int difficultyMod = 0;

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
    }
}
