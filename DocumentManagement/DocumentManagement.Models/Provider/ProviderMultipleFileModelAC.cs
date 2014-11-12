﻿using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderMultipleFileModelAC
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

        public ProviderMultipleFileModelAC() { }

        public ProviderMultipleFileModelAC(string strJson, string ProviderInfoId)
        {
            ProviderMultipleFileModel oObjAux =
                (DocumentManagement.Models.Provider.ProviderMultipleFileModel)
                    (new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize
                        (strJson,
                        typeof(ProviderMultipleFileModel));

            this.ProviderInfoId = ProviderInfoId;
            this.IsDelete = oObjAux.IsDelete;
            this.ProviderInfoUrl = oObjAux.ProviderInfoUrl;
        }

        public Dictionary<int, List<DocumentManagement.Provider.Models.Util.CatalogModel>> ProviderOptions { get; set; }
    }
}

