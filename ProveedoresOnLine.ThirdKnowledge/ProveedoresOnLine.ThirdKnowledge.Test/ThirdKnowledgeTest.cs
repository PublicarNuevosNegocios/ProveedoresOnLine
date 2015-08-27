using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProveedoresOnLine.ThirdKnowledge.Models;

namespace ProveedoresOnLine.ThirdKnowledge.Test
{
    [TestClass]
    public class ThirdKnowledgeTest
    {
        [TestMethod]
        public void SimpleRequest()
        {
            ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.SimpleRequest("70041053", "Alvaro uribe Velez");
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
            oToUpsert.Status = new CatalogModel()
                {
                    ItemId = 101001
                };
            PlanModel oReturn = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.PlanUpsert(oToUpsert);

            Assert.IsNotNull(oReturn);
        }
    }
}
