using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace HotelBookingMVC.Models
{
    public class BookingRequest
    {
        [Required(ErrorMessage = "Please enter a value for this field.")]
        public int BranchId { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CheckinDate { get; set; }
        public DateTime CheckoutDate { get; set; }
        public string SelectedRooms { get; set; }
        public List<int> RoomIds { get; set; } // List<int> to store room IDs

        // Constructor or method to populate RoomIds from SelectedRooms
        public void ParseSelectedRooms()
        {
            if (!string.IsNullOrEmpty(SelectedRooms))
            {
                RoomIds = SelectedRooms
                    .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(id => int.TryParse(id.Trim(), out var roomId) ? roomId : (int?)null)
                    .Where(id => id.HasValue)
                    .Select(id => id.Value)
                    .ToList();
            }
            else
            {
                RoomIds = new List<int>();
            }
        }
    }
}