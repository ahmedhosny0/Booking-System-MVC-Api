namespace HotelManagement.Models
{
    public class ViewsinDB
    {
        public string RptBookingDetails = @"
select 
  case when (cast(checkoutdate as date)<cast(GETDATE() as date) or BookingId is null) then 'Avaliable' else 'Booked' end as RoomStatus,
  Bookings.BookingId ,Bookings.TotalPrice, rooms.RoomId RoomId,RoomNumber,rt.RoomTypeId,rt.TypeName,rt.BasePrice,
  Hb.BranchId,hb.BranchName,hb.Location , cust.CustomerId,cust.CustomerName,cust.PhoneNumber,cast(checkoutdate as date)checkoutdate,cast(checkindate as date)checkindate
   from Bookings
   right join Rooms on rooms.RoomId=Bookings.RoomId
  left join RoomTypes rt on rt.RoomTypeId=rooms.RoomTypeId
  left join HotelBranches HB on Hb.BranchId=rooms.BranchId
  left join Customers Cust on Cust.CustomerId=Bookings.CustomerId  
";
        public string RptRoomDetails = @"
  select rooms.RoomId RoomId,RoomNumber,rt.RoomTypeId,rt.TypeName,rt.BasePrice,
  Hb.BranchId,hb.BranchName,hb.Location
   from Rooms
  left join RoomTypes rt on rt.RoomTypeId=rooms.RoomTypeId
  left join HotelBranches HB on Hb.BranchId=rooms.BranchId
";

    }
}
