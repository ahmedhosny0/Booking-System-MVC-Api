using HotelManagement.Models;

namespace HotelManagement.DTO
{
    public class AddBookingDTO
    {
        public string BookingName { get; set; }
        public int CustomerId { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }

        public Customer Customer { get; set; }
    }
}
