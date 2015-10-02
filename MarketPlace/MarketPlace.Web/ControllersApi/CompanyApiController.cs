using MarketPlace.Models.General;
using MarketPlace.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MarketPlace.Web.ControllersApi
{
    public class CompanyApiController : BaseApiController
    {
        [HttpPost]
        [HttpGet]
        public List<string> UserCompanySearchAC
            (string UserCompanySearchAC,
            string SearchParam)
        {
            List<string> oReturn = new List<string>();

            if (UserCompanySearchAC == "true")
            {
                List<ProveedoresOnLine.Company.Models.Company.UserCompany> SearchResult = ProveedoresOnLine.Company.Controller.Company.MP_UserCompanySearch
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    SearchParam,
                    null,
                    0,
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()));

                if (SearchResult != null && SearchResult.Count > 0)
                {
                    oReturn = SearchResult.
                        OrderBy(x => x.User).
                        Select(x => x.User).
                        ToList();
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public List<string> UserCompanySearchByRoleAC
            (string UserCompanySearchByRoleAC,
            string RolId,
            string SearchParam)
        {
            List<string> oReturn = new List<string>();

            if (UserCompanySearchByRoleAC == "true")
            {
                List<ProveedoresOnLine.Company.Models.Company.UserCompany> SearchResult = ProveedoresOnLine.Company.Controller.Company.MP_UserCompanySearch
                    (MarketPlace.Models.General.SessionModel.CurrentCompany.CompanyPublicId,
                    SearchParam,
                    Convert.ToInt32(RolId),
                    0,
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()));               
               
                if (SearchResult != null && SearchResult.Count > 0)
                {                   
                    oReturn = SearchResult.
                        OrderBy(x => x.User).
                        Select(x => x.User).
                        ToList();
                }
            }

            return oReturn;
        }

        #region Util

        #region Category

        [HttpPost]
        [HttpGet]
        public List<EconomicActivityViewModel> CategorySearchByActivityAC
            (string CategorySearchByActivityAC,
            string TreeId,
            string SearchParam)
        {
            List<EconomicActivityViewModel> oReturn = new List<EconomicActivityViewModel>();

            if (CategorySearchByActivityAC == "true")
            {
                List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oCategories = ProveedoresOnLine.Company.Controller.Company.MPCategorySearchByActivity
                    (Convert.ToInt32(TreeId),
                    SearchParam,
                    Convert.ToInt32(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Grid_RowCountDefault].Value.Trim()));

                if (oCategories != null && oCategories.Count > 0)
                {
                    oCategories.OrderBy(cat => cat.ItemName).All(cat =>
                    {
                        oReturn.Add(new EconomicActivityViewModel()
                        {
                            EconomicActivityId = cat.ItemId.ToString(),
                            ActivityName = cat.ItemName,
                        });

                        return true;
                    });
                }
            }

            return oReturn;
        }


        #endregion

        #endregion

        #region Notifications

        [HttpPost]
        [HttpGet]
        public List<MarketPlace.Models.General.NotificationViewModel> NGetNotifications
            (string NGetNotifications, string User, string CompanyPublicId, string Enable)
        {
            List<MarketPlace.Models.General.NotificationViewModel> oReturn = new List<NotificationViewModel>();

            if (NGetNotifications == "true")
            {
                List<MessageModule.Client.Models.NotificationModel> oNotifications =
                    MessageModule.Client.Controller.ClientController.NotificationGetByUser
                    (CompanyPublicId, User, Enable == "true" ? true : false);

                if (oNotifications != null &&
                    oNotifications.Count > 0)
                {
                    oNotifications.All(x =>
                    {
                        oReturn.Add(new NotificationViewModel(x));
                        return true;
                    });
                }
            }

            return oReturn;
        }

        [HttpPost]
        [HttpGet]
        public void NDeleteNotifications
            (string NDeleteNotifications, string NotificationId)
        {
            if (NDeleteNotifications == "true")
            {
                MessageModule.Client.Controller.ClientController.NotificationDeleteById(Convert.ToInt32(NotificationId));
            }
        }

        #endregion
    }
}
