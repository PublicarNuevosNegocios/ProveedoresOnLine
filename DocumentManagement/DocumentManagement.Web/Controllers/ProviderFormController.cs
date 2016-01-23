using DocumentManagement.Customer.Models.Form;
using DocumentManagement.Models.General;
using DocumentManagement.Models.Provider;
using DocumentManagement.Provider.Models.Provider;
using ProveedoresOnLine.AsociateProvider.Client.Models;
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

            string oCurrentActionName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();

            ProviderFormModel oModel = new ProviderFormModel()
            {
                ProviderOptions = DocumentManagement.Provider.Controller.Provider.CatalogGetProviderOptions(),
                RealtedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId),
                RealtedProvider = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, oStepId),
                ChangesControlModel = DocumentManagement.Provider.Controller.Provider.ChangesControlGetByProviderPublicId(ProviderPublicId),
                ShowChanges = SessionModel.CurrentLoginUser != null ? true : false,
                errorMessage = msg != null ? msg : string.Empty,
            };

            oModel.RealtedForm = oModel.RealtedCustomer.
                RelatedForm.
                Where(x => x.FormPublicId == FormPublicId).
                FirstOrDefault();

            if (oModel.RealtedForm != null && oModel.RealtedForm.RelatedStep != null && oModel.ShowChanges)
            {
                oModel.RealtedForm.RelatedStep.All(x =>
                    {
                        if (oModel.ChangesControlModel.Any(y => y.StepId == x.StepId))
                            x.ShowChanges = true;
                        return true;
                    });
            }
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

            if (RealtedProvider != null && !string.IsNullOrEmpty(RealtedProvider.ProviderPublicId) && ProviderPublicId == RealtedProvider.ProviderPublicId)
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

        public virtual ActionResult UpsertGenericStep(string ProviderPublicId, string FormPublicId, string StepId, string NewStepId, string IsSync)
        {
            if (IsSync == "true")
            {
                #region Form Sync Upsert
                // Split Sync_ fields whit values and infoType
                // 
                GetSyncRequestDate(ProviderPublicId);
                #endregion
            }
            else
            {
                #region Form Upsert
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

                    //PrevModel
                    ProviderModel oPervProviderModel = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, Convert.ToInt32(StepId));

                    //get request into generic model
                    GetUpsertGenericStepRequest(oGenericModels);

                    //Call Function to get differences and return a ChangesModel
                    List<ChangesControlModel> oChangesToUpsert = GetChangesToUpdate(oPervProviderModel, oGenericModels.RealtedProvider, FormPublicId, Convert.ToInt32(StepId));
                    if (oChangesToUpsert.Count > 0)
                    {
                        oChangesToUpsert.All(x =>
                        {
                            x.ChangesPublicId = DocumentManagement.Provider.Controller.Provider.ChangesControlUpsert(x);
                            return true;
                        });
                    }
                    else
                    {
                        DocumentManagement.Provider.Controller.Provider.ProviderInfoUpsert(oGenericModels.RealtedProvider);
                        oPervProviderModel = DocumentManagement.Provider.Controller.Provider.ProviderGetById(ProviderPublicId, Convert.ToInt32(StepId));

                        oChangesToUpsert = GetChangesToUpdate(oPervProviderModel, oGenericModels.RealtedProvider, FormPublicId, Convert.ToInt32(StepId));
                        oChangesToUpsert.All(x =>
                        {
                            x.ChangesPublicId = DocumentManagement.Provider.Controller.Provider.ChangesControlUpsert(x);
                            return true;
                        });
                    }

                    //upsert fields in database
                    DocumentManagement.Provider.Controller.Provider.ProviderInfoUpsert(oGenericModels.RealtedProvider);
                }
                #endregion
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

        public virtual ActionResult AdminLogProvider(string ProviderPublicId, string FormPublicId)
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

        public virtual ActionResult LoginProviderChangesControl(string ProviderPublicId, string FormPublicId, string CustomerPublicId, string IdentificationType, string IdentificationNumber)
        {
            if (string.IsNullOrEmpty(CustomerPublicId) && !string.IsNullOrEmpty(FormPublicId))
                CustomerPublicId = DocumentManagement.Customer.Controller.Customer.CustomerGetByFormId(FormPublicId).CustomerPublicId;

            //get Provider info
            DocumentManagement.Provider.Models.Provider.ProviderModel RealtedProvider =
                DocumentManagement.Provider.Controller.Provider.ProviderGetByIdentification(IdentificationNumber, Convert.ToInt32(IdentificationType), CustomerPublicId);

            if (RealtedProvider != null && !string.IsNullOrEmpty(RealtedProvider.ProviderPublicId) && ProviderPublicId == RealtedProvider.ProviderPublicId)
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

        #region Private Functions

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

        #region ChangesControl

        private List<ChangesControlModel> GetChangesToUpdate(ProviderModel PrevModel, ProviderModel NewModel, string FormPublicId, int StepId)
        {
            List<ChangesControlModel> oReturn = new List<ChangesControlModel>();

            if (PrevModel != null && PrevModel.RelatedProviderInfo != null && PrevModel.RelatedProviderInfo.Count > 0)
            {
                NewModel.RelatedProviderInfo.All(inf =>
                {
                    if ((inf.ProviderInfoId == PrevModel.RelatedProviderInfo.Where(p => p.ProviderInfoId == inf.ProviderInfoId).
                        Select(p => p.ProviderInfoId).FirstOrDefault() &&
                        inf.Value != PrevModel.RelatedProviderInfo.Where(p => p.ProviderInfoId == inf.ProviderInfoId).
                        Select(p => p.Value).FirstOrDefault()
                        || inf.LargeValue != PrevModel.RelatedProviderInfo.Where(p => p.ProviderInfoId == inf.ProviderInfoId).
                        Select(p => p.LargeValue).FirstOrDefault())
                        && inf.ProviderInfoId != 0
                        && inf.ProviderInfoType.ItemId != (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.ComercialExpirience
                        && inf.ProviderInfoType.ItemId != (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.Designations
                        && inf.ProviderInfoType.ItemId != (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.DifferentsFile
                        && inf.ProviderInfoType.ItemId != (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.FinancialStatus
                        && inf.ProviderInfoType.ItemId != (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.QualityCertificate)
                    {
                        oReturn.Add(new ChangesControlModel
                        {
                            ProviderInfoId = inf.ProviderInfoId,
                            FormUrl = FormPublicId,
                            StepId = StepId,
                            Status = new Provider.Models.Util.CatalogModel()
                            {
                                ItemId = (int)DocumentManagement.Provider.Models.Enumerations.enumChangesStatus.NotValidated
                            },
                            Enable = true,
                        });
                    }
                    else if ((inf.ProviderInfoType.ItemId == (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.ComercialExpirience
                        || inf.ProviderInfoType.ItemId == (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.Designations
                        || inf.ProviderInfoType.ItemId == (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.DifferentsFile
                        || inf.ProviderInfoType.ItemId == (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.FinancialStatus
                        || inf.ProviderInfoType.ItemId == (int)DocumentManagement.Customer.Models.enumFormMultipleFieldType.QualityCertificate)
                        && !string.IsNullOrEmpty(inf.LargeValue)
                        )
                    {
                        if (inf.ProviderInfoId != 0)
                        {
                            if (inf.LargeValue.Split('"')[6] == ":false,")
                            {
                                oReturn.Add(new ChangesControlModel
                                {
                                    ProviderInfoId = inf.ProviderInfoId,
                                    FormUrl = FormPublicId,
                                    StepId = StepId,
                                    Status = new Provider.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)DocumentManagement.Provider.Models.Enumerations.enumChangesStatus.NotValidated
                                    },
                                    Enable = true,
                                });
                            }
                            else
                            {
                                List<ChangesControlModel> oChangeToUpdate = DocumentManagement.Provider.Controller.Provider.ChangesControlGetByProviderPublicId(PrevModel.ProviderPublicId);
                                ChangesControlModel oToUpsert = null;
                                if (oChangeToUpdate != null)
                                    oToUpsert = oChangeToUpdate.Where(x => x.ProviderInfoId == inf.ProviderInfoId).Select(x => x).FirstOrDefault();

                                oReturn.Add(new ChangesControlModel
                                {
                                    ChangesPublicId = oToUpsert.ChangesPublicId,
                                    ProviderInfoId = inf.ProviderInfoId,
                                    FormUrl = FormPublicId,
                                    StepId = StepId,
                                    Status = new Provider.Models.Util.CatalogModel()
                                    {
                                        ItemId = (int)DocumentManagement.Provider.Models.Enumerations.enumChangesStatus.NotValidated
                                    },
                                    Enable = false,
                                });
                            }
                        }
                        else
                        {
                            DocumentManagement.Provider.Controller.Provider.ProviderInfoUpsert(NewModel);
                            PrevModel = DocumentManagement.Provider.Controller.Provider.ProviderGetById(NewModel.ProviderPublicId, Convert.ToInt32(StepId));

                            oReturn = GetChangesToUpdate(PrevModel, NewModel, FormPublicId, Convert.ToInt32(StepId));
                        }
                    }
                    return true;
                });

                if (oReturn.Count == 0)
                {
                    PrevModel.RelatedProviderInfo.All(x =>
                    {
                        if (x.ProviderInfoId != 0 && !string.IsNullOrEmpty(x.Value)
                                                  || !string.IsNullOrEmpty(x.LargeValue))
                        {
                            oReturn.Add(new ChangesControlModel
                            {
                                ProviderInfoId = x.ProviderInfoId,
                                FormUrl = FormPublicId,
                                StepId = StepId,
                                Status = new Provider.Models.Util.CatalogModel()
                                {
                                    ItemId = (int)DocumentManagement.Provider.Models.Enumerations.enumChangesStatus.NotValidated
                                },
                                Enable = true,
                            });
                        }
                        return true;
                    });
                }
            }
            return oReturn;
        }

        private ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel GetSyncRequestDate(string ProviderPublicId)
        {
            ProviderFormModel oModel = new ProviderFormModel()
            {
                ChangesControlModel = DocumentManagement.Provider.Controller.Provider.ChangesControlGetByProviderPublicId(ProviderPublicId),
            };

            ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel oReturn = new ProveedoresOnLine.CompanyProvider.Models.Provider.ProviderModel();
            string oCompanyPublicid = ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.GetAsociateProviderByProviderPublicId(ProviderPublicId, string.Empty).RelatedProviderBO.ProviderPublicId;

            List<string> oSyncFieldsrequest = Request.Form.
            AllKeys.Where(x => x.Split('_')[0] == "Sync" && Request.Form[x] == "on").ToList();

            List<Tuple<string, HomologateModel>> oGeneralHomologateData = new List<Tuple<string, HomologateModel>>();

            oSyncFieldsrequest.All(x =>
            {
                oGeneralHomologateData.Add(new Tuple<string, HomologateModel>(x,
                        ProveedoresOnLine.AsociateProvider.Client.Controller.AsociateProviderClient.GetHomologateItemBySourceID(int.Parse(Request.Form[x.Replace("Sync_", "") + " " + "_ItemType".TrimStart()]))));
                return true;
            });

            //SerpareData Type
            #region Contact Info
            List<Tuple<string, HomologateModel>> oContactHomologateData = oGeneralHomologateData.Where(x => (x.Item2 != null && x.Item2.Target.CatalogId == (int)DocumentManagement.Provider.Models.Enumerations.enumCatalog.CompanyContactInfoType ||
                                                                                                             x.Item2 != null && x.Item2.Target.CatalogId == (int)DocumentManagement.Provider.Models.Enumerations.enumCatalog.DistributorInfoType ||
                                                                                                             x.Item2 != null && x.Item2.Target.CatalogId == (int)DocumentManagement.Provider.Models.Enumerations.enumCatalog.PersonContactInfoType ||
                                                                                                             x.Item2 != null && x.Item2.Target.CatalogId == (int)DocumentManagement.Provider.Models.Enumerations.enumCatalog.BrachInfoType)).Select(x => x).ToList();
            if (oContactHomologateData != null)
            {
                #region Contact Sync
                List<Tuple<string, HomologateModel>> oContactToSync = oContactHomologateData.Where(x => x.Item2.Target.CatalogId == (int)DocumentManagement.Provider.Models.Enumerations.enumCatalog.CompanyContactInfoType).Select(x => x).ToList();

                if (oContactToSync != null && oContactToSync.Count > 0)
                {
                    //Build obj contact
                    ProveedoresOnLine.Company.Models.Company.CompanyModel oCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = oCompanyPublicid,
                        RelatedContact = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                                        {
                                            new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                            {
                                                ItemId = 0,
                                                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)DocumentManagement.Provider.Models.Enumerations.enumContactType.CompanyContact,
                                                },
                                                ItemName = null,
                                                Enable = true,
                                                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                                            },
                                        }
                    };
                    oContactToSync.All(x =>
                    {
                        if (oCompany.RelatedContact.FirstOrDefault().ItemId != null)
                        {
                            oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = x.Item2.Target.ItemId
                                },
                                Value = Request.Form[x.Item1.Replace("Sync_", "")],
                                Enable = true,
                            });
                        }
                        return true;
                    });

                    //oCompany = ProveedoresOnLine.Company.Controller.Company.ContactUpsert(oCompany);

                    //List<ChangesControlModel> oChangesToUpsert = oModel.ChangesControlModel.Where(y => y.ProviderInfoId == )

                    //oContactToSync.All(x =>
                    //    {

                    //        //DocumentManagement.Provider.Controller.Provider.ChangesControlUpsert()
                    //        return true;
                    //    });
                }
                #endregion
                #region Branch Sync
                List<Tuple<string, HomologateModel>> oBranchToSync = oContactHomologateData.Where(x => x.Item2.Target.CatalogId == (int)DocumentManagement.Provider.Models.Enumerations.enumCatalog.BrachInfoType).Select(x => x).ToList();

                if (oBranchToSync != null && oBranchToSync.Count > 0)
                {
                    //Build obj contact
                    ProveedoresOnLine.Company.Models.Company.CompanyModel oCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = oCompanyPublicid,
                        RelatedContact = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                                        {
                                            new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                            {
                                                ItemId = 0,
                                                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)DocumentManagement.Provider.Models.Enumerations.enumContactType.Brach,
                                                },
                                                ItemName = null,
                                                Enable = true,
                                                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                                            },
                                        }
                    };
                    oBranchToSync.All(x =>
                    {
                        if (oCompany.RelatedContact.FirstOrDefault().ItemId != null)
                        {
                            oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = x.Item2.Target.ItemId
                                },
                                Value = Request.Form[x.Item1.Replace("Sync_", "")],
                                Enable = true,
                            });
                        }
                        return true;
                    });

                    //Upsertinfo BO
                    oCompany = ProveedoresOnLine.Company.Controller.Company.ContactUpsert(oCompany);

                    //Upsert Changes Sincronized
                    oContactToSync.All(x =>
                        {
                            List<ChangesControlModel> oChangesToUpsert =
                                oModel.ChangesControlModel.Where(y => y.ProviderInfoId == int.Parse(x.Item1.Split('-').Last())).Select(y => { y.Enable = false; return y; }).ToList();
                            oChangesToUpsert.All(
                                ch =>{
                                    DocumentManagement.Provider.Controller.Provider.ChangesControlUpsert(ch);
                                    return true;
                                });

                            return true;
                        });                    
                }
                #endregion
                #region PersonContact Sync
                List<Tuple<string, HomologateModel>> oPersonToSync = oContactHomologateData.Where(x => x.Item2.Target.CatalogId == (int)DocumentManagement.Provider.Models.Enumerations.enumCatalog.PersonContactInfoType).Select(x => x).ToList();

                if (oPersonToSync != null && oPersonToSync.Count > 0)
                {
                    //Build obj contact
                    ProveedoresOnLine.Company.Models.Company.CompanyModel oCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = oCompanyPublicid,
                        RelatedContact = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                                        {
                                            new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                            {
                                                ItemId = 0,
                                                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)DocumentManagement.Provider.Models.Enumerations.enumContactType.PersonContact,
                                                },
                                                ItemName = null,
                                                Enable = true,
                                                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                                            },
                                        }
                    };
                    oPersonToSync.All(x =>
                    {
                        if (oCompany.RelatedContact.FirstOrDefault().ItemId != null)
                        {
                            oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = x.Item2.Target.ItemId
                                },
                                Value = Request.Form[x.Item1.Replace("Sync_", "")],
                                Enable = true,
                            });
                        }
                        return true;
                    });
                }
                #endregion
                #region Distributor Sync
                List<Tuple<string, HomologateModel>> oDistributorToSync = oContactHomologateData.Where(x => x.Item2.Target.CatalogId == (int)DocumentManagement.Provider.Models.Enumerations.enumCatalog.DistributorInfoType).Select(x => x).ToList();

                if (oDistributorToSync != null && oDistributorToSync.Count > 0)
                {
                    //Build obj contact
                    ProveedoresOnLine.Company.Models.Company.CompanyModel oCompany = new ProveedoresOnLine.Company.Models.Company.CompanyModel()
                    {
                        CompanyPublicId = oCompanyPublicid,
                        RelatedContact = new List<ProveedoresOnLine.Company.Models.Util.GenericItemModel>()
                                        {
                                            new ProveedoresOnLine.Company.Models.Util.GenericItemModel()
                                            {
                                                ItemId = 0,
                                                ItemType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                                {
                                                    ItemId = (int)DocumentManagement.Provider.Models.Enumerations.enumContactType.Distributor,
                                                },
                                                ItemName = null,
                                                Enable = true,
                                                ItemInfo = new List<ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel>(),
                                            },
                                        }
                    };
                    oDistributorToSync.All(x =>
                    {
                        if (oCompany.RelatedContact.FirstOrDefault().ItemId != null)
                        {
                            oCompany.RelatedContact.FirstOrDefault().ItemInfo.Add(new ProveedoresOnLine.Company.Models.Util.GenericItemInfoModel()
                            {
                                ItemInfoId = 0,
                                ItemInfoType = new ProveedoresOnLine.Company.Models.Util.CatalogModel()
                                {
                                    ItemId = x.Item2.Target.ItemId
                                },
                                Value = Request.Form[x.Item1.Replace("Sync_", "")],
                                Enable = true,
                            });
                        }
                        return true;
                    });
                }
                #endregion
            }
            #endregion

            List<Tuple<string, HomologateModel>> oLegalHomologateData = null;

            return oReturn;
        }

        #endregion

        #endregion
    }
}