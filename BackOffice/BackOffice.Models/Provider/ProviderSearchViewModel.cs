using BackOffice.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class ProviderSearchViewModel
    {
        public ProveedoresOnLine.Company.Models.Company.CompanyModel RelatedCompany { get; private set; }

        public int TotalRows { get; set; }

        public string ImageUrl
        {
            get
            {
                return RelatedCompany.CompanyInfo.
                    Where(x => x.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumCompanyInfoType.CompanyLogo).
                    Select(x => x.Value).
                    DefaultIfEmpty(BackOffice.Models.General.InternalSettings.Instance[BackOffice.Models.General.Constants.C_Settings_DefaultImage].Value).
                    FirstOrDefault();
            }
        }

        public string ProviderPublicId { get { return RelatedCompany.CompanyPublicId; } }

        public string ProviderName { get { return RelatedCompany.CompanyName; } }

        public string ProviderType { get { return RelatedCompany.CompanyType.ItemName; } }

        public string IdentificationType { get { return RelatedCompany.IdentificationType.ItemName; } }

        public string IdentificationNumber { get { return RelatedCompany.IdentificationNumber; } }

        public bool IsOnRestrictiveList 
        { get 
            { return RelatedCompany.CompanyInfo.
                Where(x => x.ItemInfoType.ItemId == (int)enumCompanyInfoType.Alert).Select(x => Convert.ToInt32(x.Value) == (int)enumBlackList.BL_ShowAlert ? true:false).FirstOrDefault()
                ; 
            }
        }

        public bool Enable { get { return RelatedCompany.Enable; } }

        public ProviderSearchViewModel(ProveedoresOnLine.Company.Models.Company.CompanyModel oRelatedCompany, int oTotalRows)
        {
            RelatedCompany = oRelatedCompany;
            TotalRows = oTotalRows;
        }
    }
}
