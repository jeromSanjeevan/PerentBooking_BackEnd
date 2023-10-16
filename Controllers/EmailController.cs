using Microsoft.AspNetCore.Mvc;
using ParentBookingAPI.Helper;
using ParentBookingAPI.Model.DTO;
using ParentBookingAPI.Repository;
using ParentBookingAPI.Repository.Interfaces;

namespace ParentBookingAPI.Controllers
{
    public class EmailController : Controller
    {
        private readonly   IEmailRepository _emailRepository;
        public EmailController(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
        }


        [HttpPost("SendMail")]
        public async Task<IActionResult> SendMail([FromBody] SendEmailRequestDto sendEmailRequestDto)
        {
            try
            {
                // Now, mailRequest contains the data sent from the client
                await _emailRepository.SendEmailAsync(sendEmailRequestDto);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
