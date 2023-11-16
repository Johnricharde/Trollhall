namespace Trollhall
{
    internal class Program
    {
        public static Player currentPlayer = new Player();
        public static bool mainLoop = true;
        static void Main(string[] args)
        {
            Start();
            Encounters.FirstEncounter();
            while (mainLoop)
            {
                Encounters.RandomEncounter();
            }
        }


        static void Start()
        {
            Console.WriteLine("Halls of Trollhall");
            Console.WriteLine("Enter your name:");
            currentPlayer.name = Console.ReadLine();
            while (currentPlayer.name == "")
                currentPlayer.name = Console.ReadLine();
            Console.Clear();
            Console.WriteLine("You are at the entrance to the Halls of Trollhall.\nYour adventure begins...");
            Console.WriteLine($"Your name is {currentPlayer.name} and you're a proud dwarf of the city of Fjellheim.");
            Console.WriteLine("You were sent by King Thorim the heartless to retrieve an ancient ledger deep withing these ruined halls.");
        

            Console.ReadKey();
            Console.Clear();
            Console.WriteLine("You encounter a troll!");

        }
    }
}
