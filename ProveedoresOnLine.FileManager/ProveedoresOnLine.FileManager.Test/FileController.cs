using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ProveedoresOnLine.FileManager.Test
{
    [TestClass]
    public class FileController
    {
        [TestMethod]
        public void LoadFile()
        {
            string FilePath = @"D:\Proyectos\Github\ProveedoresOnLine\ProveedoresOnLine.FileManager\ProveedoresOnLine.FileManager.Test\Desert.jpg";

            string oReturn = ProveedoresOnLine.FileManager.FileController.LoadFile(FilePath, "\\tmp\\");

            Assert.AreEqual(true, !string.IsNullOrEmpty(oReturn));
        }
    }
}
