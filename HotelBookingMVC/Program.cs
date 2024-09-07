using HotelBookingMVC.Services;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// Register the HttpClient for BookingService
builder.Services.AddHttpClient<BookingService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7171/api/"); // Replace with your API base address
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

// Register the BookingService
builder.Services.AddTransient<BookingService>();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30); // Set an appropriate timeout
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true; // Make the session cookie essential
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Booking}/{action=Login}/{id?}");

app.Run();
