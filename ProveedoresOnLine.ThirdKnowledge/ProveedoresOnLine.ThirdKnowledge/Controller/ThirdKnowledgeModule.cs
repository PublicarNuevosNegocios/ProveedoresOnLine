using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Controller
{
    public class ThirdKnowledgeModule
    {
        public static List<string[]> SimpleRequest(string IdentificationNumber, string Name)
        {
            try
            {
                WS_Consulting.Autenticacion oAuth = new WS_Consulting.Autenticacion();
                WS_Consulting.WSInspektorSoapClient oClient = new WS_Consulting.WSInspektorSoapClient();

                oAuth.UsuarioNombre = ProveedoresOnLine.ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_AuthServiceUser].Value;
                oAuth.UsuarioClave = ProveedoresOnLine.ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_AuthServicePass].Value;

                string oResutl = oClient.ConsultaInspektor(oAuth, IdentificationNumber, Name);

                string[] split = oResutl.Split('#');
                List<string[]> oReturn = new List<string[]>();
                if (split != null)
                {
                    split.All(x =>
                    {
                        oReturn.Add(x.Split('|'));
                        return true;
                    });
                }
                //oReturn = oReturn.Where(x => x.Contains("Prioridad:") == true).Select(x => x).ToList();

                return oReturn;
            }
            catch (Exception)
            {
                
                throw;
            }
           
        }
    }
}
