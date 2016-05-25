using ProveedoresOnLine.CalificationProject.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProveedoresOnLine.CalificationProject.DAL.MySqlDAO
{
    internal class CalificationProject_MySqlDao : ICalificationProjectData
    {
        private ADO.Interfaces.IADO DataInstance;

        public CalificationProject_MySqlDao()
        {
            DataInstance = new ADO.MYSQL.MySqlImplement(ProveedoresOnLine.CalificationProject.Models.Constants.C_POL_CalificatioProjectConnectionName);
        }

        #region ConfigItem

        public int CalificationProjectConfigItemUpsert(int CalificationProjectConfigId, int CalificationProjectConfigItemId, string CalificationProjectConfigItemName, int CalificationProjectConfigItemType, bool Enable)
        {
            List<System.Data.IDbDataParameter> lstParams = new List<System.Data.IDbDataParameter>();

            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigId", CalificationProjectConfigId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemId", CalificationProjectConfigItemId));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemName", CalificationProjectConfigItemName));
            lstParams.Add(DataInstance.CreateTypedParameter("vCalificationProjectConfigItemType", CalificationProjectConfigItemType));
            lstParams.Add(DataInstance.CreateTypedParameter("vEnable", Enable == true ? 1 : 0));

            ADO.Models.ADOModelResponse response = DataInstance.ExecuteQuery(new ADO.Models.ADOModelRequest()
            {
                CommandExecutionType = ADO.Models.enumCommandExecutionType.Scalar,
                CommandText = "CC_CalificationProjectConfigItem_Upsert",
                CommandType = System.Data.CommandType.StoredProcedure,
                Parameters = lstParams,
            });

            return Convert.ToInt32(response.ScalarResult);
        }

        #endregion        
    }
}
