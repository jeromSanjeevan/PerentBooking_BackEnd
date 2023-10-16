using Microsoft.Data.SqlClient;
using ParentBookingAPI.Helper;
using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;
using ParentBookingAPI.Repository.Interfaces;
using System.Data;

namespace ParentBookingAPI.Repository
{
    public class TourRepository : ITourRepository
    {

        private readonly DatabaseHelper _dbHelper;

        public TourRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }


        public async Task<List<TourResposeDto>> GetAllToursAsync()
        {
            SqlParameter[] parameters = null; // If you don't need any parameters

            List<TourResposeDto> tours = await _dbHelper.ExecuteStoredProcedureAsync(
                "SP_GetAllTours",
                parameters,
                MapTourResponseDto);

            return tours;
        }



        public async Task<TourResposeDto> GetTourByIdAsync(int id)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
        new SqlParameter("@TourID", SqlDbType.Int) { Value = id }
            };

            List<TourResposeDto> tours = await _dbHelper.ExecuteStoredProcedureAsync(
                "SP_GetTourByID",
                parameters,
                MapTourResponseDto);

            return tours.FirstOrDefault();
        }

        private TourResposeDto MapTourResponseDto(SqlDataReader reader)
        {
            TourResposeDto tour = new TourResposeDto
            {
                TourID = reader.GetInt32(reader.GetOrdinal("TourID")),
                Day = reader.GetString(reader.GetOrdinal("Day"))
            };

            return tour;
        }
    }
}
