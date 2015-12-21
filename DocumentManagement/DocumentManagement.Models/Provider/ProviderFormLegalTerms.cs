using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderFormLegalTerms
    {
        public ProviderFormLegalTermsResource Rsx { get; set; }

        public ProviderFormLegalTermsData data { get; set; }

        public ProviderFormLegalTerms() { }

        public ProviderFormLegalTerms(string strJson, string strResource, string ProviderInfoId)
        {
            if (!string.IsNullOrEmpty(strResource))
            {
                this.Rsx = new ProviderFormLegalTermsResource(strResource);
            }
            if (!string.IsNullOrEmpty(strJson))
            {
                this.data = new ProviderFormLegalTermsData(strJson, ProviderInfoId);
            }
        }
    }

    public class ProviderFormLegalTermsResource
    {
        public string txtHeader { get; set; }

        public string txtCheckCommercial { get; set; }

        public string txtCheckRestrictiveList { get; set; }

        public string txtFooter { get; set; }

        public ProviderFormLegalTermsResource() { }

        public ProviderFormLegalTermsResource(string strJson)
        {
            ProviderFormLegalTermsResource oObjAux =
                (DocumentManagement.Models.Provider.ProviderFormLegalTermsResource)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize
                        (strJson,
                        typeof(ProviderFormLegalTermsResource));

            this.txtHeader = oObjAux.txtHeader;
            this.txtCheckCommercial = oObjAux.txtCheckCommercial;
            this.txtCheckRestrictiveList = oObjAux.txtCheckRestrictiveList;
            this.txtFooter = oObjAux.txtFooter;
        }
    }

    public class ProviderFormLegalTermsData
    {
        public string ProviderInfoId { get; set; }

        public bool CheckData { get; set; }

        public bool CheckCommercial { get; set; }

        public bool CheckRestrictiveList { get; set; }

        public ProviderFormLegalTermsData() { }

        public ProviderFormLegalTermsData(string strJson, string ProviderInfoId)
        {
            ProviderFormLegalTermsData oObjAux =
                (DocumentManagement.Models.Provider.ProviderFormLegalTermsData)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize
                        (strJson,
                        typeof(ProviderFormLegalTermsData));

            this.ProviderInfoId = ProviderInfoId;
            this.CheckData = oObjAux.CheckData;
            this.CheckCommercial = oObjAux.CheckCommercial;
            this.CheckRestrictiveList = oObjAux.CheckRestrictiveList;
        }
    }
}

