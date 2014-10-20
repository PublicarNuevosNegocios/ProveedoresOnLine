using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SettingsManager.Models
{
    public class ModuleModel : System.Collections.DictionaryBase
    {
        //property to get like [ModuleName][SettingsName]
        public SettingModel this[string SettingName]
        {
            get { return RelatedSettings[SettingName]; }
        }
        public int Count
        {
            get
            {
                return RelatedSettings.Count;
            }
        }

        public string Name { get; set; }
        public string Location { get; set; }
        public Dictionary<string, SettingModel> RelatedSettings { get; set; }
    }
}
