using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace DocumentManagement.Customer.Test
{
    [TestClass]
    public class CustomerTest
    {
        [TestMethod]
        public void CustomerSearch()
        {
            int oTotalRows;
            List<DocumentManagement.Customer.Models.Customer.CustomerModel> oResult =
                DocumentManagement.Customer.Controller.Customer.CustomerSearch
                    (null, 0, 20, out oTotalRows);

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void CustomerGetByFormId()
        {
            DocumentManagement.Customer.Models.Customer.CustomerModel oResult =
                DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId("aaa");

            Assert.IsNotNull(oResult);
        }

        [TestMethod]
        public void CustomerGetById()
        {
            DocumentManagement.Customer.Models.Customer.CustomerModel oResult =
                DocumentManagement.Customer.Controller.Customer.CustomerGetById("1D4F2724");

            Assert.IsNotNull(oResult);
        }

        [TestMethod]
        public void FormSearch()
        {
            int oTotalRows;
            List<DocumentManagement.Customer.Models.Form.FormModel> oResult =
                DocumentManagement.Customer.Controller.Customer.FormSearch
                    ("aaaa", null, 0, 20, out oTotalRows);

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void StepGetByFormId()
        {
            List<DocumentManagement.Customer.Models.Form.StepModel> oResult =
                DocumentManagement.Customer.Controller.Customer.StepGetByFormId("aaaa");

            Assert.AreEqual(true, oResult.Count > 0);
        }

        [TestMethod]
        public void FieldGetByStepId()
        {
            List<DocumentManagement.Customer.Models.Form.FieldModel> oResult =
                DocumentManagement.Customer.Controller.Customer.FieldGetByStepId(0);

            Assert.AreEqual(true, oResult.Count > 0);
        }


        [TestMethod]
        public void CatalogGetCustomerOptions()
        {

            List<DocumentManagement.Customer.Models.Util.CatalogModel> oResult =
                DocumentManagement.Customer.Controller.Customer.CatalogGetCustomerOptions();

            Assert.AreEqual(true, oResult.Count > 0);
        }

    }
}
