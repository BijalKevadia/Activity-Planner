using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using activityPlanner.Models;
namespace activityPlanner.Controllers {
    public class ActivityController : Controller {
        private Context dbContext;
        public ActivityController(Context context) {
            dbContext = context;
        }
        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            List<ActivityMod> AllActivities = dbContext.Activities
                                        .Include(activity => activity.Guests)
                                            .ThenInclude(guest => guest.User)
                                            .OrderBy(activity => activity.ActivityDate)
                                        .ToList();
            List<Rsvp> UserRsvps = dbContext.Rsvps.Where(rsvp => rsvp.User.Equals(CurrentUser)).ToList();
            
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.AllActivities = AllActivities;
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.UserRsvps = UserRsvps;
            return View();
        }

        [HttpGet]
        [Route("NewActivity")]
        public IActionResult NewActivity() {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            string Planner= CurrentUser.FirstName;
            ViewBag.Planner= Planner;
            return View();
        }

        [HttpPost]
        [Route("AddActivity")]
        public IActionResult AddActivity(ActivityMod model) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            if(model.ActivityDate < DateTime.Now) {
                ModelState.AddModelError("Datetime", "Activity must be in the future");
            }
            if(ModelState.IsValid) {
                dbContext.Add(model);
                dbContext.SaveChanges();
                return View("Dashboard");
            }
            else {
                ViewBag.errors = ModelState.Values;
                return View("NewActivity");
            }
        }

        [HttpGet]
        [Route("Activity/{ActivityId}")]
        public IActionResult Activity(int ActivityId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            ActivityMod CurrentActivity = dbContext.Activities
                                        .Include(activity => activity.Guests)
                                            .ThenInclude(guest => guest.User)
                                        .SingleOrDefault(activity => activity.ActivityId == ActivityId);
            ViewBag.CurrentActivity = CurrentActivity;
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            string Planner= CurrentUser.FirstName;
            ViewBag.Planner= Planner;
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.CurrentUser = CurrentUser;
            return View("Activity");
        }

        [HttpGet]
        [Route("RSVP/{ActivityId}")]
        public IActionResult RSVP(int ActivityId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            ActivityMod CurrentActivity = dbContext.Activities
                                            .Include(activity => activity.Guests)
                                                .ThenInclude(guest => guest.User)
                                            .SingleOrDefault(activity => activity.ActivityId == ActivityId);
            Rsvp NewRsvp = new Rsvp {
                UserId = CurrentUser.UserId,
                User = CurrentUser,
                ActivityId = CurrentActivity.ActivityId,
                Activity = CurrentActivity
            };
            CurrentActivity.Guests.Add(NewRsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("Decline/{ActivityId}")]
        public IActionResult Decline(int ActivityId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = dbContext.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            Rsvp CurrentRsvp = dbContext.Rsvps.SingleOrDefault(rsvp => rsvp.UserId == HttpContext.Session.GetInt32("UserId") && rsvp.ActivityId == ActivityId);
            ActivityMod CurrentActivity = dbContext.Activities
                                            .Include(activity => activity.Guests)
                                                .ThenInclude(guest => guest.User)
                                            .SingleOrDefault(activity => activity.ActivityId == ActivityId);
            CurrentActivity.Guests.Remove(CurrentRsvp);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        [HttpGet]
        [Route("Delete/{ActivityId}")]
        public IActionResult Delete(int ActivityId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            ActivityMod CurrentActivity = dbContext.Activities
                                            .SingleOrDefault(activity => activity.ActivityId == ActivityId);
            dbContext.Activities.Remove(CurrentActivity);
            dbContext.SaveChanges();
            return RedirectToAction("Dashboard");
        }
    }
}