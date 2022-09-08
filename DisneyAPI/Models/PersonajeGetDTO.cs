using System.Collections.Generic;

namespace DisneyAPI.Models
{
    public class PersonajeGetDTO
    {
        public int Id { get; set; }
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public int Edad { get; set; }
        public double Peso { get; set; }
        public string Historia { get; set; }
        public ICollection<PeliculaSinListaPersonajes> Peliculas { get; set; }
    }
}
