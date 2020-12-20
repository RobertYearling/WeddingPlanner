using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PlannerTwo.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PlannerTwo.Context;
using Microsoft.EntityFrameworkCore;

namespace PlannerTwo.Controllers
{
    public class HomeController : Controller
    {
        private HomeContext _context;

        public HomeController(HomeContext context)
        {
            _context = context;
        }

        private User GetUserFromDB()
        {
            return _context.Users.FirstOrDefault(u => u.UserId == HttpContext.Session.GetInt32("UserId"));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("signin")]
        public IActionResult SignIn()
        {
            return View();
        }

        // Register Page
        [HttpPost("register")]
        public  IActionResult Register(User reg)
        {
            if(ModelState.IsValid)
            {
                if(_context.Users.Any(u => u.Email == reg.Email))
                {
                    ModelState.AddModelError("Email", "Email is already used.");
                    return View("Index");
                }
                PasswordHasher<User> hasher = new PasswordHasher<User>();
                reg.Password = hasher.HashPassword(reg,reg.Password);
                _context.Users.Add(reg);
                _context.SaveChanges();
                HttpContext.Session.SetInt32("UserId",reg.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("Index");
        }

        // Login Page
        [HttpPost("login")]
        public IActionResult Login(LoginUser log)
        {
            if(ModelState.IsValid)
            {
                User userInDb = _context.Users.FirstOrDefault(u => u.Email == log.LoginEmail);
                if(userInDb == null)
                {
                    ModelState.AddModelError("LoginEmail","Invalid Email/Password");
                    ModelState.AddModelError("LoginPassword","Invalid Email/Password");
                    return View("SignIn");
                }
                PasswordHasher<LoginUser> hash = new PasswordHasher<LoginUser>();
                var result = hash.VerifyHashedPassword(log,userInDb.Password,log.LoginPassword);
                if(result == 0)
                {
                    ModelState.AddModelError("LoginEmail","Invalid Email/Password");
                    ModelState.AddModelError("LoginPassword","Invalid Email/Password");
                    return View("SignIn");
                }
                HttpContext.Session.SetInt32("UserId",userInDb.UserId);
                return RedirectToAction("Dashboard");
            }
            return View("SignIn");
        }

        // Dashboard Page Layout
        [HttpGet("dashboard")]
        public IActionResult Dashboard()
        {
            User userInDb = GetUserFromDB();
            if(userInDb == null)
            {
                return RedirectToAction("logout");
            }
            ViewBag.User = userInDb;
            List<Wedding> AllWeddings = _context.Weddings
                                        .Include( w => w.Creator)
                                        .Include( w => w.Attendees)
                                        .ThenInclude( r => r.Attendee)
                                        .ToList();
            return View(AllWeddings);
        }

        // Logout Clearing the Session
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // Link to New Weding Page
        [HttpGet("new/wedding")]
        public IActionResult NewWedding()
        {
            return View();
        }

        // Adding a Party to the Database
        [HttpPost("plan/wedding")]
        public IActionResult PlanWedding(Wedding party)
        {
            User userInDb = GetUserFromDB();
            if(userInDb != null)
            {
                if(ModelState.IsValid)
                {
                    party.UserId = userInDb.UserId;
                    _context.Weddings.Add(party);
                    _context.SaveChanges();
                    return RedirectToAction("Dashboard");
                }
                return View("NewWedding");
            }
            return RedirectToAction("logout");
        }

        // Actions
        // Action RSVP
        [HttpGet("rsvp/{userId}/{weddingId}")]
        public IActionResult RSVP(int userId, int weddingId)
        {
            RSVP goin = new RSVP();
            goin.UserId = userId;
            goin.WeddingId = weddingId;
            _context.RSVPs.Add(goin);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // Action Better Plans
        [HttpGet("other/{userId}/{weddingId}")]
        public IActionResult Other(int userId, int weddingId)
        {
            RSVP nogo = _context.RSVPs.FirstOrDefault( r => r.UserId == userId && r.WeddingId == weddingId);
            _context.RSVPs.Remove(nogo);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        // Action Parties Over
        [HttpGet("destroy/{weddingId}")]
        public IActionResult DestroyWedding(int weddingId)
        {
            Wedding cancelled = _context.Weddings.FirstOrDefault( w => w.WeddingId == weddingId);
            _context.Weddings.Remove(cancelled);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet("{weddingId}")]
        public IActionResult WedParty(int weddingId)
        {
            Wedding party = _context.Weddings
                            .Include( w => w.Attendees)
                            .ThenInclude( r => r.Attendee)
                            .Include( w => w.Creator)
                            .FirstOrDefault( w => w.WeddingId == weddingId);
            return View(party);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
