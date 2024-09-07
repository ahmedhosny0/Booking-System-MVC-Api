namespace HotelManagement.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Booking> Bookings { get; set; } // Navigation property to Bookings

    }
}
