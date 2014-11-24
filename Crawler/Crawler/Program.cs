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
            var lstCompany =
                Crawler.Manager.Models.InternalSettings.Instance[Constants.C_Settings_CompanyList].Value
                .Split(';').
                Where(x => x.Contains(",")).
                Select(x =>
                    new
                    {
                        key = Convert.ToInt32(x.Split(',')[0].Replace(" ", "")).ToString(),
                        val = x.Split(',')[1].Replace(" ", "")
                    });

            lstCompany.All(x =>
            {
                Crawler.Manager.CrawlerManager.CrawlerBasicInfo(x.key, x.val);
                return true;
            });

            //crawl provider info
            //Crawler.Manager.CrawlerManager.CrawlerBasicInfo("2056", "ADAC5173");
            Console.WriteLine("\n Presione cualquier tecla...");
            //Console.ReadLine();
        }
    }
}
