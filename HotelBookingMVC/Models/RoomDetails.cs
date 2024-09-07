namespace HotelBookingMVC.Models
{
    public class RoomDetail
    {
        public int BookingId { get; set; }
        public string CheckinDate { get; set; }
        public string CheckoutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public string TypeName { get; set; }
        public decimal BasePrice { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Location { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string RoomStatus { get; set; } // "Available" or "Booked" based on the condition

    }

}
