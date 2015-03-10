using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SessionManager.Models.POLMarketPlace
{
    public class Session_GenericItemInfoModel
    {
        public int ItemInfoId { get; set; }

        public Session_CatalogModel ItemInfoType { get; set; }

        public string Value { get; set; }

        public string LargeValue { get; set; }

        public string ValueName { get; set; }
    }
}
