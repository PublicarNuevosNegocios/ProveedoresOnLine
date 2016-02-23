using ProveedoresOnLine.CompanyProvider.Models.Provider;
using ProveedoresOnLine.RestrictiveListVerificator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListVerificator.Controller
{
    public class RestrictiveListVerificator
    {
        public static byte[] GenerateXLSByStatus()
        {


            //Write the document
            StringBuilder data = new StringBuilder();
            string strSep = ";";

            List<ProviderModel> oProviderResult =
                ProveedoresOnLine.RestrictiveListVerificator.DAL.Controller.RestrictiveListVerificatorDataController.Instance.GetProviderByStatus(902001, "DA5C572E");
            #region Crete Excel
            /*
            oProviderResult.All(x =>
            {
                if (oProviderResult.IndexOf(x) == 0)
                {
                    data.AppendLine
                    ("\"" + "Tipo Identificacion" + "\"" + strSep +
                        "\"" + "Numero Identificacion" + "\"" + strSep +
                        "\"" + "Razon Social" + "\"" + strSep +
                        "\"" + "País" + "\"" + strSep +

                        "\"" + "Ciudad" + "\"" + strSep +
                        "\"" + "Estado" + "\"" + strSep +

                        "\"" + "Dirección" + "\"" + strSep +
                        "\"" + "Telefono" + "\"" + strSep +

                        "\"" + "Representante" + "\"");
                    data.AppendLine
                        ("\"" + x.RelatedProvider.IdentificationType.ItemName + "\"" + strSep +
                        "\"" + x.RelatedProvider.IdentificationNumber + "\"" + strSep +
                        "\"" + x.RelatedProvider.CompanyName + "\"" + "" + strSep +
                        "\"" + x.Country + "\"" + "" + strSep +
                        "\"" + x.City + "\"" + strSep +
                        "\"" + x.State + "\"" + "" + strSep +
                        "\"" + x.Address + "\"" + strSep +
                        "\"" + x.Telephone + "\"" + strSep +
                        "\"" + x.Representant + "\"");
                }
                else
                {
                    data.AppendLine
                        ("\"" + x.RelatedProvider.IdentificationType.ItemName + "\"" + strSep +
                        "\"" + x.RelatedProvider.IdentificationNumber + "\"" + strSep +
                        "\"" + x.RelatedProvider.CompanyName + "\"" + "" + strSep +
                        "\"" + x.Country + "\"" + "" + strSep +
                        "\"" + x.City + "\"" + strSep +
                        "\"" + x.State + "\"" + "" + strSep +
                        "\"" + x.Address + "\"" + strSep +
                        "\"" + x.Telephone + "\"" + strSep +
                        "\"" + x.Representant + "\"");
                }
                return true;
            });
            */
            byte[] buffer = Encoding.Default.GetBytes(data.ToString().ToCharArray());

            #endregion Crete Excel
            
            return buffer;
        }
    }
}
