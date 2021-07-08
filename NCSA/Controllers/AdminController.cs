using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using NCSA.Models;
using NCSA.ViewModel;
using Microsoft.AspNet.Identity.EntityFramework;
using Newtonsoft.Json;

namespace NCSA.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AdminController()
        {
            _context = new ApplicationDbContext();
        }

        // admin home page
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        /* Teams ACTIONS */

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult Teams()
        {
            try
            {
                var teamsQuery =
                from teams in _context.Teams
                join locations in _context.Locations on teams.LocationId equals locations.ID
                orderby teams.IsWalkertonTeam descending
                select new { teams, locations };

                List<AdminTeamVM> vm = new List<AdminTeamVM>();
                foreach (var team in teamsQuery)
                {
                    vm.Add(new AdminTeamVM()
                    {
                        Team = team.teams,
                        Location = team.locations
                    });
                }

                return View(vm.ToList());
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the Admin Teams Page. Have the admin look at the logs to see the error." });
            }
            
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult TeamsCreate()
        {
            try
            {
                var viewmodel = new AdminTeamEditVM()
                {
                    Team = null,
                    Locations = GetAllLocations(),
                    GradeLevelList = GetAllGradeLevels()
                };

                return View(viewmodel);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error","Home",  new { Message = "There was an error connecting to the Admin Teams Create Page. Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeamsCreate([Bind(Include = "TeamId,Description,TownName,IsWalkertonTeam,GradeLevel,GenderOfTeam,Coach1,Coach1Phone,Coach2,PracticeTimes,WhatToBring,LocationId")] Team team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Teams.Add(team);
                    _context.SaveChanges();
                    return RedirectToAction("Teams");
                }

                LogError("Error creating team  Town: " + team.TownName + ",  Age Group: " + team.GradeLevel, "");

                var viewmodel = new AdminTeamEditVM()
                {
                    Team = team,
                    Locations = GetAllLocations(),
                    GradeLevelList = GetAllGradeLevels()
                };

                return View(viewmodel);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error adding the team. Have the admin look at the logs to see the error." });
            }
        }

        /// <summary>
        /// Gets the different grade levels from the different teams
        /// </summary>
        /// <returns></returns> List of all grade levels
        public Dictionary<int, string> GetAllGradeLevels()
        {
            // get all grade levels for dropdown 
            Dictionary<int, string> gradeLevels = new Dictionary<int, string>();

            foreach (var team in _context.Teams.ToList())
            {
                if (!gradeLevels.Keys.Contains(team.GradeLevel))
                {
                    gradeLevels.Add(team.GradeLevel, team.GradeLevel.ToString().Substring(0, 1) + " / " + team.GradeLevel.ToString().Substring(1, 1));
                }
            }

            return gradeLevels;
        }

        /// <summary>
        /// Gets all locations for the teams into a list for dropdown
        /// </summary>
        /// <returns></returns> List of all locations
        public Dictionary<int, string> GetAllLocations()
        {
            Dictionary<int, string> locations = new Dictionary<int, string>();

            foreach (var loc in _context.Locations.ToList())
            {
                locations.Add(loc.ID, loc.StreetName + " " + loc.City + " " + loc.State + " " + loc.Zip);
            }

            return locations;
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult TeamsEdit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // get all grade levels for dropdown 
                Dictionary<int, string> gradeLevels = new Dictionary<int, string>();

                foreach (var team in _context.Teams.ToList())
                {
                    if (!gradeLevels.Keys.Contains(team.GradeLevel))
                    {
                        gradeLevels.Add(team.GradeLevel, team.GradeLevel.ToString().Substring(0, 1) + " / " + team.GradeLevel.ToString().Substring(1, 1));
                    }
                }

                // get all locations for dropdown
                Dictionary<int, string> locations = new Dictionary<int, string>();

                foreach (var loc in _context.Locations.ToList())
                {
                    locations.Add(loc.ID, loc.StreetName + " " + loc.City + " " + loc.State + " " + loc.Zip);
                }

                var viewmodel = new AdminTeamEditVM()
                {
                    Team = _context.Teams.FirstOrDefault(c => c.TeamId == id),
                    Locations = locations,
                    GradeLevelList = gradeLevels
                };

                if (viewmodel.Team == null)
                {
                    return HttpNotFound();
                }

                return View(viewmodel);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the admin teams edit page. Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TeamsEdit([Bind(Include = "TeamId,Description,TownName,IsWalkertonTeam,GradeLevel,GenderOfTeam,Coach1,Coach1Phone,Coach2,PracticeTimes,WhatToBring,LocationId")] Team team)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(team).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Teams");
                }

                LogError("Error editing team  Town: " + team.TownName + ",  Age Group: " + team.GradeLevel, "");

                return View(team);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error editing the team " + team.Description + ". Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult TeamsDelete(int id)
        {
            try
            {
                Team team = _context.Teams.Find(id);
                _context.Teams.Remove(team);
                _context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        /****** SCHEDULE Functions *******/

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult Schedule()
        {
            try
            {
                var games =
                    from sched in _context.Schedules
                    join hometeam in _context.Teams on sched.HomeTeamID equals hometeam.TeamId
                    join awayteam in _context.Teams on sched.AwayTeam2ID equals awayteam.TeamId
                    //where sched.GameDateTime > DateTime.Today
                    orderby sched.GameDateTime
                    select new { Game = sched, HomeTeam = hometeam, AwayTeam = awayteam };

                List<AdminScheduleIndexVM> gameList = new List<AdminScheduleIndexVM>();

                foreach (var game in games)
                {
                    gameList.Add(new AdminScheduleIndexVM()
                    {
                        Id = game.Game.ID,
                        HomeTeam = game.HomeTeam.TownName + " - " + game.HomeTeam.Description,
                        AwayTeam = game.AwayTeam.TownName + " - " + game.AwayTeam.Description,
                        GameDateTime = game.Game.GameDateTime.ToString("MM/dd/yyyy - h:mm tt"),
                        CenterRef = game.Game.CenterRef,
                        ARs = game.Game.AR2 != null ? game.Game.AR1 + " and " + game.Game.AR2 : game.Game.AR1
                    });
                }

                return View(gameList);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the admin schedule page. Have the admin look at the logs to see the error." });
            }

        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult ScheduleCreate()
        {
            try
            {
                var vm = new AdminScheduleEditVM()
                {
                    Game = null,
                    Teams = GetAllTeams()
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the schedule create page. Have the admin look at the logs to see the error." });                
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleCreate([Bind(Include = "ID,HomeTeamID,AwayTeam2ID,GameDateTime,CenterRef,AR1,AR2")] Schedule game)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Schedules.Add(game);
                    _context.SaveChanges();
                    return RedirectToAction("Schedule");
                }

                var viewmodel = new AdminScheduleEditVM()
                {
                    Game = game,
                    Teams = GetAllTeams()
                };

                return View(viewmodel);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error creating the game. Have the admin look at the logs to see the error." });
            }
        }

        /// <summary>
        /// Returns all teams in a dictionary
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetAllTeams()
        {
            Dictionary<int, string> teams = new Dictionary<int, string>();

            foreach (var team in _context.Teams.ToList())
            {
                teams.Add(team.TeamId, team.TownName + " - " + team.Description);
            }

            return teams;
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult ScheduleEdit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var vm = new AdminScheduleEditVM()
                {
                    Game = _context.Schedules.FirstOrDefault(c => c.ID == id),
                    Teams = GetAllTeams()
                };

                if (vm.Game == null)
                {
                    return HttpNotFound();
                }

                return View(vm);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the schedule edit page. Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleEdit([Bind(Include = "ID,HomeTeamID,AwayTeam2ID,GameDateTime,CenterRef,AR1,AR2")] Schedule game)
        {

            try
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(game).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("schedule");
                }

                return View(game);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error editing the game. Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult ScheduleDelete(int id)
        {
            try
            {
                Schedule game = _context.Schedules.Find(id);
                _context.Schedules.Remove(game);
                _context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Checks if the team id is used in any games. Returns true or false accordingly. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool IsTeamIdInUse(int id)
        {
            if (_context.Schedules.Any(c => c.HomeTeamID == id || c.AwayTeam2ID == id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /****** LOCATION Functions ******/

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult Locations()
        {
            return View(_context.Locations.ToList());
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult LocationCreate()
        {
            return View();
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocationCreate([Bind(Include = "ID,StreetName,City,State,Zip,Longitude,Latitude")] Location loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Locations.Add(loc);
                    _context.SaveChanges();
                    return RedirectToAction("Locations");
                }

                return View(loc);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the locations create page. Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        public ActionResult LocationEdit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                var location = _context.Locations.Find(id);

                if (location == null)
                {
                    return HttpNotFound();
                }

                return View(location);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the locations edit page. Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LocationEdit([Bind(Include = "ID,StreetName,City,State,Zip,Longitude,Latitude")] Location loc)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _context.Entry(loc).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("locations");
                }

                return View(loc);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error editing the location " + loc.StreetName + " " + loc.City + ". Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin + "," + RoleName.CommunityLeader)]
        [HttpDelete]
        [ValidateAntiForgeryToken]
        public ActionResult LocationDelete(int id)
        {
            try
            {
                Location loc = _context.Locations.Find(id);
                _context.Locations.Remove(loc);
                _context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        /// <summary>
        /// Checks if the location id is used by any teams. Returns true or false accordingly. 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        public bool IsLocationIdInUse(int id)
        {
            if (_context.Teams.Any(c => c.LocationId == id))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /****** USER Functions ******/

        [Authorize(Roles = RoleName.Admin)]
        public ActionResult Users() {
            try
            {
                List<UserVM> listOfUsers = new List<UserVM>();

                foreach (var user in _context.Users.ToList())
                {
                    var roles = "";

                    foreach (var role in user.Roles)
                    {
                        var roleName = _context.Roles.Find(role.RoleId).Name;
                        roles += roleName + ",";
                    }

                    listOfUsers.Add(new UserVM
                    {
                        UserId = user.Id,
                        Username = user.UserName,
                        RoleName = roles == "" ? "No Roles Assigned" : roles.TrimEnd(','),
                        LockoutCount = user.AccessFailedCount.ToString(),
                        IsLockedout = user.LockoutEndDateUtc.HasValue
                    });
                }

                return View(listOfUsers);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the admin users page. Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin)]
        [HttpPost]
        public ActionResult UnlockUser(string userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                user.LockoutEndDateUtc = null;
                user.AccessFailedCount = 0;
                _context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        [Authorize(Roles = RoleName.Admin)]
        public ActionResult UserEdit(string userId)
        {
            try
            {
                var user = _context.Users.Find(userId);
                if (user == null)
                {
                    LogError("User not found UserID: " + userId, "");
                    return RedirectToAction("Error", "Home", new { Message = "Could not find the user specified." });
                }

                var roles = _context.Roles.Select(r => r.Name);
                string userRoleId;
                var currentRole = "0";
                if (user.Roles.Count == 0)
                {
                    userRoleId = "";
                }
                else
                {
                    userRoleId = user.Roles.First().RoleId;
                }
                if (user.Roles.Count > 0)
                    currentRole = _context.Roles.First(c => c.Id == userRoleId).Name;

                var vm = new UserEditVM()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                    FullName = user.FullName,
                    CurrentRole = currentRole,
                    RoleNames = new SelectList(roles)
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the admin user edit page for userid: " + userId + ". Have the admin look at the logs to see the error." });
            }
        }

        [Authorize(Roles = RoleName.Admin)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserEdit([Bind(Include = "UserId,UserName,FullName,CurrentRole")] UserEditVM userEdit)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = _context.Users.Find(userEdit.UserId);
                    if (user.UserName != userEdit.UserName)
                    {
                        user.UserName = userEdit.UserName;
                    }

                    if (user.FullName != userEdit.FullName)
                    {
                        user.FullName = userEdit.FullName;
                    }

                    // every user can only have 1 roles so remove them all 
                    user.Roles.Clear();

                    // if there is something in the role, add that to the users roles
                    if (userEdit.CurrentRole != "No Role")
                    {
                        var role = _context.Roles.First(c => c.Name == userEdit.CurrentRole).Id;
                        user.Roles.Add(new IdentityUserRole() { UserId = userEdit.UserId, RoleId = role });
                    }

                    _context.Entry(user).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("users");
                }

                userEdit.RoleNames = new SelectList(_context.Roles.Select(r => r.Name));

                return View(userEdit);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);

                userEdit.RoleNames = new SelectList(_context.Roles.Select(r => r.Name));
                return View(userEdit);
            }

        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult UserDelete(string id)
        {
            try
            {
                ApplicationUser user = _context.Users.Find(id);
                _context.Users.Remove(user);
                _context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public ActionResult Logs()
        {
            var logs = _context.Logs.OrderByDescending(c => c.Date).Take(200);
            return View(logs.ToList());
        }

        /****** Referee Functions *******/

        public ActionResult Referee()
        {
            try
            {
                var user = _context.Users.Find(User.Identity.GetUserId());

                var games =
                        from sched in _context.Schedules
                        join hometeam in _context.Teams on sched.HomeTeamID equals hometeam.TeamId
                        join awayteam in _context.Teams on sched.AwayTeam2ID equals awayteam.TeamId
                        where sched.GameDateTime.Year == DateTime.Today.Year
                        orderby sched.GameDateTime
                        select new { Game = sched, HomeTeam = hometeam, AwayTeam = awayteam };

                List<RefereeGame> assignedGameList = new List<RefereeGame>();
                List<RefereeGame> openGameList = new List<RefereeGame>();

                foreach (var game in games)
                {
                    var gameModel = new RefereeGame
                    {
                        GameId = game.Game.ID,
                        GameDescription = game.HomeTeam.TownName + " vs " + game.AwayTeam.TownName + " - " + game.HomeTeam.Description,
                        GameDateTime = game.Game.GameDateTime.ToString("ddd MM/dd/yyyy - h:mm tt"),
                        CenterRef = game.Game.CenterRef,
                        AR1 = game.Game.AR1,
                        AR2 = game.Game.AR2,
                    };

                    // user must have a name in order to assign themselves to games
                    if (!string.IsNullOrEmpty(user.FullName) && (game.Game.CenterRef == user.FullName || game.Game.AR1 == user.FullName || game.Game.AR2 == user.FullName))
                    {
                        assignedGameList.Add(gameModel);
                    }
                    else if (string.IsNullOrEmpty(game.Game.CenterRef) || string.IsNullOrEmpty(game.Game.AR1) || string.IsNullOrEmpty(game.Game.AR2))
                    {
                        openGameList.Add(gameModel);
                    }
                }

                RefereeVM vm = new RefereeVM()
                {
                    UserFullName = user.FullName,
                    GamesAssignedTo = assignedGameList,
                    OpenGames = openGameList
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the admin referee page. Have the admin look at the logs to see the error." });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public object AddRefToGame(string gameID, string pos)
        {
            var user = _context.Users.Find(User.Identity.GetUserId());

            try
            {
                if (!long.TryParse(gameID, out var parsedGameId))
                {
                    Response.StatusCode = 500;
                    var err = new { msg = "Could not find the game. If have any questions, please contact Jeff Strycker to look into it." };
                    return JsonConvert.SerializeObject(err);
                }

                var game = _context.Schedules.Find(parsedGameId);

                if (pos == "center")
                {
                    if (string.IsNullOrEmpty(game.CenterRef))
                    {
                        game.CenterRef = user.FullName;
                    }
                    else
                    {
                        LogError("Game already has a center ref: " + game.CenterRef, "  gameid: " + game.ID);
                        Response.StatusCode = 500;
                        var err = new { msg = "This game already has a center referee assigned. If have any questions, please contact Jeff Strycker to look into it." };
                        return JsonConvert.SerializeObject(err);
                    }
                }
                else if (pos == "ar1")
                {
                    if (string.IsNullOrEmpty(game.AR1))
                    {
                        game.AR1 = user.FullName;
                    }
                    else
                    {
                        LogError("Game already has a AR1 ref: " + game.AR1, "  gameid: " + game.ID);
                        Response.StatusCode = 500;
                        var err = new { msg = "This game already has a AR1 referee assigned. If have any questions, please contact Jeff Strycker to look into it." };
                        return JsonConvert.SerializeObject(err);
                    }
                }
                else if (pos == "ar2")
                {
                    if (string.IsNullOrEmpty(game.AR2))
                    {
                        game.AR2 = user.FullName;
                    }
                    else
                    {
                        LogError("Game already has a AR2 ref: " + game.AR2, "  gameid: " + game.ID);
                        Response.StatusCode = 500;
                        var err = new { msg = "This game already has a AR2 referee assigned. If have any questions, please contact Jeff Strycker to look into it." };
                        return JsonConvert.SerializeObject(err);
                    }
                }
                else
                {
                    Response.StatusCode = 500;
                    var err = new { msg = "Could not find the position you chose. If have any questions, please contact Jeff Strycker to look into it." };
                    return JsonConvert.SerializeObject(err);
                }

                _context.SaveChanges();
                var userName = new { name = user.FullName };
                return JsonConvert.SerializeObject(userName);
            } 
            catch (Exception ex)
            {
                LogError("Error when adding referee to game - Name: " + user.FullName + "  " + ex.Message, ex.StackTrace);
                Response.StatusCode = 500;
                var err = new { msg = "There was an error adding you to the game. If the error persists, please contact Jeff Strycker to look into it." };
                return JsonConvert.SerializeObject(err);
            }
        }

        /****** Player Functions *******/

        public ActionResult Players()
        {
            try
            {
                var players =
                    from play in _context.Players
                    join teams in _context.Teams on play.TeamId equals teams.TeamId
                    where play.SignUpDate.Year == DateTime.Today.Year
                    orderby play.TeamId
                    select new { play, teams };

                Dictionary<string, string> listOfGrades = new Dictionary<string, string>();

                foreach (var item in _context.Teams.Where(c=>c.IsWalkertonTeam).Select(c => c.Description).Distinct().ToList())
                {
                    listOfGrades.Add(item, item);
                }

                PlayerVM vm = new PlayerVM();
                vm.Players = new List<PlayerIndexVM>();
                vm.ListOfGrades = listOfGrades;

                foreach (var item in players)
                {
                    vm.Players.Add(new PlayerIndexVM()
                    {
                        ID = item.play.ID,
                        Name = item.play.PlayerFirstName + " " + item.play.PlayerLastName,
                        PhoneNumber = item.play.PhoneNumber,
                        Email = item.play.Email,
                        TeamDesc = item.teams.Description
                    });
                }
                return View(vm);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the admin players page. Have the admin look at the logs to see the error." });
            }
        }

        [HttpDelete]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = RoleName.Admin)]
        public ActionResult PlayerDelete(int id)
        {
            try
            {
                var player = _context.Players.Find(id);
                _context.Players.Remove(player);
                _context.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.Accepted);
            }
            catch (Exception ex)
            {
                LogError(ex.Message, ex.StackTrace);
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }

        public ActionResult PlayerDetails(int id)
        {
            try
            {
                var player = _context.Players.Find(id);

                var vm = new PlayerDetailsVM()
                {
                    player = player,
                    Team = _context.Teams.Find(player.TeamId).Description
                };

                return View(vm);
            }
            catch (Exception ex)
            {
                LogError(ex.Message.ToString(), ex.StackTrace.ToString());
                return RedirectToAction("Error", "Home", new { Message = "There was an error connecting to the admin players detail page for player id " + id + ". Have the admin look at the logs to see the error." });
            }            
        }

        /****** Other Functions *******/

        public void LogError(string Message, string Stacktrace)
        {
            _context.Logs.Add(new Log()
            {
                Controller = "Admin",
                Message = Message,
                Stacktrace = Stacktrace,
                Date = DateTime.Now
            });
            _context.SaveChanges();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}