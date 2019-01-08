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
using Absolutly;

namespace YandexEdaBot
{
    public partial class Program
    {
        private static async void Start(Message msg)
        {
            var chatId = msg.Chat.Id;
            var userName = msg.Chat.Username;
            Console.WriteLine($"{userName} {chatId}");
            // если корректен юзернейм
            if (userName.StartsWith("foodfox"))
            {
                // если юзер уже авторизирован
                if (Courier.IsAuth(chatId))
                {
                    await bot.SendTextMessageAsync(chatId, $"Добро пожаловать, {userName}!");
                }
                // иначе переход к регистрации
                else
                {
                    await bot.SendTextMessageAsync(chatId, "Введите ссылку на персональный график");
                    Courier.FindById(msg.Chat.Id).UserState = UserState.WaitLink;
                    DataBase.SaveCourers();
                }
            }
            // не корректный юзернейм
            else
            {
                await bot.SendTextMessageAsync(chatId, "Ваш юзернейм некорректен!");
            }
        }

        private static async void KeyboardHandler(Message msg)
        {

        }

        private static async void TextHandler(Message msg)
        {
            var text = msg.Text.Trim().ToLower();
            var user = Courier.FindById(msg.Chat.Id);
            if (user == null)
            {
                await bot.SendTextMessageAsync(msg.Chat.Id, "Вы не зарегистрированы!");
                return;
            }
            if (user.UserState == UserState.WaitLink && text.IsUrl())
            {
                user.UserState = UserState.CheckLink;
                await bot.SendTextMessageAsync(msg.Chat.Id, "Ваша ссылка проверяется.");
                DataBase.SaveCourers();
            }
        }

        private static async void CommandHandler(Message msg)
        {
            var com = msg.Text.Trim().Substring(1).ToLower();
            switch (com)
            {
                case "start":
                    Start(msg);
                    break;
                default:
                    break;
            }
        }
    }
}
