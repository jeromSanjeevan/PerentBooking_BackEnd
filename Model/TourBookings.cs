namespace ParentBookingAPI.Model
{
    public class TourBookings
    {
        public int UniqID { get; set; }
        public int TourID { get; set; }
        public int SlotNumber { get; set; }
        public bool IsBooked { get; set; }
        public string? ParentName { get; set; }
        public string? Email { get; set; }
    }
}
