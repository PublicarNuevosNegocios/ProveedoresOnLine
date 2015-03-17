using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackOffice.Models.Provider
{
    public class TrackingViewModel
    {
        public ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel RelatedCustomerProviderInfo { get; set; }

        public string CPI_CustomerProviderInfoId { get { return RelatedCustomerProviderInfo.ItemInfoId.ToString(); } }

        public string CPI_TrackingType { get { return RelatedCustomerProviderInfo.ItemInfoType.ItemName.ToString(); } }

        public TrackingDetailViewModel CPI_Tracking
        {
            get
            {
                if (oCPI_Tracking == null &&
                    !string.IsNullOrEmpty(RelatedCustomerProviderInfo.LargeValue) &&
                    (RelatedCustomerProviderInfo.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumProviderCustomerType.CustomerMonitoring ||
                    RelatedCustomerProviderInfo.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumProviderCustomerType.InternalMonitoring))
                {
                    oCPI_Tracking = (TrackingDetailViewModel)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                        Deserialize(RelatedCustomerProviderInfo.LargeValue, typeof(TrackingDetailViewModel));
                }
                return oCPI_Tracking;
            }
        }
        private TrackingDetailViewModel oCPI_Tracking;

        public string CPI_LastModify { get { return RelatedCustomerProviderInfo.LastModify.ToString(); } }

        public bool CPI_Enable { get { return RelatedCustomerProviderInfo.Enable; } }

        public TrackingViewModel(GenericItemInfoModel oRelatedCustomerProviderInfo)
        {
            RelatedCustomerProviderInfo = oRelatedCustomerProviderInfo;
        }
    }
}
