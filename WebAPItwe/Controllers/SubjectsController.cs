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
    public class SubjectsController : ControllerBase
    {
        private readonly dbEWTContext _context;

        public SubjectsController(dbEWTContext context)
        {
            _context = context;
        }

        // GET: api/Subjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Subject>>> GetSubjects(int pageIndex, int pageSize)
        {
            try
            {
                var subject = await (from c in _context.Subjects
                                  select new
                                  {
                                      Id = c.Id,
                                      Name = c.Name,
                                      MajorId = c.MajorId

                                  }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();


                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = subject });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // GET: api/Subjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Subject>> GetSubject(string id)
        {
            var subject = await (from c in _context.Subjects
                              where c.Id == id
                              select new
                              {
                                  c.Id,
                                  c.Name,
                                  c.MajorId

                              }).ToListAsync();


            if (!subject.Any())
            {
                return BadRequest(new { StatusCode = 404, message = "Not Found" });
            }

            return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = subject });
        }

        /// <summary>
        /// Search Skill by name
        /// </summary>
        [HttpGet("name")]
        public async Task<ActionResult<Subject>> GetByName(string name)
        {
            try
            {
                var result = await (from Subject in _context.Subjects
                                    where Subject.Name.Contains(name)    // search gần đúng
                                    select new
                                    {
                                        Subject.Id,
                                        Subject.Name,
                                        Subject.MajorId
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


        [HttpGet("MajorId")]
        public async Task<ActionResult<Subject>> GetByMajorId(string majorId)
        {
            try
            {
                var result = await (from Subject in _context.Subjects
                                    where Subject.MajorId.Contains(majorId)    // search gần đúng
                                    select new
                                    {
                                        Subject.Id,
                                        Subject.MajorId,
                                        Subject.Name
                                    }).ToListAsync();
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
        // PUT: api/Subjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSubject(string id, Subject subject)
        {
            try
            {
                var result = _context.Subjects.Find(id);
                result.Id = subject.Id;
                result.Name = subject.Name;
                result.MajorId = subject.MajorId;
                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // POST: api/Subjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Subject>> PostSubject(Subject subject)
        {
            try
            {
                var result = new Subject();

                result.Id = subject.Id;
                result.Name = subject.Name;
                result.MajorId = subject.MajorId;

                _context.Subjects.Add(result);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // DELETE: api/Subjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSubject(string id)
        {
            var subject = await _context.Subjects.FindAsync(id);
            if (subject == null)
            {
                return NotFound();
            }
            _context.Subjects.Remove(subject);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
