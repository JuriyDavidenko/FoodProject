using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Absolutly;

namespace YandexEdaBot
{
    public class Courier
    {
        public static List<Courier> Couriers { private set; get; } = new List<Courier>();

        public BotLocalState BotState { set; get; } 

        // рабочий минимум
        public long ChatId { private set; get; }
        public string UserName { private set; get; }
        public string PersonalLink { set; get; }

        // будущие фичи
        public string PhoneNumber { private set; get; }
        public string Metro { private set; get; }
        public bool IsWalker { private set; get; }
        public int Reputation { private set; get; }

        public Courier(long chatid, string userName, string link)
        {
            Console.WriteLine($"Add new courier {userName}");
            Couriers.Add(this);
            BotState = BotLocalState.None;
            ChatId = chatid;
            UserName = userName;
            PersonalLink = link;
        }

        // данные курьера в массив строк
        public string[] Peek()
        {
            return new string[]
            {
               ChatId.ToString(),
               UserName,
               PersonalLink
            };
        }

        // есть ли в базе курьер с таким Id
        public static bool IsAuth(long chatId)
        {
            return Couriers.Any(x => x.ChatId == chatId);
        }

        // найти курьера с нужным Id
        public static Courier FindById(long chatId)
        {
            return Couriers.Where(x => x.ChatId == chatId)?.FirstOrDefault();
        }

        // сохранить базу
        public static void Save(string path = "cours.db")
        {
            using (var sw = new StreamWriter(path, false, Encoding.UTF8))
            {
                foreach (var cour in Couriers)
                {
                    sw.WriteLine(string.Join("\t", cour.Peek()));
                }
                sw.Close();
            }
        }

        // загрузить базу
        public static void Load()
        {
            Couriers = DataBase.LoadCouriers();
        }
    }

    // в каком состоянии бот для конкретно этого юзера
    public enum BotLocalState
    {
        // null
        None,
        // авторизация прошла, делай, что хочешь
        Free,
        // требуется ввести персональную ссылку
        WaitLink,

    }
}
