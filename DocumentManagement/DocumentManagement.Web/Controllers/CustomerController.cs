using DocumentManagement.Customer.Models.Customer;
using DocumentManagement.Customer.Models.Form;
using DocumentManagement.Models.Customer;
using DocumentManagement.Models.Provider;
using DocumentManagement.Provider.Models.Provider;
using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class CustomerController : BaseController
    {
        public virtual ActionResult ListCustomer()
        {
            return View();
        }

        public virtual ActionResult UpsertCustomer(string CustomerPublicId)
        {
            UpserCustomerModel oModel = new UpserCustomerModel()
            {
                CustomerOptions = DocumentManagement.Customer.Controller.Customer.CatalogGetCustomerOptions(),
                RelatedCustomer = new CustomerModel() { CustomerPublicId = CustomerPublicId },
            };

            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                oModel.RelatedCustomer = GetCustomerRequest();

                oModel.RelatedCustomer.CustomerPublicId = DocumentManagement.Customer.Controller.Customer.CustomerUpsert(oModel.RelatedCustomer);

                return RedirectToAction(MVC.Customer.ActionNames.UpsertCustomer, MVC.Customer.Name, new { CustomerPublicId = oModel.RelatedCustomer.CustomerPublicId });
            }

            if (!string.IsNullOrEmpty(oModel.RelatedCustomer.CustomerPublicId))
                oModel.RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(oModel.RelatedCustomer.CustomerPublicId);

            return View(oModel);
        }

        public virtual ActionResult ListForm(string CustomerPublicId)
        {
            UpserCustomerModel oModel = new UpserCustomerModel()
            {
                RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId),
            };

            return View(oModel);
        }

        public virtual ActionResult UpsertForm(string CustomerPublicId, string FormPublicId)
        {
            UpserCustomerModel oModel = new UpserCustomerModel()
            {
                RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId),
                RelatedForm = new FormModel() { FormPublicId = FormPublicId },
            };

            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                oModel.RelatedForm = GetFormRequest();

                oModel.RelatedForm.FormPublicId = DocumentManagement.Customer.Controller.Customer.FormUpsert
                    (oModel.RelatedCustomer.CustomerPublicId,
                    oModel.RelatedForm);

                return RedirectToAction(MVC.Customer.ActionNames.UpsertForm, MVC.Customer.Name,
                    new
                    {
                        CustomerPublicId = oModel.RelatedCustomer.CustomerPublicId,
                        FormPublicId = oModel.RelatedForm.FormPublicId,
                    });
            }

            if (!string.IsNullOrEmpty(oModel.RelatedForm.FormPublicId))
            {
                oModel.RelatedForm = DocumentManagement.Customer.Controller.Customer.
                    CustomerGetByFormId(oModel.RelatedForm.FormPublicId).
                    RelatedForm.
                    Where(x => x.FormPublicId == FormPublicId).
                    FirstOrDefault();
            }

            return View(oModel);
        }

        public virtual ActionResult UpsertFormLogo(string CustomerPublicId, string FormPublicId, HttpPostedFileBase UploadFile)
        {
            //get folder
            string strFolder = Server.MapPath(DocumentManagement.Models.General.InternalSettings.Instance
                [DocumentManagement.Models.General.Constants.C_Settings_File_TempDirectory].Value);

            if (!System.IO.Directory.Exists(strFolder))
                System.IO.Directory.CreateDirectory(strFolder);

            //get File
            string strFile = strFolder.TrimEnd('\\') +
                "\\Logo_" +
                CustomerPublicId + "_" +
                DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpeg";

            UploadFile.SaveAs(strFile);

            //load file to s3
            string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                (strFile,
                DocumentManagement.Models.General.InternalSettings.Instance[DocumentManagement.Models.General.Constants.C_Settings_File_RemoteDirectory].Value);

            //update file into db
            DocumentManagement.Customer.Controller.Customer.FormUpsertLogo
                                (FormPublicId,
                                strRemoteFile);

            //remove temporal file
            if (System.IO.File.Exists(strFile))
                System.IO.File.Delete(strFile);

            return RedirectToAction(MVC.Customer.ActionNames.UpsertForm, MVC.Customer.Name, new
            {
                CustomerPublicId = CustomerPublicId,
                FormPublicId = FormPublicId
            });
        }

        public virtual ActionResult UploadProvider(string CustomerPublicId, HttpPostedFileBase ExcelFile)
        {
            if (ExcelFile != null)
            {
                string strFolder = Server.MapPath(DocumentManagement.Models.General.Constants.C_Settings_File_TempDirectory);

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //get File
                string strFile = strFolder.TrimEnd('\\') +
                    "\\ProviderUploadFile_" +
                    CustomerPublicId + "_" +
                    DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";
                string ErrorFilePath = strFile.Replace(".xls", "_log.csv");
                ExcelFile.SaveAs(strFile);

                //load file to s3
                string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                    (strFile,
                    DocumentManagement.Models.General.InternalSettings.Instance[DocumentManagement.Models.General.Constants.C_Settings_File_ExcelDirectory].Value);

                //update file into db          
                string logFile = this.ProccessProviderFile(strFile, ErrorFilePath, CustomerPublicId);

                //remove temporal file
                if (System.IO.File.Exists(strFile))
                    System.IO.File.Delete(strFile);

                //ViewData.Add(strRemoteFile);
                List<string> urlList = new List<string>();
                urlList.Add(strRemoteFile);
                urlList.Add(logFile);

                ViewData["UrlReturn"] = urlList;

                UpserCustomerModel oModel = new UpserCustomerModel()
                {
                    RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId),
                };
                return View(oModel);
                //return RedirectToAction(MVC.Customer.ActionNames.UploadProvider, MVC.Customer.Name, new
                //{
                //    CustomerPublicId = CustomerPublicId
                //});
            }
            else
            {
                UpserCustomerModel oModel = new UpserCustomerModel()
                {
                    RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId),
                };
                return View(oModel);
            }
        }
               
        #region private methods

        private CustomerModel GetCustomerRequest()
        {
            CustomerModel oReturn = new CustomerModel();

            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                oReturn.CustomerPublicId = Request["CustomerPublicId"];
                oReturn.Name = Request["Name"];
                oReturn.IdentificationType = new Customer.Models.Util.CatalogModel() { ItemId = Convert.ToInt32(Request["IdentificationType"]), };
                oReturn.IdentificationNumber = Request["IdentificationNumber"];
            }

            return oReturn;
        }

        private FormModel GetFormRequest()
        {
            FormModel oReturn = new FormModel();

            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"].Trim() == "true")
            {
                oReturn.FormPublicId = Request["FormPublicId"];
                oReturn.Name = Request["Name"];
            }

            return oReturn;
        }

        private string ProccessProviderFile(string FilePath, string ErrorFilePath, string CustomerPublicId)
        {
            //get excel rows
            LinqToExcel.ExcelQueryFactory XlsInfo = new LinqToExcel.ExcelQueryFactory(FilePath);

            List<ExcelProviderModel> oPrvToProcess =
                (from x in XlsInfo.Worksheet<ExcelProviderModel>(0)
                 select x).ToList();

            List<ExcelProviderResultModel> oPrvToProcessResult = new List<ExcelProviderResultModel>();

            //process Provider
            oPrvToProcess.Where(prv => !string.IsNullOrEmpty(prv.NumeroIdentificacion)).All(prv =>
            {
                try
                {
                    #region Operation
                    //Validar el provider
                    ProviderModel Provider = new ProviderModel();

                    ProviderModel oResultValidate = new ProviderModel();
                    oResultValidate = DocumentManagement.Provider.Controller.Provider.ProviderGetByIdentification(prv.NumeroIdentificacion, Convert.ToInt32(prv.TipoIdentificacion), CustomerPublicId);

                    //Create ProviderCustomerInfo
                    List<ProviderInfoModel> ListCustomerProviderInfo = new List<ProviderInfoModel>();
                    ProviderInfoModel CustomerProviderInfo = new ProviderInfoModel();
                    if (oResultValidate ==null)
                    {
                        CustomerProviderInfo.ProviderInfoType = new CatalogModel() { ItemId = 401 };
                        CustomerProviderInfo.Value = "201";
                        ListCustomerProviderInfo.Add(CustomerProviderInfo);
                    }                    

                    CustomerProviderInfo = new ProviderInfoModel();

                    CustomerProviderInfo.ProviderInfoType = new CatalogModel() { ItemId = 403 };
                    CustomerProviderInfo.Value = prv.CampanaSalesforce;

                    ListCustomerProviderInfo.Add(CustomerProviderInfo);

                    //Create Provider
                    ProviderModel ProviderToCreate = new ProviderModel()
                    {
                        CustomerPublicId = CustomerPublicId,
                        Name = prv.RazonSocial,
                        IdentificationType = new Provider.Models.Util.CatalogModel() { ItemId = Convert.ToInt32(prv.TipoIdentificacion) },
                        IdentificationNumber = prv.NumeroIdentificacion,
                        Email = prv.Email,
                        RelatedProviderCustomerInfo = ListCustomerProviderInfo
                    };
                    if (oResultValidate == null)
                        DocumentManagement.Provider.Controller.Provider.ProviderUpsert(ProviderToCreate);                   

                    oPrvToProcessResult.Add(new ExcelProviderResultModel()
                    {
                        PrvModel = prv,
                        Success = true,
                        Error = "Se ha creado el Proveedor '" + ProviderToCreate.ProviderPublicId + "'",
                    }); 
                    #endregion
                }
                catch (Exception err)
                {
                    oPrvToProcessResult.Add(new ExcelProviderResultModel()
                    {
                        PrvModel = prv,
                        Success = false,
                        Error = "Error :: " + err.Message + " :: " +
                                    err.StackTrace +
                                    (err.InnerException == null ? string.Empty :
                                    " :: " + err.InnerException.Message + " :: " +
                                    err.InnerException.StackTrace),
                    });
                }
                return true;
            });

            //save log file
            #region Error log file
            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(ErrorFilePath))
                {
                    string strSep = ";";

                    sw.WriteLine
                            ("\"RazonSocial\"" + strSep +
                            "\"TipoIdentificacion\"" + strSep +
                            "\"NumeroIdentificaion\"" + strSep +
                            "\"Email\"" + strSep +
                            "\"CampanaSalesforce\"" + strSep +

                            "\"Success\"" + strSep +
                            "\"Error\"");

                    oPrvToProcessResult.All(lg =>
                    {
                        sw.WriteLine
                            ("\"" + lg.PrvModel.RazonSocial + "\"" + strSep +
                            "\"" + lg.PrvModel.TipoIdentificacion + "\"" + strSep +
                            "\"" + lg.PrvModel.NumeroIdentificacion + "\"" + strSep +
                            "\"" + lg.PrvModel.Email + "\"" + strSep +
                            "\"" + lg.PrvModel.CampanaSalesforce + "\"" + strSep +

                            "\"" + lg.Success + "\"" + strSep +
                            "\"" + lg.Error + "\"");

                        return true;
                    });

                    sw.Flush();
                    sw.Close();
                }

                //load file to s3
                string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                    (ErrorFilePath,
                    DocumentManagement.Models.General.InternalSettings.Instance[DocumentManagement.Models.General.Constants.C_Settings_File_ExcelDirectory].Value);
                //remove temporal file
                if (System.IO.File.Exists(ErrorFilePath))
                    System.IO.File.Delete(ErrorFilePath);

                return strRemoteFile;
            }
            catch { }

            return null;
            #endregion
        }
        #endregion
    }
}