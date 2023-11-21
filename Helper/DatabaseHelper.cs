using Microsoft.Data.SqlClient;
using System.Data;

namespace ParentBookingAPI.Helper
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;
        private int _id;
        public DatabaseHelper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void SetId(int id)
        {
            _id = id;
        }
        public async Task<List<T>> ExecuteStoredProcedureAsync<T>(string storedProcedureName, SqlParameter[] parameters, Func<SqlDataReader, T> map)
        {
            // Use the _id field as needed, e.g., in the SQL query or as a default parameter
            // ...

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(storedProcedureName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            // Use _id in your SQL query or as needed
            // cmd.Parameters.Add(new SqlParameter("@TourID", SqlDbType.Int) { Value = _id });

            await connection.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            List<T> result = new List<T>();

            while (await reader.ReadAsync())
            {
                T item = map(reader);
                result.Add(item);
            }

            await reader.CloseAsync();
            await connection.CloseAsync();

            return result;
        }

        public async Task<T> ExecuteStoredProcedureSingleAsync<T>(string storedProcedureName, SqlParameter[] parameters, Func<SqlDataReader, T> map)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand(storedProcedureName, connection);
            cmd.CommandType = CommandType.StoredProcedure;

            if (parameters != null)
            {
                cmd.Parameters.AddRange(parameters);
            }

            await connection.OpenAsync();
            SqlDataReader reader = await cmd.ExecuteReaderAsync();

            T result = default(T); // Default value for T, can be null for reference types

            if (await reader.ReadAsync())
            {
                result = map(reader);
            }

            await reader.CloseAsync();
            await connection.CloseAsync();

            return result;
        }


        public async Task<int> ExecuteUpdateStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    int rowsAffected = await command.ExecuteNonQueryAsync();
                    return rowsAffected;
                }
            }
        }

        public async Task<int> ExecuteInsertStoredProcedureAsync(string storedProcedureName, SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    // Execute the stored procedure and return the auto-generated UserId
                    return Convert.ToInt32(await command.ExecuteScalarAsync());
                }
            }
        }

    }
}
