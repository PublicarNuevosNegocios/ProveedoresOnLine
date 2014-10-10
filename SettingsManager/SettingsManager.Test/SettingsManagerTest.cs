using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SettingsManager.Test
{
    [TestClass]
    public class SettingsManagerTest
    {
        [TestMethod]
        public void ReadStaticSettings()
        {
            Assert.AreEqual(SettingsManager.SettingsController.SettingsInstance.ModulesParams.Count, 2);
            Assert.AreEqual(SettingsManager.SettingsController.SettingsInstance.ModulesParams["Module1"]["Parametro7"].Value, "valor 7");
            Assert.AreEqual(SettingsManager.SettingsController.SettingsInstance.ModulesParams["Module1"].Count, 7);
            Assert.AreEqual(SettingsManager.SettingsController.SettingsInstance.ModulesParams["Module2"]["Parametro3"].Value, "<html> <div>este es un html 3</div></html>");
            Assert.AreEqual(SettingsManager.SettingsController.SettingsInstance.ModulesParams["Module2"].Count, 3);
        }


        [TestMethod]
        public void ReadStandarSettings()
        {
            SettingsManager.SettingsController sc = new SettingsManager.SettingsController();
            Assert.AreEqual(sc.ModulesParams.Count, 2);
            Assert.AreEqual(sc.ModulesParams["Module1"]["Parametro7"].Value, "valor 7");
            Assert.AreEqual(sc.ModulesParams["Module1"].Count, 7);
            Assert.AreEqual(sc.ModulesParams["Module2"]["Parametro3"].Value, "<html> <div>este es un html 3</div></html>");
            Assert.AreEqual(sc.ModulesParams["Module2"].Count, 3);
        }

        [TestMethod]
        public void ReadPersonalizedSettings()
        {
            SettingsManager.SettingsController sc = new SettingsManager.SettingsController(@"D:\Proyectos\GitHub\SaludGuru\SettingsManager\SettingsManager\RsxStructure\Rsx-SettingsConfig.xml");
            Assert.AreEqual(sc.ModulesParams.Count, 2);
            Assert.AreEqual(sc.ModulesParams["Module1"]["Parametro7"].Value, "valor 7");
            Assert.AreEqual(sc.ModulesParams["Module1"].Count, 7);
            Assert.AreEqual(sc.ModulesParams["Module2"]["Parametro3"].Value, "<html> <div>este es un html 3</div></html>");
            Assert.AreEqual(sc.ModulesParams["Module2"].Count, 3);
        }

        [TestMethod]
        public void ReadOneModuleSettings()
        {
            SettingsManager.SettingsController sc = new SettingsManager.SettingsController(@"D:\Proyectos\GitHub\SaludGuru\SettingsManager\SettingsManager\RsxStructure\Rsx-SettingsConfig.xml", "Module1");
            Assert.AreEqual(sc.ModulesParams.Count, 1);
            Assert.AreEqual(sc.ModulesParams["Module1"]["Parametro7"].Value, "valor 7");
            Assert.AreEqual(sc.ModulesParams["Module1"].Count, 7);
        }

        [TestMethod]
        public void ReadOneModuleSettingsDefaultLoc()
        {
            SettingsManager.SettingsController sc = new SettingsManager.SettingsController(null, "Module1");
            Assert.AreEqual(sc.ModulesParams.Count, 1);
            Assert.AreEqual(sc.ModulesParams["Module1"]["Parametro7"].Value, "valor 7");
            Assert.AreEqual(sc.ModulesParams["Module1"].Count, 7);
        }
    }
}
