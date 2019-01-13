using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Parser;
using System.Net;
using AngleSharp.Html.Dom;
using AngleSharp.Dom;
using Absolutly;
using OpenQA.Selenium.Chrome;

namespace YandexEdaBot
{
    public static class WebParser
    {
        private static string GetSource(string url)
        {
            var chrome = new ChromeDriver("C:/ChromeDriver/");
            chrome.Url = url;
            var source = chrome.PageSource;
            return source;
        }

        public static IHtmlDocument GetPage(string url)
        {
            var parser = new HtmlParser();
            var tmp = GetSource(url);
            var doc = parser.ParseDocument(tmp);
            return doc;
        }
    }

    public static class DocEx
    {
        public static List<IElement> GetDays(this IDocument page)
        {
            return page.QuerySelectorAll("td > div").Skip(1).ToList();
        }
    }
}
