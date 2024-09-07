using HotelBookingMVC.Models;
using HotelBookingMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingMVC.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingService _bookingService;

        public BookingController(BookingService bookingService)
        {
            _bookingService = bookingService;
        }
        public async Task<IActionResult> CheckAvailability(int branchId, DateTime startDate, DateTime endDate)
        {
            var available = await _bookingService.CheckAvailabilityAsync(branchId, startDate, endDate);
            ViewBag.Availability = available;
            return View(); // Create a view to display availability
        }
        // GET: /Booking/BookRoom 
        public async Task<IActionResult> GetRoomsStatus()
        {
			var Login = HttpContext.Session.GetString("Login");
			TempData["Login"] = Login;
			var RoomStatus = await _bookingService.GetRoomsStatusAsync();
            ViewBag.Data = RoomStatus;
            return View(); // Returns the view to display the booking form
        }
        public IActionResult DefineRoomType()
        {
			var Login = HttpContext.Session.GetString("Login");
			TempData["Login"] = Login;
			return View(); // Returns the view to display the booking form
        }
        [HttpPost]
        public async Task<IActionResult> DefineRoomType(RoomTypeDTO roomTypeDTO)
        {
			var Login = HttpContext.Session.GetString("Login");
			TempData["Login"] = Login;
			var response = await _bookingService.DefineRoomType(roomTypeDTO);
            if (response)
            {
                return RedirectToAction("DefineRoomType", "Booking");
                return View();
            }
            else
            {
                return RedirectToAction("DefineRoomType", "Booking");
            }
        }
        public IActionResult Login()
        {
            HttpContext.Session.SetString("Login", "Guest");
            TempData["Login"] = "Guest";
            return View(); // Returns the view to display the booking form
        }
        [HttpPost]
        public async Task<IActionResult> Login(UsersDTO usersDTO)
        {
            var response = await _bookingService.Login(usersDTO);
            if (response)
            {
                HttpContext.Session.SetString("Login", "Admin");
				TempData["Login"] = "Admin";
                return RedirectToAction("Index","Home");
            }
            else
            {
                TempData["error"] = "error";
                return RedirectToAction("Login", "Booking");
            }
        }
        public IActionResult Index()
		{
			var Login = HttpContext.Session.GetString("Login");
			TempData["Login"] = Login;
			return View();
        }
        [HttpGet]
        public async Task<IActionResult> BookRoom()
        {
			var Login = HttpContext.Session.GetString("Login");
			TempData["Login"] = Login;
			var x = TempData["i"];
            if (x != null)
            {
                ViewBag.x = x;
            }
            else
                ViewBag.x = 0;
            var hotels =  await _bookingService.GetBranchesAsync();
            ViewBag.hotels = hotels;
            var RoomType = await _bookingService.GetAvailableRoomsByBranchAsync(ViewBag.x);
            ViewBag.RoomType = RoomType;
            var Result = TempData["AddSuccess"];
            ViewBag.State = Result;
            return View(); // Returns the view to display the booking form
        }
        [HttpPost]
        public async Task<IActionResult> BookRoom(BookingRequest request)
        {
			var Login = HttpContext.Session.GetString("Login");
			TempData["Login"] = Login;
			var hotels = await _bookingService.GetBranchesAsync();
            ViewBag.hotels = hotels;
            var RoomType = await _bookingService.GetAvailableRoomsByBranchAsync(request.BranchId);
            ViewBag.RoomType = RoomType;
            try
            {
                if (ModelState.IsValid || request.RoomIds == null)
                {
                    var response = await _bookingService.BookRoomAsync(request);
                    if (response != null && response.Success)
                    {
                        TempData["AddSuccess"] = "AddSuccess";
                        return View();
                    }
                }
            }
           catch
            {
                TempData["AddSuccess"] = "AddSuccess";
                return RedirectToAction ("BookRoom","Booking");
            }
                return View(); // Return to view with error message
        }

        [HttpPost]
        public async Task<IActionResult> CancelBooking(RoomDetail b)
        {
            var Login = HttpContext.Session.GetString("Login");
            TempData["Login"] = Login;
            var success = await _bookingService.CancelBookingAsync(b);
            if (success !=null)
            {
                return RedirectToAction("GetRoomsStatus", "Booking");
            }
            else
            {
                ModelState.AddModelError("", "Error cancelling booking");
                return RedirectToAction("GetRoomsStatus", "Booking");
            }
        }
        [HttpGet]
        public async Task<IActionResult> CancelBooking()
        {
                return RedirectToAction("GetRoomsStatus", "Booking");
        }
        [HttpGet]
        public async Task<IActionResult> GetAvailableRoomsByBranch(int branchId)
        {
			var Login = HttpContext.Session.GetString("Login");
			TempData["Login"] = Login;
			var rooms = await _bookingService.GetAvailableRoomsByBranchAsync(branchId);
            return Json(rooms);
        }

    }
}
