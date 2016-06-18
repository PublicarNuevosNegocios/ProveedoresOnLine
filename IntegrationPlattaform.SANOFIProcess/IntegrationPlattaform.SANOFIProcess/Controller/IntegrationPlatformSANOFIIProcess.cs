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
                     IntegrationPlattaform.SANOFIProcess.Models.Constants.C_SANOFI_ProviderPublicId].Value, LastProcess != null && LastProcess.ProviderPublicId != null? LastProcess.LastModify : DateTime.Now.AddYears(-50));

                Tuple<bool, string, string> oGeneralResult = new Tuple<bool, string, string>(false, "", "");
                Tuple<bool, string, string> oComercialResult = new Tuple<bool, string, string>(false, "", "");
                Tuple<bool, string, string> oContableResult = new Tuple<bool, string, string>(false, "", "");
                if (oProviders != null)
                {
                    List<SanofiGeneralInfoModel> oGeneralInfo = new List<SanofiGeneralInfoModel>();
                    List<SanofiComercialInfoModel> oComercialInfo = new List<SanofiComercialInfoModel>();
                    List<SanofiContableInfoModel> oContableInfo = new List<SanofiContableInfoModel>();
                    oProviders.All(p =>
                        {
                            //Get Last Process
                            //Modify Date against Last Created process

                            SanofiGeneralInfoModel oGeneralRow = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetInfoByProvider(p.CompanyPublicId).FirstOrDefault();
                            SanofiComercialInfoModel oComercialRow = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetComercialInfoByProvider(p.CompanyPublicId).FirstOrDefault(); 
                            SanofiContableInfoModel oContableRow = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetContableInfoByProvider(p.CompanyPublicId).FirstOrDefault(); 

                            if (oGeneralRow != null)
                                oGeneralInfo.Add(oGeneralRow);
                            if (oComercialRow != null)
                                oComercialInfo.Add(oComercialRow);
                            if (oContableRow != null)
                                oContableInfo.Add(oContableRow);

                            return true;
                        });

                    //Call Function to create the txt;
                    if (oGeneralInfo.Count > 0)
                        oGeneralResult = GeneralInfoProcess(oGeneralInfo);

                    //Call Function to create the txt;
                    if (oComercialInfo.Count > 0)
                        oComercialResult = ComercialInfoProcess(oComercialInfo);

                    //Call Function to create the txt;
                    if (oContableInfo.Count > 0)
                        oContableResult = ContableInfoProcess(oContableInfo);

                }

                LogFile("Success:: SANOFI_Process:::Is:::OK '"  + DateTime.Now + oGeneralResult.Item2 + ":::" 
                                                                + oComercialResult.Item2 + ":::" 
                                                                + oContableResult.Item2 +":::");
                
            }
            catch (Exception err)
            {
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
                throw;
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
                    data.Append(
                          "\"" + "NOMBRE1" + "\"" + strSep + x.CompanyName + strSep +
                         "NOMBRE2" + strSep + x.ComercialName + strSep +
                         "NOMBRE3" + strSep + NaturalName + strSep +
                         "NOMBRE4" + strSep + NaturalLastName + strSep +
                         "CONCEPTO DE BUSQUEDA" + strSep + x.IdentificationNumber + strSep +
                         "NUMERO DE IDENTIFICACION FISCAL" + strSep + x.FiscalNumber + strSep +
                         "CALLE NUMERO" + strSep + x.Address + strSep +
                         "POBLACION" + strSep + x.City + strSep +
                         "REGION" + strSep + x.Region + strSep +
                         "PAIS" + strSep + x.Country + strSep +
                         "TELEFONO" + strSep + string.Empty + strSep +
                         "FAX" + strSep + x.Fax + strSep +
                         "EMAIL OC" + strSep + x.Email_OC + strSep +
                         "EMAIL PAGOS" + strSep + x.Email_P + strSep +
                         "EMAIL CERTIFICADOS RETENCION" + strSep + x.Email_Cert + strSep +
                         "COMENTARIOS" + strSep + x.Comentaries.ToShortDateString() + strSep);

                    #endregion

                    return true;
                });

                byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());
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
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });

            }
            catch (Exception ex)
            {
                //todo: Write log
                return new Tuple<bool, string, string>(false, ex.Message, "::::GeneralInfoProcess::::");
            }

            //Return Log process true
            return new Tuple<bool, string, string>(false, "Validation Is Success", "::::GeneralInfoProcess::::");
        }

        private static Tuple<bool, string, string> ComercialInfoProcess(List<SanofiComercialInfoModel> oComercialInfoModel)
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
                oComercialInfoModel.All(x =>
                {
                    #region Write Process
                    data.Append(
                          "\"" + "NOMBRE1" + "\"" + strSep + x.CompanyName + strSep +
                          "NUMERO DE IDENTIFICACION FISCAL" + strSep + x.FiscalNumber + strSep +
                          "CONCEPTO DE BUSQUEDA" + strSep + x.IdentificationNumber + strSep +
                          "TIPO NIF" + strSep + x.NIFType + strSep +
                          "GRUPO DE CUENTAS" + strSep + x.CountsGroupItemName + strSep +
                          "CLASE DE IMPUESTO" + strSep + x.TaxClassName + strSep +
                          "MONEDA PEDIDO" + strSep + x.CurrencyName + strSep +
                          "RAMO" + strSep + x.Ramo + strSep +
                          "CONDICION DE PAGO" + strSep + x.PayCondition + strSep +
                          "GRUPO ESQUEMA PROVEEDOR" + strSep + x.GroupSchemaProvider + strSep +
                          "VENDEDOR" + strSep + x.ContactName + strSep +
                          "1" + strSep +
                          "GRUPO DE COMPRAS" + strSep + x.BuyCod + strSep);

                    #endregion

                    return true;
                });

                byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());
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
                oComercialInfoModel.All(x =>
                {
                    SanofiProcessLogModel oLogModel = new SanofiProcessLogModel()
                    {
                        ProviderPublicId = x.CompanyId.ToString(),
                        ProcessName = "ComercialInfo",
                        IsSucces = true,
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });

            }
            catch (Exception ex)
            {
                //todo: Write log
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
                oContableInfoModel.All(x =>
                {
                    #region Write Process
                    data.Append(
                          "\"" + "NOMBRE1" + "\"" + strSep + x.CompanyName + strSep +
                          "NUMERO DE IDENTIFICACION FISCAL" + strSep + x.FiscalNumber + strSep +
                          "CONCEPTO DE BUSQUEDA" + strSep + x.IdentificationNumber + strSep +
                          "PAIS" + strSep + x.Country + strSep +                          
                          "CLAVE DE BANCOS" + strSep + x.BankPassword + strSep +
                          "CUENTA BANCARIA" + strSep + x.BankCountNumber + strSep +
                          "CC" + strSep + "1" + strSep +
                          "IBAN" + strSep + x.IBAN + strSep +
                          "CUENTA ASOCIADA" + strSep + x.AssociatedCount + strSep +
                          "COND. PAGO" + strSep + x.PayCondition + strSep +
                          "GRUPO TOLERANCIA" + strSep + "0010" + strSep +
                          "VERIF. FRA DOB." + strSep + "1" + strSep +
                          "TP.RECT" + strSep + "1" + strSep +
                          "VIAS DE PAGO" + strSep + x.PayCondition + strSep);

                    #endregion

                    return true;
                });

                byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());
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
                    };
                    SanofiProcessLogInsert(oLogModel);
                    return true;
                });

            }
            catch (Exception ex)
            {
                //todo: Write log
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
                    [IntegrationPlattaform.SANOFIProcess.Models.Constants.C_Settings_File_TempDirectory].Trim().TrimEnd(new char[] { '\\' });

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
