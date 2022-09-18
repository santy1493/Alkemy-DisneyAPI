using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models
{
    public class PersonajePostDTO
    {
        [Required]
        public string Imagen { get; set; }
        [Required]
        public string Nombre { get; set; }
        [Required]
        [Range(1, 100)]
        public int Edad { get; set; }
        [Required]
        [Range(1, 300)]
        public double Peso { get; set; }
        [Required]
        public string Historia { get; set; }
        public ICollection<int> PeliculasId { get; set; }
    }
}
