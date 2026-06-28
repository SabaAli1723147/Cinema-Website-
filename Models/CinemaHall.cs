using System.ComponentModel.DataAnnotations;

namespace CinemaBookingApp.Models
{
    public class CinemaHall
    {
        [Key]
        public int HallID { get; set; }

        [Required]
        public string HallName { get; set; }

        public int Capacity { get; set; }

    }
}
