using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.ElasticSearchModels
{
    public class ElasticSearchModel
    {
        public Uri Node { get; set; }

        public ConnectionSettings ConnectionSettting { get; set; }

    }
}
