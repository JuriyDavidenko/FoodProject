using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace YandexEdaBot
{
    public partial class Program
    {
        private static async void Start(Message msg)
        {
            var chatId = msg.Chat.Id;
            var userName = msg.Chat.Username;
            // если корректен юзернейм
            if (userName.StartsWith("foodfox"))
            {
                // если юзер уже авторизирован
                if (Courier.IsAuth(chatId))
                {
                    User = Courier.FindById(chatId);
                    await bot.SendTextMessageAsync(chatId, $"Добро пожаловать, {userName}!");
                }
                // иначе переход к регистрации
                else
                {
                    await bot.SendTextMessageAsync(chatId, "Введите ссылку на персональный график");

                }
            } else
            {
                await bot.SendTextMessageAsync(chatId, "Ваш юзернейм некорректен!");
            }
        }

        private static async void KeyboardHandler(Message msg)
        {

        }

        private static async void TextHandler(Message msg)
        {

        }

        private static async void CommandHandler(Message msg)
        {
            var com = msg.Text.Trim().Substring(1).ToLower();
            switch (com)
            {
                case "/start":
                    Start(msg);
                    break;
                default:
                    break;
            }
        }
    }
}
