﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Project
{
    public class ProjectProviderViewModel
    {
        public ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel RelatedProjectProvider { get; private set; }

        public MarketPlace.Models.Provider.ProviderViewModel RelatedProvider { get; private set; }

        public ProjectProviderViewModel(ProveedoresOnLine.ProjectModule.Models.ProjectProviderModel oRelatedProjectProvider)
        {
            RelatedProjectProvider = oRelatedProjectProvider;
            RelatedProvider = new Provider.ProviderViewModel()
            {
                RelatedLiteProvider = new Provider.ProviderLiteViewModel(RelatedProjectProvider.RelatedProvider),
            };

            #region Commercial

            RelatedProvider.RelatedComercialInfo = new List<MarketPlace.Models.Provider.ProviderComercialViewModel>();

            if (RelatedProjectProvider.RelatedProvider != null &&
                RelatedProjectProvider.RelatedProvider.RelatedCommercial != null &&
                RelatedProjectProvider.RelatedProvider.RelatedCommercial.Count > 0)
            {
                RelatedProjectProvider.RelatedProvider.RelatedCommercial.All(cm =>
                {
                    RelatedProvider.RelatedComercialInfo.Add(new MarketPlace.Models.Provider.ProviderComercialViewModel(cm));
                    return true;
                });

            }

            #endregion

            #region HSEQ Info

            RelatedProvider.RelatedHSEQlInfo = new List<Provider.ProviderHSEQViewModel>();

            if (RelatedProjectProvider.RelatedProvider != null &&
                RelatedProjectProvider.RelatedProvider.RelatedCertification != null &&
                RelatedProjectProvider.RelatedProvider.RelatedCertification.Count > 0)
            {
                RelatedProjectProvider.RelatedProvider.RelatedCertification.All(cr =>
                {
                    RelatedProvider.RelatedHSEQlInfo.Add(new MarketPlace.Models.Provider.ProviderHSEQViewModel(cr));
                    return true;
                });

            }

            #endregion

            #region Lega Info

            RelatedProvider.RelatedLegalInfo = new List<Provider.ProviderLegalViewModel>();

            if (RelatedProjectProvider.RelatedProvider != null &&
                RelatedProjectProvider.RelatedProvider.RelatedLegal != null &&
                RelatedProjectProvider.RelatedProvider.RelatedLegal.Count > 0)
            {
                RelatedProjectProvider.RelatedProvider.RelatedLegal.All(lg =>
                {
                    RelatedProvider.RelatedLegalInfo.Add(new MarketPlace.Models.Provider.ProviderLegalViewModel(lg));
                    return true;
                });

            }

            #endregion
        }

        #region Projectprovider info

        public MarketPlace.Models.General.enumApprovalStatus? ApprovalStatus
        {
            get
            {
                if(RelatedProjectProvider.ItemInfo == null)
                {
                    return null;
                }
                else
                {
                    return RelatedProjectProvider.ItemInfo.
                    Where(pjpvinf => pjpvinf.RelatedEvaluationItem == null &&
                                    pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus &&
                                    !string.IsNullOrEmpty(pjpvinf.Value)).
                    Select(pjpvinf => (MarketPlace.Models.General.enumApprovalStatus?)Convert.ToInt32(pjpvinf.Value.Replace(" ", ""))).
                    DefaultIfEmpty(null).
                    FirstOrDefault();
                }
                
            }
        }

        public int ApprovalStatusId
        {
            get
            {
                if (RelatedProjectProvider.ItemInfo == null)
                {
                    return 0;
                }
                else
                {
                    return RelatedProjectProvider.ItemInfo.
                    Where(pjpvinf => pjpvinf.RelatedEvaluationItem == null &&
                                    pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus).
                    Select(pjpvinf => pjpvinf.ItemInfoId).
                    DefaultIfEmpty(0).
                    FirstOrDefault();
                }
            }
        }

        #endregion

        #region Methods

        public decimal GetRatting(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem != null &&
                                pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.Ratting &&
                                !string.IsNullOrEmpty(pjpvinf.Value)).
                Select(pjpvinf => Convert.ToDecimal(pjpvinf.Value, System.Globalization.CultureInfo.CreateSpecificCulture("EN-us"))).
                DefaultIfEmpty(0).
                FirstOrDefault();
        }

        public List<int> GetInfoItems(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem != null &&
                                pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.InfoItems &&
                                !string.IsNullOrEmpty(pjpvinf.LargeValue)).
                Select(pjpvinf => pjpvinf.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault().
                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                Select(strint => !string.IsNullOrEmpty(strint) ? Convert.ToInt32(strint) : 0).
                ToList();
        }

        public MarketPlace.Models.General.enumApprovalStatus? GetApprovalStatusByArea(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem != null &&
                                pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus &&
                                !string.IsNullOrEmpty(pjpvinf.Value)).
                Select(pjpvinf => (MarketPlace.Models.General.enumApprovalStatus?)Convert.ToInt32(pjpvinf.Value.Replace(" ", ""))).
                DefaultIfEmpty(null).
                FirstOrDefault();
        }

        public int GetApprovalStatusIdByArea(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem != null &&
                                pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus).
                Select(pjpvinf => pjpvinf.ItemInfoId).
                DefaultIfEmpty(0).
                FirstOrDefault();
        }

        public string GetApprovalTextByArea(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem != null &&
                                pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalText &&
                                !string.IsNullOrEmpty(pjpvinf.LargeValue)).
                Select(pjpvinf => pjpvinf.LargeValue).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
        }

        public string GetEvaluatorByArea(int vEvaluationItemId)
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem != null &&
                                pjpvinf.RelatedEvaluationItem.ItemId == vEvaluationItemId &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.Evaluator &&
                                !string.IsNullOrEmpty(pjpvinf.Value)).
                Select(pjpvinf => pjpvinf.Value).
                DefaultIfEmpty(string.Empty).
                FirstOrDefault();
        }

        /// <summary>
        /// get status for all areas
        /// </summary>
        /// <returns>areaid,areastatus</returns>
        public Dictionary<int, MarketPlace.Models.General.enumApprovalStatus> GetApprovalStatusAllAreas()
        {
            return RelatedProjectProvider.ItemInfo.
                Where(pjpvinf => pjpvinf.RelatedEvaluationItem != null &&
                                pjpvinf.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumProjectCompanyInfoType.ApprovalStatus &&
                                !string.IsNullOrEmpty(pjpvinf.Value)).
                Select(pjpvinf => new
                {
                    oKey = pjpvinf.RelatedEvaluationItem.ItemId,
                    oValue = (MarketPlace.Models.General.enumApprovalStatus)Convert.ToInt32(pjpvinf.Value.Replace(" ", "")),
                }).
                ToDictionary(k => k.oKey, v => v.oValue);
        }

        #endregion
    }
}
