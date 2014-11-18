using Crawler.Manager.Models;
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
            //crawl provider info
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("1733");
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("2248");
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("4259");
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("4483");
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("4860");
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("9759");
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("11033");
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("15977");
            Console.WriteLine("\n Presione cualquier tecla...");
            Console.ReadLine();
        }
    }
}
