using ParentBookingAPI.Helper;
using ParentBookingAPI.Model.DTO;

namespace ParentBookingAPI.Repository.Interfaces
{
    public interface IEmailRepository
    {
        Task SendEmailAsync(SendEmailRequestDto sendEmailRequestDto);
    }
}

