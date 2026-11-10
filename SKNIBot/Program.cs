using SKNIBot.Core;

namespace SKNIBot
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await new Bot().Run();
            while (Console.ReadLine() != "quit");
        }
    }
}