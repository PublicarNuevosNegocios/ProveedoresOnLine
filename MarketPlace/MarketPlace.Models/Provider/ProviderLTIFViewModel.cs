using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.Provider
{
    public class ProviderLTIFViewModel
    {
        public ProviderViewModel RelatedViewProvider { get; set; }

        public ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedLTIFInfo { get; set; }

        #region CertificatesAccident

        public string CA_Year { get; set; }
        public string CA_ManHoursWorked { get; set; }
        public string CA_Fatalities { get; set; }
        public string CA_NumberAccident { get; set; }
        public string CA_NumberAccidentDisabling { get; set; }
        public string CA_DaysIncapacity { get; set; }
        public string CA_CertificateAccidentARL { get; set; }
        public string CA_LTIFResult { get; set; }
        public string CA_YearsResult { get; set; }

        #endregion

        public ProviderLTIFViewModel(ProveedoresOnLine.Company.Models.Util.GenericItemModel oLTIFResult)
        {
            #region CertificatesAcccident

            CA_Year = oLTIFResult.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_Year).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CA_ManHoursWorked = oLTIFResult.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_ManHoursWorked).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CA_Fatalities = oLTIFResult.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_Fatalities).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CA_NumberAccident = oLTIFResult.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_NumberAccident).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CA_NumberAccidentDisabling = oLTIFResult.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_NumberAccidentDisabling).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CA_DaysIncapacity = oLTIFResult.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_DaysIncapacity).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault();

            CA_CertificateAccidentARL = oLTIFResult.ItemInfo.
               Where(y => y.ItemInfoType.ItemId == (int)MarketPlace.Models.General.enumHSEQInfoType.CA_CertificateAccidentARL).
               Select(y => y.Value).
               DefaultIfEmpty(string.Empty).
               FirstOrDefault(); 

            #endregion
        }

        public ProviderLTIFViewModel() { }
    }
}
