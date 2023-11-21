namespace ParentBookingAPI.Model.DTO
{
    public class LockRequestDto
    {
        public int TourID { get; set; }
        public int SlotNumber { get; set; }
        public bool IsLoked { get; set; }
    }
}
