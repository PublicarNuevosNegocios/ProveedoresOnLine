using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompanyProvider.Controller
{
    public class CompanyProvider
    {
        #region Provider Info

        public static ProviderModel ProviderUpsert(ProviderModel ProviderToUpsert)
        {
            LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
            try
            {

                ProviderToUpsert.RelatedCompany = Company.Controller.Company.CompanyUpsert
                    (ProviderToUpsert.RelatedCompany);

                CommercialUpsert(ProviderToUpsert);
                CertificationUpsert(ProviderToUpsert);
                FinancialUpsert(ProviderToUpsert);
                BalanceSheetUpsert(ProviderToUpsert);
                LegalUpsert(ProviderToUpsert);

                oLog.IsSuccess = true;

                return ProviderToUpsert;
            }
            catch (Exception err)
            {
                oLog.IsSuccess = false;
                oLog.Message = err.Message + " - " + err.StackTrace;

                throw err;
            }
            finally
            {
                oLog.LogObject = ProviderToUpsert;
                LogManager.ClientLog.AddLog(oLog);
            }
        }

        #endregion

        #region Provider Commercial

        public static ProviderModel CommercialUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedCommercial != null &&
                ProviderToUpsert.RelatedCommercial.Count > 0)
            {
                ProviderToUpsert.RelatedCommercial.All(pcom =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pcom.ItemId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.CommercialUpsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId,
                            pcom.ItemId > 0 ? (int?)pcom.ItemId : null,
                            pcom.ItemType.ItemId,
                            pcom.ItemName,
                            pcom.Enable);

                        CommercialInfoUpsert(pcom);

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
                        oLog.LogObject = pcom;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = ProviderToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProviderToUpsert;
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel CommercialInfoUpsert
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel CommercialToUpsert)
        {
            if (CommercialToUpsert.ItemId > 0 &&
                CommercialToUpsert.ItemInfo != null &&
                CommercialToUpsert.ItemInfo.Count > 0)
            {
                CommercialToUpsert.ItemInfo.All(pcominf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pcominf.ItemInfoId = DAL.Controller.CompanyProviderDataController.Instance.CommercialInfoUpsert
                            (CommercialToUpsert.ItemId,
                            pcominf.ItemInfoId > 0 ? (int?)pcominf.ItemInfoId : null,
                            pcominf.ItemInfoType.ItemId,
                            pcominf.Value,
                            pcominf.LargeValue,
                            pcominf.Enable);

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
                        oLog.LogObject = pcominf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "ExperienceId",
                            Value = CommercialToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return CommercialToUpsert;
        }

        public static List<Company.Models.Util.GenericItemModel> CommercialGetBasicInfo(string CompanyPublicId, int? CommercialType, bool Enable)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.CommercialGetBasicInfo(CompanyPublicId, CommercialType, Enable);
        }

        #endregion

        #region Provider Certification

        public static ProviderModel CertificationUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedCertification != null &&
                ProviderToUpsert.RelatedCertification.Count > 0)
            {
                ProviderToUpsert.RelatedCertification.All(pcert =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pcert.ItemId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.CertificationUpsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId,
                            pcert.ItemId > 0 ? (int?)pcert.ItemId : null,
                            pcert.ItemType.ItemId,
                            pcert.ItemName,
                            pcert.Enable);

                        CertificationInfoUpsert(pcert);

                        if (pcert.ItemType.ItemId == 701004)
                        {
                            GenericItemInfoModel oLTIFResult = GetLTIFValue(ProviderToUpsert.RelatedCompany.CompanyPublicId, pcert);
                            if (oLTIFResult != null)
                            {
                                CertificationInfoUpsert(new GenericItemModel()
                                {
                                    ItemId = pcert.ItemId,
                                    ItemInfo = new List<GenericItemInfoModel>() { oLTIFResult }
                                });
                            }
                        }
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
                        oLog.LogObject = pcert;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = ProviderToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProviderToUpsert;
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel CertificationInfoUpsert
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel CertificationToUpsert)
        {
            if (CertificationToUpsert.ItemId > 0 &&
                CertificationToUpsert.ItemInfo != null &&
                CertificationToUpsert.ItemInfo.Count > 0)
            {
                CertificationToUpsert.ItemInfo.All(certinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        certinf.ItemInfoId = DAL.Controller.CompanyProviderDataController.Instance.CertificationInfoUpsert
                            (CertificationToUpsert.ItemId,
                            certinf.ItemInfoId > 0 ? (int?)certinf.ItemInfoId : null,
                            certinf.ItemInfoType.ItemId,
                            certinf.Value,
                            certinf.LargeValue,
                            certinf.Enable);

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
                        oLog.LogObject = certinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CertificationId",
                            Value = CertificationToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });

            }

            return CertificationToUpsert;
        }

        public static List<GenericItemModel> CertficationGetBasicInfo(string CompanyPublicId, int? CertificationType, bool Enable)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.CertificationGetBasicInfo(CompanyPublicId, CertificationType, Enable);
        }

        #region Private HSEQ Functions

        private static ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel GetLTIFValue(string ProviderPublicId, ProveedoresOnLine.Company.Models.Util.GenericItemModel oHSEQModel)
        {
            //Get the CertificationsInfo
            List<GenericItemModel> oCertificatesResult = CertficationGetBasicInfo(ProviderPublicId, 701004, true);

            if (oCertificatesResult != null)
            {
                decimal actualManWorkersHours = 0;
                List<int> actualManWorkersHoursId = new List<int>();
                decimal actualFatalities = 0;
                List<int> actualFatalitiesId = new List<int>();
                decimal actualIncapacity = 0;
                List<int> actualDaysIncapacityId = new List<int>();
                decimal LTIFResult = 0;

                oCertificatesResult.All(x =>
                {
                    actualManWorkersHours += Convert.ToDecimal(!string.IsNullOrEmpty(x.ItemInfo.Where(y => y.ItemInfoType.ItemId == 705002 && !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.Value).DefaultIfEmpty("0").FirstOrDefault()) ?
                                    x.ItemInfo.Where(y => y.ItemInfoType.ItemId == 705002 && !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.Value).DefaultIfEmpty("0").FirstOrDefault() : "0");
                    actualFatalities += Convert.ToDecimal(!string.IsNullOrEmpty(x.ItemInfo.Where(y => y.ItemInfoType.ItemId == 705003 && !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.Value).DefaultIfEmpty("0").FirstOrDefault()) ?
                                    x.ItemInfo.Where(y => y.ItemInfoType.ItemId == 705003 && !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.Value).DefaultIfEmpty("0").FirstOrDefault() : "0");
                    actualIncapacity += Convert.ToDecimal(!string.IsNullOrEmpty(x.ItemInfo.Where(y => y.ItemInfoType.ItemId == 705004 && !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.Value).DefaultIfEmpty("0").FirstOrDefault()) ?
                                    x.ItemInfo.Where(y => y.ItemInfoType.ItemId == 705004 && !string.IsNullOrWhiteSpace(y.Value)).Select(y => y.Value).DefaultIfEmpty("0").FirstOrDefault() : "0");
                    return true;
                });

                if (actualManWorkersHours != 0)
                    LTIFResult = (((actualFatalities + actualIncapacity) / actualManWorkersHours) * 1000000);

                List<GenericItemModel> oRiskCertificate = CertficationGetBasicInfo(ProviderPublicId, 701003, true);
                int oItemInfoId = 0;
                if (oRiskCertificate != null && oRiskCertificate.Count > 0)
                {
                    oRiskCertificate.Where(x => x.ItemInfo != null).All(x =>
                    {
                        oItemInfoId = x.ItemInfo.
                            Where(y => y.ItemInfoType != null && y.ItemInfoType.ItemId == 704004).
                            Select(y => y.ItemInfoId).
                            DefaultIfEmpty(0).
                            FirstOrDefault();
                        return true;
                    });
                }

                GenericItemInfoModel oltifModel = new GenericItemInfoModel()
                {
                    ItemInfoId = oItemInfoId,
                    ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                    {
                        ItemId = 704004
                    },
                    Value = LTIFResult.ToString("#,0.##"),
                    Enable = true,
                };
                return oltifModel;
            }
            return null;
        }

        #endregion

        #endregion

        #region Provider financial

        public static ProviderModel FinancialUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedFinantial != null &&
                ProviderToUpsert.RelatedFinantial.Count > 0)
            {
                ProviderToUpsert.RelatedFinantial.All(pfin =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pfin.ItemId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.FinancialUpsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId,
                            pfin.ItemId > 0 ? (int?)pfin.ItemId : null,
                            pfin.ItemType.ItemId,
                            pfin.ItemName,
                            pfin.Enable);

                        FinancialInfoUpsert(pfin);

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
                        oLog.LogObject = pfin;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = ProviderToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProviderToUpsert;
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel FinancialInfoUpsert
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel FinancialToUpsert)
        {
            if (FinancialToUpsert.ItemId > 0 &&
                FinancialToUpsert.ItemInfo != null &&
                FinancialToUpsert.ItemInfo.Count > 0)
            {
                FinancialToUpsert.ItemInfo.All(fininf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        fininf.ItemInfoId = DAL.Controller.CompanyProviderDataController.Instance.FinancialInfoUpsert
                            (FinancialToUpsert.ItemId,
                            fininf.ItemInfoId > 0 ? (int?)fininf.ItemInfoId : null,
                            fininf.ItemInfoType.ItemId,
                            fininf.Value,
                            fininf.LargeValue,
                            fininf.Enable);

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
                        oLog.LogObject = fininf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "FinancialId",
                            Value = FinancialToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return FinancialToUpsert;
        }

        public static ProviderModel BalanceSheetUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedBalanceSheet != null && ProviderToUpsert.RelatedBalanceSheet.Count > 0)
            {
                ProviderToUpsert.RelatedBalanceSheet.All(rbs =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        //get and validate balancesheet
                        rbs = ValidateBalanceSheet(rbs, ProviderToUpsert.RelatedCompany.CompanyPublicId);

                        rbs.ItemId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.FinancialUpsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId,
                            rbs.ItemId > 0 ? (int?)rbs.ItemId : null,
                            rbs.ItemType.ItemId,
                            rbs.ItemName,
                            rbs.Enable);

                        FinancialInfoUpsert((ProveedoresOnLine.Company.Models.Util.GenericItemModel)rbs);

                        BalanceSheetInfoUpsert(rbs);

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
                        oLog.LogObject = rbs;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = ProviderToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }
            return ProviderToUpsert;
        }

        public static BalanceSheetModel BalanceSheetInfoUpsert
            (BalanceSheetModel BalanceSheetToUpsert)
        {
            if (BalanceSheetToUpsert.ItemId > 0 &&
                BalanceSheetToUpsert.BalanceSheetInfo != null &&
                BalanceSheetToUpsert.BalanceSheetInfo.Count > 0)
            {
                BalanceSheetToUpsert.BalanceSheetInfo.All(bsinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        bsinf.BalanceSheetId = DAL.Controller.CompanyProviderDataController.Instance.BalanceSheetUpsert
                            (BalanceSheetToUpsert.ItemId,
                            bsinf.BalanceSheetId > 0 ? (int?)bsinf.BalanceSheetId : null,
                            bsinf.RelatedAccount.ItemId,
                            bsinf.Value,
                            bsinf.Enable);

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
                        oLog.LogObject = bsinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "FinancialId",
                            Value = BalanceSheetToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return BalanceSheetToUpsert;
        }

        public static List<GenericItemModel> FinancialGetBasicInfo(string CompanyPublicId, int? FinancialType, bool Enable)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.FinancialGetBasicInfo(CompanyPublicId, FinancialType, Enable);
        }

        public static List<BalanceSheetDetailModel> BalanceSheetGetByFinancial(int FinancialId)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.BalanceSheetGetByFinancial(FinancialId);
        }

        #region generic Math operations

        /// <summary>
        /// evaluate comparison expressions only constants
        /// </summary>
        /// <param name="Expression">5==(4+1)==(5*1)</param>
        /// <returns></returns>
        public static bool MathComparison(string Expression)
        {
            bool oReturn = true;

            decimal? oTmpResult = null;

            Expression.Split(new string[] { "==" }, StringSplitOptions.RemoveEmptyEntries).All(exp =>
            {
                decimal oCurrentResult = MathEval(exp);
                if (oTmpResult != null && oCurrentResult != oTmpResult.Value)
                {
                    oReturn = false;
                }
                oTmpResult = oCurrentResult;
                return true;
            });

            return oReturn;
        }

        /// <summary>
        /// evaluate math expression only constants
        /// </summary>
        /// <param name="Expression">3+5*(1+2)</param>
        /// <returns></returns>
        public static decimal MathEval(string Expression)
        {
            try
            {
                var oDataTable = new System.Data.DataTable();
                var oDataColumn = new System.Data.DataColumn("Eval", typeof(decimal), Expression);
                oDataTable.Columns.Add(oDataColumn);
                oDataTable.Rows.Add(0);
                return (decimal)(oDataTable.Rows[0]["Eval"]);
            }
            catch { }

            return 0;
        }

        #endregion

        #region Private Methods

        private static BalanceSheetModel ValidateBalanceSheet(BalanceSheetModel BalanceToEval, string ProviderPublicId)
        {
            //duplicate balance model to return
            BalanceSheetModel oReturn = new BalanceSheetModel()
            {
                ItemId = BalanceToEval.ItemId,
                ItemName = BalanceToEval.ItemName,
                ItemType = BalanceToEval.ItemType,
                ItemInfo = BalanceToEval.ItemInfo,
                Enable = BalanceToEval.Enable,
                ParentItem = BalanceToEval.ParentItem,

                BalanceSheetInfo = new List<Models.Provider.BalanceSheetDetailModel>(),
            };

            //get account info
            List<GenericItemModel> olstAccount =
                ProveedoresOnLine.Company.Controller.Company.CategoryGetFinantialAccounts();

            //get current values
            List<BalanceSheetDetailModel> olstBalanceSheetDetail = null;

            if (BalanceToEval.ItemId > 0)
            {
                olstBalanceSheetDetail = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BalanceSheetGetByFinancial
                    (BalanceToEval.ItemId);
            }

            if (olstBalanceSheetDetail == null)
                olstBalanceSheetDetail = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel>();

            //get averange
            List<BalanceSheetDetailModel> olstBalanceSheetAverange =
                ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.BalanceSheetGetCompanyAverage
                    (ProviderPublicId,
                    BalanceToEval.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == 502001).
                        Select(x => Convert.ToInt32(x.Value) - 1).
                        DefaultIfEmpty(DateTime.Now.Year - 1).
                        FirstOrDefault());

            if (olstBalanceSheetAverange == null)
                olstBalanceSheetAverange = new List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel>();

            //get account info

            //key: AccountId
            //Tuple<Order,AccountObject,BalanceObject,OriginalValue,CurrencyValue>
            Dictionary<int, Tuple<int, GenericItemModel, BalanceSheetDetailModel, decimal, decimal>> AccountValues = new Dictionary<int, Tuple<int, GenericItemModel, BalanceSheetDetailModel, decimal, decimal>>();

            //fill account object
            GetAccountArray(
                AccountValues,

                BalanceToEval,
                //oCurrencyRate,
                null,

                olstAccount,
                olstBalanceSheetDetail);

            //execute account formula and refresh related balancesheet
            AccountValues.Select(aci => aci.Value).OrderBy(aci => aci.Item1).All(aci =>
            {
                int oAccountInfoType = aci.Item2.ItemInfo.
                    Where(x => x.ItemInfoType.ItemId == 109002).
                    Select(x => Convert.ToInt32(x.Value.Replace(" ", ""))).
                    DefaultIfEmpty(1).
                    FirstOrDefault();

                //balance only for value types exlude title types (2)
                if (oAccountInfoType == 0 || oAccountInfoType == 1)
                {
                    decimal oAccountDecimalValue = 0;

                    #region Get account value

                    if (oAccountInfoType == 0)
                    {
                        //is formula field
                        string strFormula = aci.Item2.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == 109003).
                            Select(x => x.LargeValue).
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault().ToLower().Replace(" ", "");

                        if (!string.IsNullOrEmpty(strFormula))
                        {
                            //loop for prom values
                            foreach (var RegexResult in (new Regex("prom+\\(+\\[+\\d*\\]+\\)+", RegexOptions.IgnoreCase)).Matches(strFormula))
                            {
                                int oAccountId = Convert.ToInt32(RegexResult.ToString().Replace("prom([", "").Replace("])", ""));

                                decimal PromValue = olstBalanceSheetAverange.
                                    Where(x => x.RelatedAccount.ItemId == oAccountId).
                                    Select(x => x.Value).
                                    DefaultIfEmpty(AccountValues[oAccountId].Item3.Value).
                                    FirstOrDefault();

                                PromValue = (PromValue + AccountValues[oAccountId].Item3.Value) / 2;

                                strFormula = strFormula.Replace
                                    (RegexResult.ToString(),
                                    PromValue.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")));
                            }

                            //loop for standar values
                            foreach (var RegexResult in (new Regex("\\[+\\d*\\]+", RegexOptions.IgnoreCase)).Matches(strFormula))
                            {
                                int oAccountId = Convert.ToInt32(RegexResult.ToString().Replace("[", "").Replace("]", ""));

                                strFormula = strFormula.Replace
                                    (RegexResult.ToString(),
                                    AccountValues[oAccountId].Item3.Value.ToString("0.0").Replace(",", "."));
                            }
                            oAccountDecimalValue = MathEval(strFormula);
                        }
                    }
                    else if (oAccountInfoType == 1)
                    {
                        //is value field
                        oAccountDecimalValue = aci.Item5;
                    }

                    #endregion

                    #region refresh balance object

                    if (aci.Item3.BalanceSheetId > 0)
                    {
                        //update balance value
                        aci.Item3.Value = oAccountDecimalValue;
                        aci.Item3.Enable = true;
                    }
                    else
                    {
                        //new balance
                        aci.Item3.RelatedAccount = aci.Item2;
                        aci.Item3.Value = oAccountDecimalValue;
                        aci.Item3.Enable = true;
                    }
                    #endregion
                }
                else
                {
                    //-1 to exlude balance sheet item in database
                    aci.Item3.BalanceSheetId = -1;
                }
                return true;
            });

            //execute validations
            AccountValues.Select(aci => aci.Value).OrderBy(aci => aci.Item1).All(aci =>
            {
                //is formula field
                string strValidationFormula = aci.Item2.ItemInfo.
                    Where(x => x.ItemInfoType.ItemId == 109004).
                    Select(x => x.LargeValue).
                    DefaultIfEmpty(string.Empty).
                    FirstOrDefault().ToLower().Replace(" ", "");

                if (!string.IsNullOrEmpty(strValidationFormula))
                {
                    foreach (var RegexResult in (new Regex("[\\[\\d\\]]+", RegexOptions.IgnoreCase)).Matches(strValidationFormula))
                    {
                        int oAccountId = Convert.ToInt32(RegexResult.ToString().Replace("[", "").Replace("]", ""));

                        strValidationFormula = strValidationFormula.Replace
                            (RegexResult.ToString(),
                            AccountValues[oAccountId].Item3.Value.ToString(System.Globalization.CultureInfo.CreateSpecificCulture("EN-us")));
                    }
                    if (!MathComparison(strValidationFormula))
                    {
                        throw new Exception("No se cumple la regla de validación para la cuenta " + aci.Item2.ItemId + "-" + aci.Item2.ItemInfo);
                    }
                }
                return true;
            });

            //add balancesheet to result

            oReturn.BalanceSheetInfo = AccountValues.
                Select(aci => aci.Value).
                Where(aci => aci.Item3.BalanceSheetId > -1).
                OrderBy(aci => aci.Item1).
                Select(aci => aci.Item3).
                ToList();

            return oReturn;
        }

        //recursive hierarchy get account
        private static void GetAccountArray
            (Dictionary<int, Tuple<int, GenericItemModel, BalanceSheetDetailModel, decimal, decimal>> AccountValues,

            BalanceSheetModel BalanceToEval,
            //decimal CurrencyRate,
            ProveedoresOnLine.Company.Models.Util.GenericItemModel RelatedAccount,


            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> lstAccount,
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetDetailModel> lstBalanceSheetDetail)
        {
            lstAccount.
                Where(ac => RelatedAccount != null ?
                        (ac.ParentItem != null && ac.ParentItem.ItemId == RelatedAccount.ItemId) :
                        (ac.ParentItem == null)).
                OrderBy(ac => ac.ItemInfo.
                    Where(aci => aci.ItemInfoType.ItemId == 109001).
                    Select(aci => Convert.ToInt32(aci.Value)).
                    DefaultIfEmpty(0).
                    FirstOrDefault()).
                All(ac =>
                {
                    GetAccountArray(
                        AccountValues,

                        BalanceToEval,
                        //CurrencyRate,
                        ac,

                        lstAccount,
                        lstBalanceSheetDetail);

                    decimal oCurrentOriginalValue = BalanceToEval.BalanceSheetInfo.
                        Where(bsi => bsi.RelatedAccount.ItemId == ac.ItemId).
                        Select(x => x.Value).
                        DefaultIfEmpty(0).
                        FirstOrDefault();

                    string oAccountUnit = ac.ItemInfo.
                        Where(aci => aci.ItemInfoType.ItemId == 109007).
                        Select(aci => aci.Value).
                        DefaultIfEmpty(string.Empty).
                        FirstOrDefault();

                    AccountValues[ac.ItemId] = new Tuple<int, GenericItemModel, BalanceSheetDetailModel, decimal, decimal>
                        (AccountValues.Count,
                        ac,
                        lstBalanceSheetDetail.
                            Where(bsd => bsd.RelatedAccount.ItemId == ac.ItemId).
                            DefaultIfEmpty(new BalanceSheetDetailModel()).
                            FirstOrDefault(),
                        oCurrentOriginalValue,
                        oCurrentOriginalValue);
                    //oAccountUnit.IndexOf("$") >= 0 ? oCurrentOriginalValue * CurrencyRate : oCurrentOriginalValue);

                    return true;
                });
        }

        #endregion

        #endregion

        #region Provider Legal

        public static ProviderModel LegalUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedLegal != null &&
                ProviderToUpsert.RelatedLegal.Count > 0)
            {
                ProviderToUpsert.RelatedLegal.All(plegal =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        plegal.ItemId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.LegalUpsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId,
                            plegal.ItemId > 0 ? (int?)plegal.ItemId : null,
                            plegal.ItemType.ItemId,
                            plegal.ItemName,
                            plegal.Enable);

                        LegalInfoUpsert(plegal);

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
                        oLog.LogObject = plegal;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = ProviderToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProviderToUpsert;
        }

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel LegalInfoUpsert
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel LegalToUpsert)
        {
            if (LegalToUpsert.ItemId > 0 &&
                LegalToUpsert.ItemInfo != null &&
                LegalToUpsert.ItemInfo.Count > 0)
            {
                LegalToUpsert.ItemInfo.All(legalinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        legalinf.ItemInfoId = DAL.Controller.CompanyProviderDataController.Instance.LegalInfoUpsert
                            (LegalToUpsert.ItemId,
                            legalinf.ItemInfoId > 0 ? (int?)legalinf.ItemInfoId : null,
                            legalinf.ItemInfoType.ItemId,
                            legalinf.Value,
                            legalinf.LargeValue,
                            legalinf.Enable);

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
                        oLog.LogObject = legalinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "LegalId",
                            Value = LegalToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return LegalToUpsert;
        }

        public static List<GenericItemModel> LegalGetBasicInfo(string CompanyPublicId, int? LegalType, bool Enable)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.LegalGetBasicInfo(CompanyPublicId, LegalType, Enable);
        }

        #endregion

        #region Provider BlackList

        public static ProviderModel BlackListInsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedBlackList != null &&
                ProviderToUpsert.RelatedBlackList.Count > 0)
            {

                ProviderToUpsert.RelatedBlackList.All(bList =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        bList.BlackListId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.BlackListInsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId, bList.BlackListStatus.ItemId, bList.User, bList.FileUrl);

                        BlackListInfoInsert(bList);

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
                        oLog.LogObject = bList;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = ProviderToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }
            return ProviderToUpsert;
        }

        public static ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel BlackListInfoInsert
          (ProveedoresOnLine.CompanyProvider.Models.Provider.BlackListModel BlackListToInsert)
        {
            if (BlackListToInsert.BlackListInfo != null &&
                BlackListToInsert.BlackListInfo.Count > 0)
            {
                BlackListToInsert.BlackListInfo.All(blackinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        blackinf.ItemInfoId = DAL.Controller.CompanyProviderDataController.Instance.BlackListInfoInsert
                            (BlackListToInsert.BlackListId, blackinf.ItemInfoType.ItemName, blackinf.Value);

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
                        oLog.LogObject = blackinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "LegalId",
                            Value = BlackListToInsert.BlackListId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }
            return BlackListToInsert;
        }

        #endregion

        #region Util

        public static List<Company.Models.Util.CatalogModel> CatalogGetProviderOptions()
        {
            return DAL.Controller.CompanyProviderDataController.Instance.CatalogGetProviderOptions();
        }

        #endregion

        #region MarketPlace

        public static List<Models.Provider.ProviderModel> MPProviderSearch(string CustomerPublicId, string SearchParam, string SearchFilter, int SearchOrderType, bool OrderOrientation, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPProviderSearch(CustomerPublicId, SearchParam, SearchFilter, SearchOrderType, OrderOrientation, PageNumber, RowCount, out TotalRows);
        }

        public static List<Company.Models.Util.GenericFilterModel> MPProviderSearchFilter(string CustomerPublicId, string SearchParam, string SearchFilter)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPProviderSearchFilter(CustomerPublicId, SearchParam, SearchFilter);
        }

        public static List<Models.Provider.ProviderModel> MPProviderSearchById(string CustomerPublicId, string lstProviderPublicId)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPProviderSearchById(CustomerPublicId, lstProviderPublicId);
        }

        public static Company.Models.Company.CompanyModel MPCompanyGetBasicInfo(string CompanyPublicId)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPCompanyGetBasicInfo(CompanyPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPContactGetBasicInfo(string CompanyPublicId, int? ContactType)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPContactGetBasicInfo(CompanyPublicId, ContactType);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCommercialGetBasicInfo(string CompanyPublicId, int? CommercialType, string CustomerPublicId)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPCommercialGetBasicInfo(CompanyPublicId, CommercialType, CustomerPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCertificationGetBasicInfo(string CompanyPublicId, int? CertificationType)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPCertificationGetBasicInfo(CompanyPublicId, CertificationType);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPFinancialGetBasicInfo(string CompanyPublicId, int? FinancialType)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPFinancialGetBasicInfo(CompanyPublicId, FinancialType);
        }

        public static List<Models.Provider.BalanceSheetModel> MPBalanceSheetGetByYear(string CompanyPublicId, int? Year, int CurrencyType)
        {
            List<Models.Provider.BalanceSheetModel> oReturn = new List<BalanceSheetModel>();

            //get all provider balance
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oBalanceBasicInfo = MPFinancialGetBasicInfo(CompanyPublicId, 501001);
            if (oBalanceBasicInfo == null)
                oBalanceBasicInfo = new List<GenericItemModel>();

            //get values by year
            List<Models.Provider.BalanceSheetModel> oBalanceAccountInfo =
                DAL.Controller.CompanyProviderDataController.Instance.MPBalanceSheetGetByYear(CompanyPublicId, Year);
            if (oBalanceAccountInfo == null)
                oBalanceAccountInfo = new List<BalanceSheetModel>();

            oBalanceBasicInfo.All(bbi =>
            {
                Models.Provider.BalanceSheetModel oBalanceToAdd = new BalanceSheetModel()
                {
                    ItemId = bbi.ItemId,
                    ItemType = bbi.ItemType,
                    ItemName = bbi.ItemName,
                    Enable = bbi.Enable,
                    LastModify = bbi.LastModify,
                    CreateDate = bbi.CreateDate,
                    ItemInfo = bbi.ItemInfo,
                    BalanceSheetInfo = new List<BalanceSheetDetailModel>(),
                };

                //get currency rate
                decimal oExchangeRate = ProveedoresOnLine.Company.Controller.Company.CurrencyExchangeGetRate
                    (oBalanceToAdd.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == 502003).
                        Select(x => Convert.ToInt32(x.Value)).
                        DefaultIfEmpty(CurrencyType).
                        FirstOrDefault(),
                    CurrencyType,
                    oBalanceToAdd.ItemInfo.
                        Where(x => x.ItemInfoType.ItemId == 502001).
                        Select(x => Convert.ToInt32(x.Value)).
                        DefaultIfEmpty(Year != null ? Year.Value : DateTime.Now.Year).
                        FirstOrDefault());

                oBalanceAccountInfo.
                    Where(bai => bai.ItemId == oBalanceToAdd.ItemId).
                    All(bai => bai.BalanceSheetInfo.All(baid =>
                    {
                        string oAccountUnit = baid.RelatedAccount.ItemInfo.
                            Where(x => x.ItemInfoType.ItemId == 109007).
                            Select(x => x.Value).
                            DefaultIfEmpty("$").
                            FirstOrDefault();

                        oBalanceToAdd.BalanceSheetInfo.Add(new BalanceSheetDetailModel()
                        {
                            BalanceSheetId = baid.BalanceSheetId,
                            RelatedAccount = baid.RelatedAccount,
                            Value = oAccountUnit.Replace(" ", "") == "$" ? (baid.Value * oExchangeRate) : baid.Value,
                        });

                        return true;
                    }));

                oReturn.Add(oBalanceToAdd);

                return true;
            });

            return oReturn;
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPLegalGetBasicInfo(string CompanyPublicId, int? LegalType)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPLegalGetBasicInfo(CompanyPublicId, LegalType);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCustomerProviderGetTracking(string CustomerPublicId, string ProviderPublicId)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPCustomerProviderGetTracking(CustomerPublicId, ProviderPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPFinancialGetLastyearInfoDeta(string ProviderPublicId)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPFinancialGetLastyearInfoDeta(ProviderPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel> MPCertificationGetSpecificCert(string ProviderPublicId)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPCertificationGetSpecificCert(ProviderPublicId);
        }

        public static List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> MPCustomerProviderGetAllTracking(string CustomerPublicId, string ProviderPublicId)
        {
            return DAL.Controller.CompanyProviderDataController.Instance.MPCustomerProviderGetAllTracking(CustomerPublicId, ProviderPublicId);
        }

        #endregion

        #region BatchProcess

        public static List<ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel> BPGetRecruitmentProviders()
        {
            return DAL.Controller.CompanyProviderDataController.Instance.BPGetRecruitmentProviders();
        }

        #endregion

    }
}
