using DisneyAPI.Models;
using DisneyAPI.Models.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public interface IPeliculaRepository
    {
        public Task<List<PeliculaGetAllDTO>> GetPeliculas(PeliculaQuery? query);
        public Task<PeliculaGetDTO> GetPelicula(int id);
        public Task<Pelicula> PostPelicula(PeliculaPostDTO peliculaDTO);
        public Task<PeliculaGetDTO> Put(int id, PeliculaPostDTO peliculaDTO);
        public Task Delete(int id);
    }
}
