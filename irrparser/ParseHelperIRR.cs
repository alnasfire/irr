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
    class ParseHelperIRR
    {
        private static WebClient wClient = new WebClient();        
        private static HtmlDocument document = new HtmlDocument();

        private static List<String> ParseAdvertLinks()  //Parsing advert links and making list of links from irr.by
        {
            List<String> links = new List<string>();
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            Console.WriteLine("ParseAdvertLinks is begining");
            for (int j = 1; j < 61; j++)
            {
                if (j < 2)
                {
                    document.LoadHtml(wClient.DownloadString(string.Format("http://irr.by/realestate/longtime/")));
                }
                else
                {
                    document.LoadHtml(wClient.DownloadString(string.Format("http://irr.by/realestate/longtime/page" + j + "/")));
                }

                if (document.DocumentNode != null)
                {
                    HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[10]/div/div/div/div[5]/div[4]/div[4]/table/tbody/tr");
                    if (nodes != null)
                    {
                        for (int i = 1; i < nodes.Count; i++)
                        {
                            if (i % 2 != 0)
                            {
                                HtmlNode trs = document.DocumentNode.SelectSingleNode("/html/body/div[10]/div/div/div/div[5]/div[4]/div[4]/table/tbody/tr[" + i + "]/td[2]/div/a");
                                if (trs != null)
                                {
                                    links.Add("http://irr.by" + trs.Attributes["href"].Value);
                                }
                            }
                        }
                    }
                }
            }
            Console.WriteLine("ParseAdvertLinks finished");
            return links;
        }

        public static List<String> ParseInfoAdvert()    //Parsing info about adverts from main table which contains all adverts DEPRICATED
        {
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            List<String> data = new List<string>();
            String str = "";

            for (int j = 1; j < 51; j++)
            {
                if (j < 2)
                {
                    document.LoadHtml(wClient.DownloadString(string.Format("http://irr.by/realestate/longtime/")));
                }
                else
                {
                    document.LoadHtml(wClient.DownloadString(string.Format("http://irr.by/realestate/longtime/page" + j + "/")));
                }

                if (document.DocumentNode != null)
                {                                                   
                    HtmlNodeCollection nodes = document.DocumentNode.SelectNodes("/html/body/div[10]/div/div/div/div[5]/div[4]/div[4]/table/tbody/tr");
                    if (nodes != null)
                    {
                        for (int i = 1; i < nodes.Count; i++)
                        {
                            HtmlNode trs = document.DocumentNode.SelectSingleNode("/html/body/div[10]/div/div/div/div[5]/div[4]/div[4]/table/tbody/tr[" + i + "]/td[2]");
                            if (trs != null)
                                str = trs.InnerText.Trim();
                            if (!str.Equals("") && !str.Contains("аген") && !str.Contains("по фак"))
                            {
                                data.Add(str);
                            }
                            i++;
                        }
                    }
                }
            }
            return data;
        }

        public static List<Advert> MakeAdvertsList()    //Make list of adverts which contains all adverts objects from irr.by
        {
            List<Advert> adverts = new List<Advert>();
            List<String> links = ParseAdvertLinks();
            Console.WriteLine("MakeAdvertsList is begining");
            Advert advert = null;
            foreach(String url in links)
            {
                try
                {
                    advert = ParseAdvert(url);
                    if (advert != null)
                        adverts.Add(advert);
                }
                catch
                {
                    continue;
                }
            }
            Console.WriteLine("MakeAdvertsList finished");
            return adverts;
        }

        private static Advert ParseAdvert(String url)   // Parsing single advert page from irr.by
        {
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            Advert advert = new Advert();
            document.LoadHtml(wClient.DownloadString(string.Format(url)));
            if (document.DocumentNode != null)
            {
                HtmlNode header = document.DocumentNode.SelectSingleNode("/html/body/div[9]/div/div/div/div[3]/div/div[2]/h1");                
                HtmlNode price = document.DocumentNode.SelectSingleNode("//*[@id=\"priceSelected\"]");
                if (header != null)
                    advert.setHeader(header.InnerText);                
                advert.setPhone(ParsePhones());
                if (price != null)
                    advert.setPrice(price.InnerText);
            }
            HtmlNode flag = document.DocumentNode.SelectSingleNode("/html/body/div[9]/div/div/div/div[4]/div/div[2]/div[7]/div/div[6]/div[5]/div");
            if (flag != null)
                advert.SetAgent(true);
            return advert;
        }

        private static String ParsePhones()     //Parsing phones in single advert and convert it in string
        {
            String phones = "";
            HtmlNode phone = document.DocumentNode.SelectSingleNode("/html/body/div[9]/div/div/div/div[4]/div/div[2]/div[6]/div/div[4]/div/div");
            HtmlNode phone1 = document.DocumentNode.SelectSingleNode("/html/body/div[9]/div/div/div/div[4]/div/div[2]/div[7]/div/div[6]/div[6]/div");
            HtmlNode phone2 = document.DocumentNode.SelectSingleNode("/html/body/div[9]/div/div/div/div[4]/div/div[2]/div[7]/div/div[6]/div[7]/div");
            HtmlNode phone3 = document.DocumentNode.SelectSingleNode("/html/body/div[9]/div/div/div/div[4]/div/div[2]/div[7]/div/div[4]/div/div");

            if (phone1 != null)
                phones += phone1.InnerText;
            if (phone2 != null)
                phones += phone2.InnerText;
            if (phone3 != null && phones.Equals(""))
                phones += phone3.InnerText;
            if (phone != null && phones.Equals(""))
                phones += phone.InnerText;   
            /*String[] ph = File.ReadAllLines("C://Users/nasgor/My Documents/agentsphones.txt", Encoding.UTF8);
            foreach (String s in ph)
            {
                if (phones.Contains(s))
                    return null;
            }*/
            return phones;
        }

        public static List<String> GetIRRAgentsLinks()
        {
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            List<String> list = new List<string>();
            document.LoadHtml(wClient.DownloadString("http://irr.by/psellers/list/realestate/"));
            if (document.DocumentNode != null)
            {
                HtmlNodeCollection links = document.DocumentNode.SelectNodes("//div[@class='wrTxt']/a");                
                if (links != null)
                {
                    foreach (HtmlNode node in links)
                        list.Add(node.Attributes["href"].Value);                    
                }
            }
            return list;
        }

        public static List<String> GetIRRAgentsPhones(List<String> links)
        {
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");            
            List<String> phonesList = new List<string>();
            foreach (String url in links)
            {
                document.LoadHtml(wClient.DownloadString(url));
                if (document.DocumentNode != null)
                {
                    HtmlNode phone = document.DocumentNode.SelectSingleNode("//span[@class='phone']");
                    HtmlNode mobile = document.DocumentNode.SelectSingleNode("//span[@class='mobile']");
                    if (phone != null)
                    {
                        phonesList.Add(phone.InnerText);
                    }
                    if (mobile != null)
                    {
                        phonesList.Add(mobile.InnerText);
                    }
                }
            }
            return phonesList;
        }

        public static void Test() // Method for testing new functions DEPRICATED
        {

            List<Advert> adverts = MakeAdvertsList();
            List<String> clean = new List<string>();            
            foreach (Advert s in adverts)
            {   
                if (!s.IsAgent())
                    clean.Add(s.MakeString());
            }
            File.WriteAllLines("C://Users/nasgor/My Documents/clear.txt", clean);
        }
    }
}