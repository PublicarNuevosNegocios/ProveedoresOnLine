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

        public int TotalRows { get; set; }

        public string CPI_CustomerProviderInfoId { get { return RelatedCustomerProviderInfo.ItemInfoId.ToString(); } }

        public string CPI_TrackingType { get { return RelatedCustomerProviderInfo.ItemInfoType.ItemName.ToString(); } }

        public TrackingDetailViewModel CPI_Tracking { get; set; }

        public string CPI_CreateDate { get { return RelatedCustomerProviderInfo.CreateDate.ToString("dd/MM/yyyy hh:MM:ss"); } }

        public bool CPI_Enable { get; set; }

        public TrackingViewModel() { }

        public TrackingViewModel(GenericItemInfoModel oRelatedCustomerProviderInfo, int oTotalRows)
        {
            RelatedCustomerProviderInfo = oRelatedCustomerProviderInfo;

            CPI_Enable = RelatedCustomerProviderInfo.Enable;

            if (!string.IsNullOrEmpty(RelatedCustomerProviderInfo.LargeValue) &&
                    (RelatedCustomerProviderInfo.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumProviderCustomerType.CustomerMonitoring ||
                    RelatedCustomerProviderInfo.ItemInfoType.ItemId == (int)BackOffice.Models.General.enumProviderCustomerType.InternalMonitoring))
            {
                CPI_Tracking = (TrackingDetailViewModel)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                        Deserialize(RelatedCustomerProviderInfo.LargeValue, typeof(TrackingDetailViewModel));
            }            

            TotalRows = oTotalRows;
        }
    }
}
