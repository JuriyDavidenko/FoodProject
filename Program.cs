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
                    await bot.SendTextMessageAsync(chatId, StaticData.START_MSG);
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
                    // todo need check and more
                    var link = Courier.FindById(id)?.PersonalLink;
                    var days = WebParser.GetPage(link).GetDays();
                    var sb = new StringBuilder();
                    foreach (var day in days)
                    {
                        sb.AppendLine(day.GetDayData());
                        sb.AppendLine();
                    }
                    var text = sb.ToString();
                    await bot.SendTextMessageAsync(id, text.Replace("Локация старта:", "🗺").Replace("метро", "🚇").Replace("Метро", "🚇").Replace("Время", "⏰"));
                    break;
                case StaticData.KB_BTN_FEEDBACK:
                    await bot.SendTextMessageAsync(id, StaticData.FEEDBACK);
                    Courier.FindById(id).PressHelp = true;
                    break;
                case StaticData.KB_BTN_FAQ:
                    // todo faq
                    await bot.SendTextMessageAsync(id, StaticData.FAQ);
                    break;
                case StaticData.KB_BTN_HELP:
                    await bot.SendTextMessageAsync(id, StaticData.HELP);
                    break;
                case StaticData.KB_BTN_ABOUT:
                    await bot.SendTextMessageAsync(id, StaticData.ABOUT);
                    break;
                case StaticData.KB_BTN_MY_DATA:
                    await bot.SendTextMessageAsync(id, StaticData.MY_DATA);
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
                if (text.IsRegexSuccess("https?://grafik-beta\\.foodfox\\.ru/login_link\\.html\\?id=.+"))
                {
                    user.UserState = UserState.Free;
                    user.PersonalLink = text;
                    ReplyKeyboardMarkup ReplyKeyboard = StaticData.KEYBOARD;
                    await bot.SendTextMessageAsync(
                        msg.Chat.Id,
                        StaticData.LINK_MSG,
                        replyMarkup: ReplyKeyboard);
                    DataBase.SaveCourers();
                } else
                {
                    await bot.SendTextMessageAsync(msg.Chat.Id, "Некорректная ссылка!");
                }
            }
            else if (user.PressHelp)
            {
                user.PressHelp = false;
                await bot.ForwardMessageAsync(CHAT_FEEDBACK_ID, msg.Chat.Id, msg.MessageId);
                await bot.SendTextMessageAsync(msg.Chat.Id, "Вашее сообщение отправленно.");
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
