using Microsoft.AspNetCore.Mvc;
using ParentBookingAPI.Model;
using ParentBookingAPI.Model.DTO;
using ParentBookingAPI.Repository;
using ParentBookingAPI.Repository.Interfaces;

namespace ParentBookingAPI.Controllers
{
    [Route("api/LockHandling")]
    [ApiController]
    public class LockController : Controller
    {
        private readonly ILockSlot _lockSlot;
        public LockController(IConfiguration configuration, ILockSlot lockSlot)
        {
            _lockSlot = lockSlot;
        }

        [HttpPost("lockSlot/{tourID}/{slotNumber}")]
        public async Task<IActionResult> LockSlot(int tourID, int slotNumber, LockRequestDto lockRequest)
        {
            try
            {
                lockRequest.TourID = tourID;
                lockRequest.SlotNumber = slotNumber;

                var lockBookingSlot = await _lockSlot.LockSlotAsync(lockRequest);

                if (lockBookingSlot != null)
                {
                    return Ok(lockBookingSlot);
                }

                return NotFound(); // Return 404 Not Found when the booking doesn't exist.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("releaseSlot/{tourID}/{slotNumber}")]
        public async Task<IActionResult> ReleaseSlot(int tourID, int slotNumber, LockRequestDto lockRequest)
        {
            try
            {
                lockRequest.TourID = tourID;
                lockRequest.SlotNumber = slotNumber;

                var releaseBookingSlot = await _lockSlot.ReleaseSlotAsync(lockRequest);

                if (releaseBookingSlot != null)
                {
                    return Ok(releaseBookingSlot);
                }

                return NotFound(); // Return 404 Not Found when the booking doesn't exist.
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        //[HttpGet("lockStatus/{id}/{slotNumber}")]

        [HttpGet("lockStatus/{tourid}/{slotNumbr}")]
        public async Task<IActionResult> LockStatus(int tourid, int slotNumbr)
        {
            try
            {
                var lockStatus = await _lockSlot.LockStatusAsync(tourid, slotNumbr);

                if (lockStatus != null)
                {
                    // If a lock status is found, return it as JSON
                    return Ok(lockStatus);
                }

                // Handle the case where no lock status is found
                return NotFound();
            }
            catch (Exception ex)
            {
                // Handle exceptions
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }



    }
}
