using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System.Collections.Generic;

namespace ProveedoresOnLine.ThirdKnowledge.Test
{
    [TestClass]
    public class ThirdKnowledgeTest
    {
        [TestMethod]
        public void SimpleRequest()
        {
            TDQueryModel oQuery = new TDQueryModel();
            ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.SimpleRequest("Customer","70041053", "Alvaro uribe Velez", oQuery );
        }

        [TestMethod]
        public void PlanUpsert()
        {
            PlanModel oToUpsert = new PlanModel();
            oToUpsert.InitDate = Convert.ToDateTime("2014/01/01");
            oToUpsert.EndDate = Convert.ToDateTime("2015/01/01");
            oToUpsert.CompanyPublicId = "AAAAA";
            oToUpsert.CreateDate = DateTime.Now;
            oToUpsert.DaysByPeriod = 30;
            oToUpsert.Enable = true;
            oToUpsert.LastModify = DateTime.Now;
            oToUpsert.QueriesByPeriod = 100;
            oToUpsert.Status = new TDCatalogModel()
                {
                    ItemId = 101001
                };
            PlanModel oReturn = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.PlanUpsert(oToUpsert);

            Assert.IsNotNull(oReturn);
        }

        [TestMethod]
        public void GetAllPlanByCustomer()
        {
            List<PlanModel> oReturn = new List<PlanModel>();
            oReturn = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetAllPlanByCustomer("DA5C572E", true);
            Assert.IsNull(oReturn);     
        }

        [TestMethod]
        public void GetPeriodByPlanPublicId()
        {
            List<PeriodModel> oPeriodModel = new List<PeriodModel>();

            oPeriodModel = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.GetPeriodByPlanPublicId("DA5C572E", true);

            Assert.IsNull(oPeriodModel);     
        }

        [TestMethod]
        public void ThirdKnoledgeSearch()
        {
            int TotalRows = 0;

            List<TDQueryModel> oReturn = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.ThirdKnoledgeSearch
                ("DA5C572E", 1, true, 0, 10000, out TotalRows);

            Assert.AreEqual(true, oReturn != null);
        }
    }
}
