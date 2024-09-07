namespace HotelManagement.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public string RoomNumber { get; set; }
        public int RoomTypeId { get; set; }
        public int BranchId { get; set; }
        public RoomType RoomType { get; set; }
        public HotelBranch HotelBranch { get; set; }
        public ICollection<Booking> Bookings { get; set; } // Navigation property to Bookings
    }
}
