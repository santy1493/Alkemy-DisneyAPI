using System;
using System.Collections.Generic;

namespace DisneyAPI.Models
{
    public class Pelicula
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public double Calificacion { get; set; }
        public ICollection<Personaje> Personajes { get; set; } 

    }
}
