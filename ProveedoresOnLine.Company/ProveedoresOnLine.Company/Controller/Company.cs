using ProveedoresOnLine.Company.Models.Company;
using ProveedoresOnLine.Company.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.Company.Controller
{
    public class Company
    {
        #region Util

        public static TreeModel TreeUpsert(TreeModel TreeToUpsert)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();
            try
            {
                TreeToUpsert.TreeId = DAL.Controller.CompanyDataController.Instance.TreeUpsert
                    (TreeToUpsert.TreeId > 0 ? (int?)TreeToUpsert.TreeId : null,
                    TreeToUpsert.TreeName,
                    TreeToUpsert.Enable);

                oLog.IsSuccess = true;

                return TreeToUpsert;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = TreeToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static GenericItemModel CategoryUpsert(int? TreeId, GenericItemModel CategoryToUpsert)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();

            try
            {
                CategoryToUpsert.ItemId = DAL.Controller.CompanyDataController.Instance.CategoryUpsert
                    (CategoryToUpsert.ItemId,
                    CategoryToUpsert.ItemName,
                    CategoryToUpsert.Enable);

                CategoryInfoUpsert(CategoryToUpsert);

                if (TreeId != null && TreeId > 0)
                {
                    TreeCategoryUpsert((int)TreeId, CategoryToUpsert);
                }

                oLog.IsSuccess = true;

                return CategoryToUpsert;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = CategoryToUpsert;

                if (TreeId != null)
                {
                    oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                    {
                        LogInfoType = "TreeId",
                        Value = TreeId.ToString(),
                    });
                }

                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static GenericItemModel CategoryInfoUpsert(GenericItemModel CategoryToUpsert)
        {
            if (CategoryToUpsert.ItemId > 0 &&
                CategoryToUpsert.ItemInfo != null &&
                CategoryToUpsert.ItemInfo.Count > 0)
            {
                CategoryToUpsert.ItemInfo.All(catinfo =>
                {
                    LogManager.Models.LogModel oLog = GetGenericLogModel();
                    try
                    {
                        catinfo.ItemInfoId = DAL.Controller.CompanyDataController.Instance.CategoryInfoUpsert
                            (CategoryToUpsert.ItemId,
                            catinfo.ItemInfoId > 0 ? (int?)catinfo.ItemInfoId : null,
                            catinfo.ItemInfoType.ItemId,
                            catinfo.Value,
                            catinfo.LargeValue,
                            catinfo.Enable);

                        oLog.IsSuccess = true;
                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        oLog.LogObject = CategoryToUpsert;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CategoryId",
                            Value = CategoryToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return CategoryToUpsert;
        }

        public static void TreeCategoryUpsert(int TreeId, GenericItemModel CategoryToUpsert)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();
            try
            {

                DAL.Controller.CompanyDataController.Instance.TreeCategoryUpsert
                    (TreeId,
                    CategoryToUpsert.ParentItem != null && CategoryToUpsert.ParentItem.ItemId > 0 ? (int?)CategoryToUpsert.ParentItem.ItemId : null,
                    CategoryToUpsert.ItemId,
                    CategoryToUpsert.Enable);

                oLog.IsSuccess = true;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = CategoryToUpsert;

                oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                {
                    LogInfoType = "TreeId",
                    Value = TreeId.ToString(),
                });

                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static CatalogModel CatalogItemUpsert(CatalogModel CatalogItemToUpsert)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();
            try
            {

                CatalogItemToUpsert.ItemId = DAL.Controller.CompanyDataController.Instance.CatalogItemUpsert
                    (CatalogItemToUpsert.CatalogId,
                    CatalogItemToUpsert.ItemId > 0 ? (int?)CatalogItemToUpsert.ItemId : null,
                    CatalogItemToUpsert.ItemName,
                    CatalogItemToUpsert.ItemEnable);

                oLog.IsSuccess = true;

                return CatalogItemToUpsert;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = CatalogItemToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GeographyModel> CategorySearchByGeography(string SearchParam, int? CityId, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByGeography(SearchParam, CityId, PageNumber, RowCount, out TotalRows);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GeographyModel> CategorySearchByGeographyAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByGeographyAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByRules(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByRules(SearchParam, PageNumber, RowCount);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByRulesAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByRulesAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByCompanyRules(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByCompanyRules(SearchParam, PageNumber, RowCount);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByCompanyRulesAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByCompanyRulesAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByResolution(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByResolution(SearchParam, PageNumber, RowCount);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByResolutionAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByResolutionAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByActivity(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByActivity(SearchParam, PageNumber, RowCount);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByCustomActivity(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByCustomActivity(SearchParam, PageNumber, RowCount);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByARLCompany(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByARLCompany(SearchParam, PageNumber, RowCount);
        }

        public static List<GenericItemModel> CategorySearchByICA(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByICA(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByBank(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByBank(SearchParam, PageNumber, RowCount);
        }

        public static List<GenericItemModel> CategorySearchByBankAdmin(string SearchParam, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByBankAdmin(SearchParam, PageNumber, RowCount, out TotalRows);
        }

        public static List<Models.Util.GenericItemModel> CategoryGetFinantialAccounts()
        {
            return DAL.Controller.CompanyDataController.Instance.CategoryGetFinantialAccounts();
        }

        public static List<Models.Util.CurrencyExchangeModel> CurrencyExchangeGetByMoneyType(int? MoneyTypeFrom, int? MoneyTypeTo, int? Year)
        {
            return DAL.Controller.CompanyDataController.Instance.CurrencyExchangeGetByMoneyType(MoneyTypeFrom, MoneyTypeTo, Year);
        }

        public static List<Models.Util.GenericItemModel> CategorySearchByEcoActivityAdmin(string SearchParam, int PageNumber, int RowCount, int TreeId, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByEcoActivityAdmin(SearchParam, PageNumber, RowCount, TreeId, out TotalRows);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByEcoGroupAdmin(string SearchParam, int PageNumber, int RowCount, int TreeId, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByEcoGroupAdmin(SearchParam, PageNumber, RowCount, TreeId, out TotalRows);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> CategorySearchByTreeAdmin(string SearchParam, int PageNumber, int RowCount)
        {
            return DAL.Controller.CompanyDataController.Instance.CategorySearchByTreeAdmin(SearchParam, PageNumber, RowCount);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> CurrentExchangeGetAllAdmin()
        {
            return DAL.Controller.CompanyDataController.Instance.CurrentExchangeGetAllAdmin();
        }

        public static int CurrencyExchangeInsert(ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel CurrencyExchange)
        {
            return DAL.Controller.CompanyDataController.Instance.CurrencyExchangeInsert(CurrencyExchange.IssueDate, CurrencyExchange.MoneyTypeFrom.ItemId, CurrencyExchange.MoneyTypeTo.ItemId, CurrencyExchange.Rate);
        }

        public static decimal CurrencyExchangeGetRate
                    (int MoneyFrom,
                    int MoneyTo,
                    int Year)
        {
            ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel oCurrency = null;

            if (MoneyFrom != MoneyTo)
            {
                //get rate
                List<ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel> olstCurrency =
                    ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetByMoneyType
                        (MoneyFrom, MoneyTo, null);

                if (olstCurrency != null && olstCurrency.Count > 0)
                {
                    //get rate for year or current year
                    oCurrency = olstCurrency.Any(x => x.IssueDate.Year == Year) ?
                        olstCurrency.Where(x => x.IssueDate.Year == Year).FirstOrDefault() :
                        olstCurrency.OrderByDescending(x => x.IssueDate.Year).FirstOrDefault();
                }
            }

            if (oCurrency == null)
            {
                //rate not found
                oCurrency = new ProveedoresOnLine.Company.Models.Util.CurrencyExchangeModel()
                {
                    Rate = 1,
                };
            }

            return oCurrency.Rate;
        }


        #endregion

        #region Company CRUD

        public static CompanyModel CompanyUpsert(CompanyModel CompanyToUpsert)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();
            try
            {

                CompanyToUpsert.CompanyPublicId = DAL.Controller.CompanyDataController.Instance.CompanyUpsert
                    (CompanyToUpsert.CompanyPublicId,
                    CompanyToUpsert.CompanyName,
                    CompanyToUpsert.IdentificationType.ItemId,
                    CompanyToUpsert.IdentificationNumber,
                    CompanyToUpsert.CompanyType.ItemId,
                    CompanyToUpsert.Enable);

                CompanyInfoUpsert(CompanyToUpsert);
                ContactUpsert(CompanyToUpsert);
                RoleCompanyUpsert(CompanyToUpsert);

                oLog.IsSuccess = true;

                return CompanyToUpsert;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = CompanyToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static CompanyModel CompanyInfoUpsert(CompanyModel CompanyToUpsert)
        {
            if (!string.IsNullOrEmpty(CompanyToUpsert.CompanyPublicId) &&
                CompanyToUpsert.CompanyInfo != null &&
                CompanyToUpsert.CompanyInfo.Count > 0)
            {
                CompanyToUpsert.CompanyInfo.All(cmpinf =>
                {
                    LogManager.Models.LogModel oLog = GetGenericLogModel();
                    try
                    {
                        cmpinf.ItemInfoId = DAL.Controller.CompanyDataController.Instance.CompanyInfoUpsert
                            (CompanyToUpsert.CompanyPublicId,
                            cmpinf.ItemInfoId > 0 ? (int?)cmpinf.ItemInfoId : null,
                            cmpinf.ItemInfoType.ItemId,
                            cmpinf.Value,
                            cmpinf.LargeValue,
                            cmpinf.Enable);

                        oLog.IsSuccess = true;
                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        oLog.LogObject = cmpinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = CompanyToUpsert.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }
            return CompanyToUpsert;
        }

        public static CompanyModel RoleCompanyUpsert(CompanyModel CompanyToUpsert)
        {
            if (!string.IsNullOrEmpty(CompanyToUpsert.CompanyPublicId) &&
                CompanyToUpsert.RelatedRole != null &&
                CompanyToUpsert.RelatedRole.Count > 0)
            {
                CompanyToUpsert.RelatedRole.All(cmpinf =>
                {
                    LogManager.Models.LogModel oLog = GetGenericLogModel();
                    try
                    {
                        cmpinf.ItemId = DAL.Controller.CompanyDataController.Instance.RoleCompanyUpsert
                            (CompanyToUpsert.CompanyPublicId,
                            cmpinf.ItemId > 0 ? (int?)cmpinf.ItemId : null,
                            cmpinf.ItemName,
                            cmpinf.ParentItem != null && cmpinf.ParentItem.ItemId > 0 ? (int?)cmpinf.ParentItem.ItemId : null,
                            cmpinf.Enable);

                        RoleCompanyInfoUpsert(cmpinf);
                        oLog.IsSuccess = true;

                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        oLog.LogObject = cmpinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = CompanyToUpsert.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }
            return CompanyToUpsert;

        }

        public static GenericItemModel RoleCompanyInfoUpsert(GenericItemModel RoleCompanyToUpsert)
        {
            if (RoleCompanyToUpsert.ItemId > 0 &&
                RoleCompanyToUpsert.ItemInfo != null &&
                RoleCompanyToUpsert.ItemInfo.Count > 0)
            {
                RoleCompanyToUpsert.ItemInfo.All(ctinf =>
                {
                    LogManager.Models.LogModel oLog = GetGenericLogModel();
                    try
                    {
                        ctinf.ItemInfoId = DAL.Controller.CompanyDataController.Instance.RoleCompanyInfoUpsert
                            (RoleCompanyToUpsert.ItemId,
                            ctinf.ItemInfoId > 0 ? (int?)ctinf.ItemInfoId : null,
                            ctinf.ItemInfoType.ItemId,
                            ctinf.Value,
                            ctinf.LargeValue,
                            ctinf.Enable);

                        oLog.IsSuccess = true;

                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        oLog.LogObject = ctinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "RoleCompanyId",
                            Value = RoleCompanyToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return RoleCompanyToUpsert;
        }

        

        public static UserCompany UserCompanyUpsert(UserCompany UserCompanyToUpsert)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();
            try
            {

                UserCompanyToUpsert.UserCompanyId = DAL.Controller.CompanyDataController.Instance.UserCompanyUpsert
                    (UserCompanyToUpsert.UserCompanyId > 0 ? (int?)UserCompanyToUpsert.UserCompanyId : null,
                    UserCompanyToUpsert.User,
                    UserCompanyToUpsert.RelatedRole.ItemId,
                    UserCompanyToUpsert.Enable);

                return UserCompanyToUpsert;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = UserCompanyToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static void CompanyFilterFill(string CompanyPublicId)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();
            try
            {
                DAL.Controller.CompanyDataController.Instance.CompanyFilterFill(CompanyPublicId);

                oLog.IsSuccess = true;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = CompanyPublicId;
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        public static void CompanySearchFill(string CompanyPublicId)
        {
            LogManager.Models.LogModel oLog = GetGenericLogModel();
            try
            {
                DAL.Controller.CompanyDataController.Instance.CompanySearchFill(CompanyPublicId);

                oLog.IsSuccess = true;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = CompanyPublicId;
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        #endregion

        #region Company Search

        public static CompanyModel CompanyGetBasicInfo(string CompanyPublicId)
        {
            return DAL.Controller.CompanyDataController.Instance.CompanyGetBasicInfo(CompanyPublicId);
        }

        public static List<GenericFilterModel> CompanySearchFilter(string CompanyType, string SearchParam, string SearchFilter)
        {
            return DAL.Controller.CompanyDataController.Instance.CompanySearchFilter(CompanyType, SearchParam, SearchFilter);
        }

        public static List<CompanyModel> CompanySearch(string CompanyType, string SearchParam, string SearchFilter, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyDataController.Instance.CompanySearch(CompanyType, SearchParam, SearchFilter, PageNumber, RowCount, out TotalRows);
        }

        #endregion

        #region Company Index

        public static List<int> InfoTypeRegenerateIndex
        {
            get
            {
                if (oInfoTypeRegenerateIndex == null)
                {
                    oInfoTypeRegenerateIndex = new List<int>() 
                    { 
                        //for principal tables first digit of asigned catalogs 
                        //2 for company
                        //3 for commercial
                        //7 for HSEQ
                        //5 for finantial
                        //6 for legal
                        2,
                        203002,
                        203003,
                        203004,
                        602007,
                        702004,
                        302013,
                        302014,
                    };
                }
                return oInfoTypeRegenerateIndex;
            }
        }
        private static List<int> oInfoTypeRegenerateIndex;

        public static void CompanyPartialIndex(string CompanyPublicId, List<int> InfoTypeModified)
        {
            if (!string.IsNullOrEmpty(CompanyPublicId) &&
                InfoTypeModified != null &&
                InfoTypeModified.Count > 0)
            {
                bool oDoPartialIndex = InfoTypeRegenerateIndex.Any(x => InfoTypeModified.Any(y => x == y));

                if (oDoPartialIndex)
                {
                    LogManager.Models.LogModel oLog = GetGenericLogModel();
                    try
                    {
                        CompanySearchFill(CompanyPublicId);
                        CompanyFilterFill(CompanyPublicId);
                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        oLog.LogObject = CompanyPublicId;
                        LogManager.ClientLog.AddLog(oLog);
                    }
                }
            }
        }

        #endregion

        #region Contact

        public static CompanyModel ContactUpsert(CompanyModel CompanyToUpsert)
        {
            if (!string.IsNullOrEmpty(CompanyToUpsert.CompanyPublicId) &&
                CompanyToUpsert.RelatedContact != null &&
                CompanyToUpsert.RelatedContact.Count > 0)
            {
                CompanyToUpsert.RelatedContact.All(cmpinf =>
                {
                    LogManager.Models.LogModel oLog = GetGenericLogModel();
                    try
                    {
                        cmpinf.ItemId = DAL.Controller.CompanyDataController.Instance.ContactUpsert
                            (CompanyToUpsert.CompanyPublicId,
                            cmpinf.ItemId > 0 ? (int?)cmpinf.ItemId : null,
                            cmpinf.ItemType.ItemId,
                            cmpinf.ItemName,
                            cmpinf.Enable);

                        ContactInfoUpsert(cmpinf);

                        oLog.IsSuccess = true;

                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        oLog.LogObject = cmpinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = CompanyToUpsert.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return CompanyToUpsert;
        }

        public static GenericItemModel ContactInfoUpsert(GenericItemModel ContactToUpsert)
        {
            if (ContactToUpsert.ItemId > 0 &&
                ContactToUpsert.ItemInfo != null &&
                ContactToUpsert.ItemInfo.Count > 0)
            {
                ContactToUpsert.ItemInfo.All(ctinf =>
                {
                    LogManager.Models.LogModel oLog = GetGenericLogModel();
                    try
                    {
                        ctinf.ItemInfoId = DAL.Controller.CompanyDataController.Instance.ContactInfoUpsert
                            (ContactToUpsert.ItemId,
                            ctinf.ItemInfoId > 0 ? (int?)ctinf.ItemInfoId : null,
                            ctinf.ItemInfoType.ItemId,
                            ctinf.Value,
                            ctinf.LargeValue,
                            ctinf.Enable);

                        oLog.IsSuccess = true;
                    }
                    catch (Exception err)
                    {
                        oLog.IsSuccess = false;
                        oLog.Message = err.Message + " - " + err.StackTrace;

                        throw err;
                    }
                    finally
                    {
                        oLog.LogObject = ctinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "ContactId",
                            Value = ContactToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ContactToUpsert;
        }

        public static List<Models.Util.GenericItemModel> ContactGetBasicInfo(string CompanyPublicId, int? ContactType, bool GetAll)
        {
            return DAL.Controller.CompanyDataController.Instance.ContactGetBasicInfo(CompanyPublicId, ContactType, GetAll);
        }

        #endregion

        #region User Roles

        public static List<ProveedoresOnLine.Company.Models.Company.CompanyModel> MP_RoleCompanyGetByUser(string User)
        {
            return DAL.Controller.CompanyDataController.Instance.MP_RoleCompanyGetByUser(User);
        }

        public static List<ProveedoresOnLine.Company.Models.Company.CompanyModel> RoleCompanyGetByPublicId(string CompanyPublicId)
        {
            return DAL.Controller.CompanyDataController.Instance.RoleCompanyGetByPublicId(CompanyPublicId);
        }

        #endregion

        #region Generic Log

        public static LogManager.Models.LogModel GetGenericLogModel()
        {
            LogManager.Models.LogModel oReturn = new LogManager.Models.LogModel()
            {
                RelatedLogInfo = new List<LogManager.Models.LogInfoModel>(),
            };

            try
            {
                System.Diagnostics.StackTrace oStack = new System.Diagnostics.StackTrace();

                if (System.Web.HttpContext.Current != null)
                {
                    //get user info
                    if (SessionManager.SessionController.Auth_UserLogin != null)
                    {
                        oReturn.User = SessionManager.SessionController.Auth_UserLogin.Email;
                    }
                    else
                    {
                        oReturn.User = System.Web.HttpContext.Current.Request.UserHostAddress;
                    }

                    //get source invocation
                    oReturn.Source = System.Web.HttpContext.Current.Request.Url.ToString();
                }
                else if (oStack.FrameCount > 0)
                {
                    oReturn.Source = oStack.GetFrame(oStack.FrameCount - 1).GetMethod().Module.Assembly.Location;
                }

                //get appname
                if (oStack.FrameCount > 1)
                {
                    oReturn.Application = oStack.GetFrame(1).GetMethod().Module.Name + " - " + oStack.GetFrame(1).GetMethod().Name;
                }
                else
                {
                    oReturn.Application = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                }
            }
            catch { }

            return oReturn;
        }

        #endregion

        #region Restrictive List

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> BlackListGetByCompanyPublicId(string CompanyPublicId)
        { 
            return DAL.Controller.CompanyDataController.Instance.BlackListGetByCompanyPublicId(CompanyPublicId);
        }
        #endregion
    }
}
