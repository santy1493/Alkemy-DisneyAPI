using DisneyAPI.Data;
using DisneyAPI.Models;
using DisneyAPI.Models.Queries;
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
    [Route("api/movies")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepository repository;

        public PeliculasController(IPeliculaRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PeliculaGetAllDTO>> GetPeliculas([FromQuery] string name, [FromQuery] int genre, [FromQuery] string order)
        {
            PeliculaQuery query = new PeliculaQuery()
            {
                Name = name,
                Genre = genre,
                Order = order
            };

            try
            {
                return Ok(await repository.GetPeliculas(query));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaGetDTO>> GetPelicula(int id)
        {
            try
            {
                var result = await repository.GetPelicula(id);
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
        public async Task<ActionResult> PostPelicula(PeliculaPostDTO peliculaDTO)
        {
            try
            {
                if (peliculaDTO == null)
                {
                    return BadRequest();
                }

                var createdPelicula = await repository.PostPelicula(peliculaDTO);

                return CreatedAtAction(nameof(PostPelicula), createdPelicula.AsGetDTO());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new pelicula record");
            }

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, PeliculaPostDTO peliculaDTO)
        {
            try
            {
                var peliculaToUpdate = await repository.GetPelicula(id);

                if (peliculaToUpdate == null)
                {
                    return BadRequest();
                }

                return Ok(await repository.Put(id, peliculaDTO));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error updating pelicula record");
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var peliculaToDelete = await repository.GetPelicula(id);

                if (peliculaToDelete == null)
                {
                    return BadRequest();
                }

                await repository.Delete(id);
                return Ok("Pelicula with id = " + id + " deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting pelicula record");
            }
        }
    }
}
