using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ProveedoresOnLine.CalificationBatch
{
    public class CalificationProcess
    {
        public static void StartProcess()
        {
            try
            {
                List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> oCalificationProjectConfigModel =
                    ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetAll();

                oCalificationProjectConfigModel.All(cnf =>
                {
                    List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oRelatedProvider =
                        ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId(cnf.Company.CompanyPublicId);

                    oRelatedProvider.All(prv =>
                    {
                        List<Models.CalificationProjectBatch.CalificationProjectBatchModel> oRelatedCalificationProject =
                           ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetByCustomer(cnf.Company.CompanyPublicId, prv.CompanyPublicId, true);

                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel oCalificaitonProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel();

                        int oTotalScore = 0;

                        if (oRelatedCalificationProject != null &&
                            oRelatedCalificationProject.Count > 0)
                        {
                            //validate date calification process config
                            oRelatedCalificationProject.All(cp =>
                            {
                                oCalificaitonProjectUpsert = cp;

                                cp.ProjectConfigModel = oCalificationProjectConfigModel.Where(x => x.CalificationProjectConfigId == cp.ProjectConfigModel.CalificationProjectConfigId).Select(x => x).FirstOrDefault();

                                List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel> oCalCalificationProjectItemBatchToUpsert = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>();

                                //Modules
                                cp.CalificationProjectItemBatchModel.All(cpib =>
                                {
                                    cpib.CalificationProjectConfigItem = cp.ProjectConfigModel.ConfigItemModel.Where(y => y.CalificationProjectConfigItemId == cpib.CalificationProjectConfigItem.CalificationProjectConfigItemId).Select(y => y).FirstOrDefault();

                                    //add config item info
                                    cpib.CalificatioProjectItemInfoModel.All(inf =>
                                    {
                                        inf.CalificationProjectConfigItemInfoModel = cpib.CalificationProjectConfigItem.CalificationProjectConfigItemInfoModel.Where(z => z.CalificationProjectConfigItemInfoId == inf.CalificationProjectConfigItemInfoModel.CalificationProjectConfigItemInfoId).Select(z => z).FirstOrDefault();
                                        return true;
                                    });

                                    switch (cpib.CalificationProjectConfigItem.CalificationProjectConfigItemType.ItemId)
                                    {
                                        #region LegalModule

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                            ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oLegalModule =
                                                ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                            oTotalScore += oLegalModule.ItemScore;

                                            oCalCalificationProjectItemBatchToUpsert.Add(oLegalModule);

                                            break;

                                        #endregion

                                        #region FinancialModule

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

                                            ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
                                                ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                            oTotalScore += oFinancialModule.ItemScore;

                                            oCalCalificationProjectItemBatchToUpsert.Add(oFinancialModule);

                                            break;

                                        #endregion

                                        #region CommercialModule

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                            ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
                                                ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                            oTotalScore += oCommercialModule.ItemScore;

                                            oCalCalificationProjectItemBatchToUpsert.Add(oCommercialModule);

                                            break;

                                        #endregion

                                        #region HSEQModule

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

                                            ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
                                                ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                            oTotalScore += oCertificationModule.ItemScore;
                                            oCalCalificationProjectItemBatchToUpsert.Add(oCertificationModule);

                                            break;
                                        
                                        #endregion

                                        #region BalanceModule

                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                            ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
                                                ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                            oTotalScore += oBalanceModule.ItemScore;

                                            oCalCalificationProjectItemBatchToUpsert.Add(oBalanceModule);

                                            break;

                                        #endregion
                                    }

                                    return true;
                                });

                                cp.CalificationProjectItemBatchModel = oCalCalificationProjectItemBatchToUpsert;

                                return true;
                            });
                        }
                        else
                        {
                            oCalificaitonProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                            {
                                CalificationProjectId = 0,
                                CalificationProjectPublicId = "",
                                ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                                {
                                    CalificationProjectConfigId = cnf.CalificationProjectConfigId,
                                },
                                RelatedProvider = new Company.Models.Company.CompanyModel()
                                {
                                    CompanyPublicId = prv.CompanyPublicId,
                                },
                                CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>(),
                                Enable = true,
                            };

                            //execute all calification process
                            cnf.ConfigItemModel.All(md =>
                            {
                                switch (md.CalificationProjectConfigItemType.ItemId)
                                {
                                    #region LegalModule

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oLegalModule =
                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, md, null);

                                        oTotalScore += oLegalModule.ItemScore;

                                        oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oLegalModule);

                                        break;

                                    #endregion

                                    #region FinancialModule

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, md, null);

                                        oTotalScore += oFinancialModule.ItemScore;

                                        oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oFinancialModule);

                                        break;

                                    #endregion

                                    #region CommercialModule

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
                                                ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, md, null);

                                            oTotalScore += oCommercialModule.ItemScore;

                                            oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCommercialModule);
                                        break;

                                    #endregion

                                    #region HSEQModule

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:
                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
                                                ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, md, null);

                                            oTotalScore += oCertificationModule.ItemScore;
                                            oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCertificationModule);

                                        break;

                                    #endregion

                                    #region BalanceModule

                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, md, null);

                                        oTotalScore += oBalanceModule.ItemScore;

                                        oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oBalanceModule);

                                        break;

                                    #endregion
                                }

                                return true;
                            });
                        }

                        oCalificaitonProjectUpsert.TotalScore = oTotalScore;

                        //Upsert
                        oCalificaitonProjectUpsert = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oCalificaitonProjectUpsert);
                        
                        LogFile("ProcesId:: " + oCalificaitonProjectUpsert.CalificationProjectId + " RelatedProviderPublicId:: " + prv.CompanyPublicId + " RelatedCustomerPublicId:: " + cnf.Company.CompanyPublicId);

                        return true;
                    });

                    return true;
                });
            }
            catch (Exception )
            {
                //ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }

        private void GetCustomer()
        {
        }

        #region Log File

        public static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                //string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                //System.Configuration.ConfigurationManager.AppSettings[ProveedoresOnLine.CalificationBatch.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                string LogFile = "D:\\UploadLog\\";

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_CalificationProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                using (System.IO.StreamWriter sw = System.IO.File.AppendText(LogFile))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "::" + LogMessage);
                    sw.Close();
                }
            }
            catch { }
        }

        #endregion
    }
}
