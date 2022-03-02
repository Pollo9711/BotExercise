using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotExercise
{
    internal class Program
    {
        public static TelegramBotClient BotClient;



        static void Main(string[] args)
        {
            var url = "https://www.ansa.it/sito/notizie/mondo/mondo_rss.xml";

            using var reader = XmlReader.Create(url);

            var feed = SyndicationFeed.Load(reader);

            var list = feed.Items.OrderByDescending(x => x.PublishDate).Take(10).Select(a => new
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

            Console.WriteLine(newString);

            BotClient = new TelegramBotClient("5266813309:AAHOOC0zxmCyU-eJJA5Zf_31yYFIHnmgz6A");

            var me = BotClient.GetMeAsync();

        }
    }
}
