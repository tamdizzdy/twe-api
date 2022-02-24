﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPItwe.Entities;
using WebAPItwe.InRepositories; 
using WebAPItwe.Models;

namespace WebAPItwe.Controllers
{
    [Route("api/v1/cafe")]
    [ApiController]
    public class CafeController : ControllerBase
    {
        private readonly dbEWTContext _context;

        public CafeController(dbEWTContext context)
        {
            _context = context;
        }
        //GET: api/v1/cafe?pageIndex=1&pageSize=3
        /// <summary>
        /// Get list all cafe with pagination
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Cafe>>> GetAll(int pageIndex, int pageSize)
        {
            try
            {
                var caf = await (from c in _context.Cafes
                                 select new
                                 {
                                     Id = c.Id,
                                     name = c.Name,
                                     Image = c.Image,
                                     Street = c.Street,
                                     Distric = c.Distric,
                                     Description = c.Description
                                 }).Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();


                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = caf });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }
        }
        //GET: api/v1/cafe/{id}
        /// <summary>
        /// Get a cafe by id
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<Cafe>> GetById(string id)
        {
            var cafe = await (from c in _context.Cafes
                              where c.Id == id
                              select new
                              {
                                  c.Id,
                                  c.Name,
                                  c.Image,
                                  c.OpenTime,
                                  c.CloseTime,
                                  c.Street,
                                  c.Distric,
                                  c.Description
                              }).ToListAsync();


            if (!cafe.Any())
            {
                return BadRequest(new { StatusCode = 404, message = "Not Found" });
            }

            return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = cafe });
        }
        //GET: api/v1/cafe/byName?name=xxx
        /// <summary>
        /// Search cafe by name
        /// </summary>
        [HttpGet("name")]
        public async Task<ActionResult<Cafe>> GetByName(string name)
        {
            try
            {
                var result = await (from Cafe in _context.Cafes
                                    where Cafe.Name.Contains(name)    // search gần đúng
                                    select new
                                    {
                                        Cafe.Id,
                                        Cafe.Name,
                                        Cafe.Image,
                                        Cafe.OpenTime,
                                        Cafe.CloseTime,
                                        Cafe.Street,
                                        Cafe.Distric,
                                        Cafe.Description
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
        /// Update cafe by Id
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCafe(string id, Cafe cafe)
        {
            try
            {
                var result = _context.Cafes.Find(id);
                result.Distric = cafe.Distric;
                result.Description = cafe.Description;
                result.Street = cafe.Street;
                result.Image = cafe.Image;
                result.OpenTime = cafe.OpenTime;
                result.CloseTime = cafe.CloseTime;
                result.Name = cafe.Name;

                await _context.SaveChangesAsync();
                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });

            }catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }    
        }
        /// <summary>
        /// Create cafe by Id
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Skill>> PostCafe(Cafe cafe)
        {
            try
            {
                var result = new Cafe();

                result.Id = cafe.Id;
                result.Name = cafe.Name;
                result.Image = cafe.Image;
                result.Description = cafe.Description;
                result.Distric = cafe.Distric;
                result.Street = cafe.Street;

                _context.Cafes.Add(result);
                await _context.SaveChangesAsync();

                return Ok(new { StatusCode = 200, message = "The request was succesfully completed", data = result });
            }
            catch (Exception ex)
            {
                return StatusCode(409, new { StatusCode = 409, message = ex.Message });
            }

        }
        /// <summary>
        /// Delete cafe by Id
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCafe(string id)
        {
            var caf = await _context.Cafes.FindAsync(id);
            if (caf == null)
            {
                return NotFound();
            }
            _context.Cafes.Remove(caf);
            await _context.SaveChangesAsync();

            return NoContent();
           
        }
    }
}

