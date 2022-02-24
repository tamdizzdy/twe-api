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
    [Route("api/v1/skills")]
    [ApiController]
    public class SkillsController : ControllerBase
    {
        private readonly dbEWTContext _context;

        public SkillsController(dbEWTContext context)
        {
            _context = context;
        }
        //GET: api/v1/Skill?pageIndex=1&pageSize=3
        /// <summary>
        /// Get list all Skill with pagination
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Skill>>> GetAll(int pageIndex, int pageSize)
        {
            try
            {
                var skil = await (from c in _context.Skills
                                 select new
                                 {
                                     Id = c.Id,
                                     Name = c.Name,
                                     
                                 }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();


                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = skil });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }  
        //GET: api/v1/Skill/{id}
        /// <summary>
        /// Get a Skill by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Skill>> GetById(string id)
        {
            var Skil = await (from c in _context.Skills
                              where c.Id == id
                              select new
                              {
                                  c.Id,
                                  c.Name,
                                  
                              }).ToListAsync();


            if (!Skil.Any())
            {
                return BadRequest(new { StatusCode = 404, message = "Not Found" });
            }

            return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = Skil });
        }
        //GET: api/v1/cafe/byName?name=xxx
        /// <summary>
        /// Search Skill by name
        /// </summary>
        [HttpGet("name")]
        public async Task<ActionResult<Skill>> GetByName(string name)
        {
            try
            {
                var result = await (from Skill in _context.Skills
                                    where Skill.Name.Contains(name)    // search gần đúng
                                    select new
                                    {
                                        Skill.Id,
                                        Skill.Name,
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
        /// <summary>
        /// Update Skill by Id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSkill(string id, Skill skill)
        {
            try
            {
                var result = _context.Skills.Find(id);
                result.Id = skill.Id;
                result.Name = skill.Name;
                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });

            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }
        /// <summary>
        /// Create Skill by Id
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Skill>> PostSill(Skill skill)
        {
            try
            {
                var result = new Skill();

                result.Id = skill.Id;
                result.Name = skill.Name;

                _context.Skills.Add(result);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }

        }
        /// <summary>
        /// Delete skill by Id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSkill(string id)
        {
            var skill = await _context.Skills.FindAsync(id);
            if (skill == null)
            {
                return NotFound();
            }
            _context.Skills.Remove(skill);
            await _context.SaveChangesAsync();

            return NoContent();

        }
    }
}
