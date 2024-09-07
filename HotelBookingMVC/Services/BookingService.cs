using HotelBookingMVC.Models;
using Microsoft.AspNetCore.Components.Server;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace HotelBookingMVC.Services
{
    public class BookingService
    {
        private readonly HttpClient _httpClient;

        public BookingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new System.Uri("https://localhost:7171/"); // API base address
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        public async Task<bool> Login(UsersDTO usersDTO)
        {

            var content = new StringContent(JsonConvert.SerializeObject(usersDTO), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/booking/Login", content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(result); // API response
            }
            return false;
        }
        public async Task<bool> DefineRoomType(RoomTypeDTO roomtype)
        {
            
            var content = new StringContent(JsonConvert.SerializeObject(roomtype), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/booking/defineroomtype",content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(result); // API response
            }
            return false;
        }
        public async Task<bool> CheckAvailabilityAsync(int branchId, DateTime startDate, DateTime endDate)
        {
            var response = await _httpClient.GetAsync($"booking/availability?branchId={branchId}&startDate={startDate:yyyy-MM-dd}&endDate={endDate:yyyy-MM-dd}");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<bool>(result); //  API response
            }
            return false;
        }
        public List<int> ParseRoomIds(string roomIdsInput)
        {
            if (string.IsNullOrWhiteSpace(roomIdsInput))
            {
                return new List<int>(); // Return an empty list if the input is null or empty
            }

            return roomIdsInput
                .Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(id => int.TryParse(id.Trim(), out var roomId) ? roomId : (int?)null)
                .Where(id => id.HasValue)
                .Select(id => id.Value)
                .ToList();
        }
        public async Task<BookingResponse> BookRoomAsync(BookingRequest request)
        {
            request.ParseSelectedRooms();

            if (request.RoomIds == null || !request.RoomIds.Any())
            {
                throw new ArgumentException("RoomIds must contain at least one room ID.");
            }

            // No need to parse if RoomIds is already a List<int>
            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/booking/book", content);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<BookingResponse>(result); //  API response
            }

            return null;
        }
        public async Task<RoomDetail> CancelBookingAsync(RoomDetail b)
        {
            var content = new StringContent(JsonConvert.SerializeObject(b), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/booking/cancelbooking",content);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<RoomDetail>(result); //  API response
            }
            return null; 
        }
        public async Task<List<RoomTypeDTO>> GetRoomsnewAsync()
        {
            var response = await _httpClient.GetAsync($"api/booking/GetRoomsnew");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RoomTypeDTO>>(result); //  API response
            }
            return null;
        }

        public async Task<List<HotelBranch>> GetBranchesAsync()
        {
            var response = await _httpClient.GetAsync($"api/booking/GetBranches");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<HotelBranch>>(result); //  API response
            }
            return null;
        }
        public async Task<List<RoomDetail>> GetRoomsStatusAsync()
        {
            var response = await _httpClient.GetAsync($"api/booking/GetRoomsStatus");
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RoomDetail>>(result); //  API response
            }
            return null;
        }

        public async Task<List<RoomsDTO>> GetAvailableRoomsByBranchAsync(int branchId)
        {
            var response = await _httpClient.GetAsync($"api/booking/GetAvailableRoomsByBranch?branchId=" + branchId);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<List<RoomsDTO>>(result); 
            }
            return null;
        }
    }
}
