using Microsoft.Data.SqlClient;
using ParentBookingAPI.Helper;
using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;
using ParentBookingAPI.Repository.Interfaces;
using System.Data;

namespace ParentBookingAPI.Repository
{
    public class LockSlot : ILockSlot
    {
        private readonly DatabaseHelper _dbHelper;
        public LockSlot(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }
        public async Task<LockRequestDto> LockSlotAsync(LockRequestDto tourBooking)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@TourID", tourBooking.TourID),
                 new SqlParameter("@SlotNumber", tourBooking.SlotNumber),


            };

            int rowsAffected = await _dbHelper.ExecuteUpdateStoredProcedureAsync("SP_LockSlot", parameters);

            if (rowsAffected == 1)
            {
                return tourBooking;
            }
            else
            {
                throw new Exception("Failed to Lock The Slot");
            }
        }

        public  async Task<LockRequestDto> LockStatusAsync(int tourid, int slotNumbr)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@TourID", SqlDbType.Int) { Value = tourid },
                 new SqlParameter("@SlotNumber", SqlDbType.Int) { Value = slotNumbr }
            };

            LockRequestDto lockStatus = await _dbHelper.ExecuteStoredProcedureSingleAsync(
                "SP_CheckLockStatus",
                parameters,
                MapLockStatus);

            return lockStatus;
        }

        private LockRequestDto MapLockStatus(SqlDataReader reader)
        {
            LockRequestDto lockStaus = new LockRequestDto();

          
            if (!reader.IsDBNull(reader.GetOrdinal("IsLocked")))
            {
                lockStaus.IsLoked = reader.GetBoolean(reader.GetOrdinal("IsLocked"));
            }

            return lockStaus;
        }


        public async Task<LockRequestDto> ReleaseSlotAsync(LockRequestDto tourBooking)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                 new SqlParameter("@TourID", tourBooking.TourID),
                 new SqlParameter("@SlotNumber", tourBooking.SlotNumber),


            };

            int rowsAffected = await _dbHelper.ExecuteUpdateStoredProcedureAsync("SP_ReleaseSlot", parameters);

            if (rowsAffected == 1)
            {
                return tourBooking;
            }
            else
            {
                throw new Exception("Failed to Release The Slot");
            }
        }
    }
}
