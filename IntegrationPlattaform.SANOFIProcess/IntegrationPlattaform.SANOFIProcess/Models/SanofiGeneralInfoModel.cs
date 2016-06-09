using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlattaform.SANOFIProcess.Models
{
    public class SanofiGeneralInfoModel
    {
        public int CompanyId { get; set; }

        public string CompanyName { get; set; }

        public string ComercialName { get; set; }

        public string NaturalPersonName { get; set; }

        public int IdentificationNumber { get; set; }

        public int FiscalNumber { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public int Region { get; set; }

        public string Country { get; set; }

        public string PhoneNumber { get; set; }

        public string Fax { get; set; }

        public string Email_OC { get; set; }

        public string Email_P { get; set; }

        public string Email_Cert { get; set; }

        public string Comentaries { get; set; }
    }
}
