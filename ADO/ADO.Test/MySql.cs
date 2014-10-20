using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ADO.Test
{
    [TestClass]
    public class MySql
    {
        [TestMethod]
        public void DataTableTest()
        {
            ADO.MYSQL.MySqlImplement DataInstance = new MYSQL.MySqlImplement("Conn");

            var param = DataInstance.CreateTypedParameter();
            param.ParameterName = "vUserPublicId";
            param.Value = "asdfadf";

            ADO.Models.ADOModelRequest req = new Models.ADOModelRequest()
            {
                CommandText = "UI_GetUser",
                CommandType = System.Data.CommandType.StoredProcedure,
                CommandExecutionType = Models.enumCommandExecutionType.DataTable,
                Parameters = new System.Collections.Generic.List<System.Data.IDbDataParameter>(),
            };

            req.Parameters.Add(param);

            var result = DataInstance.ExecuteQuery(req);
        }
    }
}
