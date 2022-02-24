using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPItwe.Entities;

namespace WebAPItwe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MajorsController : ControllerBase
    {
        private readonly dbEWTContext _context;

        public MajorsController(dbEWTContext context)
        {
            _context = context;
        }

        // GET: api/Majors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Major>>> GetMajors(int pageIndex, int pageSize)
        {
            try
            {
                var major = await (from c in _context.Majors
                                 select new
                                 {
                                     Id = c.Id,
                                     name = c.Name,
                                 }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();


                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = major });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // GET: api/Majors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Major>> GetMajor(string id)
        {
            var major = await (from c in _context.Majors
                              where c.Id == id
                              select new
                              {
                                  c.Id,
                                  c.Name,
                              }).ToListAsync();
            if (!major.Any())
            {
                return BadRequest(new { StatusCode = 404, message = "Not Found" });
            }

            return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = major });
        }

        [HttpGet("name")]
        public async Task<ActionResult<Major>> GetByName(string name)
        {
            try
            {
                var result = await (from Major in _context.Majors
                                    where Major.Name.Contains(name)    // search gần đúng
                                    select new
                                    {
                                        Major.Id,
                                        Major.Name,
                                    }
                               ).ToListAsync();
                if (!result.Any())
                {
                    return BadRequest(new { StatusCode = 404, message = "Name is not found!" });
                }
                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // PUT: api/Majors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMajor(string id, Major major)
        {
            try
            {
                var result = _context.Majors.Find(id);
                result.Id = major.Id;
                result.Name = major.Name;

                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // POST: api/Majors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Major>> PostMajor(Major major)
        {
            try
            {
                var result = new Major();

                result.Id = major.Id;
                result.Name = major.Name;

                _context.Majors.Add(result);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // DELETE: api/Majors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMajor(string id)
        {
            var major = await _context.Majors.FindAsync(id);
            if (major == null)
            {
                return NotFound();
            }   
            _context.Majors.Remove(major);
            await _context.SaveChangesAsync();

            return NoContent();

          }

    }
}
