using Microsoft.Data.SqlClient;
using ParentBookingAPI.Helper;
using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;
using ParentBookingAPI.Repository.Interfaces;
using System.Data;

namespace ParentBookingAPI.Repository
{
    public class TourBookingRepository : ITourBookingRespository
    {
        private readonly DatabaseHelper _dbHelper;

        public TourBookingRepository(DatabaseHelper dbHelper)
        {

            _dbHelper = dbHelper;
        }

        public async Task<List<TourBookingResponssDto_Parent>> GetAllTourBookings_ParentAsync(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@TourID", SqlDbType.Int) { Value = id }
            };

            List<TourBookingResponssDto_Parent> tourBookings = await _dbHelper.ExecuteStoredProcedureAsync(
                "SP_GetAllTourBookings",
                parameters,
                MapTourBookingResponseDto_Parent);

            return tourBookings;
        }
        private TourBookingResponssDto_Parent MapTourBookingResponseDto_Parent(SqlDataReader reader)
        {
            TourBookingResponssDto_Parent tourBooking = new TourBookingResponssDto_Parent();

            if (!reader.IsDBNull(reader.GetOrdinal("TourID")))
            {
                tourBooking.TourID = reader.GetInt32(reader.GetOrdinal("TourID"));
            }

            if (!reader.IsDBNull(reader.GetOrdinal("SlotNumber")))
            {
                tourBooking.SlotNumber = reader.GetInt32(reader.GetOrdinal("SlotNumber"));
            }

            if (!reader.IsDBNull(reader.GetOrdinal("IsBooked")))
            {
                tourBooking.IsBooked = reader.GetBoolean(reader.GetOrdinal("IsBooked"));
            }

            if (!reader.IsDBNull(reader.GetOrdinal("IsLocked")))
            {
                tourBooking.IsLoked = reader.GetBoolean(reader.GetOrdinal("IsLocked"));
            }

            return tourBooking;
        }

        public async Task<List<TourBookingResponssDto_Admin>> GetAllTourBookings_AdminAsync(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@TourID", SqlDbType.Int) { Value = id }
            };

            List<TourBookingResponssDto_Admin> tourBookings = await _dbHelper.ExecuteStoredProcedureAsync(
                "SP_GetAllTourBookings",
                parameters,
                MapTourBookingResponseDto_Admin);

            return tourBookings;
        }      

        private TourBookingResponssDto_Admin MapTourBookingResponseDto_Admin(SqlDataReader reader)
        {
            TourBookingResponssDto_Admin tourBooking = new TourBookingResponssDto_Admin();

            if (!reader.IsDBNull(reader.GetOrdinal("TourID")))
            {
                tourBooking.TourID = reader.GetInt32(reader.GetOrdinal("TourID"));
            }

            if (!reader.IsDBNull(reader.GetOrdinal("SlotNumber")))
            {
                tourBooking.SlotNumber = reader.GetInt32(reader.GetOrdinal("SlotNumber"));
            }

            if (!reader.IsDBNull(reader.GetOrdinal("IsBooked")))
            {
                tourBooking.IsBooked = reader.GetBoolean(reader.GetOrdinal("IsBooked"));
            }

            if (!reader.IsDBNull(reader.GetOrdinal("ParentName")))
            {
                tourBooking.ParentName = reader.GetString(reader.GetOrdinal("ParentName"));
            }

            if (!reader.IsDBNull(reader.GetOrdinal("Email")))
            {
                tourBooking.Email = reader.GetString(reader.GetOrdinal("Email"));
            }


            if (!reader.IsDBNull(reader.GetOrdinal("MobileNumber")))
            {
                tourBooking.MobileNumber = reader.GetString(reader.GetOrdinal("MobileNumber"));
            }

            return tourBooking;
        }


        //public async Task<TourBookings> AddTourBookingAsync(TourBookings tourBooking)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        using (var command = new SqlCommand("YourStoredProcedure", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.Add(new SqlParameter("@TourID", tourBooking.TourID));
        //            command.Parameters.Add(new SqlParameter("@SlotNumber", tourBooking.SlotNumber));
        //            command.Parameters.Add(new SqlParameter("@ParentName", tourBooking.ParentName));
        //            command.Parameters.Add(new SqlParameter("@Email", tourBooking.Email));

        //            var result = await command.ExecuteNonQueryAsync();
        //            if (result == 1)
        //            {
        //                return tourBooking;
        //            }
        //            else
        //            {
        //                throw new Exception("Failed to create TourBooking.");
        //            }
        //        }
        //    }

        //}

        //public async Task<TourBookings> UpdateTourBookingAsync(TourBookings tourBooking)
        //{
        //    using (var connection = new SqlConnection(_connectionString))
        //    {
        //        await connection.OpenAsync();
        //        using (var command = new SqlCommand("SP_UpdateTourBookinSlot", connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;

        //            command.Parameters.Add(new SqlParameter("@TourID", tourBooking.TourID));
        //            command.Parameters.Add(new SqlParameter("@SlotNumber", tourBooking.SlotNumber));
        //            command.Parameters.Add(new SqlParameter("@ParentName", tourBooking.ParentName));
        //            command.Parameters.Add(new SqlParameter("@Email", tourBooking.Email));

        //            var result = await command.ExecuteNonQueryAsync();
        //            if (result == 1)
        //            {
        //                return tourBooking;
        //            }
        //            else
        //            {
        //                throw new Exception("Failed to update TourBooking.");
        //            }
        //        }
        //    }
        //}


        public async Task<TourBookings> UpdateTourBookingAsync(TourBookings tourBooking)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@TourID", tourBooking.TourID),
                 new SqlParameter("@SlotNumber", tourBooking.SlotNumber),
                 new SqlParameter("@ParentName", tourBooking.ParentName),
                 new SqlParameter("@Email", tourBooking.Email),
                   new SqlParameter("@MobileNumber", tourBooking.MobileNumber)
            };

            int rowsAffected = await _dbHelper.ExecuteUpdateStoredProcedureAsync("SP_UpdateTourBookinSlot", parameters);

            if (rowsAffected == 1)
            {
                return tourBooking;
            }
            else
            {
                throw new Exception("Failed to update TourBooking.");
            }
        }

    }


}
