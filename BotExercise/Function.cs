using Google.Cloud.Functions.Framework;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace BotExercise
{
    public class Function : IHttpFunction
    {
        private readonly ILogger _logger;

        public Function(ILogger<Function> logger) =>
            _logger = logger;

        public async Task HandleAsync(HttpContext context)
        {
            HttpRequest request = context.Request;

            using TextReader reader = new StreamReader(request.Body);
            
            string bodyString = await reader.ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(bodyString))
            {
                _logger.LogError("No body was found in request's body");
            }

            Message message = GetResponseFromBodyString(bodyString);

            var url = "https://www.ansa.it/sito/notizie/mondo/mondo_rss.xml";

            try
            {
                var botClient = new TelegramBotClient("5195273815:AAFGqS7fRNLpkuzJ9fD2uQMbM4MUqykRFa8");

                using var cts = new CancellationTokenSource();

                using var feedReader = XmlReader.Create(url);

                var feed = SyndicationFeed.Load(feedReader);

                var list = feed.Items
                    .OrderByDescending(x => x.PublishDate)
                    .Take(10).Select(a => new
                    {
                        Title = a.Title.Text,
                        PublishDate = a.PublishDate,
                        Link = a.Id,
                    });

                var newsString = "";

                //var newsString = feed.Items
                //    .OrderByDescending(x => x.PublishDate)
                //    .Take(10)
                //    .Aggregate("", (cumulator, current) => cumulator + $"{current.Title} - {current.PublishDate} - {current.Id}");


                foreach (var lists in list)
                {
                    newsString += lists.Title + " " + lists.PublishDate + " " + lists.Link + "\n";
                }

                Message sentMessage = await botClient.SendTextMessageAsync(
                    chatId: message.Chat.Id,
                    text: newsString,
                    cancellationToken: cts.Token);

                cts.Cancel();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message, exception);
            }      
        }

        Message GetResponseFromBodyString(string bodyString)
        {
            var message = new Message();

            JsonElement json = JsonSerializer.Deserialize<JsonElement>(bodyString);

            if(json.TryGetProperty("message", out JsonElement messageElement))
            {
                if(messageElement.TryGetProperty("text", out JsonElement textElement) 
                    && textElement.ValueKind == JsonValueKind.String)
                {
                    message.Text = textElement.GetString();
                }

                if(messageElement.TryGetProperty("chat", out JsonElement chatElement)
                    && chatElement.ValueKind == JsonValueKind.Number)
                {
                    message.Chat.Id = chatElement.GetInt64();
                }
            }

            return message;
        }
    }
}