﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.ThirdKnowledgeBatch.Models
{
    public class InternalSettings : System.Collections.DictionaryBase
    {
        private static InternalSettings oInstance;
        public static InternalSettings Instance
        {
            get
            {
                if (oInstance == null)
                    oInstance = new InternalSettings();
                return oInstance;
            }
        }

        public SettingsManager.Models.SettingModel this[string SettingName]
        {
            get { return SettingsManager.SettingsController.SettingsInstance.ModulesParams[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_SettingsModuleName][SettingName]; }
        }
        public int Count
        {
            get
            {
                return SettingsManager.SettingsController.SettingsInstance.ModulesParams[ProveedoresOnLine.ThirdKnowledge.Models.Constants.C_SettingsModuleName].Count;
            }
        }
    }
}
