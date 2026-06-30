using CinemaBookingApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CinemaBookingApp.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            // Only seed if all tables are empty
            if (context.Movies.Any() || context.CinemaHalls.Any())
                return;

            // --- Movies ---
            var movies = new List<Movie>
            {
                new Movie { Title = "Spider-Man: No Way Home", Genre = "Action", AgeLimit = 13, Description = "Peter Parker seeks help from Doctor Strange after his identity is revealed." },
                new Movie { Title = "The Lion King", Genre = "Animation", AgeLimit = 0, Description = "A young lion prince flees his kingdom after the murder of his father." },
                new Movie { Title = "Inception", Genre = "Sci-Fi", AgeLimit = 16, Description = "A thief who steals corporate secrets through dream-sharing technology." },
                new Movie { Title = "The Dark Knight", Genre = "Action", AgeLimit = 16, Description = "Batman faces the Joker, a criminal mastermind who wants to plunge Gotham into chaos." }
            };
            context.Movies.AddRange(movies);
            context.SaveChanges();

            // --- Cinema Halls ---
            var halls = new List<CinemaHall>
            {
                new CinemaHall { HallName = "Hall 1", Capacity = 100 },
                new CinemaHall { HallName = "Hall 2", Capacity = 80 },
                new CinemaHall { HallName = "Hall 3", Capacity = 60 }
            };
            context.CinemaHalls.AddRange(halls);
            context.SaveChanges();

            // --- Seats ---
            var seats = new List<Seat>();
            string[] seatTypes = { "Standard", "VIP" };
            // Add seats for Hall 1
            for (int i = 1; i <= 10; i++)
            {
                seats.Add(new Seat
                {
                    HallID = halls[0].HallID,
                    SeatNumber = "A" + i,
                    SeatType = i <= 8 ? "Standard" : "VIP",
                    Status = "Available"
                });
            }
            // Add seats for Hall 2
            for (int i = 1; i <= 8; i++)
            {
                seats.Add(new Seat
                {
                    HallID = halls[1].HallID,
                    SeatNumber = "B" + i,
                    SeatType = i <= 6 ? "Standard" : "VIP",
                    Status = "Available"
                });
            }
            context.Seats.AddRange(seats);
            context.SaveChanges();

            // --- ShowTimes ---
            var showTimes = new List<ShowTime>
            {
                new ShowTime
                {
                    MovieID = movies[0].MovieID,
                    HallID = halls[0].HallID,
                    Date = DateTime.Now.Date.AddDays(1),
                    Time = new TimeSpan(10, 0, 0),
                    Price = 15.00m,
                    Status = "Available"
                },
                new ShowTime
                {
                    MovieID = movies[0].MovieID,
                    HallID = halls[1].HallID,
                    Date = DateTime.Now.Date.AddDays(1),
                    Time = new TimeSpan(14, 0, 0),
                    Price = 15.00m,
                    Status = "Available"
                },
                new ShowTime
                {
                    MovieID = movies[1].MovieID,
                    HallID = halls[0].HallID,
                    Date = DateTime.Now.Date.AddDays(2),
                    Time = new TimeSpan(11, 0, 0),
                    Price = 12.00m,
                    Status = "Available"
                },
                new ShowTime
                {
                    MovieID = movies[2].MovieID,
                    HallID = halls[2].HallID,
                    Date = DateTime.Now.Date.AddDays(2),
                    Time = new TimeSpan(18, 0, 0),
                    Price = 18.00m,
                    Status = "Available"
                },
                new ShowTime
                {
                    MovieID = movies[3].MovieID,
                    HallID = halls[1].HallID,
                    Date = DateTime.Now.Date.AddDays(3),
                    Time = new TimeSpan(20, 0, 0),
                    Price = 20.00m,
                    Status = "Available"
                }
            };
            context.ShowTimes.AddRange(showTimes);
            context.SaveChanges();

            // --- Bookings ---
            var bookings = new List<Booking>
            {
                new Booking
                {
                    ShowTimeID = showTimes[0].ShowTimeID,
                    BookingDate = DateTime.Now,
                    TotalPrice = 30.00m,
                    Status = "Confirmed"
                },
                new Booking
                {
                    ShowTimeID = showTimes[2].ShowTimeID,
                    BookingDate = DateTime.Now,
                    TotalPrice = 12.00m,
                    Status = "Confirmed"
                },
                new Booking
                {
                    ShowTimeID = showTimes[4].ShowTimeID,
                    BookingDate = DateTime.Now,
                    TotalPrice = 40.00m,
                    Status = "Pending"
                }
            };
            context.Bookings.AddRange(bookings);
            context.SaveChanges();
        }
    }
}