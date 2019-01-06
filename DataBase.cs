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
            foreach (var row in doc.Rows.Skip(1))
            {
                var id = (row.ElementAtOrDefault(0) ?? "").ToLong();
                var userName = row.ElementAtOrDefault(1) ?? "";
                var link = row.ElementAtOrDefault(2) ?? "";
                cours.Add(new Courier(id, userName, link));
            }
            return cours;
        }

        // todo
        private static void SaveCourers(string path = StaticData.PATH_COUR_DB)
        {
            var doc = new Excel();
            lastCourDbHash = ComputeMD5Checksum(path);
            doc.FileOpen(path);
            foreach (var row in doc.Rows.Skip(1))
            {
                var id = (row.ElementAtOrDefault(0) ?? "").ToLong();
                var userName = row.ElementAtOrDefault(1) ?? "";
                var link = row.ElementAtOrDefault(2) ?? "";
            }
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
                string result = BitConverter.ToString(checkSum).Replace("-", String.Empty);
                return result;
            }
        }
    }
}
