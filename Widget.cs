using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace YandexEdaBot
{
    public class Widget
    {
        private static Dictionary<int, Widget> widgets = new Dictionary<int, Widget>();
        private static int CounterTop = 0;

        private InlineKeyboardMarkup Markup;
        private int Id;
        public string Text { private set; get; }

        public Widget(string defaultStr)
        {
            Id = CounterTop++;
            Text = defaultStr;
            var leftBtn = InlineKeyboardButton.WithCallbackData($"l{Id}");
            var rightBtn = InlineKeyboardButton.WithCallbackData($"r{Id}");
            leftBtn.Text = "Влево";
            rightBtn.Text = "Вправо";
            Markup = new InlineKeyboardMarkup(new[] {
                new[] {leftBtn, rightBtn},
            });
            widgets.Add(Id, this);
        }

        public void LeftClick(Action<Widget> me)
        {
            me(this);
        }

        public void RightClick(Action<Widget> me)
        {
            me(this);
        }

        public async void Show(TelegramBotClient bot, long chatId)
        {
            await bot.SendTextMessageAsync(
                        chatId,
                        Text,
                        replyMarkup: Markup);
        }

        public static Widget Get(int id)
        {
            return widgets[id];
        }
    }
}
