using System;
using System.Collections.Generic;

namespace DisneyAPI.Models
{
    public class PeliculaGetDTO
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public double Calificacion { get; set; }
        public ICollection<PersonajeSinListaPeliculas> Personajes { get; set; }
    }
}
