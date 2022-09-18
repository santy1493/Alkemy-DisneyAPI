using DisneyAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public interface IGeneroRepository
    {
        public Task<List<GeneroGetDTO>> GetGeneros();
        public Task<GeneroGetDTO> GetGenero(int id);
        public Task<Genero> PostGenero(GeneroPostDTO generoDTO);
        public Task<GeneroGetDTO> Put(int id, GeneroPostDTO generoDTO);
        public Task Delete(int id);
    }
}
