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

        private List<String> GetMainTableLinks()
        {
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            List<String> links = new List<string>();
            //Dictionary<String, String> adverts = new Dictionary<string, string>();
            for (int i = 1; i < 415; i ++)
            {
                document.LoadHtml(wClient.DownloadString("http://realt.open.by/results/?tableid=2001&sid=0&type=2&page="+i+"&records-per-page=10&expand-shortcuts=1&town_id=5102&sort=x_days_old"));
                
                HtmlNodeCollection linkNodes = document.DocumentNode.SelectNodes("//div[@class='streetInfo']/a");
                                            
                foreach (HtmlNode node in linkNodes)
                {
                    links.Add(node.Attributes["href"].Value);
                }
                //HtmlNodeCollection textNodes = document.DocumentNode.SelectNodes("//div[@class='des']");
                /*List<String> texts = new List<string>();
                foreach (HtmlNode node in textNodes)
                {
                    texts.Add(node.InnerText);
                }
                for (int j = 0; j < links.Count; j++)
                {
                    Console.WriteLine("http://realt.open.by" + links[j]);
                    adverts.Add(links[j], texts[j]);
                }*/
            }
            return links;
        }

        public static Advert ParseAdvert(String url)
        {
            Advert advert = new Advert();
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            document.LoadHtml(wClient.DownloadString("http://realt.open.by/viewdetails.aspx?tableid=2001&type=4&unid=R2001CLXD6CZ"));
            HtmlNode row = document.DocumentNode.SelectSingleNode("/html/body/form/div[3]/div[2]/div/div/div[5]/table/tbody/tr/td[2]");
            //bool flag = false;
           
            //foreach (HtmlNode n in rows)
            //{
                /*if (n.InnerText.Contains("Аген"))
                    advert.SetAgent(true);
                if (flag)
                {
                    advert.setPhone(n.InnerText);
                    flag = false;
                }
                if (n.InnerText.Contains("Телеф"))
                {
                    flag = true;                        
                } */
            if(row != null)
                Console.WriteLine(row.InnerText);       
            //}
                       
            return advert;
        }

        public List<Advert> MakeAdvertsList()
        {
            List<Advert> adverts = new List<Advert>();
            return adverts;
        }
    }
}
