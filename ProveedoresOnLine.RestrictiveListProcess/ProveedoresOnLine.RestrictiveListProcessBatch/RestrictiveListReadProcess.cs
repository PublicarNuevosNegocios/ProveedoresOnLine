using LinqToExcel;
using NetOffice.ExcelApi;
using ProveedoresOnLine.Company.Models.Util;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using ProveedoresOnLine.RestrictiveListProcessBatch.Models;
using ProveedoresOnLine.ThirdKnowledge.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProveedoresOnLine.RestrictiveListProcessBatch
{
    public class RestrictiveListReadProcess
    {
        public static void StartProcess()
        {
            try
            {
                //Start Process
                //Get all BlackListProcess
                List<RestrictiveListProcessModel> oProcessToValidate = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetAllProvidersInProcess();
                if (oProcessToValidate != null)
                {
                    string strFolder = ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_File_TempDirectory].Value;

                    oProcessToValidate.All(Process =>
                    {
                        //Instance App to read excel
                        Application app = new Application();

                        string FileName = Process.FilePath.Split('/').LastOrDefault();
                        //Call Function to get Coincidences
                        List<Tuple<string, string>> oCoincidences = GetCoincidences(FileName);

                        //Download Current File
                        using (WebClient webClient = new WebClient())
                        {
                            //Get file from S3 using File Name           
                            webClient.DownloadFile(Process.FilePath, strFolder + FileName);

                            //Open File DownLoaded
                            Workbook book = app.Workbooks.Open(strFolder + FileName,
                                   Missing.Value, Missing.Value, Missing.Value,
                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                   Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                   Missing.Value, Missing.Value, Missing.Value);

                            //Save As .xls
                            book.SaveAs(strFolder + FileName.Replace("xlsx", "xls"), fileFormat: Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8);
                            book.Close();
                        }
                      

                        ExcelQueryFactory XlsInfo = new ExcelQueryFactory(strFolder + FileName.Replace("xlsx", "xls"));
                        //Set model Params
                        List<ExcelModel> oExcelS3 =
                        (from x in XlsInfo.Worksheet<ExcelModel>(0)
                         select x).ToList();

                        //Get Provider by Status                        
                        Process.RelatedProvider = new List<ProviderModel>();
                        //Compare Company
                        List<ProviderModel> oProvidersToCompare = new List<ProviderModel>();
                        //Compare Persons                        
                        List<ProviderModel> oCompanyPersonToCompare = new List<ProviderModel>();
                        ProviderModel oPersonModel = new ProviderModel();

                        Process.RelatedProvider = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetProviderByStatus(Convert.ToInt32(Process.ProviderStatus), ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_PublicarPublicId].Value);

                        //Get Coincidences by Company
                        oProvidersToCompare = Process.RelatedProvider.Where(x => oCoincidences.Any(p => p.Item1 == "IdentificacionConsulta" && p.Item2 == x.RelatedCompany.IdentificationNumber || p.Item1 == "NombreConsulta" && p.Item2 == x.RelatedCompany.CompanyName)).ToList();

                        Process.RelatedProvider.All(prv =>
                        {
                            oPersonModel = prv;                            
                            List<GenericItemModel> oLegalInfo = prv.RelatedLegal.Where(x => oCoincidences.Any(p => p.Item1 == "IdentificacionConsulta" && p.Item2 == x.ItemInfo.Where(inf => inf.ItemInfoType.ItemId ==
                                                                                                      (int)enumLegalDesignationsInfoType.CD_PartnerIdentificationNumber) //Identification Number
                                                                                                      .Select(inf => inf.Value).FirstOrDefault() || 
                                                                                                           p.Item1 == "NombreConsulta" && p.Item2 == x.ItemInfo.Where(inf => inf.ItemInfoType.ItemId ==
                                                                                                      (int)enumLegalDesignationsInfoType.CD_PartnerName)                 //Person Name
                                                                                                      .Select(inf => inf.Value).FirstOrDefault())).ToList();
                            if (oLegalInfo.Count > 0)
                                oCompanyPersonToCompare.Add(prv);

                            return true;
                        });

                        //Create BalcListInfo
                        if (oProvidersToCompare.Count > 0)
                        {
                            oProvidersToCompare.All(x =>
                            {
                                //Clear BlackList By Provider
                                ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.BlackListClearProvider(x.RelatedCompany.CompanyPublicId);

                                //Get Providerbasic info                                
                                //Update date and show alert 
                                //get Coincidences for company
                                //Valid if the coincidence is active else                              
                                //Create Black List Model frmo coincidences
                                //Update model Black List and Black List Info

                                return true;
                            });
                        }
                        if (oCompanyPersonToCompare.Count > 0)
                        {
                            oCompanyPersonToCompare = new List<ProviderModel>();
                        }

                        //Remove all Files
                        //remove temporal file
                        if (System.IO.File.Exists(strFolder + FileName))
                            System.IO.File.Delete(strFolder + FileName);

                        //remove temporal file
                        if (System.IO.File.Exists(strFolder + FileName.Replace("xlsx", "xls")))
                            System.IO.File.Delete(strFolder + FileName.Replace("xlsx", "xls"));

                        return true;
                    });
                }
            }
            catch (Exception err)
            {
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }


        private static List<Tuple<string, string>> GetCoincidences(string FileName)
        {
            //Set access
            string ftpServerIP = ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_FTPServerIP].Value;
            string uploadToFolder = ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_UploadFTPFileName].Value;
            string UserName = ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_FTPUserName].Value;
            string UserPass = ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_FTPPassworUser].Value;

            FileName = FileName.Replace(FileName.Split('.').LastOrDefault(), "xml");

            string uri = "ftp://" + ftpServerIP + "/" + uploadToFolder + "/" + "Res_" + FileName;
            byte[] buffer = new byte[1024];

            FtpWebRequest request = ((FtpWebRequest)WebRequest.Create(new Uri(uri)));
            request.Credentials = new NetworkCredential(UserName, UserPass, ftpServerIP);
            request.UsePassive = true;
            request.UseBinary = true;
            request.KeepAlive = true;
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            //Connect to ftp
            FtpWebResponse response = (FtpWebResponse)request.GetResponse();

            //Download xml result
            Stream responseStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(responseStream);
            string xml = reader.ReadToEnd();
            XDocument CurrentXMLAnswer = XDocument.Parse(xml);

            List<Tuple<string, string>> oInfoCreate = new List<Tuple<string, string>>();

            //Set results to model
            CurrentXMLAnswer.Descendants("Resultado").All(
                x =>
                {
                    if (x.Element("NumeroConsulta").Value != "No existen registros asociados a los parámetros de consulta.")
                    {
                        //if (x.Element("Prioridad").Value != null && x.Element("Prioridad").Value != "3")
                        //{
                            #region QueryInfo
                            oInfoCreate.Add(new Tuple<string, string>("Alias", x.Element("Alias").Value));
                            oInfoCreate.Add(new Tuple<string, string>("DocumentoIdentidad", x.Element("DocumentoIdentidad").Value));
                            oInfoCreate.Add(new Tuple<string, string>("NombreCompleto", x.Element("NombreCompleto").Value));
                            oInfoCreate.Add(new Tuple<string, string>("CargoDelito", x.Element("CargoDelito").Value));
                            oInfoCreate.Add(new Tuple<string, string>("Peps", x.Element("Peps").Value));
                            oInfoCreate.Add(new Tuple<string, string>("Prioridad", x.Element("Prioridad").Value));
                            oInfoCreate.Add(new Tuple<string, string>("Estado", x.Element("Estado").Value));
                            oInfoCreate.Add(new Tuple<string, string>("IdTipoLista", x.Element("IdTipoLista").Value));
                            oInfoCreate.Add(new Tuple<string, string>("Prioridad", x.Element("Prioridad").Value));
                            oInfoCreate.Add(new Tuple<string, string>("FechaRegistro", x.Element("FechaRegistro").Value));
                            oInfoCreate.Add(new Tuple<string, string>("CargoDelito", x.Element("CargoDelito").Value));
                            oInfoCreate.Add(new Tuple<string, string>("DocumentoIdentidad", x.Element("DocumentoIdentidad").Value));
                            oInfoCreate.Add(new Tuple<string, string>("FechaActualizacion", x.Element("FechaActualizacion").Value));
                            oInfoCreate.Add(new Tuple<string, string>("NumeroConsulta", x.Element("NumeroConsulta").Value));

                            if (x.Element("IdGrupoLista").Value == "1" && !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value))
                                oInfoCreate.Add(new Tuple<string, string>("NumeroConsulta", x.Element("NumeroConsulta").Value + " - Criticidad Alta"));

                            if (x.Element("IdGrupoLista").Value == "2" && !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value))
                                oInfoCreate.Add(new Tuple<string, string>("NumeroConsulta", x.Element("NumeroConsulta").Value + " - Criticidad Media"));

                            if (x.Element("IdGrupoLista").Value == "3" && !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value))
                                oInfoCreate.Add(new Tuple<string, string>("NumeroConsulta", x.Element("NumeroConsulta").Value + " - Criticidad Media"));

                            if (x.Element("IdGrupoLista").Value == "4" && !string.IsNullOrEmpty(x.Element("NombreGrupoLista").Value))
                                oInfoCreate.Add(new Tuple<string, string>("NumeroConsulta", x.Element("NumeroConsulta").Value + " - Criticidad Baja"));

                            oInfoCreate.Add(new Tuple<string, string>("IdGrupoLista", x.Element("IdGrupoLista").Value));
                            oInfoCreate.Add(new Tuple<string, string>("Link", x.Element("Link").Value));
                            oInfoCreate.Add(new Tuple<string, string>("NombreCompleto", x.Element("NombreCompleto").Value));
                            oInfoCreate.Add(new Tuple<string, string>("NombreTipoLista", x.Element("NombreTipoLista").Value));
                            oInfoCreate.Add(new Tuple<string, string>("OtraInformacion", x.Element("OtraInformacion").Value));
                            oInfoCreate.Add(new Tuple<string, string>("Zona", x.Element("Zona").Value));
                            oInfoCreate.Add(new Tuple<string, string>("NombreConsulta", x.Element("NombreConsulta").Value));
                            oInfoCreate.Add(new Tuple<string, string>("IdentificacionConsulta", x.Element("IdentificacionConsulta").Value));
                            oInfoCreate.Add(new Tuple<string, string>("TipoDocumento", x.Element("TipoDocumento").Value));
                            #endregion
                        //}
                    }
                    return true;
                });
            return oInfoCreate;
        }
        #region Log File

        private static void LogFile(string LogMessage)
        {
            try
            {
                //get file Log
                string LogFile = AppDomain.CurrentDomain.BaseDirectory.Trim().TrimEnd(new char[] { '\\' }) + "\\" +
                    System.Configuration.ConfigurationManager.AppSettings[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_AppSettings_LogFile].Trim().TrimEnd(new char[] { '\\' });

                if (!System.IO.Directory.Exists(LogFile))
                    System.IO.Directory.CreateDirectory(LogFile);

                LogFile += "\\" + "Log_BlacListReadProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

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
