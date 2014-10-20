using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FileRepository.Manager;
using FileRepository.Manager.Models;

namespace FileRepository.Test
{
    [TestClass]
    public class S3Test
    {
        [TestMethod]
        public void StartUploadS3()
        {
            FileRepository.Manager.FileRepository oFileRepositoryInstance = (new FileRepositoryFactory()).GetFileRepository("S3_SaludGuru");
            oFileRepositoryInstance.OperationError += new FileRepository.Manager.FileRepository.DOperationError(OperationError);
            oFileRepositoryInstance.OperationFinish += new FileRepository.Manager.FileRepository.DOperationFinish(OperationFinish);

            oFileRepositoryInstance.CurrentOperations = new System.Collections.Generic.List<FileModel>();

            oFileRepositoryInstance.CurrentOperations.Add(
                new FileModel()
                {
                    FilePathLocalSystem = @"D:\Proyectos\Github\SaludGuru\FileRepository\FileRepository.Test\imgTest.JPG",
                    FilePathRemoteSystem = "NuevaCarpeta/imagen1.jpg",
                    Operation = enumOperation.UploadFile
                });

            oFileRepositoryInstance.StartOperation(false);

            Assert.AreEqual(true, oFileRepositoryInstance.CurrentOperations[0].ActionResult == enumActionResult.Success);

        }

        [TestMethod]
        public void StartDeleteS3()
        {
            FileRepository.Manager.FileRepository oFileRepositoryInstance = (new FileRepositoryFactory()).GetFileRepository("S3_SaludGuru");
            oFileRepositoryInstance.OperationError += new FileRepository.Manager.FileRepository.DOperationError(OperationError);
            oFileRepositoryInstance.OperationFinish += new FileRepository.Manager.FileRepository.DOperationFinish(OperationFinish);

            oFileRepositoryInstance.CurrentOperations = new System.Collections.Generic.List<FileModel>();

            oFileRepositoryInstance.CurrentOperations.Add(
                new FileModel()
                {
                    FilePathRemoteSystem = "NuevaCarpeta/imagen1.jpg",
                    Operation = enumOperation.DeleteFile
                });

            oFileRepositoryInstance.StartOperation(false);

            Assert.AreEqual(true, oFileRepositoryInstance.CurrentOperations[0].ActionResult == enumActionResult.Success);
        }

        private void OperationFinish(Manager.Models.FileModel FileDescription)
        {
            Assert.AreEqual(100, FileDescription.ProgressProcess);
        }

        private void OperationError(Manager.Models.FileModel FileDescription, Exception Error)
        {
            Assert.AreEqual(0, 1);
        }
    }
}
