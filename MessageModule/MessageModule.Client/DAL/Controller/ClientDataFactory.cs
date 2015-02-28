using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Client.DAL.Controller
{
    internal class ClientDataFactory
    {
        public MessageModule.Client.Interfaces.IClientData GetClientInstance()
        {
            Type typetoreturn = Type.GetType("MessageModule.Client.DAL.MySQLDAO.Client_MySqlDao,MessageModule.Client");
            MessageModule.Client.Interfaces.IClientData oRetorno = (MessageModule.Client.Interfaces.IClientData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
