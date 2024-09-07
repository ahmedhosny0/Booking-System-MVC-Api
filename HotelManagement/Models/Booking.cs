namespace HotelManagement.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int CustomerId { get; set; }
        public int RoomId { get; set; }
        public int BranchId { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public decimal TotalPrice { get; set; }

        public Customer Customer { get; set; }
        public Room Room { get; set; } // Navigation property to Room
        public HotelBranch Branch { get; set; } // Navigation property to Room
    }
}
