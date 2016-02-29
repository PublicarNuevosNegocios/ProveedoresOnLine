using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListProcess.Models.RestrictiveListProcess;
using ProveedoresOnLine.RestrictiveListProcessBatch.Models;
using System;
using System.Collections.Generic;
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
                
                //Build Excel File for Provider Status
                oModelToProcess.strListProviderStatus.All(x =>
                {
                    oModelToProcess.RelatedProvider = ProveedoresOnLine.RestrictiveListProcess.Controller.RestrictiveListProcessModule.GetProviderByStatus(Convert.ToInt32(x), ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_Settings_PublicarPublicId].Value);

                    if (oModelToProcess.RelatedProvider.Count > 0)
                    {
                        //Write the document
                        StringBuilder data = new StringBuilder();
                        string strSep = ";";

                        oModelToProcess.RelatedProvider.All(y =>
                        {
                            if (oModelToProcess.RelatedProvider.IndexOf(y) == 0)
                            {
                                data.AppendLine
                                ("\"" + ProveedoresOnLine.RestrictiveListProcessBatch.Models.General.InternalSettings.Instance[ProveedoresOnLine.RestrictiveListProcessBatch.Models.Constants.C_TK_CP_ColPersonType].Value  + "\"" + strSep + //Seting
                                    "\"" + "IdentificationType" + "\"" + strSep +
                                    "\"" + "IdentificationNumber" + "\"" + strSep +
                                    "\"" + "SearchType" + "\"" + strSep +
                                    "\"" + "Cargo" + "\"" + strSep +
                                    "\"" + "ProviderPublicId" + "\"" + strSep +
                                    "\"" + "BlackListStatus" + "\"");
                                data.AppendLine
                                    ("\"" + y.RelatedCompany.CompanyName + "\"" + "" + strSep +
                                    "\"" + y.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                                    "\"" + y.RelatedCompany.IdentificationNumber + "\"" + strSep +
                                    "\"" + "Company" + "\"" + strSep +
                                    "\"" + "N/A" + "\"" + strSep +
                                    "\"" + y.RelatedCompany.CompanyPublicId + "\"");
                                if (y.RelatedLegal != null && y.RelatedLegal.Count > 0)
                                {
                                    y.RelatedLegal.All(z =>
                                    {
                                        data.AppendLine
                                            (
                                                "\"" + z.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerName).Select(n => n.Value).FirstOrDefault() + "\"" + "" + strSep +
                                                "\"" + "CC" + "\"" + strSep +
                                                "\"" + z.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerIdentificationNumber).Select(n => n.Value).FirstOrDefault() + "\"" + "" + strSep +
                                                "\"" + "Person" + "\"" + strSep +
                                                "\"" + z.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerRank).Select(n => n.ValueName).FirstOrDefault() + "\"" + "" + strSep +
                                                "\"" + y.RelatedCompany.CompanyPublicId + "\""
                                            );
                                        return true;
                                    });
                                }
                            }
                            else
                            {
                                data.AppendLine
                                    ("\"" + y.RelatedCompany.CompanyName + "\"" + "" + strSep +
                                    "\"" + y.RelatedCompany.IdentificationType.ItemName + "\"" + strSep +
                                    "\"" + y.RelatedCompany.IdentificationNumber + "\"" + strSep +
                                    "\"" + "Company" + "\"" + strSep +
                                    "\"" + "N/A" + "\"" + strSep +
                                    "\"" + y.RelatedCompany.CompanyPublicId + "\"");
                                if (y.RelatedLegal != null && y.RelatedLegal.Count > 0)
                                {
                                    y.RelatedLegal.All(z =>
                                    {
                                        data.AppendLine
                                            (
                                                "\"" + z.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerName).Select(n => n.Value).FirstOrDefault() + "\"" + "" + strSep +
                                                "\"" + "CC" + "\"" + strSep +
                                                "\"" + z.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerIdentificationNumber).Select(n => n.Value).FirstOrDefault() + "\"" + "" + strSep +
                                                "\"" + "Person" + "\"" + strSep +
                                                "\"" + z.ItemInfo.Where(n => n.ItemInfoType.ItemId == (int)enumLegalDesignationsInfoType.CD_PartnerRank).Select(n => n.ValueName).FirstOrDefault() + "\"" + "" + strSep +
                                                "\"" + y.RelatedCompany.CompanyPublicId + "\""
                                            );
                                        return true;
                                    });
                                }
                            }
                            return true;
                        });

                        byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());
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

                LogFile += "\\" + "Log_SurveySendProcess_" + DateTime.Now.ToString("yyyyMMdd") + ".log";

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
