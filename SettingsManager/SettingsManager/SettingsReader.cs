using SettingsManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SettingsManager
{
    internal class SettingsReader
    {
        private string SettingsConfigLocation { get; set; }

        private XDocument oxDoc;
        private XDocument xDoc
        {
            get
            {
                if (oxDoc == null)
                {
                    oxDoc = XDocument.Load(SettingsConfigLocation);
                }
                return oxDoc;
            }
        }

        internal SettingsReader(string oSettingsConfigLocation)
        {
            SettingsConfigLocation = oSettingsConfigLocation;
        }

        //read all modules
        internal Dictionary<string, ModuleModel> LoadAll()
        {
            Dictionary<string, ModuleModel> oRetorno = new Dictionary<string, ModuleModel>();

            var xxxxx = xDoc.Descendants("SettingsConfig").Descendants("Module");

            xDoc.Descendants("SettingsConfig").Descendants("Module").All(m =>
                {
                    string ModuleName = m.Attribute("name").Value;

                    if (!oRetorno.Any(x => x.Key == ModuleName) &&
                        !string.IsNullOrEmpty(ModuleName))
                    {
                        ModuleModel oReadingModule = LoadModule(ModuleName);

                        oRetorno.Add(ModuleName, oReadingModule);
                    }

                    return true;
                });

            return oRetorno;
        }

        //read one module
        internal ModuleModel LoadModule(string ModuleName)
        {
            ModuleModel oRetorno = new ModuleModel();

            oRetorno.Name = ModuleName;

            //get module location file
            oRetorno.Location = xDoc.Descendants("SettingsConfig").Descendants("Module")
                        .Where(x => x.Attribute("name").Value == oRetorno.Name)
                        .Descendants("key")
                        .Where(x => x.Attribute("name").Value.Trim().ToLower() == "location")
                        .FirstOrDefault().Value;


            //load module config
            XDocument oModuleConfig = XDocument.Load(oRetorno.Location);

            oRetorno.RelatedSettings = new Dictionary<string, SettingModel>();

            //read module config
            oModuleConfig.Descendants(ModuleName).Descendants("key").All(NodeConfig =>
            {
                if (!oRetorno.RelatedSettings.Any(x => x.Key == NodeConfig.Attribute("name").Value))
                {
                    SettingModel CurrentSetting = new SettingModel()
                    {
                        Key = NodeConfig.Attribute("name").Value,
                        Value = NodeConfig.Value,
                    };

                    oRetorno.RelatedSettings.Add(CurrentSetting.Key, CurrentSetting);
                }
                return true;
            });

            return oRetorno;
        }
    }
}
