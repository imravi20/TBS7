//using System.ComponentModel.DataAnnotations;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using TrainInfoSystem.Data;
//using TrainInfoSystem.Models;
//using TrainInfoSystem.ViewModels;

//namespace TrainInfoSystem.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly ApplicationDbContext _context;

//        public HomeController(ApplicationDbContext context)
//        {
//            _context = context;
//        }

//        public IActionResult Index() => View();

//        [HttpGet]
//        public IActionResult SearchTrain()
//        {
//            // Pass all trains to the view for the table
//            var viewModel = new SearchTrainViewModel
//            {
//                Trains = _context.Trains.ToList()
//            };
//            return View(viewModel);
//        }

//        [HttpPost]
//        public IActionResult SearchTrain(string query)
//        {
//            var trains = _context.Trains
//                .Include(t => t.Bookings)
//                .Where(t => t.TrainName.Contains(query ?? "") || t.TrainNumber.Contains(query ?? ""))
//                .ToList();
//            ViewBag.Fares = _context.Fares.Include(f => f.Class).ToList();
//            ViewBag.Query = query;
//            return View("TrainResults", trains);
//        }

//        [HttpGet]
//        public IActionResult PNRStatus() => View();

//        [HttpPost]
//        public async Task<IActionResult> PNRStatus(string pnr)
//        {
//            var pnrRecord = await _context.PNRs
//                .Include(p => p.Booking)
//                .ThenInclude(b => b.Train)
//                .FirstOrDefaultAsync(p => p.PNRNumber == pnr);

//            if (pnrRecord == null)
//            {
//                ViewData["PNRNotFound"] = true;
//                return View(null);
//            }

//            return View(pnrRecord);
//        }

//        [HttpPost]
//        public IActionResult BookTicket(int trainId, int classId, string passengerName, DateTime journeyDate, string email, string query)
//        {
//            if (!new EmailAddressAttribute().IsValid(email))
//            {
//                TempData["Error"] = "Invalid email address.";
//                var trains = _context.Trains
//                    .Include(t => t.Bookings)
//                    .Where(t => t.TrainName.Contains(query ?? "") || t.TrainNumber.Contains(query ?? ""))
//                    .ToList();
//                ViewBag.Fares = _context.Fares.Include(f => f.Class).ToList();
//                return View("TrainResults", trains);
//            }

//            if (journeyDate < DateTime.Today || journeyDate > DateTime.Today.AddMonths(2))
//            {
//                TempData["Error"] = "Journey date must be within the next two months.";
//                var trains = _context.Trains
//                    .Include(t => t.Bookings)
//                    .Where(t => t.TrainName.Contains(query ?? "") || t.TrainNumber.Contains(query ?? ""))
//                    .ToList();
//                ViewBag.Fares = _context.Fares.Include(f => f.Class).ToList();
//                return View("TrainResults", trains);
//            }

//            var train = _context.Trains.FirstOrDefault(t => t.TrainId == trainId);
//            var fare = _context.Fares.FirstOrDefault(f => f.TrainId == trainId && f.ClassId == classId);
//            if (train == null || fare == null)
//            {
//                TempData["Error"] = "Invalid train or class selected.";
//                var trains = _context.Trains
//                    .Include(t => t.Bookings)
//                    .Where(t => t.TrainName.Contains(query ?? "") || t.TrainNumber.Contains(query ?? ""))
//                    .ToList();
//                ViewBag.Fares = _context.Fares.Include(f => f.Class).ToList();
//                return View("TrainResults", trains);
//            }

//            var booking = new Booking
//            {
//                TrainId = trainId,
//                ClassId = classId,
//                PassengerName = passengerName,
//                JourneyDate = journeyDate,
//                Email = email
//            };

//            _context.Bookings.Add(booking);
//            _context.SaveChanges();

//            TempData["Success"] = "Booking request submitted successfully.";
//            var resultTrains = _context.Trains
//                .Include(t => t.Bookings)
//                .Where(t => t.TrainName.Contains(query ?? "") || t.TrainNumber.Contains(query ?? ""))
//                .ToList();
//            ViewBag.Fares = _context.Fares.Include(f => f.Class).ToList();
//            return View("TrainResults", resultTrains);
//        }
//    }
//}

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainInfoSystem.Data;
using TrainInfoSystem.Models;
using TrainInfoSystem.ViewModels;

namespace TrainInfoSystem.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public IActionResult SearchTrain()
        {
            // Pass all trains to the view for the table
            var viewModel = new SearchTrainViewModel
            {
                Trains = _context.Trains.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SearchTrain(string query)
        {
            var trains = _context.Trains
                .Include(t => t.Bookings)
                .Where(t => t.TrainName.Contains(query ?? "") || t.TrainNumber.Contains(query ?? ""))
                .ToList();
            ViewBag.Fares = _context.Fares.Include(f => f.Class).ToList();
            ViewBag.Query = query;
            return View("TrainResults", trains);
        }

        [HttpGet]
        public IActionResult PNRStatus() => View();

        [HttpPost]
        public async Task<IActionResult> PNRStatus(string pnr)
        {
            var pnrRecord = await _context.PNRs
                .Include(p => p.Booking)
                .ThenInclude(b => b.Train)
                .FirstOrDefaultAsync(p => p.PNRNumber == pnr);

            if (pnrRecord == null)
            {
                ViewData["PNRNotFound"] = true;
                return View(null);
            }

            return View(pnrRecord);
        }

        [HttpPost]
        public IActionResult BookTicket(int trainId, int classId, string passengerName, DateTime journeyDate, string email, string query)
        {
            if (!new EmailAddressAttribute().IsValid(email))
            {
                TempData["Error"] = "Invalid email address.";
                return RedirectToAction("SearchTrain");
            }

            if (journeyDate < DateTime.Today || journeyDate > DateTime.Today.AddMonths(2))
            {
                TempData["Error"] = "Journey date must be within the next two months.";
                return RedirectToAction("SearchTrain");
            }

            var train = _context.Trains.FirstOrDefault(t => t.TrainId == trainId);
            var fare = _context.Fares.FirstOrDefault(f => f.TrainId == trainId && f.ClassId == classId);
            if (train == null || fare == null)
            {
                TempData["Error"] = "Invalid train or class selected.";
                return RedirectToAction("SearchTrain");
            }

            var booking = new Booking
            {
                TrainId = trainId,
                ClassId = classId,
                PassengerName = passengerName,
                JourneyDate = journeyDate,
                Email = email
            };

            _context.Bookings.Add(booking);
            _context.SaveChanges();

            TempData["Success"] = "Booking request submitted successfully.";
            return RedirectToAction("SearchTrain");
        }
    }
}