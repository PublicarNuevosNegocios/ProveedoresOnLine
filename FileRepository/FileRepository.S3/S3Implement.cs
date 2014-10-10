using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using FileRepository.Manager.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FileRepository.S3
{
    public class S3Implement : FileRepository.Manager.FileRepository
    {
        #region PROPERTIES
        private Dictionary<string, string> oParameters;
        private Dictionary<string, string> Parameters
        {
            get
            {
                if (oParameters == null)
                {
                    oParameters = base.GetFileParams();
                }
                return oParameters;
            }
        }
        #endregion

        #region Implemented methods

        public override void StartOperation(bool IsAsync)
        {
            base.CurrentOperations.All(oFileToWork =>
            {
                //set startup action
                oFileToWork.ActionResult = enumActionResult.NotStart;
                //set file upload start up time
                oFileToWork.StartDate = DateTime.Now;

                //get s3 file model
                S3FileModel S3File = new S3FileModel()
                {
                    RelatedFile = oFileToWork,
                };

                try
                {
                    switch (oFileToWork.Operation)
                    {
                        case enumOperation.UploadFile:
                            UploadBegin(ref S3File, IsAsync);
                            break;
                        case enumOperation.DeleteFile:
                            DeleteBegin(ref S3File, IsAsync);
                            break;
                    }
                }
                catch (Exception e)
                {
                    //set end action
                    oFileToWork.ActionResult = enumActionResult.Error;
                    //set file upload start up time
                    oFileToWork.FinishDate = DateTime.Now;

                    this.OperationError(oFileToWork, e);
                }

                return true;
            });
        }

        #endregion

        #region Upload operation

        private void UploadBegin(ref S3FileModel FileDescription, bool IsAsync)
        {
            //get and validate upload file
            FileInfo FInfo = new FileInfo(FileDescription.RelatedFile.FilePathLocalSystem);
            if (!FInfo.Exists)
                throw new Exception("El archivo '" + FileDescription.RelatedFile.FilePathLocalSystem + "' no existe.");

            //get upload file size
            FileDescription.FileSize = FInfo.Length;

            //get destination route
            FileDescription.RelatedFile.UploadedFile = new Uri
                (Parameters["S3.Server"].Trim().TrimEnd('/') + "/" +
                 FileDescription.RelatedFile.FilePathRemoteSystem.Trim().TrimStart('\\').TrimStart('/').Replace("\\", "/"));

            //get publish route
            FileDescription.RelatedFile.PublishFile = new Uri
                (Parameters["S3.Publish"].Trim().TrimEnd('/') + "/" +
                 FileDescription.RelatedFile.FilePathRemoteSystem.Trim().TrimStart('\\').TrimStart('/').Replace("\\", "/"));

            if (IsAsync)
            {
                //startup async upload
                Thread Tarea = new Thread(new ParameterizedThreadStart(UploadFile));
                Tarea.Start(FileDescription);
            }
            else
            {
                UploadFile(FileDescription);
            }
        }

        public void UploadFile(object FileToProccess)
        {
            //get file to upload
            S3FileModel FileDescription = (S3FileModel)FileToProccess;

            try
            {
                //init S3 client
                using (IAmazonS3 AWSClient = AWSClientFactory.CreateAmazonS3Client
                                (Parameters["S3.AccessKeyId"].Trim(),
                                Parameters["S3.SecretAccessKey"].Trim(),
                                RegionEndpoint.GetBySystemName(Parameters["S3.RegionEndpoint"].Trim())))
                {

                    //define request object
                    PutObjectRequest S3Request = new PutObjectRequest();
                    S3Request.BucketName = Parameters["S3.BucketName"].Trim();
                    S3Request.Key = FileDescription.RelatedFile.FilePathRemoteSystem.Trim().TrimStart('\\').TrimStart('/').Replace("\\", "/");
                    S3Request.StorageClass = S3StorageClass.FindValue(Parameters["S3.StorageClass"].Trim());
                    S3Request.FilePath = FileDescription.RelatedFile.FilePathLocalSystem;

                    //upload S3 file
                    PutObjectResponse S3Response = AWSClient.PutObject(S3Request);

                    //status 100%
                    FileDescription.RelatedFile.ProgressProcess = 100;

                    //set end action
                    FileDescription.RelatedFile.ActionResult = enumActionResult.Success;
                    //set file upload start up time
                    FileDescription.RelatedFile.FinishDate = DateTime.Now;

                    //invoca operacion de finalizacion de la transferencia
                    this.OperationFinish(FileDescription.RelatedFile);

                }
            }
            catch (Exception e)
            {
                //set end action
                FileDescription.RelatedFile.ActionResult = enumActionResult.Error;
                //set file upload start up time
                FileDescription.RelatedFile.FinishDate = DateTime.Now;

                //notifica error
                this.OperationError(FileDescription.RelatedFile, e);
            }
        }

        #endregion

        #region Delete operation

        private void DeleteBegin(ref S3FileModel FileDescription, bool IsAsync)
        {
            //get loaded file
            FileDescription.RelatedFile.UploadedFile = new Uri
                (Parameters["S3.Server"].Trim().TrimEnd('/') + "/" +
                 FileDescription.RelatedFile.FilePathRemoteSystem.Trim().TrimStart('\\').TrimStart('/').Replace("\\", "/"));

            //get publish loaded file
            FileDescription.RelatedFile.PublishFile = new Uri
                (Parameters["S3.Publish"].Trim().TrimEnd('/') + "/" +
                 FileDescription.RelatedFile.FilePathRemoteSystem.Trim().TrimStart('\\').TrimStart('/').Replace("\\", "/"));

            if (IsAsync)
            {
                //start async delete
                Thread Tarea = new Thread(new ParameterizedThreadStart(DeleteFile));
                Tarea.Start(FileDescription);
            }
            else
            {
                DeleteFile(FileDescription);
            }
        }

        private void DeleteFile(object FileToProccess)
        {
            S3FileModel FileDescription = (S3FileModel)FileToProccess;
            try
            {
                using (IAmazonS3 AWSClient = AWSClientFactory.CreateAmazonS3Client
                                (Parameters["S3.AccessKeyId"].Trim(),
                                Parameters["S3.SecretAccessKey"].Trim(),
                                RegionEndpoint.GetBySystemName(Parameters["S3.RegionEndpoint"].Trim())))
                {
                    //define request delete object
                    DeleteObjectRequest S3Request = new DeleteObjectRequest();
                    S3Request.BucketName = Parameters["S3.BucketName"].Trim();
                    S3Request.Key = FileDescription.RelatedFile.FilePathRemoteSystem.Trim().TrimStart('\\').TrimStart('/').Replace("\\", "/");

                    //delete file
                    DeleteObjectResponse S3Response = AWSClient.DeleteObject(S3Request);

                    //status 100%
                    FileDescription.RelatedFile.ProgressProcess = 100;

                    //set end action
                    FileDescription.RelatedFile.ActionResult = enumActionResult.Success;
                    //set file upload start up time
                    FileDescription.RelatedFile.FinishDate = DateTime.Now;

                    //invoca operacion de finalizacion de la transferencia
                    this.OperationFinish(FileDescription.RelatedFile);
                }
            }
            catch (Exception e)
            {
                //set end action
                FileDescription.RelatedFile.ActionResult = enumActionResult.Error;
                //set file upload start up time
                FileDescription.RelatedFile.FinishDate = DateTime.Now;

                //notifica error
                this.OperationError(FileDescription.RelatedFile, e);
            }
        }

        #endregion
    }
}
