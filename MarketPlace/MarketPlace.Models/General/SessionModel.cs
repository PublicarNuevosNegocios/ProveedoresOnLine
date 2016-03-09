using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPlace.Models.General
{
    public static class SessionModel
    {
        public static SessionManager.Models.Auth.User CurrentLoginUser { get { return SessionManager.SessionController.Auth_UserLogin; } }

        public static bool UserIsLoggedIn { get { return (CurrentLoginUser != null); } }

        public static string CurrentURL { get; set; }

        public static SessionManager.Models.POLMarketPlace.MarketPlaceUser CurrentCompanyLoginUser
        {
            get
            {
                return SessionManager.SessionController.POLMarketPlace_MarketPlaceUserLogin;
            }
            set
            {
                SessionManager.SessionController.POLMarketPlace_MarketPlaceUserLogin = value;
            }
        }

        public static SessionManager.Models.POLMarketPlace.Session_CompanyModel CurrentCompany { get { return CurrentCompanyLoginUser.RelatedCompany.Where(x => x.CurrentSessionCompany == true).FirstOrDefault(); } }

        public static enumCompanyType CurrentCompanyType { get { return CurrentCompany == null ? enumCompanyType.Provider : (enumCompanyType)CurrentCompany.CompanyType.ItemId; } }

        public static int CurrentCompany_CustomEconomicActivity
        {
            get
            {
                return CurrentCompany == null ? 0 :
                  CurrentCompany.CompanyInfo.
                  Where(cinf => cinf.ItemInfoType.ItemId == (int)enumCompanyInfoType.CustomEconomicActivity &&
                                !string.IsNullOrEmpty(cinf.Value)).
                  Select(cinf => Convert.ToInt32(cinf.Value.Replace(" ", ""))).
                  DefaultIfEmpty(0).
                  FirstOrDefault();
            }
        }

        public static string CurrentCompany_CompanyLogo
        {
            get
            {
                return CurrentCompany == null ?
                    MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Company_Customer_DefaultLogoUrl].Value :
                    CurrentCompany.CompanyInfo.
                    Where(cinf => cinf.ItemInfoType.ItemId == (int)enumCompanyInfoType.CompanyLogo &&
                                !string.IsNullOrEmpty(cinf.Value)).
                    Select(cinf => cinf.Value).
                    DefaultIfEmpty(MarketPlace.Models.General.InternalSettings.Instance[MarketPlace.Models.General.Constants.C_Settings_Company_Customer_DefaultLogoUrl].Value).
                    FirstOrDefault();
            }
        }

        #region Session methods

        public static void InitCompanyLogin(List<ProveedoresOnLine.Company.Models.Company.CompanyModel> UserCompany)
        {
            SessionManager.Models.POLMarketPlace.MarketPlaceUser oMPUser = new SessionManager.Models.POLMarketPlace.MarketPlaceUser()
            {
                RelatedUser = CurrentLoginUser,
                RelatedCompany = new List<SessionManager.Models.POLMarketPlace.Session_CompanyModel>(),
            };
            if (UserCompany != null)
            {
                oMPUser.RelatedCompany =
                    (from uc in UserCompany
                     select new SessionManager.Models.POLMarketPlace.Session_CompanyModel()
                     {
                         CurrentSessionCompany = false,

                         CompanyPublicId = uc.CompanyPublicId,
                         CompanyName = uc.CompanyName,
                         IdentificationType = new SessionManager.Models.POLMarketPlace.Session_CatalogModel()
                         {
                             CatalogId = uc.IdentificationType.CatalogId,
                             CatalogName = uc.IdentificationType.CatalogName,
                             ItemId = uc.IdentificationType.ItemId,
                             ItemName = uc.IdentificationType.ItemName,
                         },
                         IdentificationNumber = uc.IdentificationNumber,
                         CompanyType = new SessionManager.Models.POLMarketPlace.Session_CatalogModel()
                         {
                             CatalogId = uc.CompanyType.CatalogId,
                             CatalogName = uc.CompanyType.CatalogName,
                             ItemId = uc.CompanyType.ItemId,
                             ItemName = uc.CompanyType.ItemName,
                         },

                         CompanyInfo =
                            (from cinf in uc.CompanyInfo
                             select new SessionManager.Models.POLMarketPlace.Session_GenericItemInfoModel()
                             {
                                 ItemInfoId = cinf.ItemInfoId,
                                 ItemInfoType = new SessionManager.Models.POLMarketPlace.Session_CatalogModel()
                                 {
                                     CatalogId = cinf.ItemInfoType.CatalogId,
                                     CatalogName = cinf.ItemInfoType.CatalogName,
                                     ItemId = cinf.ItemInfoType.ItemId,
                                     ItemName = cinf.ItemInfoType.ItemName
                                 },
                                 Value = cinf.Value,
                                 LargeValue = cinf.LargeValue,
                                 ValueName = cinf.ValueName,
                             }).
                             ToList(),

                         RelatedUser =
                            (from ru in uc.RelatedUser
                             select new SessionManager.Models.POLMarketPlace.Session_UserCompany()
                             {
                                 UserCompanyId = ru.UserCompanyId,
                                 User = ru.User,
                                 //RelatedRole = new SessionManager.Models.POLMarketPlace.Session_GenericItemModel()
                                 //{
                                 //    ItemId = ru.RelatedRole.ItemId,
                                 //    ItemName = ru.RelatedRole.ItemName,
                                 //    ParentItem = ru.RelatedRole.ParentItem == null ? null :
                                 //           new SessionManager.Models.POLMarketPlace.Session_GenericItemModel()
                                 //           {
                                 //               ItemId = ru.RelatedRole.ParentItem.ItemId
                                 //           },
                                 //    ItemInfo =
                                 //       (from ruinf in ru.RelatedRole.ItemInfo
                                 //        select new SessionManager.Models.POLMarketPlace.Session_GenericItemInfoModel()
                                 //        {
                                 //            ItemInfoId = ruinf.ItemInfoId,
                                 //            ItemInfoType = new SessionManager.Models.POLMarketPlace.Session_CatalogModel()
                                 //            {
                                 //                CatalogId = ruinf.ItemInfoType.CatalogId,
                                 //                CatalogName = ruinf.ItemInfoType.CatalogName,
                                 //                ItemId = ruinf.ItemInfoType.ItemId,
                                 //                ItemName = ruinf.ItemInfoType.ItemName,
                                 //            },
                                 //            Value = ruinf.Value,
                                 //            LargeValue = ruinf.LargeValue,
                                 //        }).ToList(),
                                 //},
                                 RelatedCompanyRole = new SessionManager.Models.POLMarketPlace.Session_RoleCompanyModel()
                                 {
                                     RoleCompanyId = ru.RelatedCompanyRole.RoleCompanyId,
                                     RoleCompanyName = ru.RelatedCompanyRole.RoleCompanyName,
                                     ParentRoleCompany = ru.RelatedCompanyRole.ParentRoleCompany,
                                     RoleModule =
                                        (from rm in ru.RelatedCompanyRole.RoleModule
                                         select new SessionManager.Models.POLMarketPlace.Session_RoleModuleModel()
                                         {
                                             RoleModuleId = rm.RoleModuleId,
                                             RoleModuleType = new SessionManager.Models.POLMarketPlace.Session_CatalogModel()
                                             {
                                                 ItemId = rm.RoleModuleType.ItemId,
                                                 ItemName = rm.RoleModuleType.ItemName,
                                             },
                                             RoleModule = rm.RoleModule,
                                             ModuleOption =
                                                (from mo in rm.ModuleOption
                                                 select new SessionManager.Models.POLMarketPlace.Session_GenericItemModel()
                                                 {
                                                     ItemId = mo.ItemId,
                                                     ItemName = mo.ItemName,
                                                     ItemInfo =
                                                        (from moi in mo.ItemInfo
                                                         select new SessionManager.Models.POLMarketPlace.Session_GenericItemInfoModel()
                                                         {
                                                             ItemInfoId = moi.ItemInfoId,
                                                             ItemInfoType = new SessionManager.Models.POLMarketPlace.Session_CatalogModel()
                                                             {
                                                                 ItemId = moi.ItemInfoType.ItemId,
                                                                 ItemName = moi.ItemInfoType.ItemName,
                                                             },
                                                             Value = moi.Value,
                                                             LargeValue = moi.LargeValue,
                                                         }).ToList(),
                                                 }).ToList(),
                                         }).ToList(),
                                     RelatedReport =
                                        (from rr in ru.RelatedCompanyRole.RelatedReport
                                         select new SessionManager.Models.POLMarketPlace.Session_GenericItemModel()
                                         {
                                             ItemId = rr.ItemId,
                                             ItemName = rr.ItemName,
                                         }).ToList(),
                                 },
                             }).ToList(),
                     }).ToList();
            }
            //init session variable
            CurrentCompanyLoginUser = oMPUser;

            //set current session company
            SetCurrentSessionCompany(null);
        }

        public static void SetCurrentSessionCompany(string CompanyPublicIdToChange)
        {
            if (CurrentCompanyLoginUser.RelatedCompany != null && CurrentCompanyLoginUser.RelatedCompany.Count > 0)
            {
                if (CurrentCompanyLoginUser.RelatedCompany.Any(x => x.CompanyPublicId == CompanyPublicIdToChange))
                {
                    CurrentCompanyLoginUser.RelatedCompany.
                        Where(x => x.CompanyPublicId == CompanyPublicIdToChange).
                        FirstOrDefault().
                        CurrentSessionCompany = true;
                }
                else
                {
                    CurrentCompanyLoginUser.RelatedCompany.
                        FirstOrDefault().
                        CurrentSessionCompany = true;
                }
            }
        }

        public static List<int> CurrentUserModules()
        {
            List<int> oReturn = new List<int>();

            if (CurrentCompany != null && CurrentCompany.RelatedUser != null)
            {
                CurrentCompany.RelatedUser.All(x =>
                {
                    //old code
                    //x.RelatedRole.ItemInfo
                    //    .Where(y => y.ItemInfoType.ItemId == (int)enumRoleCompanyInfoType.Modules)
                    //    .All(y =>
                    //    {
                    //        oReturn.AddRange
                    //            (y.LargeValue.
                    //                Replace(" ", "").
                    //                Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                    //                        Select(z => Convert.ToInt32(z)));
                    //        return true;
                    //    });

                    //new function
                    oReturn.AddRange(x.RelatedCompanyRole.RoleModule.Select(y => Convert.ToInt32(y.RoleModule)).ToList());

                    return true;
                });
            }
            return oReturn;
        }

        public static List<int> CurrentProviderMenu()
        {
            List<int> oReturn = new List<int>();

            if (CurrentCompany != null && CurrentCompany.RelatedUser != null)
            {
                CurrentCompany.RelatedUser.All(x =>
                {
                    SessionManager.Models.POLMarketPlace.Session_RoleModuleModel oModel = x.RelatedCompanyRole.RoleModule.Where(y => Convert.ToInt32(y.RoleModule) == (int)MarketPlace.Models.General.enumModule.ProviderInfo).Select(y => y).FirstOrDefault();

                    oReturn.AddRange(oModel.ModuleOption.Select(y => Convert.ToInt32(y.ItemName)).ToList());

                    return true;
                });
            }

            return oReturn;
        }

        public static List<int> CurrentProviderSubMenu(int oMenu)
        {
            List<int> oReturn = new List<int>();

            if (CurrentCompany != null && CurrentCompany.RelatedUser != null)
            {
                CurrentCompany.RelatedUser.All(x =>
                {
                    List<SessionManager.Models.POLMarketPlace.Session_GenericItemModel> oModel = x.RelatedCompanyRole.RoleModule.Where(y => Convert.ToInt32(y.RoleModule) == (int)MarketPlace.Models.General.enumModule.ProviderInfo).Select(y => y.ModuleOption).FirstOrDefault();
                    oModel.All(z =>
                    {
                        oReturn.AddRange(z.ItemInfo.Select(i => Convert.ToInt32(i.Value)).ToList());

                        return true;
                    });

                    return true;
                });
            }

            return oReturn;
        }

        public static List<int> CurrentSelectionOption()
        {
            List<int> oReturn = new List<int>();

            if (CurrentCompany != null && CurrentCompany.RelatedUser != null)
            {
                CurrentCompany.RelatedUser.All(x =>
                {
                List<SessionManager.Models.POLMarketPlace.Session_GenericItemModel> oModel = x.RelatedCompanyRole.RoleModule.Where(y => Convert.ToInt32(y.RoleModule) == (int)MarketPlace.Models.General.enumModule.SelectionInfo).Select(y => y.ModuleOption).FirstOrDefault();
                oReturn.AddRange(oModel.Select(z => Convert.ToInt32(z.ItemName)).ToList());

                return true;
            });
        }

            return oReturn;
        }

    public static bool IsUserAuthorized()
    {
        return (CurrentUserModules().Count > 0);
    }

    #endregion

    #region Notifications

    public static bool NewNotifications
    {
        get
        {
            List<MessageModule.Client.Models.NotificationModel> oReturn = MessageModule.Client.Controller.ClientController.NotificationGetByUser(CurrentCompany.CompanyPublicId, CurrentLoginUser.Email, true);

            bool oNewNotifications = false;

            oNewNotifications = oReturn != null ? true : false;

            return oNewNotifications;
        }
    }

    #endregion
}
}
