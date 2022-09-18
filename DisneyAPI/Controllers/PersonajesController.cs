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
    [Route("api/characters")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PersonajesController : ControllerBase
    {
        private readonly IPersonajeRepository repository;

        public PersonajesController(IPersonajeRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<PersonajeGetAllDTO>> GetPersonajes([FromQuery] string name, [FromQuery] int age, [FromQuery] int movie)
        {
            PersonajeQuery query = new PersonajeQuery()
            {
                Name = name,
                Age = age,
                Movie = movie
            };

            try
            {
                return Ok(await repository.GetPersonajes(query));
            }
            catch(Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database");
            }

        }
        
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonajeGetDTO>> GetPersonaje(int id)
        {
            try
            {
                var result = await repository.GetPersonaje(id);
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
        public async Task<ActionResult> PostPersonaje(PersonajePostDTO personajeDTO)
        {
            try
            {
                if(personajeDTO == null)
                {
                    return BadRequest();
                }

                var createdPersonaje = await repository.PostPersonaje(personajeDTO);

                return CreatedAtAction(nameof(PostPersonaje), createdPersonaje.AsGetDTO());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new personaje record");
            }

        }
   
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, PersonajePostDTO personajeDTO)
        {
            try
            {
                var personajeToUpdate = await repository.GetPersonaje(id);

                if(personajeToUpdate == null)
                {
                    return BadRequest();
                }

                return Ok(await repository.Put(id, personajeDTO));
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new personaje record");
            }
        }
   
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var personajeToDelete = await repository.GetPersonaje(id);

                if (personajeToDelete == null)
                {
                    return BadRequest();
                }

                await repository.Delete(id);
                return Ok("Personaje with id = " + id + " deleted");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error creating new personaje record");
            }
        }
    }
}
