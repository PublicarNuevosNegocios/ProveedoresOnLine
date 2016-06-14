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
                // Get Providers SANOFI
               List<CompanyModel> oProviders = ProveedoresOnLine.CompanyProvider.Controller.CompanyProvider.GetAllProvidersByCustomerPublicId(
                    IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance[
                    IntegrationPlattaform.SANOFIProcess.Models.Constants.C_SANOFI_ProviderPublicId].Value);

               if (oProviders != null)
               {
                    List<SanofiGeneralInfoModel> oGeneralInfo = new List<SanofiGeneralInfoModel>();

                   oProviders.All(p =>
                       {
                           //Get Last Process
                           //Modify Date against Last Created process

                            SanofiGeneralInfoModel oGeneralRow = DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetInfo_ByProvider(p.CompanyPublicId).FirstOrDefault();
                            if (oGeneralRow != null)                           
                                oGeneralInfo.Add(oGeneralRow);                           
                           
                           return true;
                       });

                    if (oGeneralInfo.Count > 0)
                    {
                        //Call Function to create the txt;
                        GeneralInfoProcess(oGeneralInfo);
               }
                
                }

                // Get Process Last Time 
                // Get Info by Provider by lastModify 

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public static bool GeneralInfoProcess(List<SanofiGeneralInfoModel> oGeneralInfoModel)
        {
            string strFolder = IntegrationPlattaform.SANOFIProcess.Models.InternalSettings.Instance[IntegrationPlattaform.SANOFIProcess.Models.Constants.C_Settings_File_TempDirectory].Value;

            if (!System.IO.Directory.Exists(strFolder))
                System.IO.Directory.CreateDirectory(strFolder);

            //Write the document
            // Set the file name and get the output directory
            string fileName = "SANOFI_GeneralInfo_" + DateTime.Now.ToString("yyyy_MM_dd_hhmmss") + ".csv";

            var GeneralInfoFile = new FileInfo(strFolder + fileName);

            // Create the package and make sure you wrap it in a using statement
            using (var package = new ExcelPackage(GeneralInfoFile))
            {
                // add a new worksheet to the empty workbook
                ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("GeneralInfo_" + DateTime.Now.ToShortDateString());

                // --------- Data and styling goes here -------------- //
                worksheet.Cells[1, 1].Value = "NOMBRE1|";
                worksheet.Cells[1, 2].Value = "NOMBRE2|";
                worksheet.Cells[1, 3].Value = "NOMBRE3|";
                worksheet.Cells[1, 4].Value = "NOMBRE4|";
                worksheet.Cells[1, 5].Value = "CONCEPTO DE BUSQUEDA|";
                worksheet.Cells[1, 6].Value = "NUMERO DE IDENTIFICACION FISCAL|";
                worksheet.Cells[1, 7].Value = "CALLE NUMERO|";
                worksheet.Cells[1, 8].Value = "POBLACION|";
                worksheet.Cells[1, 9].Value = "REGION|";
                worksheet.Cells[1, 10].Value = "PAIS|";
                worksheet.Cells[1, 11].Value = "TELEFONO|";
                worksheet.Cells[1, 12].Value = "EXT|";
                worksheet.Cells[1, 13].Value = "FAX|";
                worksheet.Cells[1, 14].Value = "EMAIL OC|";
                worksheet.Cells[1, 15].Value = "EMAIL PAGOS|";
                worksheet.Cells[1, 16].Value = "EMAIL CERTIFICADOS RETENCION|";
                worksheet.Cells[1, 17].Value = "COMENTARIOS|";

                oGeneralInfoModel.All(x =>
                {
                    // Add the second row of header data
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 1].Value = !string.IsNullOrWhiteSpace(x.CompanyName) ? x.CompanyName + "|": string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 2].Value = !string.IsNullOrWhiteSpace(x.ComercialName) ? x.ComercialName + "|" : string.Empty;

                    #region Name3 and Name4 Field
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
                        string NaturalName = string.Join(", ", NameWords);
                        string NaturalLastName = string.Join(", ", LastNameWords);

                        worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 3].Value = NaturalName + "|";
                        worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 4].Value = NaturalLastName + "|";
                    }
                    else
                    {
                        worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 3].Value = "";
                        worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 4].Value = "";
                    } 
                    #endregion                   
                    
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 5].Value = !string.IsNullOrWhiteSpace(x.IdentificationNumber) ? x.IdentificationNumber + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 6].Value = !string.IsNullOrWhiteSpace(x.FiscalNumber) ? x.FiscalNumber + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 7].Value = !string.IsNullOrWhiteSpace(x.Address) ? x.Address + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 8].Value = !string.IsNullOrWhiteSpace(x.City) ? x.City + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 9].Value = !string.IsNullOrWhiteSpace(x.Region) ? x.Region + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 10].Value = !string.IsNullOrWhiteSpace(x.Country) ? x.Country + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 11].Value = !string.IsNullOrWhiteSpace(x.PhoneNumber) ? x.PhoneNumber + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 12].Value = string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 13].Value = !string.IsNullOrWhiteSpace(x.Fax) ? x.Fax + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 14].Value = !string.IsNullOrWhiteSpace(x.Email_OC) ? x.Email_OC + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 15].Value = !string.IsNullOrWhiteSpace(x.Email_P) ? x.Email_P + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 16].Value = !string.IsNullOrWhiteSpace(x.Email_Cert) ? x.Email_Cert + "|" : string.Empty;
                    worksheet.Cells[oGeneralInfoModel.IndexOf(x)+1, 17].Value = !string.IsNullOrWhiteSpace(x.Comentaries.ToString()) ? x.Comentaries.ToShortDateString() + "|" : string.Empty; 


                    return true;
                });

                package.Save();

            }


            return true;
        }

        public static List<SanofiGeneralInfoModel> GetInfo_ByProvider(string vProviderPublicId) 
        {
           return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetInfo_ByProvider(vProviderPublicId);
        }

        public static List<SanofiComercialInfoModel> GetComercialInfo_ByProvider(string vProviderPublicId) 
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetComercialInfo_ByProvider(vProviderPublicId);
        }

        public static List<Models.SanofiContableInfoModel> GetContableInfo_ByProvider(string vProviderPublicId) 
        {
            return DAL.Controller.IntegrationPlatformSANOFIDataController.Instance.GetContableInfo_ByProvider(vProviderPublicId);
        }
    }
}
