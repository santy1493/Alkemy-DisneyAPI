using System.Collections.Generic;

namespace DisneyAPI.Models
{
    public class Genero
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Imagen { get; set; }
        public ICollection<Pelicula> Peliculas { get; set; }
    }
}
