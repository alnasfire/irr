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
    class ParseHelper
    {
        private static WebClient wClient = new WebClient();        
        private static HtmlDocument document = new HtmlDocument();

        private static List<String> ParseAdvertLinks()
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

        public static List<String> ParseInfoAdvert()
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

        public static List<Advert> MakeAdvertsList()
        {
            List<Advert> adverts = new List<Advert>();
            List<String> links = ParseAdvertLinks();
            Console.WriteLine("MakeAdvertsList is begining");
            foreach(String url in links)
            {
                try
                {
                    adverts.Add(ParseAdvert(url));
                }
                catch
                {
                    continue;
                }
            }
            Console.WriteLine("MakeAdvertsList finished");
            return adverts;
        }

        private static Advert ParseAdvert(String url)
        {
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            Advert advert = new Advert();
            document.LoadHtml(wClient.DownloadString(string.Format(url)));
            if (document.DocumentNode != null)
            {
                HtmlNode header = document.DocumentNode.SelectSingleNode("/html/body/div[9]/div/div/div/div[3]/div/div[2]/h1");
                HtmlNode phone = document.DocumentNode.SelectSingleNode("/html/body/div[9]/div/div/div/div[4]/div/div[2]/div[6]/div/div[4]/div/div");
                HtmlNode price = document.DocumentNode.SelectSingleNode("//*[@id=\"priceSelected\"]");
                if (header != null)
                    advert.setHeader(header.InnerText);
                if (phone != null)
                    advert.setPhone(ParsePhones(phone));
                if (price != null)
                    advert.setPrice(price.InnerText);
            }
            return advert;
        }

        private static String ParsePhones(HtmlNode phoneNode)
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
            return phones;
        }

        public static void Test()
        {
            String phones = "";
            wClient.Proxy = null;
            wClient.Encoding = System.Text.Encoding.GetEncoding("utf-8");
            Advert advert = new Advert();

            document.LoadHtml(wClient.DownloadString(string.Format("http://irr.by/realestate/longtime/8946908/")));
            if (document.DocumentNode != null)
            {
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
                Console.WriteLine(phones);
            }           
        }
    }
}
