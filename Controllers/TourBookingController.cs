﻿using Microsoft.AspNetCore.Authorization;
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

        [Authorize(AuthenticationSchemes = "Token1Scheme", Roles = "Admin")]

        //[Authorize(AuthenticationSchemes = "Token1Scheme")]
        [HttpGet("GetAllTourBookings")]
        public async Task<IActionResult> GetAllBookedSlots(int id)
        {
            try
            {
                var tourBookingResponsses = await _tourBookingrepository.GetAllTourBookingsAsync(id);

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


        [HttpPut("update/{tourID}/{slotNumber}")]
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