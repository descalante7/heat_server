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

        // GET: api/ScoutingReports/Report?id=___
        [HttpGet]
        [Route("/api/ScoutingReports/Report")]
        public async Task<ActionResult<IEnumerable<ScoutingReport>>> GetReport(int reportKey)
        {
            var scoutingReport = await _context.ScoutingReport.Where(r => r.ReportKey == reportKey).ToListAsync();
            return scoutingReport;
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
