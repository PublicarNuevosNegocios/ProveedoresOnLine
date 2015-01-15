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
                         RelatedUser =
                            (from ru in uc.RelatedUser
                             select new SessionManager.Models.POLMarketPlace.Session_UserCompany()
                             {
                                 UserCompanyId = ru.UserCompanyId,
                                 User = ru.User,
                                 RelatedRole = new SessionManager.Models.POLMarketPlace.Session_GenericItemModel()
                                 {
                                     ItemId = ru.RelatedRole.ItemId,
                                     ItemName = ru.RelatedRole.ItemName,
                                     ParentItem = ru.RelatedRole.ParentItem == null ? null :
                                            new SessionManager.Models.POLMarketPlace.Session_GenericItemModel()
                                            {
                                                ItemId = ru.RelatedRole.ParentItem.ItemId
                                            },
                                     ItemInfo =
                                        (from ruinf in ru.RelatedRole.ItemInfo
                                         select new SessionManager.Models.POLMarketPlace.Session_GenericItemInfoModel()
                                         {
                                             ItemInfoId = ruinf.ItemInfoId,
                                             ItemInfoType = new SessionManager.Models.POLMarketPlace.Session_CatalogModel()
                                             {
                                                 CatalogId = ruinf.ItemInfoType.CatalogId,
                                                 CatalogName = ruinf.ItemInfoType.CatalogName,
                                                 ItemId = ruinf.ItemInfoType.ItemId,
                                                 ItemName = ruinf.ItemInfoType.ItemName,
                                             },
                                             Value = ruinf.Value,
                                             LargeValue = ruinf.LargeValue,
                                         }).ToList(),
                                 }
                             }).ToList()
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
                    x.RelatedRole.ItemInfo
                        .Where(y => y.ItemInfoType.ItemId == (int)enumRoleCompanyInfoType.Modules)
                        .All(y =>
                        {
                            oReturn.AddRange
                                (y.LargeValue.
                                    Replace(" ", "").
                                    Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                                            Select(z => Convert.ToInt32(z)));
                            return true;
                        });

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
    }
}
