using DisneyAPI.Data;
using DisneyAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public class GeneroRepository : IGeneroRepository
    {

        private readonly ApplicationDbContext context;

        public GeneroRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<GeneroGetDTO>> GetGeneros()
        {
            List<GeneroGetDTO> generosDTO = new List<GeneroGetDTO>();
            var generos = await context.Generos.Include("Peliculas").ToListAsync();

            foreach (Genero genero in generos)
            {
                generosDTO.Add(genero.AsGetDTO());
            }

            return generosDTO;

        }

        public async Task<GeneroGetDTO> GetGenero(int id)
        {
            //var genero = await context.Generos.Include("Peliculas").Where(p => p.Id == id).FirstOrDefaultAsync(c => c.Id == id);
            var genero = await context.Generos.Include("Peliculas").FirstOrDefaultAsync(c => c.Id == id);

            if (genero == null)
            {
                return null;
            }

            return genero.AsGetDTO();

        }

        public async Task<Genero> PostGenero(GeneroPostDTO generoDTO)
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

            var result = await context.AddAsync(genero);
            await context.SaveChangesAsync();

            return result.Entity;

        }

        public async Task<GeneroGetDTO> Put(int id, GeneroPostDTO generoDTO)
        {
            //var genero = await context.Personajes.Include("Peliculas").Where(p => p.Id == id).FirstOrDefaultAsync(p => p.Id == id);
            var genero = await context.Generos.Include("Peliculas").FirstOrDefaultAsync(p => p.Id == id);

            if (genero == null)
            {
                return null;
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

            var result = context.Update(genero);
            await context.SaveChangesAsync();

            return result.Entity.AsGetDTO();

        }

        public async Task Delete(int id)
        {
            var genero = await context.Generos.Include("Peliculas").FirstOrDefaultAsync(p => p.Id == id);

            if (genero != null)
            {
                context.Remove(genero);
                await context.SaveChangesAsync();
            }

        }

    }
}
