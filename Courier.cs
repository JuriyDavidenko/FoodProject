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

        private UserState userState = UserState.None;

        public UserState UserState { set
            {
                Console.WriteLine($"{UserName} {userState} -> {value}");
                userState = value;
            }
            get { return userState; } } 

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
            ChatId = chatid;
            UserName = userName;
            PersonalLink = link;
        }

        // данные курьера в массив строк
        public string[] Peek()
        {
            var status = "none";
            switch (userState)
            {
                case UserState.WaitLink:
                    status = "wait";
                    break;
                case UserState.CheckLink:
                    status = "check";
                    break;
                case UserState.Free:
                    status = "free";
                    break;
                default:
                    break;
            }
            return new string[]
            {
               ChatId.ToString(),
               UserName,
               PersonalLink,
               status
            };
        }

        public override string ToString()
        {
            return $"{ChatId} {UserName} {PersonalLink} {userState}";
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
            DataBase.TryUpdateCourers();
        }

        // загрузить базу
        public static void Load()
        {
            var temp = DataBase.TryUpdateCourers();
            if (temp != null)
                Couriers = temp;
        }
    }

    // в каком состоянии бот для конкретно этого юзера
    public enum UserState
    {
        // null
        None,
        // требуется ввести персональную ссылку
        WaitLink,
        // ссылка проверяется
        CheckLink,
        // авторизация прошла, делай, что хочешь
        Free,
    }

    public static class CListEx
    {
        public static void AddSmart(this List<Courier> list, Courier c)
        {
            if (list.Contains(c))
            {
                return;
            }
            list.Add(c);
        }
    }
}
