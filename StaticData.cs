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
        public const string PATH_COUR_DB = "cour.db";

        public static readonly string[] COMMANDS = new[]
        {
            "start"
        };

        public const string HINT_DIALOG = "Нажми одну из кнопок";
        public const string HINT_SMENA = "Нужно выбрать смену!";

        public const string BUTTON_LOG_SUPER_SELPHIEBOT = "Логист/Супервайзер/Селфибот";
        public const string BUTTON_SMENI = "Смены";
        public const string BUTTON_MANUAL = "Мануал";
        public const string BUTTON_MY_DATA = "Мои данные";

        public const string INLINE_LOG = "Написать логисту";


        public static readonly InlineKeyboardMarkup MARKUP_LOG_SUPER_SELPHIEBOT = new InlineKeyboardMarkup(new[] {
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
        });
    }
}
