using Microsoft.Data.SqlClient;
using ParentBookingAPI.Model;

namespace ParentBookingAPI.Repository
{
    public class UserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

       
    }
}
