using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledge.Controller
{
    public class ThirdKnowledgeModule
    {
        public static void SimpleRequest(string IdentificationNumber, string Name)
        {
            WS_Consulting.Autenticacion oAuth = new WS_Consulting.Autenticacion();
            WS_Consulting.WSInspektorSoapClient oClient = new WS_Consulting.WSInspektorSoapClient();

            oAuth.UsuarioNombre = ProveedoresOnLine.ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_AuthServiceUser].Value;
            oAuth.UsuarioClave = ProveedoresOnLine.ThirdKnowledge.Models.InternalSettings.Instance[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_Settings_AuthServicePass].Value;

            string oResutl = oClient.ConsultaInspektor(oAuth, IdentificationNumber, Name);       
        }
    }
}
