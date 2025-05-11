using Microsoft.EntityFrameworkCore;
using TrainInfoSystem.Models;

namespace TrainInfoSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Train> Trains { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Fare> Fares { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<PNR> PNRs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure composite key for Fare
            modelBuilder.Entity<Fare>()
                .HasKey(f => new { f.TrainId, f.ClassId });

            // Configure relationships for Fare
            modelBuilder.Entity<Fare>()
                .HasOne(f => f.Train)
                .WithMany(t => t.Fare)
                .HasForeignKey(f => f.TrainId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Fare>()
                .HasOne(f => f.Class)
                .WithMany(c => c.Fare)
                .HasForeignKey(f => f.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure relationships for Booking
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Train)
                .WithMany(t => t.Bookings)
                .HasForeignKey(b => b.TrainId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Class)
                .WithMany()
                .HasForeignKey(b => b.ClassId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.PNR)
                .WithOne(p => p.Booking)
                .HasForeignKey<PNR>(p => p.BookingId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed data for Trains (Indian trains)
            modelBuilder.Entity<Train>().HasData(
                new Train { TrainId = 1, TrainName = "Rajdhani Express", TrainNumber = "12951" },
                new Train { TrainId = 2, TrainName = "Shatabdi Express", TrainNumber = "12009" },
                new Train { TrainId = 3, TrainName = "Duronto Express", TrainNumber = "12245" }
            );

            // Seed data for Classes (including AC 2 Tier and AC 3 Tier)
            modelBuilder.Entity<Class>().HasData(
                new Class { ClassId = 1, ClassName = "First AC" },
                new Class { ClassId = 2, ClassName = "AC 2 Tier" },
                new Class { ClassId = 3, ClassName = "AC 3 Tier" },
                new Class { ClassId = 4, ClassName = "Sleeper" }
            );

            // Seed data for Fares (fare for each train-class combination)
            modelBuilder.Entity<Fare>().HasData(
                // Rajdhani Express Fares
                new Fare { TrainId = 1, ClassId = 1, FareAmount = 4500.00m, TotalSeats = 20, AvailableSeats = 15 },
                new Fare { TrainId = 1, ClassId = 2, FareAmount = 3000.00m, TotalSeats = 40, AvailableSeats = 30 },
                new Fare { TrainId = 1, ClassId = 3, FareAmount = 2000.00m, TotalSeats = 60, AvailableSeats = 50 },
                new Fare { TrainId = 1, ClassId = 4, FareAmount = 600.00m, TotalSeats = 80, AvailableSeats = 70 },
                // Shatabdi Express Fares
                new Fare { TrainId = 2, ClassId = 1, FareAmount = 4000.00m, TotalSeats = 15, AvailableSeats = 10 },
                new Fare { TrainId = 2, ClassId = 2, FareAmount = 2500.00m, TotalSeats = 30, AvailableSeats = 25 },
                new Fare { TrainId = 2, ClassId = 3, FareAmount = 1800.00m, TotalSeats = 50, AvailableSeats = 40 },
                // Duronto Express Fares
                new Fare { TrainId = 3, ClassId = 1, FareAmount = 4200.00m, TotalSeats = 18, AvailableSeats = 12 },
                new Fare { TrainId = 3, ClassId = 2, FareAmount = 2800.00m, TotalSeats = 35, AvailableSeats = 28 },
                new Fare { TrainId = 3, ClassId = 3, FareAmount = 1900.00m, TotalSeats = 55, AvailableSeats = 45 }
            );

            // Seed data for Bookings
            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,
                    PassengerName = "Amit Sharma",
                    TrainId = 1,
                    ClassId = 1,
                    JourneyDate = new DateTime(2025, 6, 10),
                    Email = "amit.sharma@example.com"
                },
                new Booking
                {
                    BookingId = 2,
                    PassengerName = "Priya Patel",
                    TrainId = 1,
                    ClassId = 2,
                    JourneyDate = new DateTime(2025, 6, 12),
                    Email = "priya.patel@example.com"
                },
                new Booking
                {
                    BookingId = 3,
                    PassengerName = "Rahul Gupta",
                    TrainId = 2,
                    ClassId = 3,
                    JourneyDate = new DateTime(2025, 6, 15),
                    Email = "rahul.gupta@example.com"
                },
                new Booking
                {
                    BookingId = 4,
                    PassengerName = "Sneha Verma",
                    TrainId = 3,
                    ClassId = 2,
                    JourneyDate = new DateTime(2025, 6, 20),
                    Email = "sneha.verma@example.com"
                },
                new Booking
                {
                    BookingId = 5,
                    PassengerName = "Vikram Singh",
                    TrainId = 3,
                    ClassId = 4,
                    JourneyDate = new DateTime(2025, 6, 22),
                    Email = "vikram.singh@example.com"
                }
            );

            // Seed data for PNRs
            modelBuilder.Entity<PNR>().HasData(
                new PNR
                {
                    PNRId = 1,
                    PNRNumber = "PNR123456789",
                    Coach = "H1",
                    BerthNumber = "10",
                    SeatNo = "10A",
                    Status = "Confirmed",
                    BookingId = 1
                },
                new PNR
                {
                    PNRId = 2,
                    PNRNumber = "PNR234567890",
                    Coach = "A1",
                    BerthNumber = "15",
                    SeatNo = "15B",
                    Status = "Confirmed",
                    BookingId = 2
                },
                new PNR
                {
                    PNRId = 3,
                    PNRNumber = "PNR345678901",
                    Coach = "B2",
                    BerthNumber = "20",
                    SeatNo = "20C",
                    Status = "Waiting",
                    BookingId = 3
                },
                new PNR
                {
                    PNRId = 4,
                    PNRNumber = "PNR456789012",
                    Coach = "A2",
                    BerthNumber = "25",
                    SeatNo = "25A",
                    Status = "Confirmed",
                    BookingId = 4
                },
                new PNR
                {
                    PNRId = 5,
                    PNRNumber = "PNR567890123",
                    Coach = "S1",
                    BerthNumber = "30",
                    SeatNo = "30D",
                    Status = "RAC",
                    BookingId = 5
                }
            );


        }
    }
}