using System;
using SKNIBot.Core;

namespace SKNIBot
{
    public class Program
    {
        public static void Main(string[] args)
        {
            new Bot().Run();
            while (Console.ReadLine() != "quit");
        }
    }
}