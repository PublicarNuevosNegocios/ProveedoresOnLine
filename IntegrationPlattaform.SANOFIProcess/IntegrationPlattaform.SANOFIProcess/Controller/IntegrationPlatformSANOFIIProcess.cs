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

            //Write Document
            StringBuilder Header = new StringBuilder();
            StringBuilder data = new StringBuilder();
            string strSep = "|";

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

                data.Append(
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
                 string.Empty + strSep +
                 x.Fax + strSep +
                 x.Email_OC + strSep +
                 x.Email_P + strSep +
                 x.Email_Cert + strSep +
                 x.Comentaries.ToShortDateString() + strSep);

                return true;
            });

            Header.AppendLine
                   ("\"" + "NOMBRE1" + "\"" + strSep +
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
                   "COMENTARIOS" + strSep + data);

            byte[] buffer = Encoding.Default.GetBytes(Header.ToString().ToCharArray());
            File.WriteAllBytes(strFolder + fileName, buffer);

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
