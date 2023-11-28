using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Runtime.CompilerServices;
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
        public static void RandomEncounter(Program program)
        {
            switch (rand.Next(1, 15))
            {
                case 0:
                    TrollBehemothEncounter(program);
                    break;
                case 1:
                    DwarfClericEncounter(program);
                    break;
                case 2:
                    PitfallTrapEncounter(program);
                    break;
                case 3:
                    DwarvenPuzzleEncounter(program);
                    break;
                case 4:
                    FirebrandBreweryEncounter(program);
                    break;
                case 5:
                    BlacksmithEncounter(program);
                    break;
                default:
                    basicFightEncounter(program);
                    break;
            }
        }
        public static string GetRandomName()
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
        private static void PitfallTrapEncounter(Program program)
        {
            Console.Clear();
            int damageTaken = program.currentPlayer.GetPower();
            damageTaken = (program.currentPlayer.currentClass == Player.PlayerClass.Ranger) ? damageTaken / 2 : damageTaken;
            program.Print(true, "As you walk through a narrow corridor.\nSuddenly, the floor gives way beneath you, revealing a primitive pitfall trap!");
            program.currentPlayer.health -= damageTaken;
            program.Print(false, $"You fall into the pit, taking {damageTaken} damage!");
            if (program.currentPlayer.health > 0)
            {
                program.Print(true, "Luckily, you manage to climb out of the pit and continue cautiously.");
                RandomEncounter(program);
            }
            else
            {
                program.currentPlayer.playerDeath("The fall was too much for you, and you succumb to your injuries.");
            }
        }
        // Lore Encounters ------------------------------------------------------------------------------ Lore Encounters //
        private static void FirebrandBreweryEncounter(Program program)
        {
            Console.Clear();
            program.Print(false, "You enter the ancient Firebrand Brewery,\nonce renowned for its Firebrand Brandy.\nThe air is thick with the scent of fermented ingredients, and\nremnants of barrels and brewing equipment are scattered around.\n");
            program.Print(false, "What would you like to do?\n");
            program.Print(false, "1. Investigate the brewing equipment.");
            program.Print(false, "2. Sample a mysterious brew left on the counter.");
            program.Print(false, "3. Continue exploring Trollhalls.");

            string playerChoice = Console.ReadLine();

            switch (playerChoice)
            {
                case "1":
                    program.Print(false, "You carefully examine the brewing equipment, discovering a hidden compartment with valuable brewing ingredients.");
                    int rewardCoins = program.currentPlayer.GetCoins() * 4;
                    program.currentPlayer.coins += rewardCoins;
                    program.Print(true, $"You find {rewardCoins} coins worth!");
                    break;

                case "2":
                    program.Print(false, "Curiosity gets the better of you, and\nyou decide to sample the mysterious brew.\nThe brew has an unexpected kick, you feel a surge of energy!");
                    int rewardXP = program.currentPlayer.GetXP() * 4;
                    program.currentPlayer.xp += rewardXP;
                    program.Print(false, $"You've gained {rewardXP} experience points!");
                    if (program.currentPlayer.CanLevelUp())
                        program.currentPlayer.LevelUp();
                    Console.ReadKey();
                    break;

                case "3":
                    program.Print(true, "You decide to continue exploring Trollhalls, leaving the Firebrand Brewery behind.");
                    break;

                default:
                    program.Print(true, "Invalid choice. Please choose a valid option.");
                    FirebrandBreweryEncounter(program);
                    break;
            }
            RandomEncounter(program);
        }

        // Riddle Encounters -------------------------------------------------------------------------- Riddle Encounters //
        private static void DwarvenPuzzleEncounter(Program program)
        {
            Console.Clear();
            program.Print(false, "You come across a chamber with an intricate dwarven puzzle.\nIt's unclear how to unlock the passage...\n");
            program.Print(false, " [1] Examine the puzzle and try to solve it.");
            program.Print(false, " [2] Attempt to brute force the puzzle.\n");
            string playerChoice = Console.ReadLine();

            switch (playerChoice)
            {
                case "1":
                    program.Print(true, "You carefully examine the dwarven puzzle,\nanalyzing the symbols and their arrangement.\n");
                    if (rand.Next(0, 2) == 0)
                    {
                        program.Print(true, "After some thought,\nyou successfully decipher the pattern and unlock the passage.");
                        SolvedPuzzle();
                    }
                    else
                        program.Print(false, "Despite your efforts,\nthe puzzle remains unsolved.\nThe symbols resist your attempts.");
                    break;

                case "2":
                    program.Print(true, "You decide to take a more direct approach and\nattempt to brute force the puzzle with you strength.\n");
                    if (rand.Next(0, (program.currentPlayer.currentClass == Player.PlayerClass.Warrior) ? 4 : 2) == 0)
                    {
                        program.Print(true, "Through trial and error,\nyou manage to brute force the puzzle and unlock the passage.");
                        SolvedPuzzle();
                    }
                    else
                        program.Print(false, "Despite your persistent attempts,\nthe puzzle seems resistant to brute force. \nIt remains unsolved.");
                    break;

                default:
                    program.Print(true, "Invalid choice. The puzzle remains unsolved.");
                    DwarvenPuzzleEncounter(program);
                    break;
            }
            void SolvedPuzzle()
            {
                int coins = program.currentPlayer.GetCoins() * program.currentPlayer.difficultyMod;
                int experience = program.currentPlayer.GetXP() * program.currentPlayer.difficultyMod;
                program.Print(false, $"\nWithin the passage you find treasure!\nYou loot {coins} coins!\nYou recieve {experience} experience!");
                program.currentPlayer.coins += coins;
                program.currentPlayer.xp += experience;
                if (program.currentPlayer.CanLevelUp())
                    program.currentPlayer.LevelUp();
            }

            Console.ReadKey();
            RandomEncounter(program);
        }

        // Social Encounters -------------------------------------------------------------------------- Social Encounters //
        private static void DwarfClericEncounter(Program program)
        {
            Console.Clear();
            program.Print(false, "You meet a Dwarf cleric who offers you a blessing.\nDo you accept? [y/n]");
            if (Console.ReadLine() == "y")
            {
                int healAmount = program.currentPlayer.GetHeal();
                program.currentPlayer.health += healAmount;
                program.Print(true, $"You accept his blessing, rejuvenating old wounds.\nYou've gained {healAmount} health!");
            }
            else
            {
                int coins = program.currentPlayer.GetCoins() * 3;
                program.currentPlayer.coins += coins;
                program.Print(true, $"You politely decline his offer.\nHe insists that you at least accept gold to\nhelp you on your quest.\nYou've gained {coins} coins!");
            }
            RandomEncounter(program);
        }
        private static void BlacksmithEncounter(Program program)
        {
            Console.Clear();
            program.Print(false, "You come across a hidden chamber.\nYou enter and find a dwarven blacksmith huddled in the corner,\nsurrounded by makeshift barricades.\nThe blacksmith, happy to see a friendly face, offers you his services:\n");
            program.Print(false, " [1] Upgrade armor");
            program.Print(false, " [2] Upgrade weapon");
            string upgradeChoice = Console.ReadLine();

            switch (upgradeChoice)
            {
                case "1":
                    program.currentPlayer.armorValue++;
                    program.Print(false, $"The blacksmith takes the armor and enhances it.\nYour armor has been upgraded!\nNew armor value: {program.currentPlayer.armorValue - 1}");
                    break;

                case "2":
                    program.currentPlayer.weaponValue++;
                    program.Print(false, $"The blacksmith takes the weapon and enhances it.\nYour weapon has been upgraded!\nNew weapon value: {program.currentPlayer.weaponValue - 1}");
                    break;

                default:
                    program.Print(true, "Invalid choice. The blacksmith looks confused.");
                    BlacksmithEncounter(program);
                    break;
            }

            Console.ReadKey();
            RandomEncounter(program);
        }
        // Combat Encounters -------------------------------------------------------------------------- Combat Encounters //
        public static void FirstEncounter(Program program)
        {
            program.Print(true, "It's at death's door.\nAn open wound is exposing it's innards.\nIt would probably die of it's own, but why risk it?");
            CombatMechanics.Combat(program, false, "Half-Dead Troll", 1, 20);
        }
        private static void TrollBehemothEncounter(Program program)
        {
            Console.Clear();
            program.Print(true, "A troll behemoth has found you!\nIt looms over you, thrice your size.\nThese monstrosities were the backbone of the troll invasion force\nthat reclaimed the Trollhalls...\n\nIt might be wise to run...");
            CombatMechanics.Combat(program, false, "Troll Behemoth", (5 * program.currentPlayer.difficultyMod), 100);
        }
        private static void basicFightEncounter(Program program)
        {
            Console.Clear();
            program.Print(true, $"You encounter an enemy!");
            CombatMechanics.Combat(program, true, "", 0, 0);
        }


    }
}
