using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;

namespace ParentBookingAPI.Repository.Interfaces
{
    public interface ITourBookingRespository
    {
        Task<List<TourBookingResponssDto_Parent>> GetAllTourBookings_ParentAsync(int id);
        Task<List<TourBookingResponssDto_Admin>> GetAllTourBookings_AdminAsync(int id);
        Task<TourBookings> UpdateTourBookingAsync(TourBookings tourBooking);
    }
}
