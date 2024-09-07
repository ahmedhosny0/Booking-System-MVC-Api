using HotelManagement.DTO;
using HotelManagement.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;

namespace HotelManagement.Service
{
    public class BookingService : IBookingService
    {
        private readonly ApplicationDbContext _context;
        public BookingService(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<List<RoomsDTO>> GetAvailableRoomsByBranchAsync(int branchId)
        {
            var now = DateTime.Now;

            // Get RoomIds that are currently occupied
            var occupiedRoomIds = await _context.Bookings
                .Where(b => b.CheckoutDate > now)
                .Select(b => b.RoomId)
                .Distinct()
                .ToListAsync();

            // Get rooms that are not occupied
            var availableRooms = await _context.Rooms
                .Where(r => r.BranchId == branchId && !occupiedRoomIds.Contains(r.RoomId))
                .Select(r => new RoomsDTO
                {
                     RoomId = r.RoomId,
                    RoomName = r.RoomType.TypeName
                })
                .ToListAsync();

            return availableRooms;

        }
        public async Task<bool> CheckAvailability(int branchId, DateTime startDate, DateTime endDate)
        {
            // Logic to check room availability
            var availableRooms = await _context.Rooms
                .Where(r => r.BranchId == branchId)
                .ToListAsync();

            return availableRooms.Any();
        }
        public async Task<List<string>> GetCustomersAsync()
        {
            return await _context.Customers
                .Select(g => g.CustomerName)
                .Distinct()
                .ToListAsync();
        }
        public async Task<bool> DefineRoomType(RoomTypeDTO roomType)
        {
            var roomTypeentity = new RoomType();
            roomTypeentity.TypeName = roomType.TypeName;
            roomTypeentity.BasePrice = roomType.BasePrice;
            _context.RoomTypes.Add(roomTypeentity);
            _context.SaveChanges();
            return true;
        }
        public async Task<bool> Login(UsersDTO usersDTO)
        {
            if(usersDTO.Username=="admin"&&usersDTO.Password=="admin")
            {
                return true;
            }
            return false;
        }

        public async Task<bool> DefineCustomer(AddCustomerDTO Cust)
        {
            var Customerentity = new Customer();
            Customerentity.PhoneNumber = Cust.PhoneNumber;
            Customerentity.CustomerName = Cust.CustomerName;
            Customerentity.Email = Cust.Email;
            _context.Customers.Add(Customerentity);
            _context.SaveChanges();
            return true;
        }
        public async Task<List<RoomDetail>> GetRoomsStatus()
        {
            ViewsinDB viewsinDB = new ViewsinDB();
            IConfiguration config = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json")
    .Build();

            // Retrieve the connection string
            string conn = config.GetConnectionString("DefaultConnection");
            using (SqlConnection connection = new SqlConnection(conn))
            {
                using (SqlCommand command = new SqlCommand($"SELECT * from ({viewsinDB.RptBookingDetails})RptBooking ", connection))
                {
                    connection.Open(); // Open the connection
                    var vi = new List<RoomDetail>();
                    var test = command.ExecuteReader();
                    while (test.Read())
                    {
                        RoomDetail si = new RoomDetail();
                        if(Convert.IsDBNull(test["BookingId"]))
                        {
                            si.BookingId = 0;
                        }
                        else
                        {
                            si.BookingId = Convert.ToInt32(test["BookingId"]);
                        }
                        si.RoomTypeId = Convert.ToInt32(test["RoomId"]);
                        si.TypeName = test["TypeName"].ToString();
                        si.BranchName = test["BranchName"].ToString();
                        if (Convert.IsDBNull(test["TotalPrice"]))
                        {
                            si.BasePrice = 0;
                        }
                        else
                        {
                            si.BasePrice = Convert.ToDecimal(test["TotalPrice"]);
                        }
                            if (Convert.IsDBNull(test["CustomerName"]))
                        {
                            si.CustomerName = "Not Booked";
                            si.PhoneNumber = "Not Booked";
                        }
                        else
                        {
                            si.CustomerName = test["CustomerName"].ToString();
                            si.PhoneNumber = test["PhoneNumber"].ToString();
                        }
                        if (Convert.IsDBNull(test["CheckinDate"]))
                        {
                            si.CheckinDate = "Not Booked";
                            si.CheckoutDate = "Not Booked";
                        }
                        else
                        {
                            DateTime startDate = Convert.ToDateTime(test["CheckinDate"]);
                            DateTime endDate = Convert.ToDateTime(test["CheckoutDate"]);
                            si.CheckinDate = startDate.ToString("yyyy-MM-dd");
                            si.CheckoutDate = endDate.ToString("yyyy-MM-dd");
                        }
                        si.RoomStatus = test["RoomStatus"].ToString();

                        vi.Add(si);
                    }
                    //var orderedVi = vi.OrderBy(sale => sale.StoreName).ToList();
                    return vi;
                }
            }
        }
        public async Task<List<RoomDetail>> GetRoomsnew()
        {
            string conn = "Server=.;Database=HotelBooking;User Id=sa;Password=123456;TrustServerCertificate=True;";
            using (SqlConnection connection = new SqlConnection(conn))
            {
                using (SqlCommand command = new SqlCommand("SELECT * from RptRooms", connection))
                {
                    connection.Open(); // Open the connection
                    var vi = new List<RoomDetail>();
                    var test = command.ExecuteReader();
                    while (test.Read())
                    { 
                        RoomDetail si = new RoomDetail();
                        si.RoomTypeId = Convert.ToInt32(test["RoomId"]);
                        si.TypeName = test["TypeName"].ToString();
                        vi.Add(si);
                    }
                    //var orderedVi = vi.OrderBy(sale => sale.StoreName).ToList();
                    return vi;
                }
            }
        }

        public async Task<decimal> GetTotalPriceAsync(List<int> roomIds)
        {
            var ids = string.Join(", ", roomIds);  // Convert list to comma-separated string
            var query = $@"
         SELECT (rt.BasePrice)BasePrice 
 FROM Rooms r
 INNER JOIN RoomTypes rt ON r.RoomTypeId = rt.RoomTypeId
 WHERE r.RoomId IN ({ids})";

            var sss = await _context.Database.ExecuteSqlRawAsync(query);

            return await _context.Database.ExecuteSqlRawAsync(query);
        }
        public async Task<decimal> GetRoomPriceAsync(int roomId)
        {
            // Retrieve the price for a single room
            var roomPrice = await _context.Rooms
                .Where(r => r.RoomId == roomId)
                .Select(r => r.RoomType.BasePrice)
                .FirstOrDefaultAsync();

            return roomPrice;
        }
        public async Task<bool> BookRoom(BookingRequest request)
        {
            // Check if customer exists
            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.PhoneNumber == request.PhoneNumber);

            if (customer == null)
            {
                // Create a new customer if not found
                customer = new Customer
                {
                    PhoneNumber = request.PhoneNumber,
                    CustomerName = request.CustomerName
                };
                _context.Customers.Add(customer);

                // Save changes to generate the CustomerId
                await _context.SaveChangesAsync();
            }

            // Create bookings for each room
            foreach (var roomId in request.RoomIds)
            {
                // Get the price for the individual room
                decimal roomPrice = await GetRoomPriceAsync(roomId);

                // Apply discount if the customer has booked before
                if (await _context.Bookings.AnyAsync(b => b.CustomerId == customer.CustomerId))
                {
                    roomPrice *= 0.95m; // 5% discount
                }

                var booking = new Booking
                {
                    CustomerId = customer.CustomerId, // Use the populated CustomerId
                    CheckinDate = request.CheckinDate,
                    CheckoutDate = request.CheckoutDate,
                    TotalPrice = roomPrice, // Price for the individual room
                    RoomId = roomId,
                    BranchId = request.BranchId
                };
                _context.Bookings.Add(booking);
            }

            // Save all bookings
            await _context.SaveChangesAsync();

            return true;
        }
        public async Task<bool> CancelBooking(RoomDetail b)
        {
            // Logic to cancel booking
            var booking = await _context.Bookings.FindAsync(b.BookingId);

            if (booking != null)
            {
                _context.Bookings.Remove(booking);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<HotelBranch>> GetBranches()
        {
            var branches = await _context.HotelBranches
                .ToListAsync();
            return branches;
        }
        public async Task<List<Room>> GetRooms(int branchId)
        {
            var rooms = await _context.Rooms
                .Include(room=>room.RoomType)
                .Where(r => r.BranchId == branchId)
                .ToListAsync();
            return rooms;
        }
    }
}
