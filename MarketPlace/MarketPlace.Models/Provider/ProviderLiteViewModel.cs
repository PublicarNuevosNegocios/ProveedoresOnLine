using ProveedoresOnLine.Company.Models.Company;
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

        public CompanyIndexModel ElasticRealtedProvider { get; private set; }

        /// <summary>
        /// provider is related for session customer
        /// </summary>
        public bool IsProviderCustomer
        {
            get
            {
                return true;
                //return RelatedProvider != null &&
                //        RelatedProvider.RelatedCustomerInfo != null &&
                //        RelatedProvider.RelatedCustomerInfo.Any(x => x.Key == MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId);
            }
        }

        /// <summary>
        /// Provider status id for session customer
        /// </summary>
        public int ProviderStatusId
        {
            get
            {
                if (ElasticRealtedProvider != null)
                {
                    return int.Parse(ElasticRealtedProvider.ProviderStatus);
                }
                else
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
        }

        /// <summary>
        /// Is certified
        /// </summary>
        public bool ProviderIsCertified
        {
            get
            {
                if (ElasticRealtedProvider != null)
                {
                    if (ElasticRealtedProvider.ProviderStatus ==
                    MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_ProviderStatus_Certified].Value ||
                    ElasticRealtedProvider.ProviderStatus == "902004" ||
                    ElasticRealtedProvider.ProviderStatus == "902008")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                if (ProviderStatusId.ToString() ==
                    MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_ProviderStatus_Certified].Value ||
                    ProviderStatusId.ToString() == "902004" ||
                    ProviderStatusId.ToString() == "902008")
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
        }

        /// <summary>
        /// Provider logo url
        /// </summary>
        public string ProviderLogoUrl
        {
            get
            {
                string pic = ElasticRealtedProvider != null ? ElasticRealtedProvider.LogoUrl :
                            MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Company_DefaultLogoUrl].Value;

                if (!string.IsNullOrEmpty(pic))
                {
                    return pic;
                }
                else
                {
                    return MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Company_DefaultLogoUrl].Value;
                }
            }
        }

        /// <summary>
        /// Provider rate for session customer
        /// </summary>
        public decimal ProviderRate
        {
            get
            {
                if (ElasticRealtedProvider != null)
                {
                    return (5 * (ElasticRealtedProvider.CatlificationRating != null ?
                            int.Parse(ElasticRealtedProvider.CatlificationRating) :
                            0) / 100);
                }
                else
                {
                    return 0;
                }
                
                //return (5 * (IsProviderCustomer &&
                //        RelatedProvider != null &&
                //        RelatedProvider.RelatedCustomerInfo != null ?
                //            RelatedProvider.
                //            RelatedCustomerInfo[MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId].
                //            ItemInfo.
                //            Where(x => x.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumCustomerProviderInfoType.ProviderRate).
                //            Select(x => Convert.ToDecimal(x.Value)).
                //            DefaultIfEmpty(0).
                //            FirstOrDefault() :
                //            0) / 100);
            }
        }

        /// <summary>
        /// Provider rate count for session customer
        /// </summary>
        public int ProviderRateCount
        {
            get
            {
                return 0;
            }
        }


        /// <summary>
        /// Provider alert risk
        /// </summary>
        public MarketPlace.Models.General.enumBlackListStatus ProviderAlertRisk
        {
            get
            {
                return ElasticRealtedProvider != null && ElasticRealtedProvider.InBlackList ?
                            MarketPlace.Models.General.enumBlackListStatus.ShowAlert :
                            MarketPlace.Models.General.enumBlackListStatus.DontShowAlert;
            }
        }

        public MarketPlace.Models.General.enumProviderLiteType ProviderLiteType { get; set; }

        public bool ProviderAddRemoveEnable { get; set; }

        public ProviderLiteViewModel(ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oRelatedProvider)
        {
            RelatedProvider = oRelatedProvider;
        }

        public ProviderLiteViewModel(CompanyIndexModel oElasticSearchModel)
        {
            ElasticRealtedProvider = oElasticSearchModel;
        }
    }
}
