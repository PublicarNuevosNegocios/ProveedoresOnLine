using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using ProveedoresOnLine.RestrictiveListProcess.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcess.Test
{
    [TestClass]
    public class RestrictiveListProcessTest
    {
        [TestMethod]
        public void GetProviderByStatus()
        {
            List<ProviderModel> objProviderModel = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetProviderByStatus(902005, "DA5C572E");
            Assert.IsNotNull(objProviderModel);
        }

        [TestMethod]
        public void GetAllProvidersInProcess()
        {
            List<RestrictiveListProcessModel> objRestictiiveListInProcess = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetAllProvidersInProcess();
            Assert.IsNotNull(objRestictiiveListInProcess);
        }

        [TestMethod]
        public void BlackListProcessUpsert()
        {
            RestrictiveListProcessModel oBlackListProcessModel = new RestrictiveListProcessModel()
            { 
                BlackListProcessId = 5,
                CreateDate = DateTime.Now,
                Enable = true,
                FilePath ="C:\\ProveedoresOnline\\Test\\",
                IsSuccess = true,
                LastModify = DateTime.Now,
                ProcessStatus = true,
                ProviderStatus = Convert.ToString((int)ProveedoresOnLine.RestrictiveListProcess.Models.enumProviderStatus.CreacionNacional),
            };

            int strUpsertStatus = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.BlackListProcessUpsert(oBlackListProcessModel);
            Assert.IsNotNull(strUpsertStatus);
        }

    }
}
