using System;

using System.Data.Common;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Domain.Context
{
    abstract public class StoreContext
    {
        public readonly string _connection;

        public StoreContext(string connection)
        {
            _connection = connection;
        }

        protected MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connection);
        }

        public async Task<string> GetSerializedData(MySqlConnection mySqlConnection, MySqlCommand mySqlCommand, Func<DbDataReader, Task<string>> ReadResponse)
        {
            using (mySqlConnection)
            {
                await mySqlConnection.OpenAsync();
                mySqlCommand.Connection = mySqlConnection;

                using (var reader = await mySqlCommand.ExecuteReaderAsync())
                {
                    if (reader.HasRows)
                    {
                        return await ReadResponse(reader);
                    }

                    return null;
                }


            }
        }

        public async Task<bool> ExecuteCommandNonQueryAsync(MySqlConnection mySqlConnection, MySqlCommand mySqlCommand)
        {
            using (mySqlConnection)
            {
                await mySqlConnection.OpenAsync();
                mySqlCommand.Connection = mySqlConnection;

                if (await mySqlCommand.ExecuteNonQueryAsync() != 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


    }
}
