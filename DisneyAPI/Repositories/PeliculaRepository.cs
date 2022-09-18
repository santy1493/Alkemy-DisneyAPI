using DisneyAPI.Data;
using DisneyAPI.Models;
using DisneyAPI.Models.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly ApplicationDbContext context;

        public PeliculaRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<PeliculaGetAllDTO>> GetPeliculas(PeliculaQuery? query)
        {
            List<PeliculaGetAllDTO> peliculasDTO = new List<PeliculaGetAllDTO>();

            var peliculas = await context.Peliculas.Include("Personajes").ToListAsync();

            if (query != null)
            {
                if (query.Genre != 0)
                {
                    var genero = await context.Generos.Include("Peliculas").FirstOrDefaultAsync(c => c.Id == query.Genre);
                    if (genero != null)
                        peliculas = genero.Peliculas.ToList();
                    else
                        peliculas = new List<Pelicula>();
                }
                if (query.Name != null)
                {
                    peliculas.RemoveAll(p => p.Titulo != query.Name);
                }
                if (query.Order != null)
                {
                    if (query.Order.ToLower() == "asc")
                    {
                        peliculas = peliculas.OrderBy(p => p.Titulo).ToList();
                    }
                    else if (query.Order.ToLower() == "desc")
                    {
                        peliculas = peliculas.OrderByDescending(p => p.Titulo).ToList();
                    }

                }
            }

            foreach (Pelicula pelicula in peliculas)
            {
                peliculasDTO.Add(pelicula.AsGetAllDTO());
            }

            return peliculasDTO;

        }

        public async Task<PeliculaGetDTO> GetPelicula(int id)
        {
            var pelicula = await context.Peliculas.Include("Personajes").Where(p => p.Id == id).FirstOrDefaultAsync(c => c.Id == id);

            if (pelicula == null)
            {
                return null;
            }

            return pelicula.AsGetDTO();
        }

        public async Task<Pelicula> PostPelicula(PeliculaPostDTO peliculaDTO)
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

            var result = await context.AddAsync(pelicula);
            await context.SaveChangesAsync();

            return result.Entity;

        }

        public async Task<PeliculaGetDTO> Put(int id, PeliculaPostDTO peliculaDTO)
        {
            var pelicula = await context.Peliculas.Include("Personajes").Where(p => p.Id == id).FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula == null)
            {
                return null;
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

            var result = context.Update(pelicula);
            await context.SaveChangesAsync();

            return result.Entity.AsGetDTO();

        }

        public async Task Delete(int id)
        {
            var pelicula = await context.Peliculas.FirstOrDefaultAsync(p => p.Id == id);

            if (pelicula != null)
            {
                context.Remove(pelicula);
                await context.SaveChangesAsync();
            }
        }
    }
}
