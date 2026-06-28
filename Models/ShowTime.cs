using System.ComponentModel.DataAnnotations.Schema;

namespace CinemaBookingApp.Models
{
    public class ShowTime
    {
        public int ShowTimeID { get; set; }

        public int MovieID { get; set; }
        public Movie? Movie { get; set; }

        public int HallID { get; set; }
        public CinemaHall? Hall { get; set; }

        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }

        [Column(TypeName = "decimal(8,2)")]
        public decimal Price { get; set; }

        public string? Status { get; set; }
    }
}
