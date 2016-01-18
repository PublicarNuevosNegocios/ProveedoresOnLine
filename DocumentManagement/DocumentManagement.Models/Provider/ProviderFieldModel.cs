using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderFieldModel
    {
        public bool RenderScripts { get; set; }

        public DocumentManagement.Customer.Models.Form.FieldModel RealtedField { get; set; }

        public List<DocumentManagement.Provider.Models.Provider.ProviderInfoModel> RealtedProviderInfo { get; set; }

        public Dictionary<int, List<DocumentManagement.Provider.Models.Util.CatalogModel>> ProviderOptions { get; set; }

        public string FieldType { get; set; }

        public bool IsModified { get; set; }

        public string ProviderPublicId { get; set; }
    }
}
