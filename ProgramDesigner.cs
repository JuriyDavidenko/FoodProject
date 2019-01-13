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
using Absolutly;

using U = Absolutly.Utility;

namespace YandexEdaBot
{
    public partial class Program
    {
        public static Telegram.Bot.TelegramBotClient bot;
        private const long CHAT_HELP_ID = -255650882;

        static void Main()
        {
            bot = new Telegram.Bot.TelegramBotClient("723889901:AAFSFuCmJI62q876qMXOnrZxjGssu6EhI00");

            // загрузка базы курьеров
            Courier.Load();
            Console.WriteLine("База загружена");

            // события TelegramAPI и их обработчики
            bot.OnMessage += BotOnMessageReceived;
            bot.OnMessageEdited += BotOnMessageReceived;
            bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            bot.OnInlineQuery += BotOnInlineQueryReceived;
            bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            bot.OnReceiveError += BotOnReceiveError;

            bot.StartReceiving(Array.Empty<UpdateType>());


            while (true)
            {
                var input = Console.ReadLine().Trim().ToLower();
                switch (input)
                {
                    case "cours":
                        Console.WriteLine(U.StrCol(Environment.NewLine, Courier.Couriers));
                        break;
                    case "end":
                    case "exit":
                        bot.StopReceiving();
                        return;
                    default:
                        break;
                }
            }
        }

        // обработка сообщений юзера
        private static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            // сообщение существует и является текстом
            if (message == null || message.Type != MessageType.Text) return;

            var user = Courier.FindById(message.Chat.Id);

            // костыль, чтобы не было ошибки, если юзера еще нет, а кидало сразу на старт
            if (user == null)
            {
                if (message.Chat.Id == CHAT_HELP_ID)
                {
                    return;
                } else
                {
                    Start(message);
                    return;
                }
            }

            if (message.Chat.Id == CHAT_HELP_ID) return;

            // является нажатием кнопки клавиатуры
            if (IsKeyboardKey(message.Text))
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
                /*case StaticData.INLINE_LOG:
                    await bot.SendTextMessageAsync(query.Message.Chat.Id, "kek");
                    break;  */
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

        private static bool IsCommand(string s)
        {
            var com = s.Trim().ToLower().Substring(1);
            var res = s[0] == '/' && StaticData.COMMANDS.Contains(com);
            return res;
        }

        private static bool IsKeyboardKey(string s)
        {
            var text = s.Trim();
            foreach (var row in StaticData.KEYBOARD)
            {
                foreach (var btn in row)
                {
                    if (btn == text) return true;
                }       
            }
            return false;
        }
    }
}
