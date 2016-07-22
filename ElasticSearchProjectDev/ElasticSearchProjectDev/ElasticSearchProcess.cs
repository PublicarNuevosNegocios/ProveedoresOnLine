using ElasticSearchProjectDev.Models;
using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchProjectDev
{
    public static class ElasticSearchProcess
    {
        public static void ElasticSearchInit()
        {
            Uri node = new Uri("http://192.168.0.94:9200/");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex("personsearch");
            ElasticClient client = new ElasticClient(settings);

            Person oPerson = new Person()
            {
                Id = 4,
                Firstname = "SuperRicas",
                Lastname = "papasFritas",
            };

            var Index = client.Index(oPerson);

        }

        public static void SearchPerson(string Name)
        {
            Uri node = new Uri("http://192.168.0.94:9200/");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex("personsearch");
            ElasticClient client = new ElasticClient(settings);
            var searchResults = client.Search<Person>(s => s
            .From(0)
            .Size(20)
            .Query(q => q
                 .Term(p => p.Id, 2)
            ));
        }
    }
}
