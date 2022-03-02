using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotExercise
{
    public class Program
    {
        public static TelegramBotClient botClient;

        static void Main(string[] args)
        {
            var url = "https://www.ansa.it/sito/notizie/mondo/mondo_rss.xml";

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

            //Console.WriteLine(newString);

            botClient = new TelegramBotClient("5266813309:AAHOOC0zxmCyU-eJJA5Zf_31yYFIHnmgz6A");

            var me = botClient.GetMeAsync().Result;

            botClient.SendTextMessageAsync(chatId: me.Id, text: newString);
        }
    }
}
