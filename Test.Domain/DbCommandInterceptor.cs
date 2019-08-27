using Microsoft.EntityFrameworkCore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace Test.Domain
{
    public class DbCommandInterceptor : IObserver<KeyValuePair<string, object>>
    {
        private const string masterConnectionString = "data source=WANGPENG;User Id=sa;Pwd=sa123;initial catalog=Demo1;integrated security=True;";
        private const string slaveConnectionString = "data source=WANGPENG;User Id=sa;Pwd=sa123;initial catalog=Demo2;integrated security=True;";
        public void OnCompleted()
        {
        }

        public void OnError(Exception error)
        {
        }

        public void OnNext(KeyValuePair<string, object> value)
        {
            if (value.Key == RelationalEventId.CommandExecuting.Name)
            {
                var command = ((CommandEventData)value.Value).Command;
                var executeMethod = ((CommandEventData)value.Value).ExecuteMethod;

                if (executeMethod == DbCommandMethod.ExecuteNonQuery)
                {
                    ResetConnection(command, masterConnectionString);
                }
                else if (executeMethod == DbCommandMethod.ExecuteScalar)
                {
                    ResetConnection(command, slaveConnectionString);
                }
                else if (executeMethod == DbCommandMethod.ExecuteReader)
                {
                    ResetConnection(command, slaveConnectionString);
                }
            }
        }

        void ResetConnection(DbCommand command, string connectionString)
        {
            if (command.Connection.State == ConnectionState.Open)
            {
                if (!command.CommandText.Contains("@@ROWCOUNT"))
                {
                    command.Connection.Close();
                    command.Connection.ConnectionString = connectionString;
                }
            }
            if (command.Connection.State == ConnectionState.Closed)
            {
                command.Connection.Open();
            }
        }
    }
}
