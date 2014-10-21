using DocumentManagement.Customer.Models.Customer;
using DocumentManagement.Customer.Models.Form;
using DocumentManagement.Models.Customer;
using DocumentManagement.Models.Provider;
using DocumentManagement.Provider.Models.Provider;
using DocumentManagement.Provider.Models.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class CustomerController : BaseController
    {
        public virtual ActionResult Index()
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
            ////get folder
            //string strFolder = Server.MapPath(DocumentManagement.Models.General.Constants.C_Settings_File_TempDirectory);

            //if (!System.IO.Directory.Exists(strFolder))
            //    System.IO.Directory.CreateDirectory(strFolder);

            ////get File
            //string strFile = strFolder.TrimEnd('\\') +
            //    "\\Logo_" +
            //    CustomerPublicId + "_" +
            //    DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpeg";

            //UploadFile.SaveAs(strFile);

            ////load file to s3
            //string strRemoteFile = DocumentManagement.Provider.Controller.Provider.LoadFile
            //    (strFile,
            //    DocumentManagement.Models.General.InternalSettings.Instance[DocumentManagement.Models.General.Constants.C_Settings_File_RemoteDirectory].Value);

            ////update file into db
            //DocumentManagement.Customer.Controller.Customer.FormUpsertLogo
            //                    (FormPublicId,
            //                    strRemoteFile);

            ////remove temporal file
            //if (System.IO.File.Exists(strFile))
            //    System.IO.File.Delete(strFile);

            //return RedirectToAction(MVC.Customer.ActionNames.UpsertForm, MVC.Customer.Name, new
            //{
            //    CustomerPublicId = CustomerPublicId,
            //    FormPublicId = FormPublicId
            //});
            return View();
        }

        public virtual ActionResult UploadProvider(string CustomerPublicId)
        {
            UpserCustomerModel oModel = new UpserCustomerModel()
            {
                RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId),
            };

            return View(oModel);
        }

        [HttpPost]
        public virtual ActionResult UploadProvider(string CustomerPublicId, HttpPostedFileBase ExcelFile)
        {
            string strFolder = Server.MapPath(DocumentManagement.Models.General.Constants.C_Settings_File_TempDirectory);

            if (!System.IO.Directory.Exists(strFolder))
                System.IO.Directory.CreateDirectory(strFolder);

            //get File
            string strFile = strFolder.TrimEnd('\\') +
                "\\ProviderUploadFile_" +
                CustomerPublicId + "_" +
                DateTime.Now.ToString("yyyyMMddHHmmss") + ".xls";

            ExcelFile.SaveAs(strFile);

            //load file to s3
            //string strRemoteFile = DocumentManagement.Provider.Controller.Provider.LoadFile
            //    (strFile,
            //    DocumentManagement.Models.General.InternalSettings.Instance[DocumentManagement.Models.General.Constants.C_Settings_File_RemoteDirectory].Value);

            //update file into db          
            this.ProccessProviderFile(strFile, CustomerPublicId);

            //remove temporal file
            if (System.IO.File.Exists(strFile))
                System.IO.File.Delete(strFile);

            return RedirectToAction(MVC.Customer.ActionNames.UploadProvider, MVC.Customer.Name, new
            {
                CustomerPublicId = CustomerPublicId
            });
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
                oReturn.TermsAndConditions = Request["TermsAndConditions"];
            }

            return oReturn;
        }

        private void ProccessProviderFile(string FilePath, string CustomerPublicId)
        {
            //get excel rows
            LinqToExcel.ExcelQueryFactory XlsInfo = new LinqToExcel.ExcelQueryFactory(FilePath);

            List<ExcelProviderModel> oPrvToProcess =
                (from x in XlsInfo.Worksheet<ExcelProviderModel>(0)
                 select x).ToList();

            List<ExcelProviderResultModel> oPrvToProcessResult = new List<ExcelProviderResultModel>();
            
            //process Provider
            oPrvToProcess.Where(prv => !string.IsNullOrEmpty(prv.numerodeidentificacion)).All(prv =>
            {
                try
                {
                    //Validar el provider
                    ProviderModel Provider = new ProviderModel();

                    int IdentificationType = 0; 

                    //Create ProviderCustomerInfo
                    List<ProviderInfoModel> ListCustomerProviderInfo = new List<ProviderInfoModel>();
                    ProviderInfoModel CustomerProviderInfo = new ProviderInfoModel();

                    CustomerProviderInfo.ProviderInfoType = new CatalogModel() { ItemId = 201 };
                    ListCustomerProviderInfo.Add(CustomerProviderInfo);

                    switch (prv.tipodeidentificacion)
                    {
                        case "nit":
                            IdentificationType = 102;
                            break;
                        case "Cedula de Ciudadania":
                            IdentificationType = 101;
                            break;
                        default:
                            break;
                    }

                    //Create Provider
                    ProviderModel ProviderToCreate = new ProviderModel()
                    {
                        CustomerPublicId = CustomerPublicId,
                        Name = prv.nombre,
                        IdentificationType = new Provider.Models.Util.CatalogModel() { ItemId = IdentificationType },
                        IdentificationNumber = prv.numerodeidentificacion,
                        Email = prv.email,
                        RelatedProviderCustomerInfo = ListCustomerProviderInfo

                    };

                    DocumentManagement.Provider.Controller.Provider.ProviderUpsert(ProviderToCreate);
                    //}
                    //else
                    //{
                    //    bool isRelated = false;
                    //    isRelated = DocumentManagement.Provider.Controller.Provider.GetRelationProviderAndCustomer(CustomerPublicId, Provider.ProviderPublicId);
                    //    if (!isRelated)
                    //    {
                    //        DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType ProvInfoType = (DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType)Enum.Parse(typeof(DocumentManagement.Provider.Models.Enumerations.enumProviderCustomerInfoType), prv.tipodeidentificacion, true);
                    //        DocumentManagement.Provider.Controller.Provider.ProviderCustomerInfoUpsert(0, Provider.ProviderPublicId, CustomerPublicId, ProvInfoType, string.Empty, string.Empty);
                    //    }
                    //}
                }
                catch (Exception err)
                {

                    //oAptToProcessResult.Add(new ExcelAppointmentResultModel()
                    //{
                    //    AptModel = apmt,
                    //    Success = false,
                    //    Error = "Error :: " + err.Message + " :: " +
                    //                err.StackTrace +
                    //                (err.InnerException == null ? string.Empty :
                    //                " :: " + err.InnerException.Message + " :: " +
                    //                err.InnerException.StackTrace),
                    //});

                }
                return true;
            });
        }
        #endregion
    }
}