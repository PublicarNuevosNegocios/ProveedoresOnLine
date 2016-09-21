using IntegrationPlattaform.SANOFIProcess.Models;
using OfficeOpenXml;
using ProveedoresOnLine.Company.Models.Company;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegrationPlattaform.SANOFIProcess.Controller
{
    public class IntegrationPlatformSANOFIIProcess
    {
        public static void StartProcess()
        {
            try
            {
                //Gt Last ModifyDate Info Log Process
                SanofiProcessLogModel LastProcess = new SanofiProcessLogModel();
                LastProcess = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetSanofiLastProcessLog();

                // Get Providers SANOFI
                List<CompanyModel> oProviders = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicIdByStartDate(
                     IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance[
                     IntegrationPlattaform.SANOFIProcess.Models.Constants.C_SANOFI_ProviderPublicId].Value, LastProcess != null && LastProcess.ProviderPublicId != null ? LastProcess.LastModify : DateTime.Now.AddYears(-50));

                Tuple<bool, string, string> oGeneralResult = new Tuple<bool, string, string>(false, "", "");
                Tuple<bool, string, string> oComercialResult = new Tuple<bool, string, string>(false, "", "");
                Tuple<bool, string, string> oContableResult = new Tuple<bool, string, string>(false, "", "");

                if (oProviders != null)
                {
                    //log file
                    LogFile("Start send " + oProviders.Count.ToString());

                    List<SanofiGeneralInfoModel> oGeneralInfo = new List<SanofiGeneralInfoModel>();
                    List<SanofiComercialInfoModel> oComercialInfo = new List<SanofiComercialInfoModel>();
                    List<SanofiComercialInfoModel> oComercialBasicInfo = new List<SanofiComercialInfoModel>();
                    List<SanofiContableInfoModel> oContableInfo = new List<SanofiContableInfoModel>();
                    oProviders.All(p =>
                    {
                        //Get Last Process
                        //Modify Date against Last Created process
                        LogFile("ProviderPublicId:::: " + p.CompanyPublicId.ToString());
                        SanofiGeneralInfoModel oGeneralRow = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetInfoByProvider(p.CompanyPublicId).FirstOrDefault();
                        SanofiComercialInfoModel oComercialGeneralRow = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetComercialInfoByProvider(p.CompanyPublicId).FirstOrDefault();
                        SanofiComercialInfoModel oComercialBasicRow = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetComercialBasicInfoByProvider(p.CompanyPublicId).FirstOrDefault();
                        //TODO: get basic comercial info


                        SanofiContableInfoModel oContableRow = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetContableInfoByProvider(p.CompanyPublicId).FirstOrDefault();

                        if (oGeneralRow != null)
                            oGeneralInfo.Add(oGeneralRow);
                        if (oComercialGeneralRow != null && oComercialBasicRow != null)
                        {
                            oComercialInfo.Add(oComercialGeneralRow);
                            oComercialBasicInfo.Add(oComercialBasicRow);
                        }
                        if (oContableRow != null)
                            oContableInfo.Add(oContableRow);

                        return true;
                    });

                    //Call Function to create the txt;
                    if (oGeneralInfo.Count > 0)
                        oGeneralResult = GeneralInfoProcess(oGeneralInfo);

                    //Call Function to create the txt;
                    if (oComercialInfo.Count > 0 && oComercialBasicInfo.Count > 0)
                        oComercialResult = ComercialInfoProcess(oComercialInfo, oComercialBasicInfo);

                    //Call Function to create the txt;
                    if (oContableInfo.Count > 0)
                        oContableResult = ContableInfoProcess(oContableInfo);

                }
                else
                {
                    LogFile("Success:: SANOFI_Process:::Is:::OK '" + DateTime.Now + ":::No Provirders to validate:::");
                }
                if (!string.IsNullOrEmpty(oGeneralResult.Item2) ||
                    !string.IsNullOrEmpty(oComercialResult.Item2) ||
                    !string.IsNullOrEmpty(oContableResult.Item2))
                {
                    LogFile("Success:: SANOFI_Process:::Is:::OK::: '" + DateTime.Now + oGeneralResult.Item2 + ":::"
                                                                + oComercialResult.Item2 + ":::"
                                                                + oContableResult.Item2 + ":::");
                }

            }
            catch (Exception err)
            {
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }

        private static Tuple<bool, string, string> GeneralInfoProcess(List<SanofiGeneralInfoModel> oGeneralInfoModel)
        {
            try
            {
                string strFolder = IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance
                                    [IntegrationPlattaform.SANOFIProcess.Models.Constants.C_Settings_File_TempDirectory].Value;

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //Write the document
                // Set the file name and get the output directory
                string fileName = "SANOFI_GeneralInfo_" + DateTime.Now.ToString("yyyy_MM_dd_hhmmss") + ".csv";

                //Write Document
                StringBuilder Header = new StringBuilder();
                StringBuilder data = new StringBuilder();
                string strSep = "|";

                #region Start Create Process
                #region Write Header File
                Header.AppendLine
                               ("NOMBRE1" + strSep +
                               "NOMBRE2" + strSep +
                               "NOMBRE3" + strSep +
                               "NOMBRE4" + strSep +
                               "CONCEPTO DE BUSQUEDA" + strSep +
                               "NUMERO DE IDENTIFICACION FISCAL" + strSep +
                               "CALLE NUMERO" + strSep +
                               "POBLACION" + strSep +
                               "REGION" + strSep +
                               "PAIS" + strSep +
                               "TELEFONO" + strSep +
                               "FAX" + strSep +
                               "EMAIL OC" + strSep +
                               "EMAIL PAGOS" + strSep +
                               "EMAIL CERTIFICADOS RETENCION" + strSep +
                               "COMENTARIOS" + strSep);                
                #endregion
                oGeneralInfoModel.All(x =>
                {
                    #region Name3 and Name4 Field
                    string NaturalName = "";
                    string NaturalLastName = "";

                    if (!string.IsNullOrWhiteSpace(x.NaturalPersonName))
                    {
                        string[] words = x.NaturalPersonName.Split(' ');
                        List<string> NameWords = new List<string>();
                        List<string> LastNameWords = new List<string>();
                        int iN = 1;
                        int iLN = 1;
                        words.All(na =>
                        {
                            if (iN <= 2)
                                NameWords.Add(na);
                            iN++;
                            return true;
                        });
                        words.All(na =>
                        {
                            if (iLN >= 2)
                                LastNameWords.Add(na);
                            iLN++;
                            return true;
                        });
                        NaturalName = string.Join(", ", NameWords);
                        NaturalLastName = string.Join(", ", LastNameWords);

                    }
                    #endregion

                    #region Write Process
                    Header.AppendLine(
                           x.CompanyName + strSep +
                           x.ComercialName + strSep +
                          NaturalName + strSep +
                          NaturalLastName + strSep +
                          x.IdentificationNumber + strSep +
                          x.FiscalNumber + strSep +
                          x.Address + strSep +
                          x.City + strSep +
                          x.Region + strSep +
                          x.Country + strSep +
                          x.PhoneNumber + strSep +
                          x.Fax + strSep +
                          x.Email_OC + strSep +
                          x.Email_P + strSep +
                          x.Email_Cert + strSep +
                          x.Comentaries.ToShortDateString() + strSep);

                    #endregion
                    return true;
                });

                byte[] buffer = Encoding.Default.GetBytes(Header.ToString().ToCharArray());
                File.WriteAllBytes(strFolder + fileName, buffer);
                #endregion

                //Send to S3
                string oFileCompleteName = strFolder + fileName;
                //UpLoad file to s3                
                string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile(oFileCompleteName,
                    IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance[IntegrationPlattaform.SANOFIProcess.Models.Constants.C_Settings_File_ExcelDirectory].Value);

                //remove temporal file
                if (System.IO.File.Exists(oFileCompleteName))
                    System.IO.File.Delete(oFileCompleteName);

                //Save Process Log
                oGeneralInfoModel.All(x =>
                {
                    SanofiProcessLogModel oLogModel = new SanofiProcessLogModel()
                    {
                        ProviderPublicId = x.CompanyId.ToString(),
                        ProcessName = "GeneralInfo",
                        IsSucces = true,
                        Enable = true,
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });

            }
            catch (Exception ex)
            {
                //Save Process Log
                oGeneralInfoModel.All(x =>
                {
                    SanofiProcessLogModel oLogModel = new SanofiProcessLogModel()
                    {
                        ProviderPublicId = x.CompanyId.ToString(),
                        ProcessName = "GeneralInfo",
                        IsSucces = false,
                        Enable = true,
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });
                //todo: Write log
                return new Tuple<bool, string, string>(false, ex.Message, "::::GeneralInfoProcess::::");
            }

            //Return Log process true
            return new Tuple<bool, string, string>(false, "Validation Is Success", "::::GeneralInfoProcess::::");
        }

        private static Tuple<bool, string, string> ComercialInfoProcess(List<SanofiComercialInfoModel> oComercialGeneralInfoModel, List<SanofiComercialInfoModel> oComercialBasicInfoModel)
        {
            try
            {
                string strFolder = IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance
                                    [IntegrationPlattaform.SANOFIProcess.Models.Constants.C_Settings_File_TempDirectory].Value;

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //Write the document
                // Set the file name and get the output directory
                string fileName = "SANOFI_ComercialInfo_" + DateTime.Now.ToString("yyyy_MM_dd_hhmmss") + ".csv";

                //Write Document
                StringBuilder Header = new StringBuilder();
                StringBuilder data = new StringBuilder();
                string strSep = "|";

                #region Start Create Process
                #region Write Header File
                Header.AppendLine
                               ("NOMBRE1" + strSep +
                               "NUMERO DE IDENTIFICACION FISCAL" + strSep +
                               "CONCEPTO DE BUSQUEDA" + strSep +
                               "TIPO NIF" + strSep +
                               "GRUPO DE CUENTAS" + strSep +
                               "CLASE DE IMPUESTO" + strSep +
                               "MONEDA PEDIDO" + strSep +
                               "RAMO" + strSep +
                               "CONDICION DE PAGO" + strSep +
                               "GRUPO ESQUEMA PROVEEDOR" + strSep +
                               "VENDEDOR" + strSep +
                               "VERIFICACION FACT BASE EM" + strSep +
                               "GRUPO DE COMPRAS" + strSep);
                                
                #endregion
                oComercialGeneralInfoModel.All(x =>
                {
                    string ContactName = !string.IsNullOrEmpty(x.ContactName) ? x.ContactName.ToUpper() : "";
                    #region Write Process
                    Header.AppendLine(
                          x.CompanyName + strSep +
                          x.FiscalNumber + strSep +
                          x.IdentificationNumber + strSep +
                          x.NIFType + strSep +
                          oComercialBasicInfoModel[oComercialBasicInfoModel.IndexOf(x) + 1].CountsGroupItemName + strSep +
                          oComercialBasicInfoModel[oComercialBasicInfoModel.IndexOf(x) + 1].TaxClassName + strSep +
                          oComercialBasicInfoModel[oComercialBasicInfoModel.IndexOf(x) + 1].CurrencyName + strSep +
                          oComercialBasicInfoModel[oComercialBasicInfoModel.IndexOf(x) + 1].Ramo + strSep +
                          "00" + x.PayCondition + strSep +                          
                          x.GroupSchemaProvider + strSep +
                          ContactName + strSep +
                          "1" + strSep +
                          x.BuyCod + strSep);

                    #endregion
                    return true;
                });
                byte[] buffer = Encoding.Default.GetBytes(Header.ToString().ToCharArray());
                File.WriteAllBytes(strFolder + fileName, buffer);
                #endregion

                //Send to S3
                string oFileCompleteName = strFolder + fileName;
                //UpLoad file to s3                
                string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile(oFileCompleteName,
                    IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance[IntegrationPlattaform.SANOFIProcess.Models.Constants.C_Settings_File_ExcelDirectory].Value);

                //remove temporal file
                if (System.IO.File.Exists(oFileCompleteName))
                    System.IO.File.Delete(oFileCompleteName);

                //Save Process Log
                oComercialGeneralInfoModel.All(x =>
                {
                    SanofiProcessLogModel oLogModel = new SanofiProcessLogModel()
                    {
                        ProviderPublicId = x.CompanyId.ToString(),
                        ProcessName = "ComercialInfo",
                        IsSucces = true,
                        Enable = true,
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });

            }
            catch (Exception ex)
            {
                //Save Process Log
                oComercialGeneralInfoModel.All(x =>
                {
                    SanofiProcessLogModel oLogModel = new SanofiProcessLogModel()
                    {
                        ProviderPublicId = x.CompanyId.ToString(),
                        ProcessName = "ComercialInfo",
                        IsSucces = false,
                        Enable = true,
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });
                //Write log
                return new Tuple<bool, string, string>(false, ex.Message, "::::ComercialInfoProcess::::");
            }

            //Return Log process true
            return new Tuple<bool, string, string>(false, "Validation Is Success", "::::ComercialInfoProcess::::");
        }

        private static Tuple<bool, string, string> ContableInfoProcess(List<SanofiContableInfoModel> oContableInfoModel)
        {
            try
            {
                string strFolder = IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance
                                    [IntegrationPlattaform.SANOFIProcess.Models.Constants.C_Settings_File_TempDirectory].Value;

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //Write the document
                // Set the file name and get the output directory
                string fileName = "SANOFI_ContableInfo_" + DateTime.Now.ToString("yyyy_MM_dd_hhmmss") + ".csv";

                //Write Document
                StringBuilder Header = new StringBuilder();
                StringBuilder data = new StringBuilder();
                string strSep = "|";

                #region Start Create Process
                #region Write Header File
                Header.AppendLine
                               ("NOMBRE1" + strSep +
                               "NUMERO DE IDENTIFICACION FISCAL" + strSep +
                               "CONCEPTO DE BUSQUEDA" + strSep +
                               "PAIS" + strSep +
                               "CLAVE DE BANCOS" + strSep +
                               "CUENTA BANCARIA" + strSep +
                               "CC" + strSep +
                               "IBAN" + strSep +
                               "CUENTA ASOCIADA" + strSep +
                               "COND. PAGO" + strSep +
                               "GRUPO TOLERANCIA" + strSep +
                               "VERIF. FRA DOB." + strSep +
                               "TP.RECT" + strSep +
                               "VIAS DE PAGO" + strSep);
                          
                #endregion
                oContableInfoModel.All(x =>
                {
                    #region Write Process
                    Header.AppendLine(
                          x.CompanyName + strSep +
                          x.FiscalNumber + strSep +
                          x.IdentificationNumber + strSep +
                          x.Country + strSep +
                          x.BankPassword + strSep +
                          x.BankCountNumber + strSep +
                          "001" + strSep +
                          x.IBAN + strSep +
                          x.AssociatedCount + strSep +
                         "00"+ x.PayCondition + strSep +
                          "0010" + strSep +
                          "1" + strSep +
                          "1" + strSep +
                          x.PayCondition + strSep);
                    #endregion

                    return true;
                });
                byte[] buffer = Encoding.Default.GetBytes(Header.ToString().ToCharArray());
                File.WriteAllBytes(strFolder + fileName, buffer);

                #endregion
                //Send to S3
                string oFileCompleteName = strFolder + fileName;
                //UpLoad file to s3                
                string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile(oFileCompleteName,
                    IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance[IntegrationPlattaform.SANOFIProcess.Models.Constants.C_Settings_File_ExcelDirectory].Value);

                //remove temporal file
                if (System.IO.File.Exists(oFileCompleteName))
                    System.IO.File.Delete(oFileCompleteName);

                //Save Process Log
                oContableInfoModel.All(x =>
                {
                    SanofiProcessLogModel oLogModel = new SanofiProcessLogModel()
                    {
                        ProviderPublicId = x.CompanyId.ToString(),
                        ProcessName = "ContableInfo",
                        IsSucces = true,
                        Enable = true,
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });

            }
            catch (Exception ex)
            {
                //Save Process Log
                oContableInfoModel.All(x =>
                {
                    SanofiProcessLogModel oLogModel = new SanofiProcessLogModel()
                    {
                        ProviderPublicId = x.CompanyId.ToString(),
                        ProcessName = "ContableInfo",
                        IsSucces = false,
                        Enable = true,
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });
                //Write log
                return new Tuple<bool, string, string>(false, ex.Message, "::::ContableInfoProcess::::");
            }

            //Return Log process true
            return new Tuple<bool, string, string>(false, "Validation Is Success", "::::ContableInfoProcess::::");
        }

        public static List<SanofiGeneralInfoModel> GetInfoByProvider(string vProviderPublicId)
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetInfoByProvider(vProviderPublicId);
        }

        public static List<SanofiComercialInfoModel> GetComercialInfoByProvider(string vProviderPublicId)
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetComercialInfoByProvider(vProviderPublicId);
        }

        public static List<Models.SanofiContableInfoModel> GetContableInfoByProvider(string vProviderPublicId)
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetContableInfoByProvider(vProviderPublicId);
        }

        public static SanofiProcessLogModel SanofiProcessLogInsert(SanofiProcessLogModel oLogModel)
        {
            try
            {
                if (oLogModel != null)
                {
                    oLogModel.SanofiProcessLogId = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.SanofiProcessLogInsert
                        (
                            oLogModel.ProviderPublicId,
                            oLogModel.ProcessName,
                            oLogModel.FileName,
                            oLogModel.SendStatus,
                            oLogModel.IsSucces,
                            oLogModel.Enable
                        );
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            return oLogModel;
        }

        public static List<SanofiProcessLogModel> GetSanofiProcessLog(bool IsSuccess)
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetSanofiProcessLog(IsSuccess);
        }

        public static SanofiProcessLogModel GetSanofiLastProcessLog()
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetSanofiLastProcessLog();
        }

        #region Log File

        private static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings
                    [IntegrationPlattaform.SANOFIProcess.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_SANOFIProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

                using (System.IO.StreamWriter sw = System.IO.File.AppendText(LogFile))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "::" + LogMessage);
                    sw.Close();
                }
            }
            catch { }
        }
        #endregion
    }
}
