using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using NCSA.Models;
using NCSA.ViewModel;
using Newtonsoft.Json;

namespace NCSA.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }

        // render home page
        public ActionResult Index()
        {
            try
            {
                var model = new HomeViewModel();

                model.Teams = _context.Teams.Where(c => c.IsWalkertonTeam == true).OrderBy(c => c.GradeLevel).ToList();

                var games =
                    from schedules in _context.Schedules
                    join hometeam in _context.Teams on schedules.HomeTeamID equals hometeam.TeamId
                    join awayteam in _context.Teams on schedules.AwayTeam2ID equals awayteam.TeamId
                    where (hometeam.IsWalkertonTeam == true || awayteam.IsWalkertonTeam == true) && schedules.GameDateTime >= DateTime.Now
                    orderby schedules.GameDateTime
                    select new { Game = schedules, HomeTeam = hometeam, AwayTeam = awayteam };

                List<GameVM> GamesList = new List<GameVM>();

                foreach (var item in games.ToList().Take(4))
                {
                    if (item.HomeTeam.IsWalkertonTeam)
                    {
                        GamesList.Add(new GameVM()
                        {
                            GameDateTime = item.Game.GameDateTime.ToString("MMM dd hh:mm tt"),
                            TeamDesc = item.HomeTeam.Description,
                            OtherTeamsTown = item.AwayTeam.TownName,
                            AtorVs = "vs ",
                            HomeOrAway = "Home"
                        });
                    }
                    else
                    {
                        GamesList.Add(new GameVM()
                        {
                            GameDateTime = item.Game.GameDateTime.ToString("MMM dd hh:mm tt"),
                            TeamDesc = item.AwayTeam.Description,
                            OtherTeamsTown = item.HomeTeam.TownName,
                            AtorVs = "at ",
                            HomeOrAway = "Away"
                        });
                    }
                }

                model.Games = GamesList;

                return View(model);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", new { Message = "There was an error connecting to the home page. If the error persists, please contact the admin of the site through our contact page. Thank you for your patience." });
            }
        }

        public ActionResult Team()
        {
            return View(_context.Teams.Where(c=>c.IsWalkertonTeam).OrderBy(c=>c.GradeLevel).ToList());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public object SubmitPlayer(Player player)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    throw new Exception();
                }

                // just clarifying the team id is valid
                if (_context.Teams.Any(c => c.TeamId == player.TeamId))
                {
                    player.SignUpDate = DateTime.Now.Date;
                    player.BirthDate = player.BirthDate.Date;

                    _context.Players.Add(player);
                    _context.SaveChanges();

                    return new HttpStatusCodeResult(HttpStatusCode.OK);
                }
                else
                {
                    LogError("Team Id was not valid when registering " + player.PlayerFirstName + " " + player.PlayerLastName,
                        Environment.StackTrace);
                    Response.StatusCode = 500;
                    var err = new { msg = "Team Id was not valid. If the error persists, please contact the admin of the site through our contact page. " };
                    return JsonConvert.SerializeObject(err);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString() + ". When registering for " + player.PlayerFirstName + " " + player.PlayerLastName
                        , ex.StackTrace.ToString());
                Response.StatusCode = 500;
                var err = new { msg = "There was an error submitting the form, verify all fields are filled out correctly. If the error persists, please contact the admin of the site through our contact use page. " };
                return JsonConvert.SerializeObject(err);
            }
        }

        public void LogError(string Message, string Stacktrace)
        {
            _context.Logs.Add(new Log()
            {
                Controller = "Home",
                Message = Message,
                Stacktrace = Stacktrace,
                Date = DateTime.Now
            });
            _context.SaveChanges();
        }

        public ActionResult Locations()
        {
            try
            {
                var locQuery =
                    from teams in _context.Teams
                    join locations in _context.Locations on teams.LocationId equals locations.ID
                    select new { teams, locations };

                List<LocationVM> vm = new List<LocationVM>();
                foreach (var loc in locQuery)
                {
                    vm.Add(new LocationVM()
                    {
                        TeamName = loc.teams.TownName.ToLower(),
                        TeamTitle = loc.teams.TownName + " - " + loc.teams.Description,
                        Location = loc.locations
                    });
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", new { Message = "There was an error loading the locations. If the error persists, please contact the admin of the site through our contact page. Thank you for your patience." });
            }
        }

        public ActionResult Error(string Message)
        {
            ViewBag.Message = Message;
            return View();
        }

        public ActionResult Schedule()
        {
            try
            {
                var scheduleVM = new ScheduleVM
                {
                    CurrentmonthName = DateTime.Now.ToString("MMM"),
                    CurrentMonth = DateTime.Now.Month
                };

                var gamesQuery =
                        from schedules in _context.Schedules
                        join hometeam in _context.Teams on schedules.HomeTeamID equals hometeam.TeamId
                        join awayteam in _context.Teams on schedules.AwayTeam2ID equals awayteam.TeamId
                        join location in _context.Locations on hometeam.LocationId equals location.ID
                        where schedules.GameDateTime.Year == DateTime.Now.Year
                        orderby schedules.GameDateTime
                        select new { Game = schedules, HomeTeam = hometeam, AwayTeam = awayteam, location };

                List<Game> gameList = new List<Game>();

                foreach (var game in gamesQuery)
                {
                    gameList.Add(new Game()
                    {
                        GameID = game.Game.ID,
                        GameDateTime = game.Game.GameDateTime,
                        GameDate = game.Game.GameDateTime.ToString("MM/dd/yyyy"),
                        GameTime = game.Game.GameDateTime.ToString("hh:mm tt"),
                        GradeLevel = game.HomeTeam.GradeLevel.ToString().Substring(0, 1) + "/" + game.HomeTeam.GradeLevel.ToString().Substring(1, 1),
                        HomeTeamName = game.HomeTeam.TownName,
                        HomeTeamDesc = game.HomeTeam.Description,
                        AwayTeamName = game.AwayTeam.TownName,
                        AwayTeamDesc = game.AwayTeam.Description,
                        Location = game.location
                    });
                }

                scheduleVM.Games = gameList;

                return View(scheduleVM);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", new { Message = "There was an error loading the schedule. If the error persists, please contact the admin of the site through our contact page. Thank you for your patience." });
            }
            
        }

        [HttpGet]
        public object GetGame(string date)
        {
            try
            {
                if (DateTime.TryParse(date.Replace("\"", ""), out DateTime parsedDate))
                {
                    var gamesQuery =
                        from schedules in _context.Schedules
                        join hometeam in _context.Teams on schedules.HomeTeamID equals hometeam.TeamId
                        join awayteam in _context.Teams on schedules.AwayTeam2ID equals awayteam.TeamId
                        join location in _context.Locations on hometeam.LocationId equals location.ID
                        where DbFunctions.TruncateTime(schedules.GameDateTime) == parsedDate.Date
                        orderby schedules.GameDateTime, hometeam.TownName
                        select new { Game = schedules, HomeTeam = hometeam, AwayTeam = awayteam, Location = location };

                    List<Game> gameList = new List<Game>();

                    foreach (var game in gamesQuery)
                    {
                        gameList.Add(new Game()
                        {
                            GameDateTime = game.Game.GameDateTime,
                            GameDate = game.Game.GameDateTime.ToString("MMM dd, yyyy"),
                            GameTime = game.Game.GameDateTime.ToString("h:mm tt"),
                            GradeLevel = game.HomeTeam.GradeLevel.ToString().Substring(0, 1) + "/" + game.HomeTeam.GradeLevel.ToString().Substring(1, 1),
                            HomeTeamName = game.HomeTeam.TownName,
                            HomeTeamDesc = game.HomeTeam.Description,
                            AwayTeamName = game.AwayTeam.TownName,
                            AwayTeamDesc = game.AwayTeam.Description,
                            Location = game.Location
                        });
                    }

                    return JsonConvert.SerializeObject(gameList, Formatting.Indented);
                }
                else
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }
    }
}