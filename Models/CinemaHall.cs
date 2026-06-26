using System.ComponentModel.DataAnnotations;

namespace CinemaBookingApp.Models
{
    public class CinemaHall
    {
        [Key]
        public int HallID { get; set; }
        public string HallName { get; set; }
        public int Capacity { get; set; }
    }
}
