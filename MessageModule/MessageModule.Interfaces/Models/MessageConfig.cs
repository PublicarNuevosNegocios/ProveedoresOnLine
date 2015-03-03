using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageModule.Interfaces.Models
{
    public class MessageConfig
    {
        /// <summary>
        /// available agents
        /// </summary>
        public static List<string> Agents
        {
            get
            {
                if (oAgents == null)
                {
                    oAgents = MessageModule.Interfaces.General.InternalSettings.Instance
                        [MessageModule.Interfaces.General.Constants.C_Settings_Agent].Value.
                        Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                        Select(x => x.Replace(" ", "").ToLower()).
                        ToList();
                }
                return oAgents;
            }
        }
        private static List<string> oAgents;



    }
}
