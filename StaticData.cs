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
        public const string KB_BTN_MY_DATA = "Мои данные";

        public const string START_MSG = "Привет, это твой персональный помощник для работы. Для авторизации пришли мне ссылку своего графика.";
        public const string LINK_MSG = @"Ты прошел авторизацию. Добро пожаловать в меню, тут ты можешь :
- посмотреть все нужные контакты
- получить ответы на частые вопросы
- заполнить свои любимые смены и краткую анкету
- отписать о проблемах сервиса
- быстро и удобно смотреть выбранные смены";
        public const string FEEDBACK = @"Тут ты можешь:
Написать 'Я хочу пожаловаться + текст' и рассказать о своей проблеме
Написать 'У меня предложение + текст' и предложить свою идею
Написать 'Меня перекинуло Откуда и Куда' и помочь улучшению сервиса.";

        public const string FAQ = "Пока что пусто";
        public const string ABOUT = "Привет, тут ты можешь указать информацию о себе и твои любимые смены";
        public const string MY_DATA = @"Заполни анкету:

Ближайшая станция метро?
Подрабатываешь или основная работа?

Пример:

Невский проспект
Основная";

        public const string HELP = @"👮Супервайзеры: 
для связи в telegram ✏📲 
+7(911) 764-17-73
@Superspb
для связи по телефону 📲 
+7(800) 600-14-10 

🦊Команда логистов: 
для связи ✏📲 
@YaEdaRegions
+7(915) 260-02-28 

📦 По вопросам склада ✏📲 
@Yandex_eda_sklad 

👩Отдел подбора персонала: 
для связи ☎ 
+7(910) 424-80-48 
+7(910) 424-76-71 

🤑Бухгалтер: 
Вопросы связанные с выплатами и оформлением 
ПИШИТЕ✏ в телеграмм:
@accountingZP
89998329837";


        public static readonly string[][] KEYBOARD = new[]
        {
            new [] { KB_BTN_GRAPHIC },
            new [] { KB_BTN_HELP, KB_BTN_FAQ },
            new [] { KB_BTN_FEEDBACK, /*KB_BTN_ABOUT,*/ KB_BTN_MY_DATA },
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
