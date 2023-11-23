using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Trollhall
{
    public class Encounters
    {
        static Random rand = new Random();
        // ENCOUNTERS ---------------------------------------------------------------------------------------- ENCOUNTERS //
        public static void FirstEncounter()
        {
            Program.Print("It's at death's door.\nAn open wound is exposing it's innards.\n" +
                "It would probably die of it's own, but why risk it?");
            Console.ReadKey();
            CombatMechanics.Combat(false, "Half-Dead Troll", 1, 20);
        }
        public static void TrollBehemothEncounter()
        { 
            Console.Clear();
            Program.Print("A troll behemoth has found you!\n" +
                "It looms over you, thrice your size.\n" +
                "These monstrosities were the backbone of the troll invasion force\n" +
                "that reclaimed the Trollhalls...\n\n" +
                "It might be wise to run...");
            Console.ReadKey();
            CombatMechanics.Combat(false, "Troll Behemoth", (5 * Program.currentPlayer.difficultyMod), 100);
        }
        public static void basicFightEncounter()
        {
            Console.Clear();
            Program.Print($"You encounter an enemy!");
            Console.ReadKey();
            CombatMechanics.Combat(true, "", 0, 0);
        }


        // ENCOUNTER TOOLS ------------------------------------------------------------------------------ ENCOUNTER TOOLS //
        public static void RandomEncounter()
        {
            switch (rand.Next(0, 100))
            {
                case 0:
                    TrollBehemothEncounter();
                    break;
                case 1:
                    TrollBehemothEncounter();
                    break;
                case 2:
                    TrollBehemothEncounter();
                    break;
                case 3:
                    TrollBehemothEncounter();
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
