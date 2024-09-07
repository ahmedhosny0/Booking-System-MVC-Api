using Microsoft.EntityFrameworkCore;

namespace HotelManagement.Models
{
    public class HotelBranch
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Location { get; set; }
        public ICollection<Booking> Bookings { get; set; } // Navigation property to Bookings
        public ICollection<Room> Room { get; set; } // Navigation property to Room
    }
}
