using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace YandexEdaBot
{
    public partial class Program
    {
        public static Telegram.Bot.TelegramBotClient bot;
        public static Courier User;

        static void Main()
        {
            bot = new Telegram.Bot.TelegramBotClient("723889901:AAFSFuCmJI62q876qMXOnrZxjGssu6EhI00");

            // загрузка базы курьеров
            Courier.Load();
            Console.WriteLine("База загружена");

            //var watcher = new FileSystemWatcher(Environment.CurrentDirectory);
            //watcher.Created += Watcher_Created;
            //watcher.Changed += Watcher_Changed;
            //watcher.EnableRaisingEvents = true;

            // события TelegramAPI и их обработчики
            bot.OnMessage += BotOnMessageReceived;
            bot.OnMessageEdited += BotOnMessageReceived;
            bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            bot.OnInlineQuery += BotOnInlineQueryReceived;
            bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            bot.OnReceiveError += BotOnReceiveError;

            bot.StartReceiving(Array.Empty<UpdateType>());
            Console.ReadLine();
            bot.StopReceiving();
        }

        /*private static void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.Name == "отчет.txt")
            
                var names = ParseReport("отчет.txt");
                foreach (var name in names)
                {
                    var cour = Courier.Couriers.Find(x => x.UserName == name);
                    if (cour != null)
                    {
                        bot.SendTextMessageAsync(cour.ChatId, StaticData.HINT_SMENA);
                    }
                }
            }
        }

        private static void Watcher_Created(object sender, FileSystemEventArgs e)
        {
            if (e.Name == "отчет.txt")
            {
                var names = ParseReport("отчет.txt");
                foreach (var name in names)
                {
                    var cour = Courier.Couriers.Find(x => x.UserName == name);
                    if (cour != null)
                    {
                        bot.SendTextMessageAsync(cour.ChatId, "Выбирай смену!");
                    }
                }
            }
        }*/

        // обработка сообщений юзера
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            // сообщение существует и является текстом
            if (message == null || message.Type != MessageType.Text) return;

            // костыль, чтобы не было ошибки, если юзера еще нет, а кидало сразу на старт
            if (User == null)
            {
                Start(message);
            }
            // является нажатием кнопки клавиатуры
            else if (IsKeyboardKey(message.Text))
            {
                // обработчик кнопок
                KeyboardHandler(message);
            }
            // команда типа /start и т.д.
            else if (IsCommand(message.Text))
            {
                // обработчик команд
                CommandHandler(message);
            }
            // просто введенный текст
            else
            {
                // обработчик текста
                TextHandler(message);
            }
        } 

        // обработчка нажатий инлайн кнопок
        private static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var query = callbackQueryEventArgs.CallbackQuery;
            switch (query.Data)
            {
                case StaticData.INLINE_LOG:
                    await bot.SendTextMessageAsync(query.Message.Chat.Id, "kek");
                    break;
                default:
                    // вывести тест выбранной кнопки
                    await bot.AnswerCallbackQueryAsync(
                query.Id,
                $"Received {query.Data}");

                    // вывести тест выбранной кнопки
                    await bot.SendTextMessageAsync(
                        query.Message.Chat.Id,
                        $"Received {query.Data}");
                    break;
            }
        }

        private static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        {
            Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");
        }

        // выбор инлайн кнопки
        private static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        private static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }

        // todo
        private static bool IsCommand(string s)
        {
            return true;
        }

        // todo
        private static bool IsKeyboardKey(string s)
        {
            return true;
        }
    }
}
