using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;

namespace ParentBookingAPI.Repository.Interfaces
{
    public interface ITourRepository
    {
        Task<List<TourResposeDto>> GetAllToursAsync();

        Task<TourResposeDto> GetTourByIdAsync(int id);
    }
}
