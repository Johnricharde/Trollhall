using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Trollhall
{
    public class Encounters
    {
        static Random rand = new Random();
        // ENCOUNTERS ---------------------------------------------------------------------------------------- ENCOUNTERS //
        // Trap Encounters ------------------------------------------------------------------------------ Trap Encounters //
        private static void PitfallTrapEncounter()
        {
            int damageTaken = Program.currentPlayer.GetPower();
            damageTaken = (Program.currentPlayer.currentClass == Player.PlayerClass.Ranger) ? damageTaken / 2 : damageTaken;
            Console.Clear();
            Program.Print("As you walk through a narrow corridor.\nSuddenly, the floor gives way beneath you, revealing a hidden pitfall trap!");
            Console.ReadKey();
            Program.currentPlayer.health -= damageTaken;
            Program.Print($"You fall into the pit, taking {damageTaken} damage!");
            if (Program.currentPlayer.health > 0)
            {
                Program.Print("Luckily, you manage to climb out of the pit and continue cautiously.");
                Console.ReadKey();
                RandomEncounter();
            }
            else
            {
                Program.currentPlayer.playerDeath("The fall was too much for you, and you succumb to your injuries.");
            }
        }
        // Lore Encounters ------------------------------------------------------------------------------ Lore Encounters //
        private static void FirebrandBreweryEncounter()
        {

        }
        // Riddle Encounters -------------------------------------------------------------------------- Riddle Encounters //
        private static void DwarfPuzzleEncounter()
        {

        }
        // Social Encounters -------------------------------------------------------------------------- Social Encounters //
        private static void DwarfClericEncounter()
        {
            SocialEncounter();
            Program.Print("You meet a Dwarf cleric who offers you a blessing.\nDo you accept? [y/n]");
            if (Console.ReadLine() == "y")
            {
                int healAmount = Program.currentPlayer.GetHeal();
                Program.currentPlayer.health += healAmount;
                Program.Print($"You accept his blessing, rejuvenating old wounds.\nYou've gained {healAmount} health!");
                Console.ReadKey();
            }
            else
            {
                int coins = Program.currentPlayer.GetCoins() * 3;
                Program.currentPlayer.coins += coins;
                Program.Print($"You politely decline his offer.\nHe insists that you at least accept gold to help\nhelp you on your quest.\nYou've gained {coins} coins!");
                Console.ReadKey();
            }
            RandomEncounter();
        }
        private static void SocialEncounter()
        {
            Console.Clear();
            Program.Print("You encounter a traveller!");
            Console.ReadKey();
        }
        // Combat Encounters -------------------------------------------------------------------------- Combat Encounters //
        public static void FirstEncounter()
        {
            Program.Print("It's at death's door.\nAn open wound is exposing it's innards.\nIt would probably die of it's own, but why risk it?");
            Console.ReadKey();
            CombatMechanics.Combat(false, "Half-Dead Troll", 1, 20);
        }
        private static void TrollBehemothEncounter()
        { 
            Console.Clear();
            Program.Print("A troll behemoth has found you!\nIt looms over you, thrice your size.\nThese monstrosities were the backbone of the troll invasion force\nthat reclaimed the Trollhalls...\n\nIt might be wise to run...");
            Console.ReadKey();
            CombatMechanics.Combat(false, "Troll Behemoth", (5 * Program.currentPlayer.difficultyMod), 100);
        }
        private static void basicFightEncounter()
        {
            Console.Clear();
            Program.Print($"You encounter an enemy!");
            Console.ReadKey();
            CombatMechanics.Combat(true, "", 0, 0);
        }


        // ENCOUNTER TOOLS ------------------------------------------------------------------------------ ENCOUNTER TOOLS //
        public static void RandomEncounter()
        {
            switch (rand.Next(0, 2))
            {
                //case 0:
                //    TrollBehemothEncounter();
                //    break;
                //case 1:
                //    DwarfClericEncounter();
                //    break;
                case 1:
                    PitfallTrapEncounter();
                    break;
                default:
                    basicFightEncounter();
                    break;
            }
        }
        public static string GetName()
        {
            switch (rand.Next(0, 5))
            {
                case 0:
                    return "Rock Spider";
                case 1:
                    return "Firebeetle";
                case 2:
                    return "Giant Bat";
                case 3:
                    return "Trollspawn";
                case 4:
                    return "Dwarf Ghost";
            }
            return "Beast";
        }
    }
}
