using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProveedoresOnLine.CalificationProject.Models.CalificationProject;
using System.Collections.Generic;

namespace ProveedoresOnLine.CalificationProject.Test
{
    [TestClass]
    public class CalificationProjectTest
    {
        #region ProjectConfig

        [TestMethod]
        public void CalificationProjectConfigGetByCompanyId()
        {
            List<CalificationProjectConfigModel> oReturn = new List<CalificationProjectConfigModel>();
            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigGetByCompanyId
                ("1B40C887",
                true);

            Assert.AreEqual(true, oReturn.Count > 0);
        }
        [TestMethod]
        public void CalificaciontProjectConfigUpsert() 
        {
            CalificationProjectConfigModel oReturn = new CalificationProjectConfigModel()
            {
                CalificationProjectConfigId =0,
                CalificationProjectConfigName= "Proceso de Calificación Prueba 1",
                Company = new Company.Models.Company.CompanyModel()
                {
                    CompanyPublicId = "1EA5A78A"
                },
                Enable = true
             
            };
            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigUpsert(oReturn);
            Assert.AreEqual(true, oReturn.CalificationProjectConfigId > 0);
        }
        [TestMethod]
        public void CalificationProjectConfigGetAll()
        {
            List<Models.CalificationProject.CalificationProjectConfigModel> oReturn = new List<CalificationProjectConfigModel>();
            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetAll();

            Assert.AreEqual(true, oReturn.Count > 0);
        }
        
        [TestMethod]
        public void CalificationProjectConfig_GetByCalificationProjectConfigId()
        {
            Models.CalificationProject.CalificationProjectConfigModel oReturn = new Models.CalificationProject.CalificationProjectConfigModel();

            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfig_GetByCalificationProjectConfigId(1);

            Assert.AreEqual(true, oReturn != null);
        }

        #endregion

        #region ConfigItem

        [TestMethod]
        public void CalificationProjectConfigItemUpsert()
        {
            CalificationProjectConfigModel oReturn = new CalificationProjectConfigModel()
            {
                CalificationProjectConfigId = 1,
                ConfigItemModel = new List<ConfigItemModel>(){
                    new ConfigItemModel(){
                        CalificationProjectConfigItemId = 5,
                CalificationProjectConfigItemName = null,
                CalificationProjectConfigItemType = new Company.Models.Util.CatalogModel()
                {
                            ItemId = 2003002, //Módulo Financiera
                },
                Enable = true,
                    },
                },
            };

            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigItemUpsert
                (oReturn);

            Assert.AreEqual(true, oReturn.CalificationProjectConfigId > 0 && oReturn.ConfigItemModel != null && oReturn.ConfigItemModel.Count > 0);
        }

        [TestMethod]
        public void CalificationProjectConfigItem_GetByCalificationProjectConfigId()
        {
            List<ConfigItemModel> oReturn = new List<ConfigItemModel>();

            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigItem_GetByCalificationProjectConfigId
                (1,
                true);

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        #endregion

        #region ConfigItemInfo

        [TestMethod]
        public void CalificationProjectConfigItemInfoUpsert()
        {
            ConfigItemModel oReturn = new ConfigItemModel()
            {
                CalificationProjectConfigItemId = 1,
                CalificationProjectConfigItemInfoModel = new List<ConfigItemInfoModel>(){
                    new ConfigItemInfoModel(){
                        CalificationProjectConfigItemInfoId = 1,
                        Question = 609001,
                        Rule = new Company.Models.Util.CatalogModel(){
                            ItemId = 2001003, //Menor que (<)
                        },
                        ValueType = new Company.Models.Util.CatalogModel(){
                            ItemId = 2002002, //Numérico
                        },
                        Value = "5",
                        Score = "5",
                        Enable = true,
                    },
                },
            };

            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigItemInfoUpsert(oReturn);

            Assert.AreEqual(true, oReturn.CalificationProjectConfigItemInfoModel.Count > 0);
        }

        [TestMethod]
        public void CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId()
        {
            List<ConfigItemInfoModel> oReturn = new List<ConfigItemInfoModel>();

            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigItemInfo_GetByCalificationProjectConfigItemId
                (1,
                true);

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        #endregion

        #region ConfigValidate
        [TestMethod]
        public void CalificationProjectConfigValidate_GetByprojectConfigId()
        {
            List<ConfigValidateModel> oReturn = new List<ConfigValidateModel>();
            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectValidate_GetByProjectConfigId(1, true);
            Assert.AreEqual(true, oReturn.Count > 0);
        }

        [TestMethod]
        public void CalificationProjectConfigValidate_Upsert()
        {
            ConfigValidateModel oReturn = new ConfigValidateModel()
            {
                CalificationProjectConfigValidateId = 0,
                CalificationProjectConfigId = 3,
                Operator = new Company.Models.Util.CatalogModel()
                {
                    ItemId = 2001001
                },
                Value = "10",
                Result = "5",
                Enable = true

            };
            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigValidate_Upsert(oReturn);
            Assert.AreEqual(true, oReturn.CalificationProjectConfigId > 0 && oReturn.CalificationProjectConfigValidateId > 0 );
        }
        #endregion

        #region CalificationProjectConfigOptions

        [TestMethod]
        public void CalificationProjectConfigOptions()
        {
            List<ProveedoresOnLine.Company.Models.Util.CatalogModel> oReturn = new List<Company.Models.Util.CatalogModel>();

            oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigOptions();

            Assert.AreEqual(true, oReturn.Count > 0);
        }

        [TestMethod]
        public void CalificationProjectConfigCategoryOptions()
        {
            List<Models.CalificationProject.CalificationProjectCategoryModel> oReturn = ProveedoresOnLine.CalificationProject.Controller.CalificationProject.CalificationProjectConfigCategoryOptions();

            Assert.AreEqual(true, oReturn != null && oReturn.Count > 0);
        }
        #endregion
    }
}
