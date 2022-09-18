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
        /*private readonly ApplicationDbContext context;

        public PersonajesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonajeGetAllDTO>>> GetPersonajes([FromQuery] string name, [FromQuery] int age, [FromQuery] int movie)
        {
            List<PersonajeGetAllDTO> personajesDTO = new List<PersonajeGetAllDTO>();
            var personajes = await context.Personajes.Include("Peliculas").ToListAsync();

            if(name != null)
            {
                personajes.RemoveAll(p => p.Nombre.ToLower() != name.ToLower());
            }
            if(age != 0)
            {
                personajes.RemoveAll(p => p.Edad != age);
            }
            if(movie != 0)
            {
                personajes.RemoveAll(p => !p.Peliculas.Any(x => x.Id == movie));
            }

            foreach (Personaje personaje in personajes)
            {
                personajesDTO.Add(personaje.AsGetAllDTO());
            }

            return Ok(personajesDTO);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PersonajeGetDTO>> GetPersonaje(int id)
        {
            var personaje = await context.Personajes.Include("Peliculas").Where(p => p.Id == id).FirstOrDefaultAsync(c => c.Id == id);

            if (personaje == null)
            {
                return NotFound();
            }

            return Ok(personaje.AsGetDTO());

        }

        [HttpPost]
        public async Task<ActionResult> PostPersonaje(PersonajePostDTO personajeDTO)
        {
            List<Pelicula> peliculas = new List<Pelicula>();

            foreach (int id in personajeDTO.PeliculasId)
            {
                var pelicula = await context.Peliculas.FirstOrDefaultAsync(p => p.Id == id);
                peliculas.Add(pelicula);
            }

            Personaje personaje = new Personaje()
            {
                Imagen = personajeDTO.Imagen,
                Nombre = personajeDTO.Nombre,
                Edad = personajeDTO.Edad,
                Peso = personajeDTO.Peso,
                Historia = personajeDTO.Historia,
                Peliculas = peliculas
            };

            context.Add(personaje);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(Personaje), personajeDTO);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, PersonajePostDTO personajeDTO)
        {
            var personaje = await context.Personajes.Include("Peliculas").Where(p => p.Id == id).FirstOrDefaultAsync(p => p.Id == id);

            if(personaje == null)
            {
                return NotFound();
            }

            List<Pelicula> peliculas = new List<Pelicula>();

            foreach (int peliculaId in personajeDTO.PeliculasId)
            {
                var pelicula = await context.Peliculas.FirstOrDefaultAsync(p => p.Id == peliculaId);
                peliculas.Add(pelicula);
            }

            personaje.Imagen = personajeDTO.Imagen;
            personaje.Nombre = personajeDTO.Nombre;
            personaje.Edad = personajeDTO.Edad;
            personaje.Peso = personajeDTO.Peso;
            personaje.Historia = personajeDTO.Historia;
            personaje.Peliculas = peliculas;

            context.Update(personaje);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var personaje = await context.Personajes.FirstOrDefaultAsync(p => p.Id == id);

            if (personaje == null)
            {
                return NotFound();
            }

            context.Remove(personaje);
            await context.SaveChangesAsync();

            return NoContent();
        }
        */

        private readonly IPersonajeRepository repository;

        public PersonajesController(IPersonajeRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult> GetPersonajes([FromQuery] string name, [FromQuery] int age, [FromQuery] int movie)
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
