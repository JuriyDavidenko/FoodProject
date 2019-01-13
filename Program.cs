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
using U = Absolutly.Utility;

namespace YandexEdaBot
{
    public partial class Program
    {
        private static async void Start(Message msg)
        {
            var chatId = msg.Chat.Id;
            var userName = msg.Chat.Username;
            if (userName.IsNullOrEmpty()) return;
            Console.WriteLine($"{userName} {chatId}");
            // если корректен юзернейм
            if (userName.ToLower().StartsWith("foodfox"))
            {
                // если юзер уже авторизирован
                if (Courier.IsAuth(chatId))
                {
                    var user = Courier.FindById(msg.Chat.Id);
                    await bot.SendTextMessageAsync(chatId, $"Добро пожаловать, {userName}!");
                    switch (user.UserState)
                    {
                        case UserState.Free:
                            ReplyKeyboardMarkup ReplyKeyboard = StaticData.KEYBOARD; 
                            await bot.SendTextMessageAsync(
                                chatId,
                                "Используйте клавиатуру",
                                replyMarkup: ReplyKeyboard);
                            break;
                        default:
                            break;
                    }
                }
                // иначе переход к регистрации
                else
                {
                    var user = new Courier(chatId, userName, "");
                    user.UserState = UserState.WaitLink;
                    Courier.Couriers.AddSmart(user);
                    await bot.SendTextMessageAsync(chatId, "Введите ссылку на персональный график");
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
            var id = msg.Chat.Id;
            var user = Courier.FindById(msg.Chat.Id);
            switch (msg.Text)
            {
                case StaticData.KB_BTN_GRAPHIC:
                    // todo need check
                    var link = Courier.FindById(id)?.PersonalLink;
                    var days = WebParser.GetPage(link).GetDays();
                    var sb = new StringBuilder();
                    foreach (var day in days)
                    {
                        var dayName = day.QuerySelector("p.contentBoldColor").TextContent;
                        var status = day.QuerySelector("p.content").TextContent;
                        sb.AppendLine($"{dayName} {status}");
                    }
                    await bot.SendTextMessageAsync(id, sb.ToString());
                    break;
                case StaticData.KB_BTN_HELP:
                    msg.Text = "Нужна помощь";
                    await bot.ForwardMessageAsync(CHAT_HELP_ID, id, msg.MessageId);
                    break;
                case StaticData.KB_BTN_FAQ:
                    await bot.SendTextMessageAsync(id, StaticData.FAQ);
                    break;
                case StaticData.KB_BTN_FEEDBACK:
                    break;
                case StaticData.KB_BTN_ABOUT:
                    await bot.SendTextMessageAsync(id, U.StrCol(user.Peek()));
                    break;
                default:
                    break;
            }
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
            if (user.UserState == UserState.WaitLink)
            {
                if (text.IsUrl())
                {
                    user.UserState = UserState.CheckLink;
                    user.PersonalLink = text;
                    await bot.SendTextMessageAsync(msg.Chat.Id, "Ваша ссылка проверяется.");
                    DataBase.SaveCourers();
                } else
                {
                    await bot.SendTextMessageAsync(msg.Chat.Id, "Некорректная ссылка!");
                }
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
