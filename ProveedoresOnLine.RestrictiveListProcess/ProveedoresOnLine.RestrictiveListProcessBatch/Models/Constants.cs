﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.RestrictiveListProcessBatch.Models
{
    public class Constants
    {
        #region AppSettings

        public const string C_AppSettings_LogFile = "LogFile";


        public const string C_SettingsModuleName = "RestrictiveListProcess";

        //Collumns Name
        public const string C_TK_CP_ColPersonType = "TK_CP_ColPersonType";
        public const string C_TK_CP_ColIdNumber = "TK_CP_ColIdNumber";
        public const string C_TK_CP_ColIdName = "TK_CP_ColIdName";

        #endregion

        #region Internal Settings

        public const string C_SettingsModuleNameBatch = "RestrictiveListProcessBatch";

        public const string C_Settings_PublicarPublicId = "PublicarPublicId";

        public const string C_Settings_File_TempDirectory = "TK_CP_File_TempDirectory";

        public const string C_Settings_File_ExcelDirectory = "TK_CP_File_ExcelDirectory";

        #endregion
    }
}
