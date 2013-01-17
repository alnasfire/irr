using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace irrparser
{
    class ParseHelperKvartirant
    {
        private static WebClient wClient = new WebClient();        
        private static HtmlDocument document = new HtmlDocument();

        public static List<String> GetAgentsPhones()    //Parsing adverts list from kvartirant.by and making list of agents phones
        {
            List<String> agentPhones = new List<string>();
            List<Advert> adverts = CheckAgentAdverts();
            foreach (Advert a in adverts)
            {
                if (a.getHeader().Contains("аген") || a.getHeader().Contains("Аген") || a.getHeader().Contains("по фак") || a.getHeader().Contains("Свой угол") ||
                    a.getHeader().Contains("Столица XXI век") || a.getHeader().Contains("Информпрогноз") || a.getHeader().Contains("Квартал Сити"))
                {
                    if (!agentPhones.Contains(a.getPhone()))
                        agentPhones.Add(a.getPhone());
                }
            }
            return agentPhones;
        }

        private static List<Advert> CheckAgentAdverts() // Parsing main table of kvartirant.by and making list of all adverts
        {
            List<Advert> adverts = new List<Advert>();
            for (int i = 1; i < 229; i++)
            {
                if (i < 2)
                {
                    document.LoadHtml(wClient.DownloadString(string.Format("http://www.kvartirant.by/rent/flats/")));
                }
                else
                {
                    document.LoadHtml(wClient.DownloadString(string.Format("http://www.kvartirant.by/rent/flats/page/" + i + "/")));
                }

                if (document.DocumentNode != null)
                {
                    Advert a = new Advert();
                    for (int j = 0; j < 10; j++)
                    {
                        HtmlNodeCollection textAdvert = document.DocumentNode.SelectNodes("//div[@class='txt_box2']/p[2]");
                        HtmlNodeCollection phone = document.DocumentNode.SelectNodes("//div[@class='txt_box2']/p[2]/strong");
                        HtmlNodeCollection price = document.DocumentNode.SelectNodes("//div[@class='price_box']/b");
                        if (textAdvert[j] != null)
                            a.setHeader(textAdvert[j].InnerText);
                        if (phone[j] != null)
                            a.setPhone(phone[j].InnerText);
                        if (price[j] != null)
                            a.setPrice(price[j].InnerText);
                        adverts.Add(a);
                    }                   
                }
            }
            return adverts;
        }
    }
}
