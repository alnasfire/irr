using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;

namespace irrparser
{
    class Program
    {
        static void Main(string[] args)
        {
            /*List<Advert> adverts = ParseHelperIRR.MakeAdvertsList();
            List<String> strs = new List<string>();
            foreach(Advert a in adverts)
            {
                strs.Add(a.MakeString());
            }
            Console.WriteLine("File writing is begining");
            File.WriteAllLines("C://Users/nasgor/My Documents/clear.txt", strs);
            Console.WriteLine("File writing finished");*/

            ParseHelperIRR.Test();
            
            Console.WriteLine("Finished. Pess any key...");
            Console.ReadKey();
        }
    }
}
