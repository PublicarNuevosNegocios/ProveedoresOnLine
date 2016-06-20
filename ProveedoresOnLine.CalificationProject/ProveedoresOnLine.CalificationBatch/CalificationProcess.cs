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
                //obtener todas las configuraciones de los procesos de calificación
                List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> oCalificationProjectConfigModel =
                    ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetAll();

                if (oCalificationProjectConfigModel != null &&
                    oCalificationProjectConfigModel.Count > 0)
                {
                    oCalificationProjectConfigModel.All(cpc =>
                    {
                        //Obtener los proveedores del comprador.
                        List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oRelatedProvider =
                            ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId(cpc.Company.CompanyPublicId);

                        if (oRelatedProvider != null &&
                            oRelatedProvider.Count > 0)
                        {
                            oRelatedProvider.All(prv =>
                            {
                                //Obtener proceso de calificación por proveedor.
                                List<Models.CalificationProjectBatch.CalificationProjectBatchModel> oRelatedCalificationProject =
                                   ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetProviderByCustomer(cpc.Company.CompanyPublicId, prv.CompanyPublicId);

                                if (oRelatedCalificationProject != null &&
                                    oRelatedCalificationProject.Count > 0)
                                {
                                    oRelatedCalificationProject.All(cp =>
                                    {
                                        //obtener configuración del proceso
                                        cp.ProjectConfigModel = oCalificationProjectConfigModel.Where(cnf => cnf.CalificationProjectConfigId == cp.ProjectConfigModel.CalificationProjectConfigId).Select(cnf => cnf).FirstOrDefault();

                                        //Validar configuración
                                        if (cp.ProjectConfigModel.Enable)
                                        {
                                            cp.CalificationProjectItemBatchModel.All(cpi =>
                                            {
                                                //obtener config del item
                                                cpi.CalificationProjectConfigItem = cp.ProjectConfigModel.ConfigItemModel.Where(ci => ci.CalificationProjectConfigItemId == cpi.CalificationProjectConfigItem.CalificationProjectConfigItemId).Select(ci => ci).FirstOrDefault();

                                                //Validar config
                                                if (cpi.CalificationProjectConfigItem.Enable)
                                                {
                                                    
                                                }
                                                else
                                                {
                                                    //Deshabilitar proceso
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
                                            //deshabilitar proceso relacionado.
                                            cp.Enable = false;

                                            cp.CalificationProjectItemBatchModel.All(ci =>
                                            {
                                                ci.Enable = false;

                                                ci.CalificatioProjectItemInfoModel.All(cinf =>
                                                {
                                                    cinf.Enable = false;
                                                     
                                                    return true;
                                                });

                                                return true;
                                            });
                                        }

                                        //upsert.
                                        return true;
                                    });
                                }
                                else
                                {
                                    //Proveedor no tiene procesos de calificación.

                                    //ejecutar nuevo proceso.
                                    ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel oCalificationProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                                    {
                                        CalificationProjectId = 0,
                                        CalificationProjectPublicId = "",
                                        ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                                        {
                                            CalificationProjectConfigId = cpc.CalificationProjectConfigId,
                                        },
                                        RelatedProvider = new Company.Models.Company.CompanyModel()
                                        {
                                            CompanyPublicId = prv.CompanyPublicId,
                                        },
                                        CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>(),
                                        Enable = true,
                                    };

                                    //Modulos
                                    cpc.ConfigItemModel.Where(md => md.Enable == true).All(md =>
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

                                    //obtener el total
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
                            //no hay proveedores relacionados.
                        }

                        return true;
                    });
                }
                else
                {
                    //no hay procesos de calificación
                }
            }
            catch (Exception)
            {

                throw;
            }




















            try
            {
                //Get all calification project config
                List<ProveedoresOnLine.CalificationProject.Models.CalificationProject.CalificationProjectConfigModel> oCalificationProjectConfigModel =
                    ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetAll();

                if (oCalificationProjectConfigModel != null &&
                    oCalificationProjectConfigModel.Count > 0)
                {
                    oCalificationProjectConfigModel.All(cpc =>
                    {
                        //Get all related provider by customer
                        List<ProveedoresOnLine.Company.Models.Company.CompanyModel> oRelatedProvider =
                            ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId(cpc.Company.CompanyPublicId);

                        if (oRelatedProvider != null &&
                            oRelatedProvider.Count > 0)
                        {
                            oRelatedProvider.All(prv =>
                            {
                                //Create model to upsert
                                ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel oCalificationProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel();

                                //Get calification process by provider
                                List<Models.CalificationProjectBatch.CalificationProjectBatchModel> oRelatedCalificationProject =
                                   ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetProviderByCustomer(cpc.Company.CompanyPublicId, prv.CompanyPublicId);

                                if (oRelatedCalificationProject != null &&
                                    oRelatedCalificationProject.Count > 0)
                                {
                                    //already exists calification project                                   
                                    oRelatedCalificationProject.All(cp =>
                                    {
                                        //Get related calification project config
                                        cp.ProjectConfigModel = oCalificationProjectConfigModel.Where(x => x.CalificationProjectConfigId == cp.ProjectConfigModel.CalificationProjectConfigId).Select(x => x).FirstOrDefault();

                                        //validate if config is enable
                                        if (cp.ProjectConfigModel.Enable)
                                        {
                                            cp.CalificationProjectItemBatchModel.All(cpib =>
                                            {
                                                bool itemEnable = false;

                                                cp.ProjectConfigModel.ConfigItemModel.Where(itcnf => itcnf.Enable == true).All(itcnf =>
                                                {
                                                    if (!itemEnable && cpib.CalificationProjectConfigItem.CalificationProjectConfigItemId == itcnf.CalificationProjectConfigItemId)
                                                    {
                                                        itemEnable = true;
                                                    }

                                                    return true;
                                                });

                                                cpib.Enable = itemEnable;

                                                return true;
                                            });

                                            cp.ProjectConfigModel.ConfigItemModel.Where(ci => ci.Enable == true).All(ci =>
                                            {
                                                if (cp.CalificationProjectItemBatchModel.Any(x => x.CalificationProjectConfigItem.CalificationProjectConfigItemId == ci.CalificationProjectConfigItemId))
                                                {
                                                    cp.CalificationProjectItemBatchModel.Where(cpi => cpi.CalificationProjectConfigItem.CalificationProjectConfigItemId == ci.CalificationProjectConfigItemId).All(cpi =>
                                                    {
                                                        //set calification project item config
                                                        cpi.CalificationProjectConfigItem = cpc.ConfigItemModel.Where(cpci => cpi.CalificationProjectConfigItem.CalificationProjectConfigItemId == cpci.CalificationProjectConfigItemId).Select(cpci => cpci).FirstOrDefault();

                                                        //update validation module
                                                        switch (cpi.CalificationProjectConfigItem.CalificationProjectConfigItemType.ItemId)
                                                        {
                                                            #region LegalModule

                                                            case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                                                cpi = ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, cpi.CalificationProjectConfigItem, cpi);

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
                                                    //create new data
                                                    switch (ci.CalificationProjectConfigItemType.ItemId)
                                                    {
                                                        #region LegalModule

                                                        case (int)ProveedoresOnLine.CalificationBatch.Models.Enumerations.enumModuleType.CP_LegalModule:

                                                            cp.CalificationProjectItemBatchModel.Add(ProveedoresOnLine.CalificationBatch.CalificationProjectModule.LegalModule.LegalRule(prv.CompanyPublicId, ci, null));

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
                                                }

                                                return true;
                                            });

                                            //get total score
                                            cp.CalificationProjectItemBatchModel.Where(sit => sit.Enable == true).All(sit =>
                                            {
                                                cp.TotalScore += sit.ItemScore;

                                                return true;
                                            });

                                            //Upsert
                                            cp = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(cp);
                                        }
                                        else
                                        {
                                            //disable calification project 
                                            cp.Enable = false;

                                            //new calification project
                                            oCalificationProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                                            {
                                                CalificationProjectId = 0,
                                                CalificationProjectPublicId = "",
                                                ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                                                {
                                                    CalificationProjectConfigId = cpc.CalificationProjectConfigId,
                                                },
                                                RelatedProvider = new Company.Models.Company.CompanyModel()
                                                {
                                                    CompanyPublicId = prv.CompanyPublicId,
                                                },
                                                CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>(),
                                                Enable = true,
                                            };

                                            //execute all calification process
                                            cpc.ConfigItemModel.Where(md => md.Enable == true).All(md =>
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
                                    //new calification project
                                    oCalificationProjectUpsert = new Models.CalificationProjectBatch.CalificationProjectBatchModel()
                                    {
                                        CalificationProjectId = 0,
                                        CalificationProjectPublicId = "",
                                        ProjectConfigModel = new CalificationProject.Models.CalificationProject.CalificationProjectConfigModel()
                                        {
                                            CalificationProjectConfigId = cpc.CalificationProjectConfigId,
                                        },
                                        RelatedProvider = new Company.Models.Company.CompanyModel()
                                        {
                                            CompanyPublicId = prv.CompanyPublicId,
                                        },
                                        CalificationProjectItemBatchModel = new List<Models.CalificationProjectBatch.CalificationProjectItemBatchModel>(),
                                        Enable = true,
                                    };

                                    //execute all calification process
                                    cpc.ConfigItemModel.Where(md => md.Enable == true).All(md =>
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
                            //return message don´t have related provider
                        }

                        return true;
                    });
                }
                else
                {
                    //return message no exist calification project config
                }
            }
            catch (Exception)
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
