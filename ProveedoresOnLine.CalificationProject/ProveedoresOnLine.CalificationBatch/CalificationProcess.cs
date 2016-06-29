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
                LogFile("Start Process:::" + DateTime.Now);
                //Get all calification project config
                List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> oCalificationProjectConfigModel =
                    ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetAll();                

                //validate calification project config list
                if (oCalificationProjectConfigModel != null &&
                    oCalificationProjectConfigModel.Count > 0)
                {                    
                    oCalificationProjectConfigModel.All(cnf =>
                    {
                        //Get all related provider by customer
                        List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oRelatedProvider =
                            ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId(cnf.Company.CompanyPublicId);

                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel oModelToUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel();

                        //validate provider list
                        if (oRelatedProvider != null &&
                            oRelatedProvider.Count > 0)
                        {
                            LogFile("Provider Process:::" + "Providers Count::::" + oRelatedProvider.Count.ToString() + "::::" + DateTime.Now);
                            oRelatedProvider.All(prv =>
                            {
                                LogFile("Provider in Process::" + prv.CompanyPublicId + ":::" + DateTime.Now);
                                //Get calification process by provider
                                List<Models.CalificationProjectBatch.CalificationProjectBatchModel> oRelatedCalificationProject =
                                   ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetProviderByCustomer(cnf.Company.CompanyPublicId, prv.CompanyPublicId);

                                LogFile("Provider in Process::" + prv.CompanyPublicId + ":::" + DateTime.Now + "::: Antes de recorrer el lista de PDC");

                                //validate calification project list
                                if (oRelatedCalificationProject != null &&
                                    oRelatedCalificationProject.Count > 0)
                                {
                                    LogFile("Provider in Process::" + prv.CompanyPublicId + ":::" + DateTime.Now + "::: Existen PDC");
                                    //update calification project!!!                                    
                                    #region Validate calification project with config

                                    //validate all calification project config (Calification project - calification project item)
                                    oRelatedCalificationProject.All(cp =>
                                    {
                                        //get related calification project config
                                        cp.ProjectConfigModel = oCalificationProjectConfigModel.Where(config => config.CalificationProjectConfigId == cp.ProjectConfigModel.CalificationProjectConfigId).Select(config => config).FirstOrDefault();

                                        //validate calification project config is enable
                                        if (cp.ProjectConfigModel.Enable)
                                        {
                                            cp.CalificationProjectItemBatchModel.All(cpi =>
                                            {
                                                //get related calification project item config
                                                cpi.CalificationProjectConfigItem = cp.ProjectConfigModel.ConfigItemModel.Where(configit => configit.CalificationProjectConfigItemId == cpi.CalificationProjectConfigItem.CalificationProjectConfigItemId).Select(configit => configit).FirstOrDefault();

                                                //validate calification project config item is enable
                                                if (!cpi.CalificationProjectConfigItem.Enable)
                                                {
                                                    //disable calification project config item
                                                    cpi.Enable = false;

                                                    cpi.CalificatioProjectItemInfoModel.All(cpinf =>
                                                    {
                                                        cpinf.Enable = false;

                                                        return true;
                                                    });
                                                }

                                                return true;
                                            });

                                            //upsert
                                            cp = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(cp);
                                        }
                                        else
                                        {
                                            //disable calification project
                                            cp.Enable = false;

                                            cp.CalificationProjectItemBatchModel.All(cpi =>
                                            {
                                                cpi.Enable = false;

                                                cpi.CalificatioProjectItemInfoModel.All(cpiinf =>
                                                {
                                                    cpiinf.Enable = false;

                                                    return true;
                                                });

                                                return true;
                                            });

                                            //upsert
                                            cp = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(cp);
                                        }
                                        return true;
                                    });

                                    #endregion

                                    #region run calification project

                                    oRelatedCalificationProject.Where(cp => cp.Enable == true).All(cp =>
                                    {
                                        //get related calification project config
                                        cp.ProjectConfigModel = oCalificationProjectConfigModel.Where(config => config.CalificationProjectConfigId == cp.ProjectConfigModel.CalificationProjectConfigId).Select(config => config).FirstOrDefault();

                                        //set data to model to upsert
                                        oModelToUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                                        {
                                            CalificationProjectId = cp.CalificationProjectId,
                                            CalificationProjectPublicId = cp.CalificationProjectPublicId,
                                            Enable = cp.Enable,
                                            ProjectConfigModel = cp.ProjectConfigModel,
                                            RelatedProvider = cp.RelatedProvider,
                                            TotalScore = cp.TotalScore,
                                            CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>(),
                                        };

                                        cp.ProjectConfigModel.ConfigItemModel.Where(ci => ci.Enable == true).All(ci =>
                                        {
                                            //validate related config with calification project
                                            if (cp.CalificationProjectItemBatchModel.Any(cpib => cpib.CalificationProjectConfigItem.CalificationProjectConfigItemId == ci.CalificationProjectConfigItemId))
                                            {
                                                cp.CalificationProjectItemBatchModel.Where(cpib => cpib.CalificationProjectConfigItem.CalificationProjectConfigItemId == ci.CalificationProjectConfigItemId).All(cpib =>
                                                {
                                                    //get related calification project item config
                                                    cpib.CalificationProjectConfigItem = ci;

                                                    //update validation module
                                                    switch (ci.CalificationProjectConfigItemType.ItemId)
                                                    {
                                                        #region LegalModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                                            cpib = ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            oModelToUpsert.CalificationProjectItemBatchModel.Add(cpib);

                                                            break;

                                                        #endregion

                                                        #region FinancialModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

                                                            cpib = ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            oModelToUpsert.CalificationProjectItemBatchModel.Add(cpib);

                                                            break;

                                                        #endregion

                                                        #region CommercialModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                                            cpib = ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            oModelToUpsert.CalificationProjectItemBatchModel.Add(cpib);

                                                            break;

                                                        #endregion

                                                        #region HSEQModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

                                                            cpib = ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            oModelToUpsert.CalificationProjectItemBatchModel.Add(cpib);

                                                            break;

                                                        #endregion

                                                        #region BalanceModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                                            cpib = ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            oModelToUpsert.CalificationProjectItemBatchModel.Add(cpib);

                                                            break;

                                                        #endregion
                                                    }

                                                    return true;
                                                });

                                                oModelToUpsert.TotalScore = 0;

                                                oModelToUpsert.CalificationProjectItemBatchModel.All(cpi =>
                                                {
                                                    oModelToUpsert.TotalScore += cpi.ItemScore;

                                                    return true;
                                                });

                                                //Upsert
                                                oModelToUpsert = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oModelToUpsert);
                                            }
                                            else
                                            {
                                                //add item
                                                switch (ci.CalificationProjectConfigItemType.ItemId)
                                                {
                                                    #region LegalModule

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oLegalModule =
                                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, ci, null);

                                                        oModelToUpsert.CalificationProjectItemBatchModel.Add(oLegalModule);

                                                        break;

                                                    #endregion

                                                    #region FinancialModule

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

                                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
                                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, ci, null);

                                                        oModelToUpsert.CalificationProjectItemBatchModel.Add(oFinancialModule);

                                                        break;

                                                    #endregion

                                                    #region CommercialModule

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
                                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, ci, null);

                                                        oModelToUpsert.CalificationProjectItemBatchModel.Add(oCommercialModule);

                                                        break;

                                                    #endregion

                                                    #region HSEQModule

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

                                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
                                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, ci, null);

                                                        oModelToUpsert.CalificationProjectItemBatchModel.Add(oCertificationModule);

                                                        break;

                                                    #endregion

                                                    #region BalanceModule

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                                        ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
                                                            ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, ci, null);

                                                        oModelToUpsert.CalificationProjectItemBatchModel.Add(oBalanceModule);

                                                        break;

                                                    #endregion
                                                }

                                                oModelToUpsert.TotalScore = 0;

                                                oModelToUpsert.CalificationProjectItemBatchModel.All(cpi =>
                                                {
                                                    oModelToUpsert.TotalScore += cpi.ItemScore;

                                                    return true;
                                                });

                                                //Upsert
                                                oModelToUpsert = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oModelToUpsert);
                                            }

                                            return true;
                                        });

                                        return true;
                                    });

                                    #endregion                                    
                                }
                                else
                                {
                                    LogFile("Provider in Process::" + prv.CompanyPublicId + ":::" + DateTime.Now + "::: Proceso Nuevo");
                                    #region New Calification project

                                    //new calification project
                                    Models.CalificationProjectBatch.CalificationProjectBatchModel oCalificationProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
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
                                    cnf.ConfigItemModel.Where(md => md.Enable == true).All(md =>
                                    {
                                        LogFile("Provider in Process::" + prv.CompanyPublicId + ":::" + DateTime.Now + "::: Validación de módulos");
                                        switch (md.CalificationProjectConfigItemType.ItemId)
                                        {
                                            #region LegalModule
                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:                                                
                                                ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oLegalModule =
                                                    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, md, null);

                                                oCalificationProjectUpsert.CalificationProjectItemBatchModel.Add(oLegalModule);

                                                break;

                                            #endregion

                                            #region FinancialModule

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

                                                ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
                                                    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, md, null);

                                                oCalificationProjectUpsert.CalificationProjectItemBatchModel.Add(oFinancialModule);

                                                break;

                                            #endregion

                                            #region CommercialModule

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                                ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
                                                    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, md, null);

                                                oCalificationProjectUpsert.CalificationProjectItemBatchModel.Add(oCommercialModule);

                                                break;

                                            #endregion

                                            #region HSEQModule

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

                                                ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
                                                    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, md, null);

                                                oCalificationProjectUpsert.CalificationProjectItemBatchModel.Add(oCertificationModule);

                                                break;

                                            #endregion

                                            #region BalanceModule

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                                ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
                                                    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, md, null);

                                                oCalificationProjectUpsert.CalificationProjectItemBatchModel.Add(oBalanceModule);

                                                break;

                                            #endregion
                                        }

                                        return true;
                                    });

                                    //get total score
                                    oCalificationProjectUpsert.CalificationProjectItemBatchModel.Where(sit => sit.Enable == true).All(sit =>
                                    {
                                        oCalificationProjectUpsert.TotalScore += sit.ItemScore;

                                        return true;
                                    });

                                    //Upsert
                                    oCalificationProjectUpsert = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oCalificationProjectUpsert);
                                    
                                    #endregion
                                }

                                LogFile("End provider Process::" + prv.CompanyPublicId + ":::" + DateTime.Now);

                                return true;
                            });

                        }
                        else
                        {
                            //Provider list is empty
                            ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Error:: el cliente " + cnf.Company.CompanyPublicId + " no tiene proveedores relacionados para ejecutar el proceso.");
                        }

                        return true;
                    });
                }
                else
                {
                    //calification project config list is empty
                    LogFile("Error:: no hay procesos de calificación configurados.");
                }
            }
            catch (Exception err)
            {
                LogFile("Fatal Error::" + err.Message + " - " + err.StackTrace);
            }

            LogFile("End Process:::" + DateTime.Now);
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
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[ProveedoresOnLine.CalificationBatch.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

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
