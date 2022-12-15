using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using heat_server.Models;

namespace heat_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScoutingReportsController : ControllerBase
    {
        private readonly ScoutingReportsContext _context;

        public ScoutingReportsController(ScoutingReportsContext context)
        {
            _context = context;
        }

        // GET: api/ScoutingReports/Scouts
        [HttpGet]
        [Route("/api/ScoutingReports/Scouts")]
        public async Task<ActionResult<IEnumerable<Scout>>> GetScout()
        {
            var activeScouts = await _context.Scout.Where(s => s.IsActiveFlag == true).ToListAsync();
            return activeScouts;
        }

        // GET: api/ScoutingReports/Report?reportId=___
        [HttpGet]
        [Route("/api/ScoutingReports/Report")]
        public async Task<ActionResult<ScoutingReport>> GetReport(int reportId)
        {
            var scoutingReport = await _context.ScoutingReport.Where(r => r.ReportKey == reportId).SingleOrDefaultAsync();
            return scoutingReport;
        }

        // GET: api/ScoutingReports/ReportsAll?scoutId=___
        [HttpGet]
        [Route("/api/ScoutingReports/ReportsAll")]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetAllReport(int scoutId)
        {
            var scoutingReports = await (
                from SR in _context.ScoutingReport
                join P in _context.Player on SR.PlayerKey equals P.PlayerKey
                where (SR.ScoutKey == scoutId)
                select new
                {
                    ReportKey = SR.ReportKey,
                    ScoutKey = SR.ScoutKey,
                    PlayerKey = SR.PlayerKey,
                    PlayerName = P.FirstName + ' ' + P.LastName,
                    TeamKey = SR.TeamKey,
                    Assist = SR.Assist,
                    Defense = SR.Defense,
                    Rebound = SR.Rebound,
                    Shooting = SR.Shooting,
                    Comments = SR.Comments,
                    LastModified = SR.ModifiedDateTime
                }).ToListAsync();
            
            return scoutingReports;
        }

        // GET: api/ScoutingReports/Reports?id=___
        [HttpGet]
        [Route("/api/ScoutingReports/Reports")]
        public async Task<ActionResult<IEnumerable<ScoutingReportsResponse>>> GetReports(int scoutId)
        {
            var teams = await (
                from T in _context.Team
                join SR in _context.ScoutingReport on T.TeamKey equals SR.TeamKey
                where (SR.ScoutKey == scoutId)               
                select new
                {
                    TeamId = SR.TeamKey,
                    TeamNickName = T.TeamNickname,
                    Conference = T.Conference
                } into x
                group x by new { x.TeamId, x.TeamNickName, x.Conference } into y
                select new
                {
                    TeamId = y.Key.TeamId,
                    TeamNickName = y.Key.TeamNickName,
                    Conference = y.Key.Conference
                }).ToListAsync();
            
            ScoutingReportsResponse[] res = new ScoutingReportsResponse[teams.Count]; 
            
            int i = 0;
            foreach(var t in teams)
            {
                ScoutingReportsResponse team = new ScoutingReportsResponse();
                team.TeamKey = t.TeamId;
                team.TeamNickName = t.TeamNickName;
                team.Conference = t.Conference;   
                
                //Query players here
                var players = await (
                    from P in _context.Player
                    join SR in _context.ScoutingReport on P.PlayerKey equals SR.PlayerKey
                    where ( SR.ScoutKey == scoutId && SR.TeamKey == t.TeamId)
                    select new
                    {
                        PlayerKey = SR.PlayerKey,
                        PlayerName = P.FirstName + P.LastName,
                        BirthDate = P.BirthDate
                    }).Distinct().ToListAsync();

                Console.WriteLine(players);
                PlayerData[] playersData = new PlayerData[players.Count];

                int j = 0;
                foreach(var p in players)
                {
                    ReportData[] reportData = new ReportData[] { };
                    PlayerData pData = new PlayerData();
                    pData.PlayerKey = p.PlayerKey;
                    pData.PlayerName = p.PlayerName;
                    pData.BirthDate = p.BirthDate;

                    //Query reports here
                    var reports = await (
                        from SR in _context.ScoutingReport
                        where (SR.ScoutKey == scoutId && SR.PlayerKey == p.PlayerKey)
                        select new ReportData
                        {
                            ScoutKey = SR.ScoutKey,
                            CreatedDateTime = SR.CreatedDateTime,
                            Assist = SR.Assist,
                            Defense = SR.Defense,
                            Rebound = SR.Rebound,
                            Shooting = SR.Shooting,
                            Comments = SR.Comments
                        }).ToListAsync();

                    reportData = reports.ToArray();
                    pData.Reports = reportData;
                    playersData[j] = pData;
                    j++;
                }
                team.PlayerData = playersData;
                res[i] = team;
                i++;
            }
            
            return res;
        }

        // GET: api/ScoutingReports/Leagues
        [HttpGet]
        [Route("/api/ScoutingReports/Leagues")]
        public async Task<ActionResult<IEnumerable<League>>> GetLeagues()
        {
            var activeLeagues = await _context.League.Where(l => l.SearchDisplayFlag == true).ToListAsync();
            return activeLeagues;
        }

        // GET: api/ScoutingReports/Teams
        [HttpGet]
        [Route("/api/ScoutingReports/Teams")]
        public async Task<ActionResult<IEnumerable<Team>>> GetTeams(int leagueKey)
        {
            var activeTeams = await _context.Team.Where(t => t.LeagueKey == leagueKey).ToListAsync();
            return activeTeams;
        }

        // GET: api/ScoutingReports/Players?seasonKey=___
        [HttpGet]
        [Route("/api/ScoutingReports/Players")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers(int seasonKey)
        {
            var players = await (
                from TP in _context.TeamPlayer
                join P in _context.Player on TP.PlayerKey equals P.PlayerKey
                where (TP.SeasonKey == seasonKey)
                select new Player
                {
                    PlayerKey = P.PlayerKey,
                    FirstName = P.FirstName,
                    LastName = P.LastName,
                    BirthDate = P.BirthDate,
                    PositionKey = P.PositionKey,
                    Height = P.Height,
                    Weight = P.Weight,
                    Wing = P.Wing
                }).ToListAsync();

            return players;
        }

        // GET: api/ScoutingReports/PlayersByTeam?seasonKey=___&teamKey=___
        [HttpGet]
        [Route("/api/ScoutingReports/PlayersByTeam")]
        public async Task<ActionResult<IEnumerable<Player>>> GetTeamPlayers(int seasonKey, int teamKey)
        {
            var players = await (
                from TP in _context.TeamPlayer
                join P in _context.Player on TP.PlayerKey equals P.PlayerKey
                where (TP.SeasonKey == seasonKey && TP.TeamKey == teamKey)
                select new Player
                {
                    PlayerKey = P.PlayerKey,
                    FirstName = P.FirstName,
                    LastName = P.LastName,
                    BirthDate = P.BirthDate,
                    PositionKey = P.PositionKey,
                    Height = P.Height,
                    Weight = P.Weight,
                    Wing = P.Wing
                }).ToListAsync();

            return players;
        }

        // GET: api/ScoutingReports/Scouts?id=___
        [HttpGet]
        [Route("/api/ScoutingReports/Scout")]
        public async Task<ActionResult<Scout>> GetScout(int id)
        {
            var scout = await _context.Scout.FindAsync(id);

            if (scout == null)
            {
                return NotFound();
            }

            return scout;
        }

        // PUT: api/ScoutingReports/updatescout?id=___
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("/api/ScoutingReports/UpdateScout")]
        public async Task<IActionResult> PutScout(int id, Scout scout)
        {
            if (id != scout.ScoutKey)
            {
                return BadRequest();
            }

            _context.Entry(scout).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScoutExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // PUT: api/ScoutingReports/updatereport?id=___
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut]
        [Route("/api/ScoutingReports/UpdateReport")]
        public async Task<IActionResult> PutReport(int reportId, ScoutingReport report)
        {
             if (reportId != report.ReportKey)
            {
                return BadRequest();
            }           

            var reportToUpdate = _context.ScoutingReport.Find(reportId);
            reportToUpdate.TeamKey = report.TeamKey;
            reportToUpdate.ModifiedDateTime = DateTime.Now;
            reportToUpdate.Comments = report.Comments;
            reportToUpdate.Assist = report.Assist;            
            reportToUpdate.Defense = report.Defense;
            reportToUpdate.Rebound = report.Rebound;
            reportToUpdate.Shooting = report.Shooting;
            _context.Entry(reportToUpdate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReportExists(reportId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ScoutingReports/CreateScout
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("/api/ScoutingReports/CreateScout")]
        public async Task<ActionResult<Scout>> PostScout(Scout scout)
        {
            _context.Scout.Add(scout);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetScout", new { id = scout.ScoutKey }, scout);
            return CreatedAtAction("GetScout", scout);
        }

        // POST: api/ScoutingReports/CreateReport
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        [Route("/api/ScoutingReports/CreateReport")]
        public async Task<ActionResult<ScoutingReport>> PostReport(ScoutingReport report)
        {
            report.ModifiedDateTime = DateTime.Now;
            _context.ScoutingReport.Add(report);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetReport", report);
        }

        // DELETE: api/ScoutingReports/report?reportId=___
        [HttpDelete]
        [Route("/api/ScoutingReports/DeleteReport")]
        public async Task<ActionResult<ScoutingReport>> DeleteReport(int reportId)
        {
            var report = _context.ScoutingReport.Find(reportId);
            if (report == null)
            {
                return NotFound();
            }

            report.IsDeleted = true;
            _context.Entry(report).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ScoutExists(int id)
        {
            return _context.Scout.Any(e => e.ScoutKey == id);
        }

        private bool ReportExists(int id)
        {
            return _context.ScoutingReport.Any(e => e.ReportKey == id);
        }
    }
}
