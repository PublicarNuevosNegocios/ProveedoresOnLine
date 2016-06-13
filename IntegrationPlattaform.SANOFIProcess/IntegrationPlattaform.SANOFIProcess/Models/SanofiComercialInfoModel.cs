using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlattaform.SANOFIProcess.Models
{
    public class SanofiComercialInfoModel : SanofiGeneralInfoModel
    {                        

        public int NIF_Type { get; set; }

        public string CountsGroupItemId { get; set; }

        public string CountsGroupItemName { get; set; }

        public int TaxClassId { get; set; }

        public string TaxClassName { get; set; }

        public string CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string GroupSchemaProvider { get; set; }

        public string ContactName { get; set; }

        public string ComprasCod { get; set; }
    }
}
