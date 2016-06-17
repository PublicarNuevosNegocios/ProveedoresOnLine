using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch;

namespace ProveedoresOnLine.CalificationProject.Test
{
    /// <summary>
    /// Summary description for CalificationProjectBatchTest
    /// </summary>
    [TestClass]
    public class CalificationProjectBatchTest
    {
        [TestMethod]
        public void StartProcess()
        {
            ProveedoresOnLine.CalificationBatch.CalificationProcess.StartProcess();
        }

        #region CalificationBatch

        [TestMethod]
        public void CalificationProject_GetByCustomer()
        {
            List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel> oReturn = new List<ProveedoresOnLine.CalificationBatch.Models.CalificationProjectBatch.CalificationProjectBatchModel>();
            oReturn = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProject_GetByCustomer("1B40C887", "4CD75091", true);

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        [TestMethod]
        public void CalificationProject_Upsert()
        {
            CalificationProjectBatchModel oReturn = new CalificationProjectBatchModel()
            {
                CalificationProjectId = 0,
                CalificationProjectPublicId = "",
                ProjectConfigModel = new Models.CalificationProject.CalificationProjectConfigModel()
                {
                    CalificationProjectConfigId = 7,
                },
                RelatedProvider = new Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = "18C25804",
                },
                CalificationProjectItemBatchModel = new List<CalificationProjectItemBatchModel>()
                {
                    new CalificationProjectItemBatchModel()
                    {
                        CalificationProjectItemId = 0,
                        CalificationProjectConfigItem = new Models.CalificationProject.ConfigItemModel()
                        {
                            CalificationProjectConfigItemId = 21,
                        },
                        CalificatioProjectItemInfoModel = new List<CalificationProjectItemInfoBatchModel>()
                        {
                            new CalificationProjectItemInfoBatchModel()
                            {
                                CalificationProjectItemInfoId = 0,
                                CalificationProjectConfigItemInfoModel = new Models.CalificationProject.ConfigItemInfoModel()
                                {
                                    CalificationProjectConfigItemInfoId = 11,
                                },
                                ItemInfoScore = 2,
                                Enable = true,
                            },
                            new CalificationProjectItemInfoBatchModel()
                            {
                                CalificationProjectItemInfoId = 0,
                                CalificationProjectConfigItemInfoModel = new Models.CalificationProject.ConfigItemInfoModel()
                                {
                                    CalificationProjectConfigItemInfoId = 12,
                                },
                                ItemInfoScore = 5,
                                Enable = true,
                            },
                        },
                        ItemScore = 10,
                        Enable = true,
                    },
                },
                TotalScore = 10,
                Enable = true,
            };

            oReturn = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectUpsert(oReturn);

            Assert.AreEqual(true, oReturn != null && oReturn.CalificationProjectItemBatchModel.Count > 0);
        }

        [TestMethod]
        public void CalificationProjectItem_Upsert()
        {
            CalificationProjectBatchModel oReturn = new CalificationProjectBatchModel()
            {
                CalificationProjectId = 0,
                CalificationProjectPublicId = "",
                ProjectConfigModel = new Models.CalificationProject.CalificationProjectConfigModel()
                {
                    CalificationProjectConfigId = 7,
                },
                RelatedProvider = new Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = "18C25804",
                },
                CalificationProjectItemBatchModel = new List<CalificationProjectItemBatchModel>()
                {
                    new CalificationProjectItemBatchModel()
                    {
                        CalificationProjectItemId = 0,
                        CalificationProjectConfigItem = new Models.CalificationProject.ConfigItemModel()
                        {
                            CalificationProjectConfigItemId = 21,
                        },
                        Enable = true,
                    },
                },
                Enable = true,
            };

            oReturn = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificatioProjectItemUpsert(oReturn);

            Assert.AreEqual(true, oReturn != null);
        }

        [TestMethod]
        public void CalificationProjectItemInfo_Upsert()
        {
            CalificationProjectItemBatchModel oReturn = new CalificationProjectItemBatchModel()
            {
                CalificationProjectItemId = 0,
                CalificationProjectConfigItem = new Models.CalificationProject.ConfigItemModel()
                {
                    CalificationProjectConfigItemId = 21,
                },
                CalificatioProjectItemInfoModel = new List<CalificationProjectItemInfoBatchModel>()
                {
                    new CalificationProjectItemInfoBatchModel()
                    {
                        CalificationProjectItemInfoId = 0,
                        CalificationProjectConfigItemInfoModel = new Models.CalificationProject.ConfigItemInfoModel()
                        {
                            CalificationProjectConfigItemInfoId = 11,
                        },
                        ItemInfoScore = 2,
                        Enable = true,
                    },
                    new CalificationProjectItemInfoBatchModel()
                    {
                        CalificationProjectItemInfoId = 0,
                        CalificationProjectConfigItemInfoModel = new Models.CalificationProject.ConfigItemInfoModel()
                        {
                            CalificationProjectConfigItemInfoId = 12,
                        },
                        ItemInfoScore = 5,
                        Enable = true,
                    },
                },
                ItemScore = 10,
                Enable = true,
            };

            oReturn = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CalificationProjectItemInfoUpsert(oReturn);

            Assert.AreEqual(true, oReturn != null && oReturn.CalificatioProjectItemInfoModel.Count > 0);
        }

        #endregion

        #region CalificationProjectBatchUtil

        #region LegalModule

        [TestMethod]
        public void LegalModuleInfo()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.LegalModuleInfo("1351D3F3", 603001);

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        #endregion

        #region Financial Module

        [TestMethod]
        public void FinancialModuleInfo()
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn =
                ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.FinancialModuleInfo("1BD9AD1B", 502002);
        }

        #endregion

        #region Balance Module

        [TestMethod]
        public void BalanceModule()
        {
            List<ProveedoresOnLine.CompanyProvider.Models.Provider.BalanceSheetModel> oReturn =
                ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.BalanceModuleInfo("158C1786", 3115);

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        #endregion

        #region Commercial Module

        [TestMethod]
        public void CommercialModule() 
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CommercialModuleInfo("1351D3F3", 302001);

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        #endregion

        #region HSEQ Module

        public void CertificationModule() 
        {
            List<ProveedoresOnLine.Company.Models.Util.GenericItemModel> oReturn = ProveedoresOnLine.CalificationBatch.Controller.CalificationProjectBatch.CertificationModuleInfo("1351D3F3",704001);
                
            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }

        #endregion

        #endregion
    }
}