﻿
using System.Media;
using Newtonsoft.Json;

namespace Trollhall
{
    public class Program
    {
        public static Player currentPlayer = new();
        public Tools tools = new();

        // Main() ------------------------------------------------------------------------------------------------------- Main() //
        static void Main(string[] args)
        {
            var _program = new Program();
            _program.StartGame();
        }
        // Start() ----------------------------------------------------------------------------------------------------- Start() //
        public void StartGame()
        {
            var _encounter = new Encounters();

            SoundPlayer backgroundMusic = new SoundPlayer("./audio/background-music.wav");
            backgroundMusic.PlayLooping();

            if (!Directory.Exists("saves"))
            {
                Directory.CreateDirectory("saves");
            }
            currentPlayer = Load(out bool isNewPlayer);
            if (isNewPlayer)
                _encounter.FirstEncounter();
            bool mainLoop = true;
            while (mainLoop)
            {
                _encounter.RandomEncounter();
            }
        }
        // NewStart() ----------------------------------------------------------------------------------------------- NewStart() //
        Player NewStart(int id)
        {
            Console.Clear();
            Player player = new Player();
            tools.Print(false, "Enter your name:");
            player._name = Console.ReadLine();
            player.id = id;
            while (player._name == "")
                player._name = Console.ReadLine();
            tools.Print(false, "Choose a class:\n [W]arriors boast great strength\n [R]angers are quick on their feet\n [C]lerics recieve great blessings");
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
                    tools.Print(false, "Not a valid class, please choose a class");
                    flag = false;
                }
                
            }
            Console.Clear();
            tools.Print(true, $"You are at the entrance to the Halls of Trollhall.\nYour adventure begins...\nYour name is {player._name}\nand you're a proud dwarf of the fallen city of Trollhall.\nThe dwarven King Thorim is criticised for not doing more to reclaim these halls.\nAnd so the burden falls to lone dwarves like yourself.");
            Console.Clear();
            tools.Print(false, "You encounter a troll!");
            return player;
        }
        // Quit() ------------------------------------------------------------------------------------------------------- Quit() //
        public void Quit()
        {
            Save(currentPlayer._name);
            Environment.Exit(0);
        }
        // Save() ------------------------------------------------------------------------------------------------------- Save() //
        public void Save(string saveFileName)
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
        public Player Load(out bool isNewPlayer)
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
                tools.Print(false, "Square brackets \"[]\" signify a valid input\n");

                foreach (Player player in players)
                {
                    tools.Print(false, $" [{player.id + 1}] {player._name} the {player.currentClass}");
                }
                Console.WriteLine();
                tools.Print(false, " [C]reate a new character\n [D]elete a character\n");

                string? userInput = Console.ReadLine().ToLower();

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
                    tools.Print(true, "Couldn't find that id...");
                }
                else
                {
                    tools.Print(true, "Couldn't find that name...");
                }
            }
        }
        // DeleteCharacter() --------------------------------------------------------------------------------- DeleteCharacter() //
        private void DeleteCharacter(List<Player> players)
        {
            tools. Print(false, "Which character do you want to delete?");
            int idToDelete;

            if (int.TryParse(Console.ReadLine(), out idToDelete))
            {
                Player playerToDelete = players.FirstOrDefault(player => player.id == idToDelete - 1);

                if (playerToDelete != null)
                {
                    tools.Print(false, $"Are you sure you want to delete {playerToDelete._name} the {playerToDelete.currentClass}? [y/n]");
                    string confirmation = Console.ReadLine().ToLower();

                    if (confirmation == "y" || confirmation == "yes")
                    {
                        string pathToDelete = $"saves/{playerToDelete._name}.json";
                        File.Delete(pathToDelete);
                        tools.Print(false, $"Player {playerToDelete._name} deleted successfully.");
                        Load(out bool isNewPlayer);
                    }
                    else
                    {
                        tools.Print(false, "Deletion canceled.");
                    }
                }
                else
                {
                    tools.Print(false, "Invalid ID. Please enter a valid character ID.");
                }
            }
            else
            {
                tools.Print(false, "Invalid input. Please enter a valid character ID.");
            }
        }
    }
}

