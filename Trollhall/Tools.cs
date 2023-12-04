using NAudio.Wave;

namespace Trollhall
{
    public class Tools
    {
        // Print() ----------------------------------------------------------------------------------------------------- Print() //
        // Basically just Console.Write() but with a delay and sound effect //
        public void Print(bool waitForInput, string text, int speed = 1)
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
            if (waitForInput)
            {
                Console.ReadKey();
            }
        }
        // ExperienceBar() ------------------------------------------------------------------------------------- ExperienceBar() //
        public void ExperienceBar(string fillerChar, decimal value, int size)
        {
            int differentiator = (int)(value * size);
            for (int i = 0; i < size; i++)
            {
                if (i < differentiator)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(fillerChar);
                    Console.ResetColor();
                }
                else
                    Console.Write("░");
            }
        }
        // PlaySoundEffect() --------------------------------------------------------------------------------- PlaySoundEffect() //
        public void PlaySoundEffect(string soundFile)
        {
            WaveOutEvent sound = new WaveOutEvent();
            sound.Init(new AudioFileReader($"./audio/{soundFile}.wav"));
            sound.Play();
        }
    }
}
