using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;
using NAudio.Wave;
using System.Media;
using System.Runtime.InteropServices;
using Newtonsoft.Json;

namespace Trollhall
{
    public class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;

        // Main() ------------------------------------------------------------------------------------------------------- Main() //
        static void Main(string[] args)
        {
            SoundPlayer backgroundMusic = new SoundPlayer("./audio/background-music.wav");
            backgroundMusic.PlayLooping();

            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            currentPlayer = Load(out bool isNewPlayer);
            if (isNewPlayer)
                Encounters.FirstEncounter();
            while (mainLoop)
            {
                Encounters.RandomEncounter();
            }
        }
        // NewStart() ----------------------------------------------------------------------------------------------- NewStart() //
        static Player NewStart(int id)
        {
            Console.Clear();
            Player player = new Player();
            Print("Enter your name:");
            player.name = Console.ReadLine();
            player.id = id;
            while (player.name == "")
                player.name = Console.ReadLine();
            Print("Choose a class:\n [W]arriors boast great strength\n [R]angers are quick on their feet\n [C]lerics recieve great blessings");
            bool flag = false;
            while (!flag)
            {
                string input = Console.ReadLine().ToLower();
                flag = true;
                if (input == "warrior" || input == "w")
                    player.currentClass = Player.PlayerClass.Warrior;
                else if (input == "ranger" || input == "r")
                    player.currentClass = Player.PlayerClass.Ranger;
                else if (input == "cleric" || input == "c")
                    player.currentClass = Player.PlayerClass.Cleric;
                else
                {
                    Print("Not a valid class, please choose a class");
                    flag = false;
                }
                
            }
            Console.Clear();
            Print($"You are at the entrance to the Halls of Trollhall.\nYour adventure begins...\nYour name is {player.name}\nand you're a proud dwarf of the fallen city of Trollhall.\nThe dwarven King Thorim is criticised for not doing more to reclaim these halls.\nAnd so the burden falls to lone dwarves like yourself.");
            Console.ReadKey();
            Console.Clear();
            Print("You encounter a troll!");
            return player;
        }
        // Quit() ------------------------------------------------------------------------------------------------------- Quit() //
        public static void Quit()
        {
            Save(currentPlayer.name);
            Environment.Exit(0);
        }
        // Save() ------------------------------------------------------------------------------------------------------- Save() //
        public static void Save(string saveFileName)
        {
            if (currentPlayer == null)
            {
                Console.WriteLine("No player data to save.");
                return;
            }

            string path = $"saves/{saveFileName}.json";

            string json = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);
            File.WriteAllText(path, json);

            Console.WriteLine("Player data saved successfully.");
        }
        // Load() ------------------------------------------------------------------------------------------------------- Load() //
        public static Player Load(out bool isNewPlayer)
        {
            isNewPlayer = false;
            Console.Clear();
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            int idCount;

            foreach (string playerPath in paths)
            {
                string json = File.ReadAllText(playerPath);
                Player temp = JsonConvert.DeserializeObject<Player>(json);
                players.Add(temp);
            }

            players = players.OrderBy(player => player.id).ToList();
            idCount = players.Count;

            while (true)
            {
                Console.Clear();
                Print("Square brackets \"[]\" signify a valid input\n");

                foreach (Player player in players)
                {
                    Print($" [{player.id + 1}] {player.name} the {player.currentClass}");
                }
                Console.WriteLine();
                Print(" [C]reate a new character\n [D]elete a character\n");

                string userInput = Console.ReadLine().ToLower();

                    if (userInput == "c" || userInput == "create")
                    {
                        Player newPlayer = NewStart(idCount);
                        isNewPlayer = true;
                        idCount++;
                        return newPlayer;
                    }
                    else if (userInput == "d" || userInput == "delete")
                    {
                    DeleteCharacter(players);
                }
                else if (int.TryParse(userInput, out int id))
                    {
                        foreach (Player player in players)
                        {
                            if (player.id == id - 1)
                            {
                                return player;
                            }
                        }
                        Print("Couldn't find that id...");
                        Console.ReadKey();
                    }
                    else
                    {
                        Print("Couldn't find that name...");
                        Console.ReadKey();
                    }
            }
        }
        // DeleteCharacter() --------------------------------------------------------------------------------- DeleteCharacter() //
        private static void DeleteCharacter(List<Player> players)
        {
            Print("Which character do you want to delete?");
            int idToDelete;

            if (int.TryParse(Console.ReadLine(), out idToDelete))
            {
                Player playerToDelete = players.FirstOrDefault(player => player.id == idToDelete - 1);

                if (playerToDelete != null)
                {
                    Print($"Are you sure you want to delete {playerToDelete.name} the {playerToDelete.currentClass}? [y/n]");
                    string confirmation = Console.ReadLine().ToLower();

                    if (confirmation == "y" || confirmation == "yes")
                    {
                        string pathToDelete = $"saves/{playerToDelete.name}.json";
                        File.Delete(pathToDelete);
                        Print($"Player {playerToDelete.name} deleted successfully.");
                        Load(out bool isNewPlayer);
                    }
                    else
                    {
                        Print("Deletion canceled.");
                    }
                }
                else
                {
                    Print("Invalid ID. Please enter a valid character ID.");
                }
            }
            else
            {
                Print("Invalid input. Please enter a valid character ID.");
            }
        }
        // Print() ----------------------------------------------------------------------------------------------------- Print() //
        // Basically just Console.Write() but with a delay and sound effect
        public static void Print(string text, int speed = 5)
        {
            WaveOutEvent typing = new WaveOutEvent();
            typing.Init(new AudioFileReader("./audio/typing.wav"));
            typing.Play();
            foreach (char character in text)
            {
                Console.Write(character);
                Thread.Sleep(speed);
            }
            Console.WriteLine();
            typing.Stop();
            typing.Dispose();
        }
        // ExperienceBar() ------------------------------------------------------------------------------------- ExperienceBar() //
        public static void ExperienceBar(string fillerChar, decimal value, int size)
        {
            int differentiator = (int)(value * size);
            for (int i = 0; i < size; i++) 
            {
                if (i < differentiator)
                    Console.Write(fillerChar);
                else
                    Console.Write("░");
            }
        }
        // PlaySoundEffect() --------------------------------------------------------------------------------- PlaySoundEffect() //
        public static void PlaySoundEffect(string soundFile)
        {
            WaveOutEvent sound = new WaveOutEvent();
            sound.Init(new AudioFileReader($"./audio/{soundFile}.wav"));
            sound.Play();
        }
    }
}

