using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;

namespace ParentBookingAPI.Repository.Interfaces
{
    public interface ILockSlot
    {
        Task<LockRequestDto> LockSlotAsync(LockRequestDto tourBooking);

        Task<LockRequestDto> ReleaseSlotAsync(LockRequestDto tourBooking);


        Task<LockRequestDto> LockStatusAsync(int tourid, int slotNumbr);

      
    }
}
