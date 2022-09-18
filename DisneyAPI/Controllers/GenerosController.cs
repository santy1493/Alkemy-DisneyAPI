using DisneyAPI.Data;
using DisneyAPI.Models;
using DisneyAPI.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [Route("api/genres")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController : ControllerBase
    {
        private readonly IGeneroRepository repository;

        public GenerosController(IGeneroRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<GeneroGetDTO>> GetGeneros()
        {

            try
            {
                return Ok(await repository.GetGeneros());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneroGetDTO>> GetGenero(int id)
        {
            try
            {
                var result = await repository.GetGenero(id);
                if (result == null)
                {
                    return NotFound();
                }

                return Ok(result);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }
        }

        [HttpPost]
        public async Task<ActionResult> PostGenero(GeneroPostDTO generoDTO)
        {
            try
            {
                if (generoDTO == null)
                {
                    return BadRequest();
                }

                var createdGenero = await repository.PostGenero(generoDTO);

                return CreatedAtAction(nameof(PostGenero), createdGenero.AsGetDTO());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new genero record");
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, GeneroPostDTO generoDTO)
        {
            try
            {
                var generoToUpdate = await repository.GetGenero(id);

                if (generoToUpdate == null)
                {
                    return BadRequest();
                }

                return Ok(await repository.Put(id, generoDTO));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating genero record");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var generoToDelete = await repository.GetGenero(id);

                if (generoToDelete == null)
                {
                    return BadRequest();
                }

                await repository.Delete(id);
                return Ok("Genero with id = " + id + " deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting genero record");
            }
        }

    }
}
