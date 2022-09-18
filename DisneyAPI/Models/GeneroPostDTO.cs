using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models
{
    public class GeneroPostDTO
    {
        [Required]
        public string Imagen { get; set; }
        [Required]
        public string Nombre { get; set; }
        public ICollection<int> PeliculasId { get; set; }
    }

}
