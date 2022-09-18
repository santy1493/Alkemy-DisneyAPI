using DisneyAPI.Models;
using System.Collections.Generic;

namespace DisneyAPI
{
    public static class Extensions
    {
        public static PersonajeSinListaPeliculas RemoverListaPeliculas(this Personaje personaje)
        {
            PersonajeSinListaPeliculas personajeSinListaPeliculas = new PersonajeSinListaPeliculas()
            {
                Id = personaje.Id,
                Imagen = personaje.Imagen,
                Nombre = personaje.Nombre,
                Edad = personaje.Edad,
                Peso = personaje.Peso,
                Historia = personaje.Historia

            };

            return personajeSinListaPeliculas;
        }

        public static ICollection<PersonajeSinListaPeliculas> RemoverListaPeliculas(this ICollection<Personaje> personajes)
        {
            List<PersonajeSinListaPeliculas> personajesSinListaPeliculas = new List<PersonajeSinListaPeliculas>();

            foreach (Personaje personaje in personajes)
            {
                personajesSinListaPeliculas.Add(personaje.RemoverListaPeliculas());
            }

            return personajesSinListaPeliculas;
        }

        public static PeliculaSinListaPersonajes RemoverListaPersonajes(this Pelicula pelicula)
        {
            PeliculaSinListaPersonajes peliculaSinListaPersonajes = new PeliculaSinListaPersonajes()
            {
                Id = pelicula.Id,
                Imagen = pelicula.Imagen,
                Titulo = pelicula.Titulo,
                FechaCreacion = pelicula.FechaCreacion,
                Calificacion = pelicula.Calificacion

            };

            return peliculaSinListaPersonajes;
        }

        public static ICollection<PeliculaSinListaPersonajes> RemoverListaPersonajes(this ICollection<Pelicula> peliculas)
        {
            List<PeliculaSinListaPersonajes> peliculasSinListaPersonajes = new List<PeliculaSinListaPersonajes>();

            foreach (Pelicula pelicula in peliculas)
            {
                peliculasSinListaPersonajes.Add(pelicula.RemoverListaPersonajes());
            }

            return peliculasSinListaPersonajes;
        }

        public static PersonajeGetDTO AsGetDTO(this Personaje personaje)
        {
            PersonajeGetDTO personajeDTO = new PersonajeGetDTO()
            {
                Id = personaje.Id,
                Imagen = personaje.Imagen,
                Nombre = personaje.Nombre,
                Edad = personaje.Edad,
                Peso = personaje.Peso,
                Historia = personaje.Historia,
                Peliculas = personaje.Peliculas.RemoverListaPersonajes()
            };

            return personajeDTO;
        }

        public static PersonajeGetAllDTO AsGetAllDTO(this Personaje personaje)
        {
            PersonajeGetAllDTO personajeDTO = new PersonajeGetAllDTO()
            {
                Id = personaje.Id,
                Imagen = personaje.Imagen,
                Nombre = personaje.Nombre,
            };

            return personajeDTO;
        }

        public static PeliculaGetDTO AsGetDTO(this Pelicula pelicula)
        {
            PeliculaGetDTO peliculaDTO = new PeliculaGetDTO()
            {
                Id = pelicula.Id,
                Imagen = pelicula.Imagen,
                Titulo = pelicula.Titulo,
                FechaCreacion = pelicula.FechaCreacion,
                Calificacion = pelicula.Calificacion,
                Personajes = pelicula.Personajes.RemoverListaPeliculas()
            };

            return peliculaDTO;
        }

        public static PeliculaGetAllDTO AsGetAllDTO(this Pelicula pelicula)
        {
            PeliculaGetAllDTO peliculaDTO = new PeliculaGetAllDTO()
            {
                Id = pelicula.Id,
                Imagen = pelicula.Imagen,
                Titulo = pelicula.Titulo,
                FechaCreacion = pelicula.FechaCreacion,
            };

            return peliculaDTO;
        }

        public static GeneroGetDTO AsGetDTO(this Genero genero)
        {
            GeneroGetDTO generoDTO = new GeneroGetDTO()
            {
                Id = genero.Id,
                Imagen = genero.Imagen,
                Nombre = genero.Nombre,
                Peliculas = genero.Peliculas.RemoverListaPersonajes()
            };

            return generoDTO;
        }
    }
}
