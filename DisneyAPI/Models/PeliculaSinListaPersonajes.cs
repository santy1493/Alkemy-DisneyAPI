using System;

namespace DisneyAPI.Models
{
    public class PeliculaSinListaPersonajes
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Titulo { get; set; }
        public DateTime FechaCreacion { get; set; }
        public double Calificacion { get; set; }
    }
}
