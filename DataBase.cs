using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Absolutly;

namespace YandexEdaBot
{
    public static class DataBase
    {
        public static List<Courier> LoadCouriers(string path = "cours.db")
        {
            var cours = new List<Courier>(); 
            using (var sr = new StreamReader(path, Encoding.UTF8))
            {
                string line;
                while (!sr.EndOfStream)
                {
                    line = sr.ReadLine();
                    if (!line.IsNullOrEmpty())
                    {
                        var args = line.Split('\t');
                        cours.Add(new Courier(long.Parse(args[0]), args[1], args[2]));
                    }
                }
                sr.Close();
            }
            return cours;
        }
    }
}
