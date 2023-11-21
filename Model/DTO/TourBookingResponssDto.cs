namespace ParentBookingAPI.Model.DTO
{
    public class TourBookingResponssDto_Parent
    {
        public int UniqID { get; set; }
        public int TourID { get; set; }
        public int SlotNumber { get; set; }
        public bool IsBooked { get; set; }
        public bool IsLoked { get; set; }


    }

    public class TourBookingResponssDto_Admin
    {
        public int UniqID { get; set; }
        public int TourID { get; set; }
        public int SlotNumber { get; set; }
        public bool IsBooked { get; set; }
        public string? ParentName { get; set; }
        public string? Email { get; set; }
        public string MobileNumber { get; set; }
    }
}
