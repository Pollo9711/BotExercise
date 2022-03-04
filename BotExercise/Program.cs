using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.ServiceModel.Syndication;
<<<<<<< HEAD
=======
using System.Threading;
>>>>>>> 5cf40ad78caf5718cf083cc313300dba05a4adc3
using System.Threading.Tasks;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace BotExercise
{
    public class Program
    {
<<<<<<< HEAD
        public static TelegramBotClient BotClient;

        public static Update update;

        static async Task Main(string[] args)
=======
        public static TelegramBotClient botClient;

        static void Main(string[] args)
>>>>>>> 5cf40ad78caf5718cf083cc313300dba05a4adc3
        {
            var url = "https://www.ansa.it/sito/notizie/mondo/mondo_rss.xml";

            BotClient = new TelegramBotClient("5195273815:AAFGqS7fRNLpkuzJ9fD2uQMbM4MUqykRFa8");

            using var reader = XmlReader.Create(url);

            var feed = SyndicationFeed.Load(reader);

            var list = feed.Items
                .OrderByDescending(x => x.PublishDate)
                .Take(10).Select(a => new
            {
                title = a.Title.Text,
                publishDate = a.PublishDate,
                link = a.Id,
            });

            var newString = "";

            foreach (var lists in list)
            {
                newString += lists.title + " " + lists.publishDate + " " + lists.link + "\n";
            }

<<<<<<< HEAD
            var message = update.Message;
                
            await BotClient.SendTextMessageAsync(message.Chat.Id, message.Text);

            Console.WriteLine(newString);
=======
            //Console.WriteLine(newString);

            botClient = new TelegramBotClient("5266813309:AAHOOC0zxmCyU-eJJA5Zf_31yYFIHnmgz6A");

            var me = botClient.GetMeAsync().Result;

            botClient.SendTextMessageAsync(chatId: me.Id, text: newString);
>>>>>>> 5cf40ad78caf5718cf083cc313300dba05a4adc3
        }

        
    }
}
