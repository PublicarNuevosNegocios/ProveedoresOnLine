﻿using ProveedoresOnLine.CompareModule.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CompareModule.Controller
{
    public class CompareModule
    {
        public static CompareModel CompareUpsert(CompareModel CompareToUpsert)
        {
            if (CompareToUpsert != null)
            {
                LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();
                try
                {
                    CompareToUpsert.CompareId =
                        DAL.Controller.CompareDataController.Instance.CompareUpsert
                        (CompareToUpsert.CompareId > 0 ? (int?)CompareToUpsert.CompareId : null,
                        CompareToUpsert.CompareName,
                        CompareToUpsert.User,
                        CompareToUpsert.Enable);

                    CompareToUpsert = CompareCompanyUpsert(CompareToUpsert);

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
                    oLog.LogObject = CompareToUpsert;
                    LogManager.ClientLog.AddLog(oLog);
                }
            }

            return CompareToUpsert;
        }

        public static CompareModel CompareCompanyUpsert(CompareModel CompareToUpsert)
        {
            if (CompareToUpsert != null &&
                CompareToUpsert.CompareId > 0 &&
                CompareToUpsert.RelatedProvider != null &&
                CompareToUpsert.RelatedProvider.Count > 0)
            {
                CompareToUpsert.RelatedProvider.
                    Where(cmpc => cmpc.RelatedCompany != null &&
                        !string.IsNullOrEmpty(cmpc.RelatedCompany.CompanyPublicId)).
                    All(cmpc =>
                {
                    LogManager.Models.LogModel oLog = Company.Controller.Company.GetGenericLogModel();

                    try
                    {
                        cmpc.CompareCompanyId = DAL.Controller.CompareDataController.Instance.CompareCompanyUpsert
                            (CompareToUpsert.CompareId,
                            cmpc.RelatedCompany.CompanyPublicId,
                            cmpc.Enable);

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
                        oLog.LogObject = cmpc;

                        oLog.RelatedLogInfo.Add(new LogManager.Models.LogInfoModel()
                        {
                            LogInfoType = "CompareId",
                            Value = CompareToUpsert.CompareId.ToString(),
                        });

                        LogManager.ClientLog.AddLog(oLog);
                    }
                    return true;
                });
            }

            return CompareToUpsert;
        }

        public static CompareModel CompareGetCompanyBasicInfo(int CompareId)
        {
            return DAL.Controller.CompareDataController.Instance.CompareGetCompanyBasicInfo(CompareId);
        }

        public static List<CompareModel> CompareSearch(string SearchParam, string User, int PageNumber, int RowCount, out int TotalRows)
        {
            return DAL.Controller.CompareDataController.Instance.CompareSearch(SearchParam, User, PageNumber, RowCount, out  TotalRows);
        }
    }
}
