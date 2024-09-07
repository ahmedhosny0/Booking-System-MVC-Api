namespace HotelManagement.Models
{
    public class RoomType
    {
        public int RoomTypeId { get; set; }
        public string TypeName { get; set; }
        public decimal BasePrice { get; set; }
        public ICollection<Room> Room { get; set; } // Navigation property to Bookings
    }
}
