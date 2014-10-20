using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DocumentManagement.Provider_Test
{
    [TestClass]
    public class ProviderTest
    {
        [TestMethod]
        public void ProviderUpsert()
        {
            string result = DocumentManagement.Provider.Controller.Provider.ProviderUpsert("1D4F2724", "", "SebastianProvider", DocumentManagement.Provider.Models.Enumerations.enumIdentificationType.Nit, "1030544724", "sebastianmartinez18@yahoo.com.co", DocumentManagement.Provider.Models.Enumerations.enumProcessStatus.New);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void LoadFile()
        {
            string FilePath = @"D:\Proyectos\Github\ProveedoresOnLine\DocumentManagement.Provider\DocumentManagement.Provider_Test\Jellyfish.jpg";

            string oReturn = DocumentManagement.Provider.Controller.Provider.LoadFile(FilePath, "\\tmp\\");

            Assert.AreEqual(true, !string.IsNullOrEmpty(oReturn));
        }
    }
}
