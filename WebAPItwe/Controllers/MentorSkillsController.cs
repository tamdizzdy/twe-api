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
    public class MentorSkillsController : ControllerBase
    {
        private readonly dbEWTContext _context;

        public MentorSkillsController(dbEWTContext context)
        {
            _context = context;
        }

        // GET: api/MentorSkills
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MentorSkill>>> GetMentorSkills(int pageIndex, int pageSize)
        {
            try
            {
                var result = await (from c in _context.MentorSkills
                                 select new
                                 {
                                     Id = c.Id,
                                     SkillId = c.SkillId,
                                     MentorId = c.MentorId

                                 }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();


                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // GET: api/MentorSkills/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MentorSkill>> GetMentorSkill(string id)
        {
            var result = await (from c in _context.MentorSkills
                              where c.Id == id
                              select new
                              {
                                  c.Id,
                                  c.SkillId,
                                  c.MentorId
                              }).ToListAsync();


            if (!result.Any())
            {
                return BadRequest(new { StatusCode = 404, message = "Not Found" });
            }

            return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
        }

        // PUT: api/MentorSkills/5
        [HttpGet("SKillId")]
        public async Task<ActionResult<MentorSkill>> GetBySKillId(string skillId)
        {
            try
            {
                var result = await (from MentorSkill in _context.MentorSkills
                                    where MentorSkill.SkillId.Contains(skillId)    // search gần đúng
                                    select new
                                    {
                                        MentorSkill.Id,
                                        MentorSkill.SkillId,
                                        MentorSkill.MentorId
                                    }  ).ToListAsync();
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

        [HttpPut("{id}")]
        public async Task<IActionResult> PutMentorSkill(string id, MentorSkill mentorSkill)
        {
            try
            {
                var result = _context.MentorSkills.Find(id);
                result.Id = mentorSkill.Id;
                result.MentorId = mentorSkill.MentorId;
                result.SkillId = mentorSkill.SkillId;
                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // POST: api/MentorSkills
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<MentorSkill>> PostMentorSkill(MentorSkill mentorSkill)
        {
            try
            {
                var result = new MentorSkill();
                result.Id = mentorSkill.Id;
                result.MentorId=mentorSkill.MentorId;
                result.SkillId=mentorSkill.SkillId;

                _context.MentorSkills.Add(result);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }

        // DELETE: api/MentorSkills/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMentorSkill(string id)
        {
            var reslut = await _context.MentorSkills.FindAsync(id);
            if (reslut == null)
            {
                return NotFound();
            }
            _context.MentorSkills.Remove(reslut);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}
