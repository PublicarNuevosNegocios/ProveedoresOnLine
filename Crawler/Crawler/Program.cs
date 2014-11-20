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
            Crawler.Manager.CrawlerManager.CrawlerBasicInfo("9986", "800134773");
            Console.WriteLine("\n Presione cualquier tecla...");
            Console.ReadLine();
        }
    }
}
