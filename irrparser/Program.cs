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
            ParseHelperIRR.Test();
                        
            Console.WriteLine("Finished. Pess any key...");
            Console.ReadKey();
        }
    }
}
