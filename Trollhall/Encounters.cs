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
        private static Random rand = new Random();

        // ENCOUNTER TOOLS ------------------------------------------------------------------------------ ENCOUNTER TOOLS //
        public static void RandomEncounter()
        {
            switch (rand.Next(1, 4))
            {
                //case 0:
                //    TrollBehemothEncounter();
                //    break;
                //case 1:
                //    DwarfClericEncounter();
                //    break;
                //case 2:
                //    PitfallTrapEncounter();
                //    break;
                //case 1:
                    //FirebrandBreweryEncounter();
                    //BlacksmithEncounter();
                    //break;
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

        // ENCOUNTERS ---------------------------------------------------------------------------------------- ENCOUNTERS //
        // Trap Encounters ------------------------------------------------------------------------------ Trap Encounters //
        private static void PitfallTrapEncounter()
        {
            int damageTaken = Program.currentPlayer.GetPower();
            damageTaken = (Program.currentPlayer.currentClass == Player.PlayerClass.Ranger) ? damageTaken / 2 : damageTaken;
            Console.Clear();
            Program.Print(true, "As you walk through a narrow corridor.\nSuddenly, the floor gives way beneath you, revealing a primitive pitfall trap!");
            Program.currentPlayer.health -= damageTaken;
            Program.Print(false, $"You fall into the pit, taking {damageTaken} damage!");
            if (Program.currentPlayer.health > 0)
            {
                Program.Print(true, "Luckily, you manage to climb out of the pit and continue cautiously.");
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
            Console.Clear();
            Program.Print(false, "You enter the ancient Firebrand Brewery,\nonce renowned for its Firebrand Brandy.\nThe air is thick with the scent of fermented ingredients, and\nremnants of barrels and brewing equipment are scattered around.\n");
            Program.Print(false, "What would you like to do?\n");
            Program.Print(false, "1. Investigate the brewing equipment.");
            Program.Print(false, "2. Sample a mysterious brew left on the counter.");
            Program.Print(false, "3. Continue exploring Trollhalls.");

            string playerChoice = Console.ReadLine();

            switch (playerChoice)
            {
                case "1":
                    Program.Print(false, "You carefully examine the brewing equipment, discovering a hidden compartment with valuable brewing ingredients.");
                    int rewardCoins = Program.currentPlayer.GetCoins() * 4;
                    Program.currentPlayer.coins += rewardCoins;
                    Program.Print(true, $"You find {rewardCoins} coins worth!");
                    break;

                case "2":
                    Program.Print(false, "Curiosity gets the better of you, and\nyou decide to sample the mysterious brew.\nThe brew has an unexpected kick, you feel a surge of energy!");
                    int rewardXP = Program.currentPlayer.GetXP() * 4;
                    Program.currentPlayer.xp += rewardXP;
                    Program.Print(false, $"You've gained {rewardXP} experience points!");
                    if (Program.currentPlayer.CanLevelUp())
                    {
                        Program.currentPlayer.LevelUp();
                    }
                    Console.ReadKey();
                    break;

                case "3":
                    Program.Print(true, "You decide to continue exploring Trollhalls, leaving the Firebrand Brewery behind.");
                    break;

                default:
                    Program.Print(true, "Invalid choice. Please choose a valid option.");
                    FirebrandBreweryEncounter();
                    break;
            }
            RandomEncounter();
        }

        // Riddle Encounters -------------------------------------------------------------------------- Riddle Encounters //
        private static void DwarfPuzzleEncounter()
        {

        }
        // Social Encounters -------------------------------------------------------------------------- Social Encounters //
        private static void DwarfClericEncounter()
        {
            Program.Print(false, "You meet a Dwarf cleric who offers you a blessing.\nDo you accept? [y/n]");
            if (Console.ReadLine() == "y")
            {
                int healAmount = Program.currentPlayer.GetHeal();
                Program.currentPlayer.health += healAmount;
                Program.Print(true, $"You accept his blessing, rejuvenating old wounds.\nYou've gained {healAmount} health!");
            }
            else
            {
                int coins = Program.currentPlayer.GetCoins() * 3;
                Program.currentPlayer.coins += coins;
                Program.Print(true, $"You politely decline his offer.\nHe insists that you at least accept gold to\nhelp you on your quest.\nYou've gained {coins} coins!");
            }
            RandomEncounter();
        }
        private static void BlacksmithEncounter()
        {
            Console.Clear();
            Program.Print(false, "You come across a hidden chamber.\nYou enter and find a dwarven blacksmith huddled in the corner,\nsurrounded by makeshift barricades.\nThe blacksmith, happy to see a friendly face, offers you his services:\n");
            Program.Print(false, " [1] Upgrade armor");
            Program.Print(false, " [2] Upgrade weapon");
            string upgradeChoice = Console.ReadLine();

            switch (upgradeChoice)
            {
                case "1":
                    Program.currentPlayer.armorValue++;
                    Program.Print(false, $"The blacksmith takes the armor and enhances it.\nYour armor has been upgraded!\nNew armor value: {Program.currentPlayer.armorValue - 1}");
                    break;

                case "2":
                    Program.currentPlayer.weaponValue++;
                    Program.Print(false, $"The blacksmith takes the weapon and enhances it.\nYour weapon has been upgraded!\nNew weapon value: {Program.currentPlayer.weaponValue - 1}");
                    break;

                default:
                    Program.Print(true, "Invalid choice. The blacksmith looks confused.");
                    BlacksmithEncounter();
                    break;
            }

            Console.ReadKey();
            RandomEncounter();
        }
        // Combat Encounters -------------------------------------------------------------------------- Combat Encounters //
        public static void FirstEncounter()
        {
            Program.Print(true, "It's at death's door.\nAn open wound is exposing it's innards.\nIt would probably die of it's own, but why risk it?");
            CombatMechanics.Combat(false, "Half-Dead Troll", 1, 20);
        }
        private static void TrollBehemothEncounter()
        { 
            Console.Clear();
            Program.Print(true, "A troll behemoth has found you!\nIt looms over you, thrice your size.\nThese monstrosities were the backbone of the troll invasion force\nthat reclaimed the Trollhalls...\n\nIt might be wise to run...");
            CombatMechanics.Combat(false, "Troll Behemoth", (5 * Program.currentPlayer.difficultyMod), 100);
        }
        private static void basicFightEncounter()
        {
            Console.Clear();
            Program.Print(true, $"You encounter an enemy!");
            CombatMechanics.Combat(true, "", 0, 0);
        }


    }
}
