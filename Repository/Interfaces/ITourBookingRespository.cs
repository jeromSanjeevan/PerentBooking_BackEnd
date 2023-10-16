using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;

namespace ParentBookingAPI.Repository.Interfaces
{
    public interface ITourBookingRespository
    {
        Task<List<TourBookingResponssDto>> GetAllTourBookingsAsync(int id);

        Task<TourBookings> UpdateTourBookingAsync(TourBookings tourBooking);
    }
}
