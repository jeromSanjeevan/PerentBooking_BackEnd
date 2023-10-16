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

        public async Task<List<TourBookingResponssDto>> GetAllTourBookingsAsync(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
            new SqlParameter("@TourID", SqlDbType.Int) { Value = id }
            };

            List<TourBookingResponssDto> tourBookings = await _dbHelper.ExecuteStoredProcedureAsync(
                "SP_GetAllTourBookings",
                parameters,
                MapTourBookingResponseDto);

            return tourBookings;
        }

        private TourBookingResponssDto MapTourBookingResponseDto(SqlDataReader reader)
        {
            TourBookingResponssDto tourBooking = new TourBookingResponssDto();

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
                 new SqlParameter("@Email", tourBooking.Email)
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
