using ADO.Interfaces;
using ADO.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADO.MYSQL
{
    public class MySqlImplement : IADO
    {

        public string CurrentConnectionString { get; private set; }

        public MySqlImplement()
        {
        }

        public MySqlImplement(string ConnectionName)
        {
            CurrentConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[ConnectionName].ConnectionString;
        }

        public void SetConnection(string ConnectionString)
        {
            CurrentConnectionString = ConnectionString;
        }

        public ADOModelResponse ExecuteQuery(ADOModelRequest QueryParams)
        {
            ADOModelResponse oRetorno = new ADOModelResponse();

            //get connection context
            MySql.Data.MySqlClient.MySqlConnection Conn = new MySql.Data.MySqlClient.MySqlConnection(CurrentConnectionString);

            try
            {
                //create mysql command
                MySql.Data.MySqlClient.MySqlCommand CurrentCommand = new MySql.Data.MySqlClient.MySqlCommand();

                CurrentCommand.Connection = Conn;
                CurrentCommand.CommandText = QueryParams.CommandText;

                if (QueryParams.Parameters != null && QueryParams.Parameters.Count > 0)
                {
                    QueryParams.Parameters.All(param =>
                    {
                        CurrentCommand.Parameters.Add((MySql.Data.MySqlClient.MySqlParameter)param);
                        return true;
                    });
                }

                CurrentCommand.CommandType = QueryParams.CommandType;

                //validate method to execute
                switch (QueryParams.CommandExecutionType)
                {
                    case enumCommandExecutionType.NonQuery:
                        using (CurrentCommand.Connection)
                        {
                            CurrentCommand.Connection.Open();
                            oRetorno.NonQueryResult = CurrentCommand.ExecuteNonQuery();
                            CurrentCommand.Connection.Close();
                        }
                        break;
                    case enumCommandExecutionType.Scalar:
                        using (CurrentCommand.Connection)
                        {
                            CurrentCommand.Connection.Open();
                            oRetorno.ScalarResult = CurrentCommand.ExecuteScalar();
                            CurrentCommand.Connection.Close();
                        }
                        break;
                    case enumCommandExecutionType.DataTable:

                        oRetorno.DataTableResult = new DataTable();
                        using (MySql.Data.MySqlClient.MySqlDataAdapter dat = new MySql.Data.MySqlClient.MySqlDataAdapter(CurrentCommand))
                        {
                            oRetorno.NonQueryResult = dat.Fill(oRetorno.DataTableResult);
                        }

                        break;
                    case enumCommandExecutionType.DataSet:

                        oRetorno.DataSetResult = new DataSet();
                        using (MySql.Data.MySqlClient.MySqlDataAdapter dads = new MySql.Data.MySqlClient.MySqlDataAdapter(CurrentCommand))
                        {
                            oRetorno.NonQueryResult = dads.Fill(oRetorno.DataSetResult);
                        }

                        break;
                    default:
                        break;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (Conn.State == ConnectionState.Open)
                    Conn.Close();
            }

            return oRetorno;
        }

        public IDbDataParameter CreateTypedParameter()
        {
            return new MySql.Data.MySqlClient.MySqlParameter();
        }

        public IDbDataParameter CreateTypedParameter(string ParameterName, object ParameterValue)
        {
            return new MySql.Data.MySqlClient.MySqlParameter(ParameterName, ParameterValue);
        }
    }
}
