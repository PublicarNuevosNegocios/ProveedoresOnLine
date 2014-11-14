using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crawler
{
    class Program
    {
        static void Main(string[] args)
        {
            //crawl basic info
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("1733");
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("2248");
            Console.WriteLine("\n Presione cualquier tecla...");
            Console.ReadLine();
        }
    }
}
