using Microsoft.AspNetCore.Identity;

namespace CinemaBookingApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}