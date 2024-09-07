using HotelManagement.Models;

namespace HotelManagement.DTO
{
    public class BookingRequest
    {
        public int BranchId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public List<int> RoomIds { get; set; }
    }
}
