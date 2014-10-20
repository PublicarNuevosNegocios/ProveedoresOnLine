using ADO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.Interfaces
{
    public interface IADO
    {
        string CurrentConnectionString { get; }

        void SetConnection(string ConnectionString);

        ADOModelResponse ExecuteQuery(ADOModelRequest QueryParams);

        System.Data.IDbDataParameter CreateTypedParameter();

        System.Data.IDbDataParameter CreateTypedParameter(string ParameterName, object ParameterValue);
    }
}
