using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MessageModule.Interfaces.Models
{
    public class MessageConfig
    {
        /// <summary>
        /// available agents
        /// </summary>
        public static List<string> Agent
        {
            get
            {
                if (oAgent == null)
                {
                    oAgent = MessageModule.Interfaces.General.InternalSettings.Instance
                        [MessageModule.Interfaces.General.Constants.C_Settings_Agent].Value.
                        Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries).
                        Select(x => x.Replace(" ", "")).
                        ToList();
                }
                return oAgent;
            }
        }
        private static List<string> oAgent;

        /// <summary>
        /// available agents config
        /// </summary>
        public static Dictionary<string, Dictionary<string, string>> AgentConfig
        {
            get
            {
                if (oAgentConfig == null)
                {
                    oAgentConfig = new Dictionary<string, Dictionary<string, string>>();
                    Agent.All(ag =>
                    {
                        if (!oAgentConfig.ContainsKey(ag))
                        {
                            Dictionary<string, string> oConfigParams = new Dictionary<string, string>();

                            string strConfig = MessageModule.Interfaces.General.InternalSettings.Instance
                                [MessageModule.Interfaces.General.Constants.C_Settings_AgentConfig.
                                Replace("{AgentName}", ag)].
                                Value;
                            strConfig = System.Web.HttpUtility.HtmlDecode(strConfig);
                            //load config agent xml
                            XDocument xDoc = XDocument.Parse(strConfig);

                            xDoc.Descendants(ag).Descendants("key").All(NodeConfig =>
                            {
                                if (NodeConfig.Attribute("name") != null &&
                                    !string.IsNullOrEmpty(NodeConfig.Attribute("name").Value) &&
                                    !oConfigParams.ContainsKey(NodeConfig.Attribute("name").Value))
                                {
                                    oConfigParams[NodeConfig.Attribute("name").Value] = NodeConfig.Value;
                                }
                                return true;
                            });
                            oAgentConfig[ag] = oConfigParams;
                        }
                        return true;
                    });
                }
                return oAgentConfig;
            }
        }
        private static Dictionary<string, Dictionary<string, string>> oAgentConfig;
    }
}
