using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocumentManagement.Models.Provider
{
    public class ProviderItemSearchModel
    {
        public DocumentManagement.Provider.Models.Provider.ProviderModel RelatedProvider { get; set; }

        public string FormUrl
        {
            get
            {
                if (RelatedProvider != null)
                {
                    return "https://" + System.Web.HttpContext.Current.Request.Url.Host +":"+ System.Web.HttpContext.Current.Request.Url.Port + "/ProviderForm/Index?ProviderPublicId=" + RelatedProvider.ProviderPublicId + "&FormPublicId=" + RelatedProvider.FormPublicId;
                }
                //https://localhost:44306/ProviderForm/Index?ProviderPublicId=184C7F1E&FormPublicId=176BF266
                return string.Empty;
            }
        }

    }
}
