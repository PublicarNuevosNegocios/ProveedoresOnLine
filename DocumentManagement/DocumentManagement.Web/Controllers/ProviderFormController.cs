using DocumentManagement.Customer.Models.Form;
using DocumentManagement.Models.Provider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DocumentManagement.Web.Controllers
{
    public partial class ProviderFormController : BaseController
    {
        public virtual ActionResult Index(string ProviderPublicId, string FormPublicId, string StepId, string msg)
        {
            int? oStepId = string.IsNullOrEmpty(StepId) ? null : (int?)Convert.ToInt32(StepId.Trim());

            ProviderFormModel oModel = new ProviderFormModel()
            {
                ProviderOptions = DocumentManagement.Provider.Controller.Provider.CatalogGetProviderOptions(),
                RealtedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId),
                RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, oStepId),
                errorMessage = msg != null ? msg : string.Empty,
            };

            oModel.RealtedForm = oModel.RealtedCustomer.
                RelatedForm.
                Where(x => x.FormPublicId == FormPublicId).
                FirstOrDefault();

            if (oStepId != null)
            {
                oModel.RealtedStep = oModel.RealtedForm.RelatedStep.
                    Where(x => x.StepId == (int)oStepId).
                    FirstOrDefault();
            }
            if (msg != null)
            {
                ViewData["ErrorMessage"] = "El Número o tipo de identificación son incorrectos";
            }
            return View(oModel);
        }

        public virtual ActionResult LoginProvider(string ProviderPublicId, string FormPublicId, string CustomerPublicId)
        {
            //get Provider info
            DocumentManagement.Provider.Models.Provider.ProviderModel RequestResult = GetLoginRequest();

            DocumentManagement.Provider.Models.Provider.ProviderModel RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetByIdentification
                (RequestResult.IdentificationNumber, RequestResult.IdentificationType.ItemId, CustomerPublicId);

            if (RealtedProvider != null && !string.IsNullOrEmpty(RealtedProvider.ProviderPublicId))
            {
                //get first step
                DocumentManagement.Customer.Models.Customer.CustomerModel RealtedCustomer =
                    DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId);

                //loggin success
                return RedirectToAction
                    (MVC.ProviderForm.ActionNames.Index,
                    MVC.ProviderForm.Name,
                    new
                    {
                        ProviderPublicId = ProviderPublicId,
                        FormPublicId = FormPublicId,
                        StepId = RealtedCustomer.
                            RelatedForm.
                            Where(x => x.FormPublicId == FormPublicId).
                            FirstOrDefault().
                            RelatedStep.OrderBy(x => x.Position).
                            FirstOrDefault().
                            StepId,
                    });
            }
            else
            {
                //loggin failed
                return RedirectToAction
                    (MVC.ProviderForm.ActionNames.Index,
                    MVC.ProviderForm.Name,
                    new
                    {
                        ProviderPublicId = ProviderPublicId,
                        FormPublicId = FormPublicId,
                        StepId = string.Empty,
                        msg = "El Número o tipo de identificación son incorrectos"
                    });
            }
        }

        public virtual ActionResult UpsertGenericStep(string ProviderPublicId, string FormPublicId, string StepId, string NewStepId)
        {
            //validate upsert action
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"] == "true")
            {
                //get generic models
                ProviderFormModel oGenericModels = new ProviderFormModel()
                {
                    //ProviderOptions = DocumentManagement.Provider.Controller.Provider.CatalogGetProviderOptions(),
                    RealtedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId),
                    RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, Convert.ToInt32(StepId)),
                };

                //get request into generic model
                GetUpsertGenericStepRequest(oGenericModels);

                //upsert fields in database
                DocumentManagement.Provider.Controller.Provider.ProviderInfoUpsert(oGenericModels.RealtedProvider);
            }

            //save success
            return RedirectToAction
                (MVC.ProviderForm.ActionNames.Index,
                MVC.ProviderForm.Name,
                new
                {
                    ProviderPublicId = ProviderPublicId,
                    FormPublicId = FormPublicId,
                    StepId = (!string.IsNullOrEmpty(NewStepId) && Convert.ToInt32(NewStepId) > 0) ? Convert.ToInt32(NewStepId) : Convert.ToInt32(StepId)
                });
        }

        public virtual ActionResult AdminProvider(string ProviderPublicId, string FormPublicId)
        {
            ProviderFormModel oModel = new ProviderFormModel()
            {
                ProviderOptions = DocumentManagement.Provider.Controller.Provider.CatalogGetProviderOptions(),
                RealtedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId),
                RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, null),
            };

            oModel.RealtedForm = oModel.RealtedCustomer.
                RelatedForm.
                Where(x => x.FormPublicId == FormPublicId).
                FirstOrDefault();

            return View(oModel);
        }

        public virtual ActionResult UpsertAdminProvider(string ProviderPublicId, string FormPublicId)
        {
            if (!string.IsNullOrEmpty(Request["UpsertAction"]) && Request["UpsertAction"] == "true")
            {
                ProviderFormModel oModel = new ProviderFormModel()
                {
                    ProviderOptions = DocumentManagement.Provider.Controller.Provider.CatalogGetProviderOptions(),
                    RealtedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId),
                    RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, null),
                };

                //get request object
                string strStatus = Request["selProviderStatus"];
                string strNote = Request["taNoteText"];

                if (!string.IsNullOrEmpty(strStatus))
                {
                    DocumentManagement.Provider.Models.Provider.ProviderInfoModel oCurrentStatus =
                        oModel.RealtedProvider.RelatedProviderCustomerInfo.
                        Where(x => x.ProviderInfoType.ItemId == 401).
                        FirstOrDefault();

                    DocumentManagement.Provider.Models.Provider.ProviderModel ProviderToUpsert =
                        new Provider.Models.Provider.ProviderModel()
                    {
                        ProviderPublicId = oModel.RealtedProvider.ProviderPublicId,
                        CustomerPublicId = oModel.RealtedProvider.CustomerPublicId,
                        RelatedProviderCustomerInfo = new List<Provider.Models.Provider.ProviderInfoModel>()
                    };

                    if (oCurrentStatus.Value.Replace(" ", "").ToLower() != strStatus.Replace(" ", "").ToLower())
                    {
                        ProviderToUpsert.RelatedProviderCustomerInfo.Add
                            (new DocumentManagement.Provider.Models.Provider.ProviderInfoModel()
                            {
                                ProviderInfoId = oCurrentStatus.ProviderInfoId,
                                ProviderInfoType = new Provider.Models.Util.CatalogModel()
                                    {
                                        ItemId = 401,
                                    },
                                Value = strStatus.Replace(" ", ""),
                            });

                        ProviderToUpsert.RelatedProviderCustomerInfo.Add
                            (new DocumentManagement.Provider.Models.Provider.ProviderInfoModel()
                            {
                                ProviderInfoId = 0,
                                ProviderInfoType = new Provider.Models.Util.CatalogModel()
                                {
                                    ItemId = 402,
                                },
                                LargeValue = strStatus.Replace(" ", "") + " - " + " Cambio de estado de " + oCurrentStatus.Value + " a " + strStatus,
                            });
                    }

                    if (!string.IsNullOrEmpty(strNote))
                    {
                        ProviderToUpsert.RelatedProviderCustomerInfo.Add
                            (new DocumentManagement.Provider.Models.Provider.ProviderInfoModel()
                            {
                                ProviderInfoId = 0,
                                ProviderInfoType = new Provider.Models.Util.CatalogModel()
                                    {
                                        ItemId = 402,
                                    },
                                LargeValue = strStatus.Replace(" ", "") + " - " + strNote,
                            });
                    }

                    DocumentManagement.Provider.Controller.Provider.ProviderCustomerInfoUpsert(ProviderToUpsert);
                }
            }

            //save success
            return RedirectToAction
                (MVC.ProviderForm.ActionNames.AdminProvider,
                MVC.ProviderForm.Name,
                new
                {
                    ProviderPublicId = ProviderPublicId,
                    FormPublicId = FormPublicId
                });
        }

        public virtual ActionResult DuplicateForm(string CustomerPublicId)
        {
            ProviderFormModel oModel = new ProviderFormModel();
            int oTotalRows;

            oModel.RealtedForm = DocumentManagement.Customer.Controller.Customer.FormSearch(CustomerPublicId, Request["formPublicId"], 0, 10000, out oTotalRows).FirstOrDefault();
            oModel.RealtedForm.RelatedStep = DocumentManagement.Customer.Controller.Customer.StepGetByFormId(Request["formPublicId"]);

            oModel.RealtedForm.FormPublicId = null;
            oModel.RealtedForm.Name = Request["NewFormName"];
            List<int> steps = new List<int>();

            //get created formId
            string FormId = DocumentManagement.Customer.Controller.Customer.FormUpsert(CustomerPublicId, oModel.RealtedForm);

            //get Related Steps to copy
            List<StepModel> oStepList = new List<StepModel>();
            oStepList = DocumentManagement.Customer.Controller.Customer.StepGetByFormId(Request["formPublicId"]);

            //Create the steps
            oStepList.All(st =>
            {

                int stepCreated = DocumentManagement.Customer.Controller.Customer.StepCreate(FormId, st);

                steps.Add(stepCreated);
                return true;
            });

            //get Related Fields
            List<StepModel> oStepByFieldList = new List<StepModel>();
            StepModel oStepByFieldItem = new StepModel();
            oStepByFieldItem.RelatedField = new List<FieldModel>();
            oStepList.All(st =>
            {
                st.RelatedField = DocumentManagement.Customer.Controller.Customer.FieldGetByStepId(st.StepId);
                return true;
            });
            for (int i = 0; i < oStepList.Count(); i++)
            {
                oStepList[i].Position = oStepList[i].StepId;
                oStepList[i].StepId = steps[i];
            }
            oStepList.All(st =>
            {
                st.RelatedField = DocumentManagement.Customer.Controller.Customer.FieldGetByStepId(st.Position);
                st.RelatedField.All(x =>
                    {
                        DocumentManagement.Customer.Controller.Customer.FieldCreate(st.StepId, x);
                        return true;
                    });

                return true;
            });

            return RedirectToAction
                (MVC.Customer.ActionNames.ListForm,
                MVC.Customer.Name,
                new
                {
                    CustomerPublicId = CustomerPublicId,
                });
        }

        public virtual FileResult GetPdfFileBytes(string FilePath)
        {
            byte[] bytes = (new System.Net.WebClient()).DownloadData(FilePath);
            return File
                (bytes,
                "application/pdf");
        }

        #region PrivateMethods

        private DocumentManagement.Provider.Models.Provider.ProviderModel GetLoginRequest()
        {
            DocumentManagement.Provider.Models.Provider.ProviderModel oReturn = new DocumentManagement.Provider.Models.Provider.ProviderModel();

            #region StepLogin

            if (!string.IsNullOrEmpty(Request["IdentificationType"]))
            {
                oReturn.IdentificationType = new Provider.Models.Util.CatalogModel()
                {
                    ItemId = Convert.ToInt32(Request["IdentificationType"]),
                };
            }

            if (!string.IsNullOrEmpty(Request["IdentificationNumber"]))
            {
                oReturn.IdentificationNumber = Request["IdentificationNumber"].Trim();
            }

            #endregion

            return oReturn;
        }

        private void GetUpsertGenericStepRequest(ProviderFormModel GenericModels)
        {
            //loop request
            Dictionary<string, string> ValidRequest = Request.
                Form.
                AllKeys.
                Select(rq => new
                {
                    Key = rq,
                    Value = rq.Split('-').
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault(),
                }).
                Where(rq => !string.IsNullOrEmpty(rq.Value) &&
                            ProviderFormModel.FieldTypes.Any(ft => ft.ToLower().Replace(" ", "") == rq.Value.ToLower().Replace(" ", ""))).
                ToDictionary(k => k.Key, v => v.Value);

            Request.Files.AllKeys.All(rqf =>
            {
                if (ProviderFormModel.FieldTypes.Any(ft =>
                    ft.ToLower().
                    Replace(" ", "") == rqf.Split('-').
                         DefaultIfEmpty(string.Empty).
                         FirstOrDefault().
                         ToLower().
                         Replace(" ", "")))
                {
                    ValidRequest.Add(rqf, rqf.Split('-').
                            DefaultIfEmpty(string.Empty).
                            FirstOrDefault());
                }

                return true;
            });


            GenericModels.RealtedProvider.RelatedProviderInfo = new List<Provider.Models.Provider.ProviderInfoModel>();

            ValidRequest.All(reqKey =>
                {
                    DocumentManagement.Provider.Models.Provider.ProviderInfoModel oProviderInfoToAdd = null;

                    if (MVC.Shared.Views._P_FieldEmail.IndexOf(reqKey.Value) >= 0 ||
                        MVC.Shared.Views._P_FieldNumber.IndexOf(reqKey.Value) >= 0 ||
                        MVC.Shared.Views._P_FieldQuantity.IndexOf(reqKey.Value) >= 0 ||
                        MVC.Shared.Views._P_FieldText.IndexOf(reqKey.Value) >= 0 ||
                        MVC.Shared.Views._P_FieldAutocomplete.IndexOf(reqKey.Value) >= 0)
                    {
                        oProviderInfoToAdd = GetFieldBasicRequest(reqKey.Key, GenericModels);
                    }

                    else if (MVC.Shared.Views._P_FieldFile.IndexOf(reqKey.Value) >= 0 ||
                            MVC.Shared.Views._P_FieldFormPdf.IndexOf(reqKey.Value) >= 0)
                    {
                        oProviderInfoToAdd = GetFieldFileRequest(reqKey.Key, GenericModels);
                    }
                    else if (MVC.Shared.Views._P_FieldPartners.IndexOf(reqKey.Value) >= 0)
                    {
                        oProviderInfoToAdd = GetFieldPartnerRequest(reqKey.Key, GenericModels);
                    }
                    else if (MVC.Shared.Views._P_FieldLegalTerms.IndexOf(reqKey.Value) >= 0)
                    {
                        oProviderInfoToAdd = GetFieldLegalTerms(reqKey.Key, GenericModels);
                    }
                    else if (MVC.Shared.Views._P_FieldMultipleFile.IndexOf(reqKey.Value) >= 0)
                    {
                        oProviderInfoToAdd = GetFieldMultipleFileRequest(reqKey.Key, GenericModels);
                    }
                    if (oProviderInfoToAdd != null)
                    {
                        GenericModels.RealtedProvider.RelatedProviderInfo.Add(oProviderInfoToAdd);
                    }
                    return true;
                });
        }

        private DocumentManagement.Provider.Models.Provider.ProviderInfoModel GetFieldBasicRequest(string RequestKey, ProviderFormModel GenericModels)
        {
            List<string> RequestKeySplit = RequestKey.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (RequestKeySplit.Count >= 2)
            {
                DocumentManagement.Provider.Models.Util.CatalogModel oProviderInfoType = GetProviderInfoType
                    (GenericModels, Convert.ToInt32(RequestKeySplit[1].Replace(" ", "")));

                if (oProviderInfoType != null)
                {
                    Provider.Models.Provider.ProviderInfoModel oReturn = new Provider.Models.Provider.ProviderInfoModel()
                    {
                        ProviderInfoId = RequestKeySplit.Count >= 3 ? Convert.ToInt32(RequestKeySplit[2].Replace(" ", "")) : 0,
                        ProviderInfoType = oProviderInfoType,
                        Value = Request[RequestKey],
                    };
                    return oReturn;
                }
            }
            return null;
        }

        private DocumentManagement.Provider.Models.Provider.ProviderInfoModel GetFieldFileRequest(string RequestKey, ProviderFormModel GenericModels)
        {
            List<string> RequestKeySplit = RequestKey.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (RequestKeySplit.Count >= 2)
            {
                //get folder
                string strFolder = Server.MapPath(DocumentManagement.Models.General.InternalSettings.Instance
                    [DocumentManagement.Models.General.Constants.C_Settings_File_TempDirectory].Value);

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //get File
                HttpPostedFileBase UploadFile = (HttpPostedFileBase)Request.Files[RequestKey];

                if (UploadFile != null && !string.IsNullOrEmpty(UploadFile.FileName))
                {
                    string strFile = strFolder.TrimEnd('\\') +
                        "\\ProviderFile_" +
                        GenericModels.RealtedProvider.ProviderPublicId + "_" +
                        RequestKeySplit[1].Replace(" ", "") + "_" +
                        DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                        UploadFile.FileName.Split('.').DefaultIfEmpty("pdf").LastOrDefault();

                    UploadFile.SaveAs(strFile);

                    //load file to s3
                    string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile
                        (strFile,
                        DocumentManagement.Models.General.InternalSettings.Instance
                            [DocumentManagement.Models.General.Constants.C_Settings_File_RemoteDirectoryProvider].Value +
                            GenericModels.RealtedProvider.ProviderPublicId + "\\");

                    //remove temporal file
                    if (System.IO.File.Exists(strFile))
                        System.IO.File.Delete(strFile);


                    DocumentManagement.Provider.Models.Util.CatalogModel oProviderInfoType = GetProviderInfoType
                        (GenericModels, Convert.ToInt32(RequestKeySplit[1].Replace(" ", "")));

                    if (oProviderInfoType != null)
                    {

                        Provider.Models.Provider.ProviderInfoModel oReturn = new Provider.Models.Provider.ProviderInfoModel()
                        {
                            ProviderInfoId = RequestKeySplit.Count >= 3 ? Convert.ToInt32(RequestKeySplit[2].Replace(" ", "")) : 0,
                            ProviderInfoType = oProviderInfoType,
                            LargeValue = strRemoteFile,
                        };
                        return oReturn;
                    }
                }
            }
            return null;
        }

        private DocumentManagement.Provider.Models.Provider.ProviderInfoModel GetFieldPartnerRequest(string RequestKey, ProviderFormModel GenericModels)
        {
            List<string> RequestKeySplit = RequestKey.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (RequestKeySplit.Count >= 2)
            {
                DocumentManagement.Provider.Models.Util.CatalogModel oProviderInfoType = GetProviderInfoType
                    (GenericModels, Convert.ToInt32(RequestKeySplit[1].Replace(" ", "")));

                ProviderFormPartners oReqObject = (ProviderFormPartners)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(Request[RequestKey], typeof(ProviderFormPartners));

                if (oProviderInfoType != null && oReqObject != null)
                {
                    Provider.Models.Provider.ProviderInfoModel oReturn = new Provider.Models.Provider.ProviderInfoModel()
                    {
                        ProviderInfoId = RequestKeySplit.Count >= 3 ? Convert.ToInt32(RequestKeySplit[2].Replace(" ", "")) : 0,
                        ProviderInfoType = oProviderInfoType,
                        LargeValue = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(oReqObject),
                    };
                    return oReturn;
                }
            }
            return null;
        }

        private DocumentManagement.Provider.Models.Provider.ProviderInfoModel GetFieldMultipleFileRequest(string RequestKey, ProviderFormModel GenericModels)
        {
            List<string> RequestKeySplit = RequestKey.Split('-').ToList();

            if (RequestKeySplit.Count >= 3)
            {
                DocumentManagement.Provider.Models.Util.CatalogModel oProviderInfoType = GetProviderInfoType
                    (GenericModels, Convert.ToInt32(RequestKeySplit[1].Replace(" ", "")));

                if (string.IsNullOrEmpty(RequestKeySplit[2]) && RequestKey.Contains("--File"))
                {
                    //insert operation

                    //get request object
                    ProviderMultipleFileModel oReqObject = new ProviderMultipleFileModel()
                    {
                        IsDelete = false,
                        Name = Request[RequestKeySplit[0] + "-" + RequestKeySplit[1] + "--Name"],
                        ProviderInfoId = string.Empty,
                    };

                    //get folder
                    string strFolder = Server.MapPath(DocumentManagement.Models.General.InternalSettings.Instance
                        [DocumentManagement.Models.General.Constants.C_Settings_File_TempDirectory].Value);

                    if (!System.IO.Directory.Exists(strFolder))
                        System.IO.Directory.CreateDirectory(strFolder);

                    //get File
                    HttpPostedFileBase UploadFile = (HttpPostedFileBase)Request.Files[RequestKeySplit[0] + "-" + RequestKeySplit[1] + "--File"];
                    string strFile = "";

                    if (UploadFile != null && !string.IsNullOrEmpty(UploadFile.FileName))
                    {
                        strFile = strFolder.TrimEnd('\\') +
                        "\\ProviderFile_" +
                        GenericModels.RealtedProvider.ProviderPublicId + "_" +
                        RequestKeySplit[1].Replace(" ", "") + "_" +
                        DateTime.Now.ToString("yyyyMMddHHmmss") + "." +
                        UploadFile.FileName.Split('.').DefaultIfEmpty("pdf").LastOrDefault();

                        UploadFile.SaveAs(strFile);

                        //load file to s3
                        oReqObject.ProviderInfoUrl = ProveedoresOnLine.FileManager.FileController.LoadFile
                            (strFile,
                            DocumentManagement.Models.General.InternalSettings.Instance
                                [DocumentManagement.Models.General.Constants.C_Settings_File_RemoteDirectoryProvider].Value +
                                GenericModels.RealtedProvider.ProviderPublicId + "\\");

                        //remove temporal file
                        if (System.IO.File.Exists(strFile))
                            System.IO.File.Delete(strFile);

                        //create return model
                        Provider.Models.Provider.ProviderInfoModel oReturn = new Provider.Models.Provider.ProviderInfoModel()
                        {
                            ProviderInfoId = 0,
                            ProviderInfoType = oProviderInfoType,
                            LargeValue = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(oReqObject),
                        };
                        return oReturn;
                    }
                }
                else if (!string.IsNullOrEmpty(RequestKeySplit[2]))
                {
                    //delete operation
                    ProviderMultipleFileModel oReqObject = (ProviderMultipleFileModel)(new System.Web.Script.Serialization.JavaScriptSerializer()).
                    Deserialize(Request[RequestKey], typeof(ProviderMultipleFileModel));

                    //create return model
                    Provider.Models.Provider.ProviderInfoModel oReturn = new Provider.Models.Provider.ProviderInfoModel()
                    {
                        ProviderInfoId = Convert.ToInt32(RequestKeySplit[2].Replace(" ", "")),
                        ProviderInfoType = oProviderInfoType,
                        LargeValue = (new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(oReqObject),
                    };
                    return oReturn;
                }
            }
            return null;
        }

        private DocumentManagement.Provider.Models.Util.CatalogModel GetProviderInfoType(ProviderFormModel GenericModels, int FieldId)
        {
            DocumentManagement.Provider.Models.Util.CatalogModel oReturn = null;

            GenericModels.RealtedCustomer.RelatedForm.All(f =>
            {
                f.RelatedStep.All(s =>
                {
                    s.RelatedField.All(fi =>
                    {
                        if (fi.FieldId == FieldId)
                        {
                            oReturn = new DocumentManagement.Provider.Models.Util.CatalogModel()
                            {
                                CatalogId = fi.ProviderInfoType.CatalogId,
                                CatalogName = fi.ProviderInfoType.CatalogName,
                                ItemId = fi.ProviderInfoType.ItemId,
                                ItemName = fi.ProviderInfoType.ItemName,
                            };
                        }

                        return true;
                    });

                    return true;
                });
                return true;
            });

            return oReturn;
        }

        private DocumentManagement.Provider.Models.Provider.ProviderInfoModel GetFieldLegalTerms(string RequestKey, ProviderFormModel GenericModels)
        {
            List<string> RequestKeySplit = RequestKey.Split(new char[] { '-' }, StringSplitOptions.RemoveEmptyEntries).ToList();

            if (RequestKeySplit.Count >= 2)
            {
                DocumentManagement.Provider.Models.Util.CatalogModel oProviderInfoType = GetProviderInfoType
                    (GenericModels, Convert.ToInt32(RequestKeySplit[1].Replace(" ", "")));

                if (oProviderInfoType != null)
                {
                    Provider.Models.Provider.ProviderInfoModel oReturn = new Provider.Models.Provider.ProviderInfoModel()
                    {
                        ProviderInfoId = RequestKeySplit.Count >= 3 ? Convert.ToInt32(RequestKeySplit[2].Replace(" ", "")) : 0,
                        ProviderInfoType = oProviderInfoType,
                        LargeValue = Request[RequestKey],
                    };
                    return oReturn;
                }
            }
            return null;
        }

        #endregion

    }
}