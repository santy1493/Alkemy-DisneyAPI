using System.Collections.Generic;

namespace DisneyAPI.Models
{
    public class GeneroGetDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public ICollection<PeliculaSinListaPersonajes> Peliculas { get; set; }
    }
}
