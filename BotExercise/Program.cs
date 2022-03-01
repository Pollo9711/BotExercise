using System;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotExercise
{
    internal class Program
    {
        public static TelegramBotClient BotClient;

        static void Main(string[] args)
        {
            BotClient = new TelegramBotClient("5266813309:AAHOOC0zxmCyU-eJJA5Zf_31yYFIHnmgz6A");

            var me = BotClient.GetMeAsync().Result;

            Console.WriteLine($"Hello {me.Username}!");

            
        }
    }
}
