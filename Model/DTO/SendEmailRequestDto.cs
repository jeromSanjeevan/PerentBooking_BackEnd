namespace ParentBookingAPI.Model.DTO
{
    public class SendEmailRequestDto
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
