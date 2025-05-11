//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using TrainInfoSystem.Data;
//using TrainInfoSystem.Models;
//using TrainInfoSystem.ViewModels;

//namespace TrainInfoSystem.Controllers
//{
//    [Route("admin")]
//    public class AdminController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public AdminController(ApplicationDbContext context)
//        {
//            _context = context;
//        }


//        public IActionResult Admin() => View();

//        [HttpGet("add-train")]
//        public IActionResult AddTrain()
//        {
//            var viewModel = new AddTrainViewModel
//            {
//                Classes = _context.Classes.Select(c => new ClassDetails
//                {
//                    ClassId = c.ClassId,
//                    ClassName = c.ClassName,
//                    IsSelected = false
//                }).ToList()
//            };
//            return View(viewModel);
//        }

//        [HttpPost("add-train")]
//        public IActionResult AddTrain(AddTrainViewModel viewModel)
//        {
//            if (!ModelState.IsValid)
//            {
//                viewModel.Classes = _context.Classes.Select(c => new ClassDetails
//                {
//                    ClassId = c.ClassId,
//                    ClassName = c.ClassName,
//                    IsSelected = viewModel.Classes.Any(vc => vc.ClassId == c.ClassId && vc.IsSelected),
//                    FareAmount = viewModel.Classes.FirstOrDefault(vc => vc.ClassId == c.ClassId) != null
//                        ? viewModel.Classes.FirstOrDefault(vc => vc.ClassId == c.ClassId).FareAmount
//                        : 0,
//                    TotalSeats = viewModel.Classes.FirstOrDefault(vc => vc.ClassId == c.ClassId) != null
//                        ? viewModel.Classes.FirstOrDefault(vc => vc.ClassId == c.ClassId).TotalSeats
//                        : 0
//                }).ToList();
//                return View(viewModel);
//            }

//            var train = new Train
//            {
//                TrainName = viewModel.TrainName,
//                TrainNumber = viewModel.TrainNumber
//            };

//            _context.Trains.Add(train);
//            _context.SaveChanges(); // Save train to get TrainId

//            foreach (var classDetail in viewModel.Classes.Where(c => c.IsSelected))
//            {
//                var fare = new Fare
//                {
//                    TrainId = train.TrainId,
//                    ClassId = classDetail.ClassId,
//                    FareAmount = classDetail.FareAmount,
//                    TotalSeats = classDetail.TotalSeats,
//                    AvailableSeats = classDetail.TotalSeats
//                };
//                _context.Fares.Add(fare);
//            }

//            _context.SaveChanges();
//            TempData["Success"] = "Train added successfully with class details.";
//            return RedirectToAction("AddTrain");
//        }


//        [HttpGet("pending-bookings")]
//        public IActionResult PendingBookings()
//        {
//            var pendingBookings = _context.Bookings
//                .Include(b => b.Train)
//                .Include(b => b.Class)
//                .Include(b => b.PNR)
//                .Where(b => b.PNR == null || b.PNR.Status == "Pending")
//                .ToList();
//            return View(pendingBookings);
//        }

//        [HttpPost("confirm-booking")]
//        public async Task<IActionResult> ConfirmBooking(int bookingId, string coach, string berth, string seatNo)
//        {
//            var booking = await _context.Bookings
//                .Include(b => b.PNR)
//                .Include(b => b.Class)
//                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

//            if (booking == null)
//            {
//                TempData["Error"] = "Booking not found.";
//                return RedirectToAction("PendingBookings");
//            }

//            // Update or create PNR
//            if (booking.PNR == null)
//            {
//                booking.PNR = new PNR
//                {
//                    PNRNumber = GeneratePNRNumber(),
//                    BookingId = bookingId
//                };
//                _context.PNRs.Add(booking.PNR);
//            }

//            booking.PNR.Coach = coach;
//            booking.PNR.BerthNumber = berth;
//            booking.PNR.SeatNo = seatNo;
//            booking.PNR.Status = "Confirmed";

//            // Update available seats
//            var fare = await _context.Fares
//                .FirstOrDefaultAsync(f => f.TrainId == booking.TrainId && f.ClassId == booking.ClassId);
//            if (fare != null && fare.AvailableSeats > 0)
//            {
//                fare.AvailableSeats--;
//            }
//            else
//            {
//                TempData["Error"] = "No available seats.Ticket Waitlisted";
//                booking.PNR.Coach = "NA";
//                booking.PNR.BerthNumber = "NA";
//                booking.PNR.SeatNo = "NA";
//                booking.PNR.Status = "Waitlisted";
//                await _context.SaveChangesAsync();

//                return RedirectToAction("PendingBookings");
//            }

//            await _context.SaveChangesAsync();
//            TempData["Success"] = "Booking confirmed successfully.";
//            return RedirectToAction("PendingBookings");
//        }

//        [HttpGet("confirmed-bookings")]
//        public IActionResult ConfirmedBookings()
//        {
//            try
//            {
//                var bookings = _context.Bookings
//                    .Include(b => b.Train)
//                    .Include(b => b.PNR)
//                    .ToList();

//                if (!bookings.Any())
//                {
//                    TempData["Info"] = "No bookings found in the database.";
//                }

//                return View(bookings);
//            }
//            catch (Exception ex)
//            {
//                TempData["Error"] = "An error occurred while loading confirmed bookings.";
//                return View(new List<Booking>());
//            }
//        }
//        private string GeneratePNRNumber()
//        {
//            return "PNR" + System.Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
//        }
//    }
//}

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainInfoSystem.Data;
using TrainInfoSystem.Models;
using TrainInfoSystem.ViewModels;

namespace TrainInfoSystem.Controllers
{
    [Route("admin")]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController(ApplicationDbContext context)
        {
            _context = context;
        }


        public IActionResult Admin() => View();

        [HttpGet("add-train")]
        public IActionResult AddTrain()
        {
            var viewModel = new AddTrainViewModel
            {
                Classes = _context.Classes.Select(c => new ClassDetails
                {
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    IsSelected = false
                }).ToList()
            };
            return View(viewModel);
        }

        [HttpPost("add-train")]
        public IActionResult AddTrain(AddTrainViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Classes = _context.Classes.Select(c => new ClassDetails
                {
                    ClassId = c.ClassId,
                    ClassName = c.ClassName,
                    IsSelected = viewModel.Classes.Any(vc => vc.ClassId == c.ClassId && vc.IsSelected),
                    FareAmount = viewModel.Classes.FirstOrDefault(vc => vc.ClassId == c.ClassId) != null
                        ? viewModel.Classes.FirstOrDefault(vc => vc.ClassId == c.ClassId).FareAmount
                        : 0,
                    TotalSeats = viewModel.Classes.FirstOrDefault(vc => vc.ClassId == c.ClassId) != null
                        ? viewModel.Classes.FirstOrDefault(vc => vc.ClassId == c.ClassId).TotalSeats
                        : 0
                }).ToList();
                return View(viewModel);
            }

            var train = new Train
            {
                TrainName = viewModel.TrainName,
                TrainNumber = viewModel.TrainNumber
            };

            _context.Trains.Add(train);
            _context.SaveChanges(); // Save train to get TrainId

            foreach (var classDetail in viewModel.Classes.Where(c => c.IsSelected))
            {
                var fare = new Fare
                {
                    TrainId = train.TrainId,
                    ClassId = classDetail.ClassId,
                    FareAmount = classDetail.FareAmount,
                    TotalSeats = classDetail.TotalSeats,
                    AvailableSeats = classDetail.TotalSeats
                };
                _context.Fares.Add(fare);
            }

            _context.SaveChanges();
            TempData["Success"] = "Train added successfully with class details.";
            return RedirectToAction("AddTrain");
        }


        [HttpGet("pending-bookings")]
        public IActionResult PendingBookings()
        {
            var pendingBookings = _context.Bookings
                .Include(b => b.Train)
                .Include(b => b.Class)
                .Include(b => b.PNR)
                .Where(b => b.PNR == null || b.PNR.Status == "Pending")
                .ToList();
            return View(pendingBookings);
        }

        [HttpPost("confirm-booking")]
        public async Task<IActionResult> ConfirmBooking(int bookingId, string coach, string berth, string seatNo)
        {
            var booking = await _context.Bookings
                .Include(b => b.PNR)
                .Include(b => b.Class)
                .FirstOrDefaultAsync(b => b.BookingId == bookingId);

            if (booking == null)
            {
                TempData["Error"] = "Booking not found.";
                return RedirectToAction("PendingBookings");
            }

            // Update or create PNR
            if (booking.PNR == null)
            {
                booking.PNR = new PNR
                {
                    PNRNumber = GeneratePNRNumber(),
                    BookingId = bookingId
                };
                _context.PNRs.Add(booking.PNR);
            }

            booking.PNR.Coach = coach;
            booking.PNR.BerthNumber = berth;
            booking.PNR.SeatNo = seatNo;
            booking.PNR.Status = "Confirmed";

            // Update available seats
            var fare = await _context.Fares
                .FirstOrDefaultAsync(f => f.TrainId == booking.TrainId && f.ClassId == booking.ClassId);
            if (fare != null && fare.AvailableSeats > 0)
            {
                fare.AvailableSeats--;
            }
            else
            {
                TempData["Error"] = "No available seats.Ticket Waitlisted";
                booking.PNR.Coach = "NA";
                booking.PNR.BerthNumber = "NA";
                booking.PNR.SeatNo = "NA";
                booking.PNR.Status = "Waitlisted";
                await _context.SaveChangesAsync();

                return RedirectToAction("PendingBookings");
            }

            await _context.SaveChangesAsync();
            TempData["Success"] = "Booking confirmed successfully.";
            return RedirectToAction("PendingBookings");
        }

        [HttpGet("confirmed-bookings")]
        public IActionResult ConfirmedBookings()
        {
            try
            {
                var bookings = _context.Bookings
                    .Include(b => b.Train)
                    .Include(b => b.PNR)
                    .ToList();

                if (!bookings.Any())
                {
                    TempData["Info"] = "No bookings found in the database.";
                }

                return View(bookings);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "An error occurred while loading confirmed bookings.";
                return View(new List<Booking>());
            }
        }
        private string GeneratePNRNumber()
        {
            return "PNR" + System.Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
        }
    }
}