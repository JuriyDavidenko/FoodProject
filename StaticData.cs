using System;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace YandexEdaBot
{
    // константы
    public static class StaticData
    {
        public const string PATH_COUR_DB = "Курьеры.xlsx";

        public static readonly string[] COMMANDS = new[]
        {
            "start"
        };

        public const string KB_BTN_GRAPHIC = "График";
        public const string KB_BTN_HELP = "Помощь";
        public const string KB_BTN_FAQ = "FAQ";
        public const string KB_BTN_FEEDBACK = "Обратная связь";
        public const string KB_BTN_ABOUT = "О себе";

        public const string FAQ = "тут будет виджет";


        public static readonly string[][] KEYBOARD = new[]
        {
            new [] { KB_BTN_GRAPHIC },
            new [] { KB_BTN_HELP, KB_BTN_FAQ },
            new [] { KB_BTN_FEEDBACK, KB_BTN_ABOUT },
        };


        /*public static readonly InlineKeyboardMarkup MARKUP_LOG_SUPER_SELPHIEBOT = new InlineKeyboardMarkup(new[] {
            new[]
            {
                InlineKeyboardButton.WithCallbackData(INLINE_LOG),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Написать супервайзеру"),
            },
            new[]
            {
                InlineKeyboardButton.WithCallbackData("Селфибот"),
            }
        });*/
    }
}
