using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderFormPartners
    {
        public string ProviderInfoId { get; set; }

        public string IdentificationNumber { get; set; }

        public string FullName { get; set; }

        public string ParticipationPercent { get; set; }

        public bool IsDelete { get; set; }

        public ProviderFormPartners() { }

        public ProviderFormPartners(string strJson, string ProviderInfoId)
        {
            ProviderFormPartners oObjAux =
                (DocumentManagement.Models.Provider.ProviderFormPartners)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize
                        (strJson,
                        typeof(ProviderFormPartners));

            if (oObjAux != null)
            {
                this.ProviderInfoId = ProviderInfoId;
                this.IdentificationNumber = oObjAux.IdentificationNumber;
                this.FullName = oObjAux.FullName;
                this.ParticipationPercent = oObjAux.ParticipationPercent;
                this.IsDelete = oObjAux.IsDelete;
            }
            else
            {
                this.ProviderInfoId = ProviderInfoId;
                this.IdentificationNumber = string.Empty;
                this.FullName = string.Empty;
                this.ParticipationPercent = string.Empty;
                this.IsDelete = false;
            }
        }
    }
}

