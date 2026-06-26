using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingApp.Models
{
    public class Booking
    {
        public int BookingID { get; set; }

        public int ShowTimeID { get; set; }
        public ShowTime ShowTime { get; set; }

        public DateTime BookingDate { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal TotalPrice { get; set; }

        public string Status { get; set; }
    }
}
