using BackOffice.Models.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Customer
{
    public class CustomerSearchViewModel
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

        public string CustomerPublicId { get { return RelatedCompany.CompanyPublicId; } }

        public string CustomerName { get { return RelatedCompany.CompanyName; } }

        public string CustomerType { get { return RelatedCompany.CompanyType.ItemName; } }

        public string IdentificationType { get { return RelatedCompany.IdentificationType.ItemName; } }

        public string IdentificationNumber { get { return RelatedCompany.IdentificationNumber; } }

        public bool Enable { get { return RelatedCompany.Enable; } }

        public CustomerSearchViewModel(ProveedoresOnLine.Company.Models.Company.CompanyModel oRelatedCompany, int oTotalRows)
        {
            RelatedCompany = oRelatedCompany;
            TotalRows = oTotalRows;
        }
    }
}
