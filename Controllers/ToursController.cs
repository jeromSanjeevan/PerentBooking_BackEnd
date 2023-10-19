using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParentBookingAPI.Model;
using ParentBookingAPI.Repository;
using ParentBookingAPI.Repository.Interfaces;

namespace ParentBookingAPI.Controllers
{
    
    [Route("api/Tour")]
    public class ToursController : Controller
    {
        private readonly ITourRepository _tourRepository;
        public ToursController(ITourRepository tourRepository)
        {

            _tourRepository = tourRepository;
        }

        [HttpGet("GetAllTours_ParentView")]
        public async Task<IActionResult> GetAllBookedSlots_Parent()
        {
            try
            {
                var tourResponse = await _tourRepository.GetAllToursAsync();
                if (tourResponse == null || tourResponse.Count == 0)
                {
                    return NotFound();
                }

                return Ok(tourResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize(AuthenticationSchemes = "Token1Scheme", Roles = "Super_Admin, Admin, Manager")]
        [HttpGet("GetAllTours_AdminView")]
        public async Task<IActionResult> GetAllBookedSlots_Admin()
        {
            try
            {

                var tourResponse = await _tourRepository.GetAllToursAsync();
                if (tourResponse == null || tourResponse.Count == 0)
                {
                    return NotFound();
                }

                return Ok(tourResponse);

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //This is for Heading Purpose in UI
        [HttpGet("GetTourById/{id}")]
        public async Task<IActionResult> GetTourById(int id)
        {
            try
            {
                var tour = await _tourRepository.GetTourByIdAsync(id);

                if (tour != null)
                {
                    return Ok(tour);
                }

                return NotFound(); // Return 404 Not Found when no matching record is found.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
