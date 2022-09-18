using DisneyAPI.Models;
using DisneyAPI.Models.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DisneyAPI.Repositories
{
    public interface IPersonajeRepository
    {
        public Task<List<PersonajeGetAllDTO>> GetPersonajes(PersonajeQuery? query);
        public Task<PersonajeGetDTO> GetPersonaje(int id);
        public Task<Personaje> PostPersonaje(PersonajePostDTO personajeDTO);
        public Task<PersonajeGetDTO> Put(int id, PersonajePostDTO personajeDTO);
        public Task Delete(int id);
    }
}
