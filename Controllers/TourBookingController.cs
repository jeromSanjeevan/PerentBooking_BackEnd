using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ParentBookingAPI.Helper;
using ParentBookingAPI.Model;
using ParentBookingAPI.Repository;
using ParentBookingAPI.Repository.Interfaces; 
using ParentBookingAPI.Service;
using System.Data;

namespace ParentBookingAPI.Controllers
{
  
    [Route("api/TourBooking")]
    [ApiController]
    public class TourBookingController : Controller
    {
        private readonly ITourBookingRespository _tourBookingrepository;

        public TourBookingController(IConfiguration configuration, ITourBookingRespository tourBookingRespository)
        {
            _tourBookingrepository = tourBookingRespository;
        }

   
        [HttpGet("GetAllTourBookings_ParentView")]
        public async Task<IActionResult> GetAllBookedSlots_ParentView(int id)
        {
            try
            {
                var tourBookingResponsses = await _tourBookingrepository.GetAllTourBookings_ParentAsync(id);

                if (tourBookingResponsses == null || tourBookingResponsses.Count == 0)
                {
                    return NotFound();
                }

                return Ok(tourBookingResponsses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = "Token1Scheme", Roles = "Super_Admin, Admin, Manager")]
        [HttpGet("GetAllTourBookings_AdminView")]
        public async Task<IActionResult> GetAllBookedSlots_AdmintView(int id)
        {
            try
            {
                var tourBookingResponsses = await _tourBookingrepository.GetAllTourBookings_AdminAsync(id);

                if (tourBookingResponsses == null || tourBookingResponsses.Count == 0)
                {
                    return NotFound();                  
                }

                return Ok(tourBookingResponsses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> CreateTourBooking(TourBookings tourBooking)
        //{
        //    try
        //    {
        //        var addedBooking = await _tourBookingRepository.AddTourBookingAsync(tourBooking);
        //        return CreatedAtAction("GetTourBooking", new { id = addedBooking.UniqID }, addedBooking);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}


        [HttpPost("update/{tourID}/{slotNumber}")]
        public async Task<IActionResult> UpdateTourBooking(int tourID, int slotNumber, TourBookings tourBooking)
        {
            try
            {
                tourBooking.TourID = tourID;
                tourBooking.SlotNumber = slotNumber;

                var updatedBooking = await _tourBookingrepository.UpdateTourBookingAsync(tourBooking);

                if (updatedBooking != null)
                {
                    return Ok(updatedBooking);
                }

                return NotFound(); // Return 404 Not Found when the booking doesn't exist.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
