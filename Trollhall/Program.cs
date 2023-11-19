using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;

namespace Trollhall
{
    public class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;

        static void Main(string[] args)
        {
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

        static Player NewStart(int id)
        {
            Console.Clear();
            Player player = new Player();
            Print("Halls of Trollhall");
            Print("Enter your name:");
            player.name = Console.ReadLine();
            player.id = id;
            while (player.name == "")
                player.name = Console.ReadLine();
            Console.Clear();
            Print("You are at the entrance to the Halls of Trollhall.\nYour adventure begins...");
            Print($"Your name is {player.name} and you're a proud dwarf of the city of Fjellheim.");
            Print("You were sent by King Thorim the heartless to retrieve an ancient ledger deep within these ruined halls.");

            Console.ReadKey();
            Console.Clear();
            Print("You encounter a troll!");
            return player;
        }

        public static void Quit()
        {
            Save();
            Environment.Exit(0);
        }

        public static void Save()
        {
            string path = $"./saves/{currentPlayer.id}.level";
            string json = JsonSerializer.Serialize(currentPlayer);
            File.WriteAllText(path, json);
        }

        public static Player Load(out bool isNewPlayer)
        {
            isNewPlayer = false;
            Console.Clear();
            string[] paths = Directory.GetFiles("saves");
            List<Player> players = new List<Player>();
            int idCount = 0;

            foreach (string player in paths)
            {
                string json = File.ReadAllText(player);
                Player temp = JsonSerializer.Deserialize<Player>(json);
                players.Add(temp);
            }

            idCount = players.Count;

            while (true)
            {
                Console.Clear();
                Print("Choose your player:");

                foreach (Player player in players)
                {
                    Print($"{player.id}: {player.name}");
                }

                Print("Input player name or id (id:# or playername) or\ninput 'create' to create a new character");
                string[] data = Console.ReadLine().Split(':');

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
                    else if (data[0] == "create")
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
        public static void Print(string text, int speed = 10)
        {
            foreach (char character in text)
            {
                Console.Write(character);
                System.Threading.Thread.Sleep(speed);
            }
            Console.WriteLine();
        }
    }
}

