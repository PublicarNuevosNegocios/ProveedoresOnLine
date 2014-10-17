using DocumentManagement.Models.Customer;
using DocumentManagement.Models.Provider;
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
            };

            if (!string.IsNullOrEmpty(CustomerPublicId))
                oModel.RelatedCustomer = DocumentManagement.Customer.Controller.Customer.CustomerGetById(CustomerPublicId);

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
            string RemoteErrorFilePath = string.Empty;

            if (ExcelFile.ContentLength > 0)
            {
                //save file into server
                string Folder = DocumentManagement.Models.General.InternalSettings.Instance
                    [DocumentManagement.Models.General.Constants.C_Settings_File_TmpExcelDir].Value.TrimEnd('\\');

                if (!System.IO.Directory.Exists(Folder)) { System.IO.Directory.CreateDirectory(Folder); };

                string FilePath = Folder + "\\" + CustomerPublicId + "_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".xls";
                string ErrorFilePath = FilePath.Replace(".xls", "_log.csv");
                ExcelFile.SaveAs(FilePath);











                //proccess file
                //    ProccessAppointmentFile(FilePath, ErrorFilePath, ProfilePublicId, CurrentOffice);

                //    //upload images
                //    BackOffice.Models.General.GenericFileLoader oLoader = new BackOffice.Models.General.GenericFileLoader()
                //    {
                //        FilesToUpload = new List<string>() { FilePath, ErrorFilePath },
                //        RemoteFolder = BackOffice.Models.General.InternalSettings.Instance
                //            [BackOffice.Models.General.Constants.C_Settings_File_RemoteExcelDir].
                //            Value,
                //    };

                //    oLoader.StartUpload();

                //    System.IO.File.Delete(FilePath);
                //    System.IO.File.Delete(ErrorFilePath);

                //    RemoteErrorFilePath = oLoader.UploadedFiles.Where(x => x.FilePathLocalSystem == ErrorFilePath).Select(x => x.PublishFile.ToString()).FirstOrDefault();
                //}

                //return RedirectToAction(MVC.Profile.ActionNames.OfficeAppointmentUpload,
                //    MVC.Profile.Name,
                //    new
                //    {
                //        ProfilePublicId = ProfilePublicId,
                //        OfficePublicId = OfficePublicId,
                //        ErrorFile = RemoteErrorFilePath,
                //    });
            }
                return View();
        }

        private void ProccessAppointmentFile(string FilePath, string ErrorFilePath, string CustomerPublicId)
        {
            
            //get excel rows
            LinqToExcel.ExcelQueryFactory XlsInfo = new LinqToExcel.ExcelQueryFactory(FilePath);

            List<ExcelProviderModel> oPrvToProcess =
                (from x in XlsInfo.Worksheet<ExcelProviderModel>(0)
                 select x).ToList();

            //get profile info
            //List<InsuranceModel> olstInsurance = SaludGuruProfile.Manager.Controller.Insurance.GetAllAdmin(string.Empty);

            List<ExcelProviderResultModel> oPrvToProcessResult = new List<ExcelProviderResultModel>();

            //process Provider
            //oPrvToProcessResult.Where(prv => !string.IsNullOrEmpty(prv.NumeroIdentificacion)).All(prv =>
            //{
            //    try
            //    {                  

            //        if (CurrentPatient == null || string.IsNullOrEmpty(CurrentPatient.PatientPublicId))
            //        {
            //            #region Create Patient
            //            CurrentPatient = new PatientModel()
            //            {
            //                Name = apmt.Nombre,
            //                LastName = apmt.Apellido,
            //                PatientInfo = new List<MedicalCalendar.Manager.Models.Patient.PatientInfoModel>()
            //                    {
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.IdentificationNumber,
            //                            Value = apmt.Identificacion,
            //                        },
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.Email,
            //                            Value = apmt.Correo,
            //                        },
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.Telephone,
            //                            Value = string.Empty,
            //                        },
            //                         new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.Mobile,
            //                            Value = apmt.Celular,
            //                        },                        
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.Birthday,
            //                            Value = string.Empty,
            //                        },
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.Gender,
            //                            Value = "true",
            //                        },
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.Insurance,
            //                            Value = olstInsurance.Where(x=>x.Name == apmt.Seguro).Select(x=>x.CategoryId.ToString()).DefaultIfEmpty(string.Empty).FirstOrDefault(),
            //                        }, 
            //                         new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.MedicalPlan,
            //                            Value = string.Empty,
            //                        },
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.Responsable,
            //                            Value = string.Empty,
            //                        },
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.SendEmail,
            //                            Value = "true",
            //                        },
            //                        new PatientInfoModel()
            //                        {
            //                            PatientInfoType = enumPatientInfoType.SendSMS,
            //                            Value = "true",
            //                        }
            //                    },
            //            };

            //            CurrentPatient.PatientPublicId = MedicalCalendar.Manager.Controller.Patient.UpsertPatientInfo
            //                (CurrentPatient, ProfilePublicId, null);

            //            #endregion
            //        }

            //        //create appointment
            //        #region Create Appointment
            //        AppointmentModel CurrentAppointment = new AppointmentModel()
            //        {
            //            OfficePublicId = CurrentOffice.OfficePublicId,
            //            Status = MedicalCalendar.Manager.Models.enumAppointmentStatus.New,
            //            StartDate = new DateTime(apmt.Ano, apmt.Mes, apmt.Dia, apmt.Hora, apmt.Minutos, 0),
            //            EndDate = new DateTime(apmt.Ano, apmt.Mes, apmt.Dia, apmt.Hora, apmt.Minutos, 0).AddMinutes(apmt.Duracion),

            //            AppointmentInfo = new List<AppointmentInfoModel>()
            //                {
            //                    new AppointmentInfoModel()
            //                    {
            //                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.Category,
            //                        Value = CurrentOffice.RelatedTreatment.Select(x=>x.CategoryId.ToString()).FirstOrDefault(),
            //                    },
            //                    new AppointmentInfoModel()
            //                    {
            //                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.AfterCare,
            //                        LargeValue = string.Empty,
            //                    },
            //                    new AppointmentInfoModel()
            //                    {
            //                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.BeforeCare,
            //                        LargeValue = string.Empty,
            //                    },
            //                    new AppointmentInfoModel()
            //                    {
            //                        AppointmentInfoType = MedicalCalendar.Manager.Models.enumAppointmentInfoType.AppointmentNote,
            //                        LargeValue = string.Empty,
            //                    },
            //                },

            //            RelatedPatient = new List<PatientModel>()
            //                {
            //                    CurrentPatient,
            //                },
            //        };

            //        CurrentAppointment.AppointmentPublicId = MedicalCalendar.Manager.Controller.Appointment.UpsertAppointmentInfo
            //            (CurrentAppointment, new List<PatientModel>());

            //        #endregion

            //        oAptToProcessResult.Add(new ExcelAppointmentResultModel()
            //        {
            //            AptModel = apmt,
            //            Success = true,
            //            Error = "Se ha creado la cita '" + CurrentAppointment.AppointmentPublicId + "'",
            //        });

            //        #region Messenger

            //        ProfileModel oSource = new ProfileModel();
            //        oSource = SaludGuruProfile.Manager.Controller.Profile.ProfileGetFullAdmin(ProfilePublicId);

            //        List<PatientModel> oTargetList = new List<PatientModel>();
            //        oTargetList.Add(CurrentPatient);
            //        //Send de Signed App
            //        BackOffice.Web.Controllers.BaseController.SendMessage(oSource, enumProfileInfoType.AsignedAppointment, oTargetList, CurrentAppointment, false);
            //        //Send de Reminder App
            //        BackOffice.Web.Controllers.BaseController.SendMessage(oSource, enumProfileInfoType.ReminderAppointment, oTargetList, CurrentAppointment, false);

            //        #endregion
            //    }
            //    catch (Exception err)
            //    {
            //        oAptToProcessResult.Add(new ExcelAppointmentResultModel()
            //        {
            //            AptModel = apmt,
            //            Success = false,
            //            Error = "Error :: " + err.Message + " :: " +
            //                        err.StackTrace +
            //                        (err.InnerException == null ? string.Empty :
            //                        " :: " + err.InnerException.Message + " :: " +
            //                        err.InnerException.StackTrace),
            //        });
            //    }
            //    return true;
            //});

            ////save log file
            //#region Error log file
            //try
            //{
            //    using (System.IO.StreamWriter sw = new System.IO.StreamWriter(ErrorFilePath))
            //    {
            //        string strSep = ";";

            //        sw.WriteLine
            //                ("\"Dia\"" + strSep +
            //                "\"Mes\"" + strSep +
            //                "\"Ano\"" + strSep +
            //                "\"Hora\"" + strSep +
            //                "\"Minutos\"" + strSep +
            //                "\"Duracion\"" + strSep +
            //                "\"Identificacion\"" + strSep +
            //                "\"Nombre\"" + strSep +
            //                "\"Apellido\"" + strSep +
            //                "\"Celular\"" + strSep +
            //                "\"Correo\"" + strSep +
            //                "\"Seguro\"" + strSep +

            //                "\"Success\"" + strSep +
            //                "\"Error\"");

            //        oAptToProcessResult.All(lg =>
            //        {
            //            sw.WriteLine
            //                ("\"" + lg.AptModel.Dia + "\"" + strSep +
            //                "\"" + lg.AptModel.Mes + "\"" + strSep +
            //                "\"" + lg.AptModel.Ano + "\"" + strSep +
            //                "\"" + lg.AptModel.Hora + "\"" + strSep +
            //                "\"" + lg.AptModel.Minutos + "\"" + strSep +
            //                "\"" + lg.AptModel.Duracion + "\"" + strSep +
            //                "\"" + lg.AptModel.Identificacion + "\"" + strSep +
            //                "\"" + lg.AptModel.Nombre + "\"" + strSep +
            //                "\"" + lg.AptModel.Apellido + "\"" + strSep +
            //                "\"" + lg.AptModel.Celular + "\"" + strSep +
            //                "\"" + lg.AptModel.Correo + "\"" + strSep +
            //                "\"" + lg.AptModel.Seguro + "\"" + strSep +

            //                "\"" + lg.Success + "\"" + strSep +
            //                "\"" + lg.Error + "\"");

            //            return true;
            //        });

            //        sw.Flush();
            //        sw.Close();
            //    }
            //}
            //catch { }

            //#endregion
        }
    }
}