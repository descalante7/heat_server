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

        // GET: api/ScoutingReports
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Scout>>> GetScout()
        {
            return await _context.Scout.ToListAsync();
        }

        // GET: api/ScoutingReports/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Scout>> GetScout(int id)
        {
            var scout = await _context.Scout.FindAsync(id);

            if (scout == null)
            {
                return NotFound();
            }

            return scout;
        }

        // PUT: api/ScoutingReports/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
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

        // POST: api/ScoutingReports
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Scout>> PostScout(Scout scout)
        {
            _context.Scout.Add(scout);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetScout", new { id = scout.ScoutKey }, scout);
            return CreatedAtAction("GetScout", scout);
        }

        // DELETE: api/ScoutingReports/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Scout>> DeleteScout(int id)
        {
            var scout = await _context.Scout.FindAsync(id);
            if (scout == null)
            {
                return NotFound();
            }

            _context.Scout.Remove(scout);
            await _context.SaveChangesAsync();

            return scout;
        }

        private bool ScoutExists(int id)
        {
            return _context.Scout.Any(e => e.ScoutKey == id);
        }
    }
}
