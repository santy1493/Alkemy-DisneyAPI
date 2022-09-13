using DisneyAPI.Data;
using DisneyAPI.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Controllers
{
    [Route("api/movies")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeliculasController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PeliculasController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PeliculaGetDTO>>> GetPeliculas([FromQuery] string name, [FromQuery] int genre, [FromQuery] string order)
        {
            List<PeliculaGetDTO> peliculasDTO = new List<PeliculaGetDTO>();

            var peliculas = await context.Peliculas.Include("Personajes").ToListAsync();

            if (genre != 0)
            {
                var genero = await context.Generos.Include("Peliculas").FirstOrDefaultAsync(c => c.Id == genre);
                if (genero != null)
                    peliculas = genero.Peliculas.ToList();
                else
                    peliculas = new List<Pelicula>();
            }
            if (name != null)
            {
                peliculas.RemoveAll(p => p.Titulo != name);
            }
            if (order != null)
            {
                if(order.ToLower() == "asc")
                {
                    peliculas = peliculas.OrderBy(p => p.Titulo).ToList();
                }
                else if(order.ToLower() == "desc")
                {
                    peliculas = peliculas.OrderByDescending(p => p.Titulo).ToList();
                }
                
            }

            foreach (Pelicula pelicula in peliculas)
            {
                peliculasDTO.Add(pelicula.AsGetDTO());
            }

            return Ok(peliculasDTO);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PeliculaGetDTO>> GetPelicula(int id)
        {
            var pelicula = await context.Peliculas.Include("Personajes").Where(p => p.Id == id).FirstOrDefaultAsync(c => c.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            return Ok(pelicula.AsGetDTO());

        }

        [HttpPost]
        public async Task<ActionResult> PostPelicula(PeliculaPostDTO peliculaDTO)
        {
            List<Personaje> personajes = new List<Personaje>();

            foreach (int id in peliculaDTO.PersonajesId)
            {
                var personaje = await context.Personajes.FirstOrDefaultAsync(p => p.Id == id);
                personajes.Add(personaje);
            }

            Pelicula pelicula = new Pelicula()
            {
                Imagen = peliculaDTO.Imagen,
                Titulo = peliculaDTO.Titulo,
                FechaCreacion = peliculaDTO.FechaCreacion,
                Calificacion = peliculaDTO.Calificacion,
                Personajes = personajes
            };

            context.Add(pelicula);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(Pelicula), peliculaDTO);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, PeliculaPostDTO peliculaDTO)
        {
            var pelicula = await context.Peliculas.Include("Personajes").Where(p => p.Id == id).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            List<Personaje> personajes = new List<Personaje>();

            foreach (int personajeId in peliculaDTO.PersonajesId)
            {
                var personaje = await context.Personajes.FirstOrDefaultAsync(p => p.Id == personajeId);
                personajes.Add(personaje);
            }



            pelicula.Imagen = peliculaDTO.Imagen;
            pelicula.Titulo = peliculaDTO.Titulo;
            pelicula.FechaCreacion = peliculaDTO.FechaCreacion;
            pelicula.Calificacion = peliculaDTO.Calificacion;
            pelicula.Personajes = personajes;

            context.Update(pelicula);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
            {
                return NotFound();
            }

            context.Remove(pelicula);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
