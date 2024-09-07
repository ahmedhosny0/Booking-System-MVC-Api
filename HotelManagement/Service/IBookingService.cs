using HotelManagement.DTO;
using HotelManagement.Models;

namespace HotelManagement.Service
{
    public interface IBookingService
    {
        Task<bool> CheckAvailability(int branchId, DateTime startDate, DateTime endDate);
        Task<bool> BookRoom(BookingRequest request);
        Task<bool> CancelBooking(RoomDetail b);
        Task<List<Room>> GetRooms(int branchId);
        Task<List<HotelBranch>> GetBranches();
        Task<List<RoomsDTO>> GetAvailableRoomsByBranchAsync(int branchId);
        Task<List<RoomDetail>> GetRoomsStatus();
        Task<List<RoomDetail>> GetRoomsnew();
        Task<bool> DefineRoomType(RoomTypeDTO roomTypeDTO);
        Task<bool> DefineCustomer(AddCustomerDTO CustDTO);
        Task<bool> Login(UsersDTO usersDTO);
    }
}
