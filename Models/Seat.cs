namespace CinemaBookingApp.Models
{
    public class Seat
    {
        public int SeatID { get; set; }
        public int HallID { get; set; }
        public CinemaHall Hall { get; set; }
        public string SeatNumber { get; set; }
        public string SeatType { get; set; }
        public string Status { get; set; }
    }
}
