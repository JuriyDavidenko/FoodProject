using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Absolutly;
using System.Security.Cryptography;

namespace YandexEdaBot
{
    public static class DataBase
    {
        private static string lastCourDbHash = "";

        public static List<Courier> LoadCourers(string path = StaticData.PATH_COUR_DB)
        {
            var cours = new List<Courier>();
            var doc = new Excel();
            lastCourDbHash = ComputeMD5Checksum(path);
            doc.FileOpen(path);
            var rows = doc?.Rows?.Skip(1);
            if (rows == null || rows.Count() < 1)
            {
                return null;
            }
            foreach (var row in rows)
            {
                var id = (row.ElementAtOrDefault(0) ?? "").ToLong();
                var userName = row.ElementAtOrDefault(1) ?? "";
                var link = row.ElementAtOrDefault(2) ?? "";
                var status = row.ElementAtOrDefault(3) ?? "";
                var user = new Courier(id, userName, link);
                switch (status.Trim().ToLower())
                {
                    case "wait":
                        user.UserState = UserState.WaitLink;
                        break;
                    case "check":
                        user.UserState = UserState.CheckLink;
                        break;
                    case "free":
                        user.UserState = UserState.Free;
                        break;
                    default:
                        user.UserState = UserState.None;
                        break;
                }
                cours.Add(user);
            }
            return cours;
        }

        public static void SaveCourers(string path = StaticData.PATH_COUR_DB)
        {
            var doc = new Excel();
            doc.FileOpen(path);
            doc.Rows.Clear();
            doc.AddRow(new[] { "id", "username", "link", "status" });
            foreach (var cour in Courier.Couriers)
            {
                doc.AddRow(cour.Peek());
            }
            doc.FileSave(path);
            lastCourDbHash = ComputeMD5Checksum(path);
        }

        public static List<Courier> TryUpdateCourers(string path = StaticData.PATH_COUR_DB)
        {
            var curSum = ComputeMD5Checksum(path);
            if (curSum != lastCourDbHash)
            {
                return LoadCourers();
            }
            return null;
        }

        private static string ComputeMD5Checksum(string path)
        {
            using (FileStream fs = File.OpenRead(path))
            {
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] fileData = new byte[fs.Length];
                fs.Read(fileData, 0, (int)fs.Length);
                byte[] checkSum = md5.ComputeHash(fileData);
                string result = BitConverter.ToString(checkSum).Replace("-", string.Empty);
                return result;
            }
        }
    }
}
