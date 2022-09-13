using DisneyAPI.Data;
using DisneyAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [Route("api/characters")]
    [ApiController]
    public class PersonajesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PersonajesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PersonajeGetDTO>>> GetPersonajes()
        {
            List<PersonajeGetDTO> personajesDTO = new List<PersonajeGetDTO>();
            var personajes = await context.Personajes.Include("Peliculas").ToListAsync();

            foreach (Personaje personaje in personajes)
            {
                personajesDTO.Add(personaje.AsGetDTO());
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

    }
}
