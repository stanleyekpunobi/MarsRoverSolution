using Dapper;
using MarsRover.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarsRover.Core.Classes
{
    public class DataAccessClass : IDataInterface
    {
        public async Task<T> GetDataAsync<T, U>(string sp, U parameters, string constring)
        {
            using (IDbConnection connection = new SqlConnection(constring))
            {
                var data = await connection.QueryFirstAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public async Task<IEnumerable<T>> GetDataListAsync<T, U>(string sp, U parameters, string constring)
        {
            using (IDbConnection connection = new SqlConnection(constring))
            {
                var data = await connection.QueryAsync<T>(sp, parameters, commandType: CommandType.StoredProcedure);

                return data;
            }
        }

        public async Task<int> SaveDataAsync(string sp, object parameters, string constring)
        {
            var sqlParameters = new DynamicParameters(parameters);

            sqlParameters.Add("RETURN_VALUE", dbType: DbType.String, direction: ParameterDirection.Output, size: 5215585);

            using (IDbConnection connection = new SqlConnection(constring))
            {
                var data = await connection.ExecuteAsync(sp, sqlParameters, commandType: CommandType.StoredProcedure);

                return Convert.ToInt32(sqlParameters.Get<string>("@RETURN_VALUE"));
            }
        }
    }
}
