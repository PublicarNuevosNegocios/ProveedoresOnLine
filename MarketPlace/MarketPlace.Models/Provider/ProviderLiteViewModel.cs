using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderLiteViewModel
    {
        public ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel RelatedProvider { get; private set; }

        /// <summary>
        /// provider is related for session customer
        /// </summary>
        public bool IsProviderCustomer
        {
            get
            {
                return RelatedProvider != null &&
                        RelatedProvider.RelatedCustomerInfo != null &&
                        RelatedProvider.RelatedCustomerInfo.Any(x => x.Key == MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);
            }
        }

        /// <summary>
        /// Provider status id for session customer
        /// </summary>
        public int ProviderStatusId
        {
            get
            {
                return IsProviderCustomer &&
                        RelatedProvider != null &&
                        RelatedProvider.RelatedCustomerInfo != null ?
                            RelatedProvider.
                            RelatedCustomerInfo[MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId].
                            ItemType.
                            ItemId :
                            0;
            }
        }

        /// <summary>
        /// Provider logo url
        /// </summary>
        public string ProviderLogoUrl
        {
            get
            {
                return RelatedProvider != null &&
                        RelatedProvider.RelatedCompany != null ?
                            RelatedProvider.
                            RelatedCompany.
                            CompanyInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCompanyInfoType.CompanyLogo).
                            Select(x => x.Value).
                            DefaultIfEmpty(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Company_DefaultLogoUrl].Value).
                            FirstOrDefault() :
                            MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Company_DefaultLogoUrl].Value;
            }
        }

        /// <summary>
        /// Provider rate for session customer
        /// </summary>
        public decimal? ProviderRate
        {
            get
            {
                return IsProviderCustomer &&
                        RelatedProvider != null &&
                        RelatedProvider.RelatedCustomerInfo != null ?
                            RelatedProvider.
                            RelatedCustomerInfo[MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId].
                            ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCustomerProviderInfoType.ProviderRate).
                            Select(x => (decimal?)Convert.ToDecimal(x.Value)).
                            DefaultIfEmpty(null).
                            FirstOrDefault() :
                            null;
            }
        }

        public ProviderLiteViewModel(ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oRelatedProvider)
        {
            RelatedProvider = oRelatedProvider;
        }
    }
}
