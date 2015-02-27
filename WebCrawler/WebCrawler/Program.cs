using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebCrawler.Manager.General;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            var lstCompany =
                WebCrawler.Manager.General.InternalSettings.Instance[Constants.C_Settings_Crawler_CompanyList].Value.
                Split(';').
                Where(x => x.Contains(",")).
                Select(x =>
                new
                {
                    key = Convert.ToInt32(x.Split(',')[0].Replace(" ", "")).ToString(),
                    val = x.Split(',')[1].Replace(" ", ""),
                });

            lstCompany.All(x =>
                {
                    //try
                    //{

                        WebCrawler.Manager.WebCrawlerManager.WebCrawlerInfo(x.key, x.val);
                    //}
                    //catch (Exception e)
                    //{
                    //    Console.WriteLine("\nError, ::" + x.key.ToString() + "::" + e.Message + "\n");
                    //}
                    return true;
                });

            Console.ReadLine();
        }
    }
}
