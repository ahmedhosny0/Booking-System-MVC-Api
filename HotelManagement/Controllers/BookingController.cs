using HotelManagement.DTO;
using HotelManagement.Models;
using HotelManagement.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("availability")]
        public async Task<IActionResult> CheckAvailability(int branchId, DateTime startDate, DateTime endDate)
        {
            var available = await _bookingService.CheckAvailability(branchId, startDate, endDate);
            return Ok(available);
        }
        [HttpPost("Addbooking")]
        public async Task<ActionResult<bool>> AddBooking([FromBody] AddBookingDTO bookingDto)
        {
            return await AddBooking(bookingDto);
        }

        [HttpGet("Getbooking")]
        public async Task<ActionResult<List<AddBookingDTO>>> GetBookings()
        {
            return await GetBookings();
        }
        [HttpPost("defineroomtype")]
        public async Task<IActionResult> DefineRoomType(RoomTypeDTO roomTypeDTO)
        {
            var result = await _bookingService.DefineRoomType(roomTypeDTO);
            return Ok(result);
        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(UsersDTO usersDTO)
        {
            var result = await _bookingService.Login(usersDTO);
            return Ok(result);
        }
        [HttpPost("defineCustomer")]
        public async Task<IActionResult> DefineCustomer(AddCustomerDTO Cust)
        {
            var result = await _bookingService.DefineCustomer(Cust);
            return Ok(result);
        }
        [HttpPost("book")]
        public async Task<IActionResult> BookRoom(BookingRequest request)
        {
            var booking = await _bookingService.BookRoom(request);
            return Ok(booking);
        }

        [HttpPost("cancelbooking")]
        public async Task<IActionResult> CancelBooking(RoomDetail b)
        {
            await _bookingService.CancelBooking(b);
            return Ok(b);
        }
        
        [HttpGet("GetRoomsStatus")]
        public async Task<IActionResult> GetRoomsStatusAsync()
        {
            var RoomStatus = await _bookingService.GetRoomsStatus();
            return Ok(RoomStatus);
        }
        [HttpGet("GetRoomsnew")]
        public async Task<IActionResult> GetRoomsnewAsync()
        {
            var branches = await _bookingService.GetRoomsnew();
            return Ok(branches);
        }
        [HttpGet("GetBranches")]
        public async Task<IActionResult> GetBranchesAsync()
        {
            var branches = await _bookingService.GetBranches();
            return Ok(branches);
        }
        [HttpGet("GetAvailableRoomsByBranch")]
        public async Task<IActionResult> GetAvailableRoomsByBranchAsync(int branchId)
        {
            var roomStatus = await _bookingService.GetAvailableRoomsByBranchAsync(branchId);
            return Ok(roomStatus);
        }
    }
}
