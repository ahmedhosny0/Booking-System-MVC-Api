using Microsoft.EntityFrameworkCore;
using System.Linq;
namespace HotelManagement.Models
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<HotelBranch> HotelBranches { get; set; }
        public DbSet<RoomType> RoomTypes { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
         .HasOne(r => r.HotelBranch)
         .WithMany(hb => hb.Room)
         .HasForeignKey(r => r.BranchId);

            modelBuilder.Entity<HotelBranch>()
                .HasMany(hb => hb.Room)
                .WithOne(r => r.HotelBranch);
            modelBuilder.Entity<RoomType>()
          .Property(rt => rt.RoomTypeId)
          .ValueGeneratedOnAdd(); //  (auto-increment)

            modelBuilder.Entity<HotelBranch>()
            .HasKey(hb => hb.BranchId);

            //Table Booking Has Three foreign key 
            // RoomId ->Room.RoomId
            modelBuilder.Entity<Booking>()
    .HasOne(b => b.Room)
    .WithMany(r => r.Bookings)
    .HasForeignKey(b => b.RoomId);

            // BranchId ->Branch.BranchId
            modelBuilder.Entity<Booking>()
    .HasOne(b => b.Branch)
    .WithMany(r => r.Bookings)
    .HasForeignKey(b => b.BranchId);

            // CustomerId ->Customer.CustomerId
            modelBuilder.Entity<Booking>()
    .HasOne(b => b.Customer)
    .WithMany(r => r.Bookings)
    .HasForeignKey(b => b.CustomerId);
            //Table Room Has two foreign key 
            // BranchId ->HotelBranch.BranchId
           // modelBuilder.Entity<Room>()
           // .HasOne(b => b.HotelBranch)
           // .WithMany(r => r.Room)
           // .HasForeignKey(b => b.BranchId)
           //.OnDelete(DeleteBehavior.NoAction); // No action on delete or update

            // RoomTypeId ->RoomType.RoomTypeId
            modelBuilder.Entity<Room>()
            .HasOne(b => b.RoomType)
            .WithMany(r => r.Room)
            .HasForeignKey(b => b.RoomTypeId);
        }
}
}

