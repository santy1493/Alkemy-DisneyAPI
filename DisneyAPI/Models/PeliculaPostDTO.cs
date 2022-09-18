using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DisneyAPI.Models
{
    public class PeliculaPostDTO
    {
        [Required]
        public string Imagen { get; set; }
        [Required]
        public string Titulo { get; set; }
        [Required]
        public DateTime FechaCreacion { get; set; }
        [Required]
        [Range(1, 5)]
        public double Calificacion { get; set; }
        public ICollection<int> PersonajesId { get; set; }
    }
}
