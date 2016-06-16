using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlattaform.SANOFIProcess.Models
{
    public class SanofiComercialInfoModel : SanofiGeneralInfoModel
    {                        

        public string NIFType { get; set; }

        public string CountsGroupItemId { get; set; }

        public string CountsGroupItemName { get; set; }

        public string TaxClassId { get; set; }

        public string TaxClassName { get; set; }

        public string CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string Ramo { get; set; }

        public string PayCondition { get; set; }

        public int GroupSchemaProvider { get; set; }

        public string ContactName { get; set; }

        public string BuyCod { get; set; }
    }
}
