using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.DAL.Controller
{
    internal class MessageDataFactory
    {
        internal MessageModule.Interfaces.IMessageModuleData GetMessageInstance()
        {
            Type typetoreturn = Type.GetType("MessageModule.DAL.MySqlDao.MessageModule_MySqlDao,MessageModule.DAL");
            MessageModule.Interfaces.IMessageModuleData oRetorno = (MessageModule.Interfaces.IMessageModuleData)Activator.CreateInstance(typetoreturn);
            return oRetorno;
        }
    }
}
