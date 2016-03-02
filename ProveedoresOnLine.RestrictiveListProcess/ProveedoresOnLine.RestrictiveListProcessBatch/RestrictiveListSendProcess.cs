using OfficeOpenXml;
using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using ProveedoresOnLine.RestrictiveListProcessBatch.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcessBatch
{
    public class RestrictiveListSendProcess
    {
        public static void StartProcess()
        {
            try
            {
                //Start Process
                //Set RestrictiveListProcessModel
                RestrictiveListProcessModel oModelToProcess = new RestrictiveListProcessModel();
                oModelToProcess.RelatedProvider = new List<ProviderModel>();

                string strFolder = ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_File_TempDirectory].Value;

                if (!System.IO.Directory.Exists(strFolder))
                    System.IO.Directory.CreateDirectory(strFolder);

                //Build Excel File for Provider Status
                oModelToProcess.strListProviderStatus.All(x =>
                {
                    oModelToProcess.RelatedProvider = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetProviderByStatus(Convert.ToInt32(x), ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_PublicarPublicId].Value);

                    if (oModelToProcess.RelatedProvider.Count > 0)
                    {
                        //Write the document
                        // Set the file name and get the output directory
                        string fileName = "BlackList_" + x +"_"+ DateTime.Now.ToString("yyyy_MM_dd_hhmmss") + ".xlsx";

                        // Create the file using the FileInfo object
                        FileInfo file = new FileInfo(strFolder + fileName);

                        // Create the package and make sure you wrap it in a using statement
                        using (var package = new ExcelPackage(file))
                        {
                            // add a new worksheet to the empty workbook
                            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Hoja 1");

                            // Start adding the header
                            // First of all the first row
                            worksheet.Cells[1, 1].Value = ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_TK_CP_ColPersonType].Value;
                            worksheet.Cells[1, 2].Value = ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_TK_CP_ColIdNumber].Value;
                            worksheet.Cells[1, 3].Value = ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_TK_CP_ColIdName].Value;

                            int row = 1;
                            oModelToProcess.RelatedProvider.All(y =>
                            {
                                row++;

                                //Company
                                worksheet.Cells[row, 1].Value = "J";
                                worksheet.Cells[row, 2].Value = y.RelatedCompany.IdentificationNumber;
                                worksheet.Cells[row, 3].Value = y.RelatedCompany.CompanyName;

                                if (y.RelatedLegal != null && y.RelatedLegal.Count > 0)
                                {
                                    y.RelatedLegal.All(z =>
                                    {
                                        row++;

                                        //Persons
                                        worksheet.Cells[row, 1].Value = "N";
                                        worksheet.Cells[row, 2].Value = z.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerIdentificationNumber).Select(n => n.Value).FirstOrDefault();
                                        worksheet.Cells[row, 3].Value = z.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerName).Select(n => n.Value).FirstOrDefault();

                                        return true;
                                    });
                                }
                                return true;
                            });

                            package.Save();
                        }

                        string oFileCompleteName = strFolder + fileName;
                        //UpLoad file to s3
                        string strRemoteFile = ProveedoresOnLine.FileManager.FileController.LoadFile(oFileCompleteName,
                            ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[
                            ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_File_ExcelDirectory].Value);

                        //Upload File
                        bool isLoaded = false;
                        isLoaded = ProveedoresOnLine.ThirdKnowledge.Controller.ThirdKnowledgeModule.AccessFTPClient(fileName, oFileCompleteName, string.Empty);

                        //remove temporal file
                        if (System.IO.File.Exists(strFolder + fileName))
                            System.IO.File.Delete(strFolder + fileName);

                        //Create BlackListProces
                        RestrictiveListProcessModel oProcessToCreate = new RestrictiveListProcessModel()
                        {
                            BlackListProcessId = 0,
                            Enable = true,
                            FilePath = strRemoteFile,
                            IsSuccess = true,
                            ProcessStatus = false,
                            ProviderStatus = x
                        };

                        int oProcessToCreateId = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.BlackListProcessUpsert(oProcessToCreate);

                        LogFile("Success:: BlackListProcessId '" + oProcessToCreateId + "':: ProviderStatus '" + oProcessToCreate.ProviderStatus + "':: Validation is success");
                    }
                    return true;
                });

            }
            catch (Exception err)
            {
                LogFile("Fatal error::" + err.Message + " - " + err.StackTrace);
            }
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

                LogFile += "\\" + "Log_BlacListWriteProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

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
