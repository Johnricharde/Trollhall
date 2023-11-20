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
            Print("Choose a class:");
            Print("[S]oldiers boast great strength");
            Print("[H]unters are quick on their feet");
            Print("[C]lerics recieve great blessings");
            bool flag = false;
            while (!flag)
            {
                string input = Console.ReadLine().ToLower();
                flag = true;
                if (input == "soldier" || input == "s")
                    player.currentClass = Player.PlayerClass.Soldier;
                else if (input == "hunter" || input == "h")
                    player.currentClass = Player.PlayerClass.Hunter;
                else if (input == "cleric" || input == "c")
                    player.currentClass = Player.PlayerClass.Cleric;
                else
                {
                    Print("Not a valid class, please choose a class");
                    flag = false;
                }
                
            }
            Console.Clear();
            Print("You are at the entrance to the Halls of Trollhall.\nYour adventure begins...");
            Print($"Your name is {player.name} and you're a proud dwarf of the city of Fjellheim.");
            Print("You were sent by King Thorim the heartless to retrieve an ancient ledger deep\nwithin these ruined halls.");
            Console.ReadKey();
            Console.Clear();
            Print("You encounter a troll!");
            return player;
        }
        // QUIT, SAVE, LOAD ----------------------------------------------------------------------------------- QUIT, SAVE, LOAD //
        public static void Quit()
        {
            Save();
            Environment.Exit(0);
        }

        public static void Save()
        {
            if (currentPlayer == null)
            {
                Console.WriteLine("No player data to save.");
                return;
            }

            string path = $"saves/{currentPlayer.id.ToString()}.json";

            string json = JsonConvert.SerializeObject(currentPlayer, Formatting.Indented);
            File.WriteAllText(path, json);

            Console.WriteLine("Player data saved successfully.");
        }

        public static Player Load(out bool isNewPlayer)
        {
            isNewPlayer = false;
            Console.Clear();
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            int idCount = 0;

            foreach (string playerPath in paths)
            {
                string json = File.ReadAllText(playerPath);
                Player temp = JsonConvert.DeserializeObject<Player>(json);
                players.Add(temp);
            }

            while (true)
            {
                Console.Clear();
                Print("Choose your player: (id:'number')");

                foreach (Player player in players)
                {
                    Print($"[{player.id}]: {player.name}");
                }
                Console.WriteLine();
                Print("[C]reate a new character");
                string[] data = Console.ReadLine().ToLower().Split(':');

                try
                {
                    if (data[0] == "id")
                    {
                        if (int.TryParse(data[1], out int id))
                        {
                            foreach (Player player in players)
                            {
                                if (player.id == id)
                                {
                                    return player;
                                }
                            }
                            Print("There is no player with that id");
                            Console.ReadKey();
                        }
                        else
                        {
                            Print("Your id needs to be a number");
                            Console.ReadKey();
                        }
                    }
                    else if (data[0] == "create" || data[0] == "c")
                    {
                        Player newPlayer = NewStart(idCount);
                        isNewPlayer = true;
                        return newPlayer;
                    }
                    else
                    {
                        foreach (Player player in players)
                        {
                            if (player.name == data[0])
                            {
                                return player;
                            }
                        }
                        Print("There is no player with that name");
                        Console.ReadKey();
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Print("Your id needs to be a number");
                    Console.ReadKey();
                }
            }
        }
        // Print() ----------------------------------------------------------------------------------------------------- Print() //
        public static void Print(string text, int speed = 1)
        {
            WaveOutEvent typing = new WaveOutEvent();
            typing.Init(new AudioFileReader("./audio/typing.wav"));
            typing.Play();
            foreach (char character in text)
            {
                Console.Write(character);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
            typing.Stop();
            typing.Dispose();
        }
        public static void ExperienceBar(string fillerChar, decimal value, int size)
        {
            int differentiator = (int)(value * size);
            for (int i = 0; i < size; i++) 
            {
                if (i < differentiator)
                    Console.Write(fillerChar);
                else
                    Console.Write(" ");
            }
        }
    }
}

