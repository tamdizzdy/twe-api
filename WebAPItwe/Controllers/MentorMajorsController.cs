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
    public class MentorMajorsController : ControllerBase
    {
        private readonly dbEWTContext _context;

        public MentorMajorsController(dbEWTContext context)
        {
            _context = context;
        }

        // GET: api/MentorMajors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MentorMajor>>> GetMentorMajors(int pageIndex, int pageSize)
        {
            try
            {
                var result = await (from c in _context.MentorMajors
                                    select new
                                    {
                                        Id = c.Id,
                                        MajorId = c.MajorId,
                                        MentorId = c.MentorId

                                    }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();


                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // GET: api/MentorMajors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MentorMajor>> GetMentorMajor(string id)
        {
            var result = await (from c in _context.MentorMajors
                                where c.Id == id
                                select new
                                {
                                    c.Id,
                                    c.MajorId,
                                    c.MentorId
                                }).ToListAsync();


            if (!result.Any())
            {
                return BadRequest(new { StatusCode = 404, message = "Not Found" });
            }

            return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
        }


        [HttpGet("mentorId")]
        public async Task<ActionResult<MentorSkill>> GetByMentorId(string mentorId)
        {
            try
            {
                var result = await (from MentorSkill in _context.MentorSkills
                                    where MentorSkill.MentorId.Contains(mentorId)    // search gần đúng
                                    select new
                                    {
                                        MentorSkill.Id,
                                        MentorSkill.SkillId,
                                        MentorSkill.MentorId
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

        [HttpGet("MajorId")]
        public async Task<ActionResult<MentorSkill>> GetByMajorId(string majorId)
        {
            try
            {
                var result = await (from MentorMajor in _context.MentorMajors
                                    where MentorMajor.MajorId.Contains(majorId)    // search gần đúng
                                    select new
                                    {
                                        MentorMajor.Id,
                                        MentorMajor.MajorId,
                                        MentorMajor.MentorId
                                    }).ToListAsync();
                if (!result.Any())
                {
                    return BadRequest(new { StatusCode = 404, message = "Name is not found!" });
                }
                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result});

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }
        // PUT: api/MentorMajors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMentorMajor(string id, MentorMajor mentorMajor)
        {
            try
            {
                var result = _context.MentorMajors.Find(id);
                result.Id = mentorMajor.Id;
                result.MentorId = mentorMajor.MentorId;
                result.MajorId = mentorMajor.MajorId;
                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // POST: api/MentorMajors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MentorMajor>> PostMentorMajor(MentorMajor mentorMajor)
        {
             try
            {
                var result = new MentorMajor();
                result.Id = mentorMajor.Id;
                result.MentorId = mentorMajor.MentorId;
                result.MajorId = mentorMajor.MajorId;

                _context.MentorMajors.Add(result);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // DELETE: api/MentorMajors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMentorMajor(string id)
        {
            var result = await _context.MentorMajors.FindAsync(id);
            if (result == null)
            {
                return NotFound();
            }
            _context.MentorMajors.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
