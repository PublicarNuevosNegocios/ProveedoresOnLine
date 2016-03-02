using LinqToExcel;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using ProveedoresOnLine.RestrictiveListProcessBatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ProveedoresOnLine.RestrictiveListProcessBatch
{
    public class RestrictiveListReadProcess
    {
        public void StartProcess()
        {
            try
            {
                //Start Process
                //Get all BlackListProcess
                List<RestrictiveListProcessModel> oProcessToValidate = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetAllProvidersInProcess();
                if (oProcessToValidate != null)
                {
                    //Call Function to get Coincidences



                    string strFolder = ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_File_TempDirectory].Value;

                    oProcessToValidate.All(Process =>
                    {
                        //Download Current File
                        using (WebClient webClient = new WebClient())
                        {
                            //Get file from S3 using File Name           
                            webClient.DownloadFile(Process.FilePath, strFolder + Process.FilePath.Split('/').LastOrDefault());
                        }

                        ExcelQueryFactory XlsInfo = new ExcelQueryFactory(strFolder + Process.FilePath.Split('/').LastOrDefault());

                        //Set model Params
                        List<ExcelModel> oExcelS3 =
                        (from x in XlsInfo.Worksheet<ExcelModel>(0)
                         select x).ToList();



                        return true;
                    });
                }

            }
            catch (Exception err)
            {
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
        }


        private List<Tuple<string, string, string>> GetCoincidences(string FileName)
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

            List<Tuple<string, string, string>> oCoincidences = new List<Tuple<string, string, string>>();

            //Set results to model
            CurrentXMLAnswer.Descendants("Resultado").All(
                x =>
                {
                    if (x.Element("NumeroConsulta").Value != "No existen registros asociados a los parámetros de consulta.")
                    {
                        //Create Info Conincidences                                        
                        oCoincidences.Add(new Tuple<string, string, string>(x.Element("TipoDocumento") != null && x.Element("TipoDocumento").Value != "N" ? x.Element("TipoDocumento").Value : string.Empty
                                                                           , x.Element("IdentificacionConsulta") != null && x.Element("IdentificacionConsulta").Value != "N" ? x.Element("IdentificacionConsulta").Value : string.Empty
                                                                           , x.Element("NombreConsulta") != null && x.Element("NombreConsulta").Value != "N" ? x.Element("NombreConsulta").Value : string.Empty));
                    }
                    return true;
                });
            return oCoincidences;
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
