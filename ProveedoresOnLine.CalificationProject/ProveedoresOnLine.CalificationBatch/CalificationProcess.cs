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

                        //validate provider list
                        if (oRelatedProvider != null &&
                            oRelatedProvider.Count > 0)
                        {
                            oRelatedProvider.All(prv =>
                            {
                                //Get calification process by provider
                                List<Models.CalificationProjectBatch.CalificationProjectBatchModel> oRelatedCalificationProject =
                                   ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetProviderByCustomer(cnf.Company.CompanyPublicId, prv.CompanyPublicId);

                                //validate calification project list
                                if (oRelatedCalificationProject != null &&
                                    oRelatedCalificationProject.Count > 0)
                                {
                                    //update calification project!!!

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
                                        }
                                        return true;
                                    });

                                    Models.CalificationProjectBatch.CalificationProjectBatchModel oModelToUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel();

                                    int oTotalScore = 0;

                                    oRelatedCalificationProject.Where(cp => cp.Enable == true).All(cp =>
                                    {
                                        //get related calification project config
                                        cp.ProjectConfigModel = oCalificationProjectConfigModel.Where(config => config.CalificationProjectConfigId == cp.ProjectConfigModel.CalificationProjectConfigId).Select(config => config).FirstOrDefault();

                                        //set data to model to upsert
                                        oModelToUpsert = cp;

                                        cp.ProjectConfigModel.ConfigItemModel.Where(ci => ci.Enable == true).All(ci =>
                                        {
                                            //set item data to model to upsert
                                            oModelToUpsert.CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>();

                                            //validate related config with calification project
                                            if (cp.CalificationProjectItemBatchModel.Any(cpib => cpib.CalificationProjectConfigItem.CalificationProjectConfigItemId == ci.CalificationProjectConfigItemId))
                                            {
                                                cp.CalificationProjectItemBatchModel.Where(cpib => cpib.CalificationProjectConfigItem.CalificationProjectConfigItemId == ci.CalificationProjectConfigItemId).All(cpib =>
                                                {
                                                    //get related calification project item config
                                                    cpib.CalificationProjectConfigItem = cp.ProjectConfigModel.ConfigItemModel.Where(configit => configit.CalificationProjectConfigItemId == cpib.CalificationProjectConfigItem.CalificationProjectConfigItemId).Select(configit => configit).FirstOrDefault();

                                                    //update validation module
                                                    switch (cpib.CalificationProjectConfigItem.CalificationProjectConfigItemType.ItemId)
                                                    {
                                                        #region LegalModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                                            oModelToUpsert.CalificationProjectItemBatchModel.Add(ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib));

                                                            break;

                                                        #endregion

                                                        #region FinancialModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

                                                            //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
                                                            //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            //oCalCalificationProjectItemBatchToUpsert.Add(oFinancialModule);

                                                            break;

                                                        #endregion

                                                        #region CommercialModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                                            //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
                                                            //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            //oCalCalificationProjectItemBatchToUpsert.Add(oCommercialModule);

                                                            break;

                                                        #endregion

                                                        #region HSEQModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

                                                            //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
                                                            //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            //oCalCalificationProjectItemBatchToUpsert.Add(oCertificationModule);

                                                            break;

                                                        #endregion

                                                        #region BalanceModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                                            //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
                                                            //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

                                                            //oCalCalificationProjectItemBatchToUpsert.Add(oBalanceModule);

                                                            break;

                                                        #endregion
                                                    }

                                                    return true;
                                                });
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

                                                        //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
                                                        //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, md, null);

                                                        //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oFinancialModule);

                                                        break;

                                                    #endregion

                                                    #region CommercialModule

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                                        //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
                                                        //        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, md, null);

                                                        //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCommercialModule);

                                                        break;

                                                    #endregion

                                                    #region HSEQModule

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

                                                        //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
                                                        //        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, md, null);

                                                        //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCertificationModule);

                                                        break;

                                                    #endregion

                                                    #region BalanceModule

                                                    case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                                        //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
                                                        //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, md, null);

                                                        //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oBalanceModule);

                                                        break;

                                                    #endregion
                                                }
                                            }

                                            oModelToUpsert.TotalScore = 0;

                                            oModelToUpsert.CalificationProjectItemBatchModel.All(cpi =>
                                            {
                                                oModelToUpsert.TotalScore += cpi.ItemScore;

                                                return true;
                                            });

                                            //Upsert
                                            oModelToUpsert = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oModelToUpsert);

                                            return true;
                                        });

                                        return true;
                                    });
                                }
                                else
                                {
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

                                                //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
                                                //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, md, null);

                                                //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oFinancialModule);

                                                break;

                                            #endregion

                                            #region CommercialModule

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

                                                //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
                                                //        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, md, null);

                                                //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCommercialModule);

                                                break;

                                            #endregion

                                            #region HSEQModule

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

                                                //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
                                                //        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, md, null);

                                                //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCertificationModule);

                                                break;

                                            #endregion

                                            #region BalanceModule

                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

                                                //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
                                                //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, md, null);

                                                //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oBalanceModule);

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
                                }

                                return true;
                            });
                        }
                        else
                        {
                            //Provider list is empty
                        }

                        return true;
                    });
                }
                else
                {
                    //calification project config list is empty
                }
            }
            catch (Exception)
            {
                throw;
            }





















            //try
            //{
            //    //Get all calification project config
            //    List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> oCalificationProjectConfigModel =
            //        ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetAll();

            //    if (oCalificationProjectConfigModel != null &&
            //        oCalificationProjectConfigModel.Count > 0)
            //    {
            //        oCalificationProjectConfigModel.All(cpc =>
            //        {
            //            //Get all related provider by customer
            //            List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oRelatedProvider =
            //                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId(cpc.Company.CompanyPublicId);

            //            if (oRelatedProvider != null &&
            //                oRelatedProvider.Count > 0)
            //            {
            //                oRelatedProvider.All(prv =>
            //                {
            //                    //Create model to upsert
            //                    ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel oCalificationProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel();

            //                    //Get calification process by provider
            //                    List<Models.CalificationProjectBatch.CalificationProjectBatchModel> oRelatedCalificationProject =
            //                       ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetProviderByCustomer(cpc.Company.CompanyPublicId, prv.CompanyPublicId);

            //                    if (oRelatedCalificationProject != null &&
            //                        oRelatedCalificationProject.Count > 0)
            //                    {
            //                        //already exists calification project                                   
            //                        oRelatedCalificationProject.All(cp =>
            //                        {
            //                            //Get related calification project config
            //                            cp.ProjectConfigModel = oCalificationProjectConfigModel.Where(x => x.CalificationProjectConfigId == cp.ProjectConfigModel.CalificationProjectConfigId).Select(x => x).FirstOrDefault();

            //                            //validate if config is enable
            //                            if (cp.ProjectConfigModel.Enable)
            //                            {
            //                                cp.CalificationProjectItemBatchModel.All(cpib =>
            //                                {
            //                                    bool itemEnable = false;

            //                                    cp.ProjectConfigModel.ConfigItemModel.Where(itcnf => itcnf.Enable == true).All(itcnf =>
            //                                    {
            //                                        if (!itemEnable && cpib.CalificationProjectConfigItem.CalificationProjectConfigItemId == itcnf.CalificationProjectConfigItemId)
            //                                        {
            //                                            itemEnable = true;
            //                                        }

            //                                        return true;
            //                                    });

            //                                    cpib.Enable = itemEnable;

            //                                    return true;
            //                                });

            //                                cp.ProjectConfigModel.ConfigItemModel.Where(ci => ci.Enable == true).All(ci =>
            //                                {
            //                                    if (cp.CalificationProjectItemBatchModel.Any(x => x.CalificationProjectConfigItem.CalificationProjectConfigItemId == ci.CalificationProjectConfigItemId))
            //                                    {
            //                                        cp.CalificationProjectItemBatchModel.Where(cpi => cpi.CalificationProjectConfigItem.CalificationProjectConfigItemId == ci.CalificationProjectConfigItemId).All(cpi =>
            //                                        {
            //                                            //set calification project item config
            //                                            cpi.CalificationProjectConfigItem = cpc.ConfigItemModel.Where(cpci => cpi.CalificationProjectConfigItem.CalificationProjectConfigItemId == cpci.CalificationProjectConfigItemId).Select(cpci => cpci).FirstOrDefault();

            //                                            //update validation module
            //                                            switch (cpi.CalificationProjectConfigItem.CalificationProjectConfigItemType.ItemId)
            //                                            {
            //                                                #region LegalModule

            //                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

            //                                                    cpi = ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, cpi.CalificationProjectConfigItem, cpi);

            //                                                    break;

            //                                                #endregion

            //                                                #region FinancialModule

            //                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

            //                                                    //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
            //                                                    //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

            //                                                    //oCalCalificationProjectItemBatchToUpsert.Add(oFinancialModule);

            //                                                    break;

            //                                                #endregion

            //                                                #region CommercialModule

            //                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

            //                                                    //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
            //                                                    //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

            //                                                    //oCalCalificationProjectItemBatchToUpsert.Add(oCommercialModule);

            //                                                    break;

            //                                                #endregion

            //                                                #region HSEQModule

            //                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

            //                                                    //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
            //                                                    //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

            //                                                    //oCalCalificationProjectItemBatchToUpsert.Add(oCertificationModule);

            //                                                    break;

            //                                                #endregion

            //                                                #region BalanceModule

            //                                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

            //                                                    //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
            //                                                    //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

            //                                                    //oCalCalificationProjectItemBatchToUpsert.Add(oBalanceModule);

            //                                                    break;

            //                                                #endregion
            //                                            }

            //                                            return true;
            //                                        });
            //                                    }
            //                                    else
            //                                    {
            //                                        //create new data
            //                                        switch (ci.CalificationProjectConfigItemType.ItemId)
            //                                        {
            //                                            #region LegalModule

            //                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

            //                                                cp.CalificationProjectItemBatchModel.Add(ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, ci, null));

            //                                                break;

            //                                            #endregion

            //                                            #region FinancialModule

            //                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

            //                                                //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
            //                                                //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

            //                                                //oCalCalificationProjectItemBatchToUpsert.Add(oFinancialModule);

            //                                                break;

            //                                            #endregion

            //                                            #region CommercialModule

            //                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

            //                                                //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
            //                                                //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

            //                                                //oCalCalificationProjectItemBatchToUpsert.Add(oCommercialModule);

            //                                                break;

            //                                            #endregion

            //                                            #region HSEQModule

            //                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

            //                                                //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
            //                                                //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

            //                                                //oCalCalificationProjectItemBatchToUpsert.Add(oCertificationModule);

            //                                                break;

            //                                            #endregion

            //                                            #region BalanceModule

            //                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

            //                                                //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
            //                                                //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, cpib.CalificationProjectConfigItem, cpib);

            //                                                //oCalCalificationProjectItemBatchToUpsert.Add(oBalanceModule);

            //                                                break;

            //                                            #endregion
            //                                        }
            //                                    }

            //                                    return true;
            //                                });

            //                                //get total score
            //                                cp.CalificationProjectItemBatchModel.Where(sit => sit.Enable == true).All(sit =>
            //                                {
            //                                    cp.TotalScore += sit.ItemScore;

            //                                    return true;
            //                                });

            //                                //Upsert
            //                                cp = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(cp);
            //                            }
            //                            else
            //                            {
            //                                //disable calification project 
            //                                cp.Enable = false;

            //                                //new calification project
            //                                oCalificationProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
            //                                {
            //                                    CalificationProjectId = 0,
            //                                    CalificationProjectPublicId = "",
            //                                    ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
            //                                    {
            //                                        CalificationProjectConfigId = cpc.CalificationProjectConfigId,
            //                                    },
            //                                    RelatedProvider = new Company.Models.Company.CompanyModel()
            //                                    {
            //                                        CompanyPublicId = prv.CompanyPublicId,
            //                                    },
            //                                    CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>(),
            //                                    Enable = true,
            //                                };

            //                                //execute all calification process
            //                                cpc.ConfigItemModel.Where(md => md.Enable == true).All(md =>
            //                                {
            //                                    switch (md.CalificationProjectConfigItemType.ItemId)
            //                                    {
            //                                        #region LegalModule

            //                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

            //                                            ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oLegalModule =
            //                                                ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, md, null);

            //                                            oCalificationProjectUpsert.CalificationProjectItemBatchModel.Add(oLegalModule);

            //                                            break;

            //                                        #endregion

            //                                        #region FinancialModule

            //                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

            //                                            //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
            //                                            //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, md, null);

            //                                            //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oFinancialModule);

            //                                            break;

            //                                        #endregion

            //                                        #region CommercialModule

            //                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

            //                                            //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
            //                                            //        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, md, null);

            //                                            //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCommercialModule);

            //                                            break;

            //                                        #endregion

            //                                        #region HSEQModule

            //                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

            //                                            //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
            //                                            //        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, md, null);

            //                                            //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCertificationModule);

            //                                            break;

            //                                        #endregion

            //                                        #region BalanceModule

            //                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

            //                                            //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
            //                                            //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, md, null);

            //                                            //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oBalanceModule);

            //                                            break;

            //                                        #endregion
            //                                    }

            //                                    return true;
            //                                });

            //                                //get total score
            //                                oCalificationProjectUpsert.CalificationProjectItemBatchModel.Where(sit => sit.Enable == true).All(sit =>
            //                                {
            //                                    oCalificationProjectUpsert.TotalScore += sit.ItemScore;

            //                                    return true;
            //                                });

            //                                //Upsert
            //                                oCalificationProjectUpsert = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oCalificationProjectUpsert);
            //                            }

            //                            return true;
            //                        });

            //                    }
            //                    else
            //                    {
            //                        //new calification project
            //                        oCalificationProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
            //                        {
            //                            CalificationProjectId = 0,
            //                            CalificationProjectPublicId = "",
            //                            ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
            //                            {
            //                                CalificationProjectConfigId = cpc.CalificationProjectConfigId,
            //                            },
            //                            RelatedProvider = new Company.Models.Company.CompanyModel()
            //                            {
            //                                CompanyPublicId = prv.CompanyPublicId,
            //                            },
            //                            CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>(),
            //                            Enable = true,
            //                        };

            //                        //execute all calification process
            //                        cpc.ConfigItemModel.Where(md => md.Enable == true).All(md =>
            //                        {
            //                            switch (md.CalificationProjectConfigItemType.ItemId)
            //                            {
            //                                #region LegalModule

            //                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

            //                                    ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oLegalModule =
            //                                        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, md, null);

            //                                    oCalificationProjectUpsert.CalificationProjectItemBatchModel.Add(oLegalModule);

            //                                    break;

            //                                #endregion

            //                                #region FinancialModule

            //                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_FinancialModule:

            //                                    //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oFinancialModule =
            //                                    //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.FinancialModule.FinancialRule(prv.CompanyPublicId, md, null);

            //                                    //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oFinancialModule);

            //                                    break;

            //                                #endregion

            //                                #region CommercialModule

            //                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_CommercialModule:

            //                                    //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCommercialModule =
            //                                    //        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.CommercialModule.CommercialRule(prv.CompanyPublicId, md, null);

            //                                    //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCommercialModule);

            //                                    break;

            //                                #endregion

            //                                #region HSEQModule

            //                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_HSEQModule:

            //                                    //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oCertificationModule =
            //                                    //        ProveedoresOnLine.CalificationBatch.CalificationProjectModule.HSEQModule.HSEQRule(prv.CompanyPublicId, md, null);

            //                                    //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oCertificationModule);

            //                                    break;

            //                                #endregion

            //                                #region BalanceModule

            //                                case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_BalanceModule:

            //                                    //ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectItemBatchModel oBalanceModule =
            //                                    //    ProveedoresOnLine.CalificationBatch.CalificationProjectModule.BalanceModule.BalanceRule(prv.CompanyPublicId, md, null);

            //                                    //oCalificaitonProjectUpsert.CalificationProjectItemBatchModel.Add(oBalanceModule);

            //                                    break;

            //                                #endregion
            //                            }

            //                            return true;
            //                        });

            //                        //get total score
            //                        oCalificationProjectUpsert.CalificationProjectItemBatchModel.Where(sit => sit.Enable == true).All(sit =>
            //                        {
            //                            oCalificationProjectUpsert.TotalScore += sit.ItemScore;

            //                            return true;
            //                        });

            //                        //Upsert
            //                        oCalificationProjectUpsert = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oCalificationProjectUpsert);
            //                    }

            //                    return true;
            //                });
            //            }
            //            else
            //            {
            //                //return message don´t have related provider
            //            }

            //            return true;
            //        });
            //    }
            //    else
            //    {
            //        //return message no exist calification project config
            //    }
            //}
            //catch (Exception)
            //{
            //    //ProveedoresOnLine.CalificationBatch.CalificationProcess.LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            //}
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
