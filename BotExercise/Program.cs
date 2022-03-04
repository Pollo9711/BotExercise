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
using Telegram.Bot.Types.Enums;

namespace BotExercise
{
    public class Program
    {

        public static TelegramBotClient BotClient;

        public static Update update;

        static async Task Main(string[] args)
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

            var message = update.Message;

            Console.WriteLine(newString);

            var me = BotClient.GetMeAsync().Result;

            await BotClient.SendTextMessageAsync(message.Chat.Id, message.Text);
        }
    }
}
