using DisneyAPI.Data;
using DisneyAPI.Models;
using DisneyAPI.Models.Queries;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public class PersonajeRepository : IPersonajeRepository
    {
        private readonly ApplicationDbContext context;

        public PersonajeRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<List<PersonajeGetAllDTO>> GetPersonajes(PersonajeQuery? query)
        {
            List<PersonajeGetAllDTO> personajesDTO = new List<PersonajeGetAllDTO>();
            var personajes = await context.Personajes.Include("Peliculas").ToListAsync();

            if(query != null)
            {
                if (query.Name != null)
                {
                    personajes.RemoveAll(p => p.Nombre.ToLower() != query.Name.ToLower());
                }
                if (query.Age != 0)
                {
                    personajes.RemoveAll(p => p.Edad != query.Age);
                }
                if (query.Movie != 0)
                {
                    personajes.RemoveAll(p => !p.Peliculas.Any(x => x.Id == query.Movie));
                }
            }
            
            foreach (Personaje personaje in personajes)
            {
                personajesDTO.Add(personaje.AsGetAllDTO());
            }

            return personajesDTO;

        }

        public async Task<PersonajeGetDTO> GetPersonaje(int id)
        {
            var personaje = await context.Personajes.Include("Peliculas").Where(p => p.Id == id).FirstOrDefaultAsync(c => c.Id == id);

            if (personaje == null)
            {
                return null;
            }

            return personaje.AsGetDTO();

        }

        public async Task<Personaje> PostPersonaje(PersonajePostDTO personajeDTO)
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

            var result = await context.AddAsync(personaje);
            await context.SaveChangesAsync();

            return result.Entity;

        }

        public async Task<PersonajeGetDTO> Put(int id, PersonajePostDTO personajeDTO)
        {
            var personaje = await context.Personajes.Include("Peliculas").Where(p => p.Id == id).FirstOrDefaultAsync(p => p.Id == id);

            if (personaje == null)
            {
                return null;
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

            var result = context.Update(personaje);
            await context.SaveChangesAsync();

            return result.Entity.AsGetDTO();
        }

        public async Task Delete(int id)
        {
            var personaje = await context.Personajes.FirstOrDefaultAsync(p => p.Id == id);

            if (personaje != null)
            {
                context.Remove(personaje);
                await context.SaveChangesAsync();
            }
        }
    }
}
