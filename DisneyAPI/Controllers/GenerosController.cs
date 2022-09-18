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
    [Route("api/genres")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GenerosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GenerosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<GeneroGetDTO>>> GetGeneros()
        {
            List<GeneroGetDTO> generosDTO = new List<GeneroGetDTO>();
            var generos = await context.Generos.Include("Peliculas").ToListAsync();

            foreach (Genero genero in generos)
            {
                generosDTO.Add(genero.AsGetDTO());
            }

            return Ok(generosDTO);

        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GeneroGetDTO>> GetGenero(int id)
        {
            //var genero = await context.Generos.Include("Peliculas").Where(p => p.Id == id).FirstOrDefaultAsync(c => c.Id == id);
            var genero = await context.Generos.Include("Peliculas").FirstOrDefaultAsync(c => c.Id == id);

            if (genero == null)
            {
                return NotFound();
            }

            return Ok(genero.AsGetDTO());

        }

        [HttpPost]
        public async Task<ActionResult> PostGenero(GeneroPostDTO generoDTO)
        {
            List<Pelicula> peliculas = new List<Pelicula>();

            foreach (int id in generoDTO.PeliculasId)
            {
                var pelicula = await context.Peliculas.FirstOrDefaultAsync(p => p.Id == id);
                peliculas.Add(pelicula);
            }

            Genero genero = new Genero()
            {
                Imagen = generoDTO.Imagen,
                Nombre = generoDTO.Nombre,
                Peliculas = peliculas
            };

            context.Add(genero);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(Genero), generoDTO);

        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, GeneroPostDTO generoDTO)
        {
            //var genero = await context.Personajes.Include("Peliculas").Where(p => p.Id == id).FirstOrDefaultAsync(p => p.Id == id);
            var genero = await context.Generos.Include("Peliculas").FirstOrDefaultAsync(p => p.Id == id);

            if (genero == null)
            {
                return NotFound();
            }

            List<Pelicula> peliculas = new List<Pelicula>();

            foreach (int peliculaId in generoDTO.PeliculasId)
            {
                var pelicula = await context.Peliculas.FirstOrDefaultAsync(p => p.Id == peliculaId);
                peliculas.Add(pelicula);
            }

            genero.Imagen = generoDTO.Imagen;
            genero.Nombre = generoDTO.Nombre;
            genero.Peliculas = peliculas;

            context.Update(genero);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var genero = await context.Generos.Include("Peliculas").FirstOrDefaultAsync(p => p.Id == id);

            if (genero == null)
            {
                return NotFound();
            }

            context.Remove(genero);
            await context.SaveChangesAsync();

            return NoContent();
        }

    }
}
