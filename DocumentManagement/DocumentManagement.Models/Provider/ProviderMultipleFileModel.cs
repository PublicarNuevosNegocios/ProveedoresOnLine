using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderMultipleFileModel
    {
        public string ProviderInfoId { get; set; }

        public bool IsDelete { get; set; }

        public string ProviderInfoUrl { get; set; }

        public string FileName
        {
            get
            {
                string oReturn = string.Empty;
                if (!string.IsNullOrEmpty(ProviderInfoUrl))
                {
                    oReturn = ProviderInfoUrl.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).DefaultIfEmpty(string.Empty).LastOrDefault();
                }
                return oReturn;
            }
        }

        public string Name { get; set; }

        public ProviderMultipleFileModel() { }

        public ProviderMultipleFileModel(string strJson, string ProviderInfoId)
        {
            ProviderMultipleFileModel oObjAux =
                (DocumentManagement.Models.Provider.ProviderMultipleFileModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize
                        (strJson,
                        typeof(ProviderMultipleFileModel));

            if (oObjAux != null)
            {
                this.ProviderInfoId = ProviderInfoId;
                this.IsDelete = oObjAux.IsDelete;
                this.ProviderInfoUrl = oObjAux.ProviderInfoUrl;
                this.Name = oObjAux.Name;
            }
            else
            {
                this.ProviderInfoId = ProviderInfoId;
                this.IsDelete = true;
                this.ProviderInfoUrl = string.Empty;
                this.Name = string.Empty;
            }
        }
    }
}

