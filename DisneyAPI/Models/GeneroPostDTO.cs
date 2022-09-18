using System.Collections.Generic;

namespace DisneyAPI.Models
{
    public class GeneroPostDTO
    {
        public string Imagen { get; set; }
        public string Nombre { get; set; }
        public ICollection<int> PeliculasId { get; set; }
    }

}
