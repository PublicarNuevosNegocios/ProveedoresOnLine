using ProveedoresOnLine.CompanyProvider.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

                ProviderCategoryUpsert(ProviderToUpsert);
                ExperienceUpsert(ProviderToUpsert);
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

        #region Provider Experience

        public static ProviderModel ProviderCategoryUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedCategory != null &&
                ProviderToUpsert.RelatedCategory.Count > 0)
            {
                ProviderToUpsert.RelatedCategory.All(pcat =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pcat.CompanyCategoryId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.ProviderCategoryUpsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId,
                            pcat.RelatedCategory.ItemId,
                            pcat.Enable);

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
                        oLog.LogObject = pcat;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompanyPublicId",
                            Value = ProviderToUpsert.RelatedCompany.CompanyPublicId,
                        });

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CategoryId",
                            Value = pcat.RelatedCategory.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ProviderToUpsert;
        }

        public static ProviderModel ExperienceUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedExperience != null &&
                ProviderToUpsert.RelatedExperience.Count > 0)
            {
                ProviderToUpsert.RelatedExperience.All(pexp =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pexp.ItemId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.ExperienceUpsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId,
                            pexp.ItemId > 0 ? (int?)pexp.ItemId : null,
                            pexp.ItemName,
                            pexp.Enable);

                        ExperienceInfoUpsert(pexp);

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
                        oLog.LogObject = pexp;

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

        public static ProveedoresOnLine.Company.Models.Util.GenericItemModel ExperienceInfoUpsert
            (ProveedoresOnLine.Company.Models.Util.GenericItemModel ExperienceToUpsert)
        {
            if (ExperienceToUpsert.ItemId > 0 &&
                ExperienceToUpsert.ItemInfo != null &&
                ExperienceToUpsert.ItemInfo.Count > 0)
            {
                ExperienceToUpsert.ItemInfo.All(expinf =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        expinf.ItemInfoId = DAL.Controller.CompanyProviderDataController.Instance.ExperienceInfoUpsert
                            (ExperienceToUpsert.ItemId,
                            expinf.ItemInfoId > 0 ? (int?)expinf.ItemInfoId : null,
                            expinf.ItemInfoType.ItemId,
                            expinf.Value,
                            expinf.LargeValue,
                            expinf.Enable);

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
                        oLog.LogObject = expinf;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "ExperienceId",
                            Value = ExperienceToUpsert.ItemId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }

                    return true;
                });
            }

            return ExperienceToUpsert;
        }

        #endregion

        #region Provider certification

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

        #endregion

        #region Provider financial

        public static ProviderModel FinancialUpsert(ProviderModel ProviderToUpsert)
        {
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedFinantial != null &&
                ProviderToUpsert.RelatedFinantial.Count > 0)
            {
                ProviderToUpsert.RelatedCertification.All(pfin =>
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
            if (ProviderToUpsert.RelatedCompany != null &&
                !string.IsNullOrEmpty(ProviderToUpsert.RelatedCompany.CompanyPublicId) &&
                ProviderToUpsert.RelatedBalanceSheet != null &&
                ProviderToUpsert.RelatedBalanceSheet.Count > 0)
            {
                ProviderToUpsert.RelatedBalanceSheet.All(pSheet =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                    try
                    {
                        pSheet.ItemId =
                            ProveedoresOnLine.CompanyProvider.DAL.Controller.CompanyProviderDataController.Instance.FinancialUpsert
                            (ProviderToUpsert.RelatedCompany.CompanyPublicId,
                            pSheet.ItemId > 0 ? (int?)pSheet.ItemId : null,
                            pSheet.ItemType.ItemId,
                            pSheet.ItemName,
                            pSheet.Enable);

                        BalanceSheetInfoUpsert(pSheet);

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
                        oLog.LogObject = pSheet;

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

        #endregion

        #region Util

        public static List<Company.Models.Util.CatalogModel> CatalogGetProviderOptions()
        {
            return DAL.Controller.CompanyProviderDataController.Instance.CatalogGetProviderOptions();
        }

        #endregion
    }
}
