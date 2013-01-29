using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;

namespace irrparser
{
    class ParseHelperRealtOpen
    {
        private static WebClient wClient = new WebClient();
        private static HtmlDocument document = new HtmlDocument();

        public List<String> GetMainTableLinks()
        {
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            List<String> links = new List<string>();
            
            for (int i = 1; i < 415; i ++)
            {
                document.LoadHtml(wClient.DownloadString("http://realt.open.by/results/?tableid=2001&sid=0&type=2&page="+i+"&records-per-page=10&expand-shortcuts=1&town_id=5102&sort=x_days_old"));
                
                HtmlNodeCollection linkNodes = document.DocumentNode.SelectNodes("//div[@class='streetInfo']/a");
                                            
                foreach (HtmlNode node in linkNodes)
                {                 
                    links.Add("http://realt.open.by" + node.Attributes["href"].Value);
                }                
            }
            return links;
        }

        public static Advert ParseAdvert(String url)
        {
            Advert advert = new Advert();
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            try
            {
                document.LoadHtml(wClient.DownloadString(url));
                if (document != null)
                {
                    HtmlNode phone = document.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_contactInfo\"]/tr/td[2]");
                    HtmlNode text = document.DocumentNode.SelectSingleNode("//*[@id=\"ctl00_ContentPlaceHolder1_title\"]");
                    HtmlNodeCollection owner = document.DocumentNode.SelectNodes("//*[@id=\"ctl00_ContentPlaceHolder1_contactInfo\"]/tr[2]/td");
                    if (owner != null)
                        foreach (HtmlNode n in owner)
                        {
                            if (n.InnerText.Contains("Аг") || n.InnerText.Contains("факт") || n.InnerText.Contains("аген") || n.InnerText.Contains("аг"))
                                advert.SetAgent(true);
                        }
                    if (phone != null)
                    {                        
                        advert.setPhone(phone.InnerText);
                    }
                    if (text != null)
                    {                     
                        advert.setHeader(text.InnerText);
                    }
                }
            }
            catch
            {
                Console.WriteLine("ERROR    "+url);
            }
            return advert;
        }

        public List<Advert> MakeAdvertsList()
        {            
            List<Advert> adverts = new List<Advert>();
            List<String> links = GetMainTableLinks();
            foreach (String url in links)
            {                
                adverts.Add(ParseAdvert(url));
            }
            return adverts;
        }
    }
}
